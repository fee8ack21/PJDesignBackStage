export class AuthLoginRequest {
  account: string;
  password: string;
}

export class AuthLoginResponse {
  id: number;
  name: string;
  token: string;
}
