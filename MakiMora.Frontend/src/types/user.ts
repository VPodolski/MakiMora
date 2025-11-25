export interface User {
  id: string;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  phone?: string;
  isActive: boolean;
  roles: Role[];
  locations: Location[];
  createdAt: string;
  updatedAt: string;
}

export interface Role {
  id: string;
  name: string;
  description?: string;
  createdAt: string;
}

export interface Location {
  id: string;
  name: string;
  address: string;
  phone?: string;
  latitude?: number;
  longitude?: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateUserRequest {
  username: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phone?: string;
  roleIds: string[];
  locationIds: string[];
  isActive: boolean;
}

export interface UpdateUserRequest {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  phone?: string;
  roleIds: string[];
  locationIds: string[];
  isActive: boolean;
}

export interface AssignCourierRequestDto {
  courierId: string;
}

export interface UpdateOrderStatusRequestDto {
  statusId: string;
  note?: string;
}

export interface PaginationParams {
  page: number;
  pageSize: number;
}