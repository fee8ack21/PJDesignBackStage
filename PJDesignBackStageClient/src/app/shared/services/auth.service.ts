import { Injectable } from '@angular/core';
@Injectable()

export class AuthService {
  private readonly tokenItemName = "PJDesignToken";
  private readonly administratorItemName = "PJDesignAdministratorName";

  constructor() { }

  checkToken() {
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenItemName);
  }

  setToken(value: string) {
    localStorage.setItem(this.tokenItemName, value);
  }

  removeToken() {
    localStorage.removeItem(this.tokenItemName);
  }

  getAdministratorName(): string | null {
    return localStorage.getItem(this.administratorItemName);
  }

  setAdministratorName(value: string) {
    localStorage.setItem(this.administratorItemName, value);
  }

  removeAdministratorName() {
    localStorage.removeItem(this.administratorItemName);
  }
}
