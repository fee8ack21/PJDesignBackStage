export class CreateOrUpdateAdministratorRequest {
  id?: number;
  account: string;
  name: string;
  password?: string;
  groupId: number;
  isEnabled: boolean;
}
