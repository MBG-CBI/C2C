import { User } from './user.model';
import { Group } from './group.model';

export interface UserGroup {
  userGroupId: number;
  userId: number;
  user: User;
  groupId: number;
  group: Group;
}
