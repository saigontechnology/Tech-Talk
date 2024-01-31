import { Injectable } from '@nestjs/common';
import { UsersDto } from './dto/users.dto';

@Injectable()
export class UsersService {
  private readonly userList = [
    { id: 1, name: 'Tan' },
    { id: 2, name: 'Khanh' },
    { id: 3, name: 'Phuc' },
    { id: 4, name: 'Minh' },
  ];
  getUsers() {
    return this.userList;
  }

  getUserById(id: number) {
    return this.userList.find((user) => user.id === id);
  }

  createUser(user: UsersDto) {
    this.userList.push(user);
    return user;
  }
}
