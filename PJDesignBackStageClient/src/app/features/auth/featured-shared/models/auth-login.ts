export class AuthLoginRequest {
  account: string;
  password: string;
}

export class AuthLoginResponse {
  id: number;
  groupId: number;
  name: string;
  token: string;
}
