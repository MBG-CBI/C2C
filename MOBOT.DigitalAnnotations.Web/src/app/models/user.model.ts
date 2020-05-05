import { UserGroup } from './user-group.model';

export interface User {
  id: number;
  email: string;
  password: string;
  groups: UserGroup[];
}
