import { Injectable } from '@angular/core';
@Injectable()

export class AuthService {
  private readonly tokenItemName = "PJDesignToken";
  private readonly administratorItemName = "PJDesignAdministrator";

  constructor() { }

  getToken(): string | null {
    return localStorage.getItem(this.tokenItemName);
  }

  setToken(value: string) {
    localStorage.setItem(this.tokenItemName, value);
  }

  removeToken() {
    localStorage.removeItem(this.tokenItemName);
  }

  getAdministrator(): { id: number, name: string, groupId: number } | null {
    const administratorJSON = localStorage.getItem(this.administratorItemName)
    return administratorJSON != null ? JSON.parse(administratorJSON) : null;
  }

  setAdministrator(value: { id: number, name: string, groupId: number }) {
    localStorage.setItem(this.administratorItemName, JSON.stringify(value));
  }

  removeAdministrator() {
    localStorage.removeItem(this.administratorItemName);
  }
}
