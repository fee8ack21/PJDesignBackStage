import { Injectable } from '@angular/core';
@Injectable()

export class AuthService {
  readonly itemName = "PJDesignToken";

  constructor() { }

  checkToken() {

  }

  getToken(): string | null {
    return localStorage.getItem(this.itemName);
  }

  setToken(value: string) {
    localStorage.setItem(this.itemName, value);
  }

  removeToken() {
    localStorage.removeItem(this.itemName);
  }
}
