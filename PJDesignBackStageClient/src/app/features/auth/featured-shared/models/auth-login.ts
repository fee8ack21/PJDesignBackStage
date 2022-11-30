export class AuthLoginRequest {
  account: string;
  password: string;
}

export class AuthLoginResponse {
  name: string;
  token: string;
}
