using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GeekShopping.Authentication.DTOs;
using GeekShopping.Authentication.Entities;
using GeekShopping.Authentication.Interfaces;

[Route("/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthController(ITokenService tokenService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost("createRole/{roleName}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> CreateRole(string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (roleResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseDTO { Status = "Success", Message = $"Role {roleName} added successfully!" });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = $"Issue adding the new {roleName} role" });
        }
        return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = $"Role already exists" });
    }

    [HttpPost("AddUserToRole/{email}/{roleName}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> AddUserRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, $"User {user.Email} added to the {roleName} role");
                return StatusCode(StatusCodes.Status200OK, new ResponseDTO { Status = "Success", Message = $"User {user.Email} added to the {roleName} role!" });
            }
            _logger.LogInformation(1, $"Error: Unable to add user {user.Email} to the {roleName} role");
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = $"Error: Unable to add user {user.Email} to the {roleName} role" });
        }
        return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = $"Unable to find User" });
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, loginDTO.Password!))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
            var refreshToken = _tokenService.GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);
            await _userManager.UpdateAsync(user);
            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken,
                Expiration = token.ValidTo,
                Email = user.Email!,
                Role = userRoles,
                Id = user.Id
            });
        }
        return Unauthorized("Email ou senha inválidos");
    }

    [HttpPut("add-refresh-token")]
    [Authorize(Policy = "ClientOrLawyer")]
    public async Task<ActionResult> RefreshToken(RevokeDTO dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "Email does not exists!" });
        }
        user.RefreshToken = Guid.NewGuid().ToString();
        return new ObjectResult(new { refreshToken = user.RefreshToken });
    }

    [HttpPost("register/client")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseDTO), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> RegisterClient([FromBody] RegisterUserDTO registerDTO)
    {
        var userExists = await _userManager.FindByEmailAsync(registerDTO.Email);
        if (userExists != null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new ResponseDTO { Status = "Error", Message = "Email already exists!" });
        }
        string randomUserName = GenerateRandomUserName(registerDTO.Name);
        User user = new()
        {
            Email = registerDTO.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = randomUserName,
            Name = registerDTO.Name,
            PhoneNumber = registerDTO.PhoneNumber,
            PhoneNumberConfirmed = false,

        };
        var result = await _userManager.CreateAsync(user, registerDTO.Password);
        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed" });
        }
        await _userManager.AddToRoleAsync(user, "Client");
        return StatusCode(StatusCodes.Status201Created, new ResponseDTO { Status = "Success", Message = "User created successfully!" });
    }

    [HttpPut("updatePassword")]
    [Authorize(Policy = "ClientOrLawyer")]
    public async Task<ActionResult> UpdatePasswordOwner([FromBody] ChangePasswordDTO dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userLogged = await _userManager.FindByIdAsync(userId);
        var passwordHasher = new PasswordHasher<IdentityUser>();
        var result = passwordHasher.VerifyHashedPassword(userLogged, userLogged.PasswordHash, dto.OldPassword);
        if (result == PasswordVerificationResult.Success)
        {
            if (dto.NewPassword == dto.ConfirmPassword)
            {
                var hashedPassword = passwordHasher.HashPassword(userLogged, dto.ConfirmPassword);
                userLogged.PasswordHash = hashedPassword;
                await _userManager.UpdateAsync(userLogged);
                return StatusCode(StatusCodes.Status201Created, new ResponseDTO { Status = "Success", Message = "Password changed successfully!" });
            }
            else
            {
                return BadRequest("The passwords do not match");
            }
        }
        else
        {
            return BadRequest("Incorrect old password");
        }
    }

    [HttpPost("refresh-token")]
    [Authorize(Policy = "ClientOrLawyer")]
    public async Task<ActionResult> RefreshToken(TokenDTO tokenDTO)
    {
        if (tokenDTO == null)
        {
            return BadRequest("Invalid client request");
        }
        string? accessToken = tokenDTO.AccessToken ?? throw new ArgumentNullException(nameof(tokenDTO));
        string? refreshToken = tokenDTO.RefreshToken ?? throw new ArgumentNullException(nameof(tokenDTO));
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);
        if (principal == null)
        {
            return BadRequest("Invalid AccessToken / RefreshToken");
        }
        string username = principal.Identity.Name;
        var user = await _userManager.FindByNameAsync(username!);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return BadRequest("Invalid AccessToken / RefreshToken");
        }
        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);
        return new ObjectResult(new { accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken), refreshToken = newRefreshToken });
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("revoke")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Revoke(RevokeDTO emailDTO)
    {
        var user = await _userManager.FindByEmailAsync(emailDTO.Email);
        if (user == null)
        {
            _logger.LogInformation(3, $"Usuário não encontrado para o e-mail: {emailDTO}");
            return BadRequest("Invalid email");
        }
        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
        _logger.LogInformation(4, $"Token revogado para o usuário com e-mail: {emailDTO}");
        return NoContent();
    }

    private static string GenerateRandomUserName(string name)
    {
        string formattedName = name.Replace(" ", "").ToLower();
        Random random = new Random();
        int randomNumber = random.Next(0, 9999999);
        string randomUserName = formattedName + randomNumber;
        return randomUserName;
    }
}

    
