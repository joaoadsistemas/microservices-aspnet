using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using GeekShopping.CartAPI.DTOs;
using GeekShopping.CartAPI.Repositories.Interfaces;

namespace GeekShopping.CartAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _httpClient;

        public CouponRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CouponDTO> GetCouponByCode(string couponCode, string token)
        {

            // ACESSANDO O OUTRO MICROSERVIÇO
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"/coupons/{couponCode}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK) return new CouponDTO();
            return JsonSerializer.Deserialize<CouponDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
