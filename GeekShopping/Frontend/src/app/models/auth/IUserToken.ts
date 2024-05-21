export interface IUserToken {
  id: string;
  email: string;
  token: string;
  refreshToken: string;
  expiration : Date | string;
  role: Array<string>;

}
