export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
}

export interface CreateUserRequest {
  firstName: string;
  lastName: string;
  email: string;
  passwordHash: string;
}

export interface UpdateUserRequest {
  firstName: string;
  lastName: string;
  email: string;
}
