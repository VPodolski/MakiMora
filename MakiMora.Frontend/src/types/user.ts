export interface UserDto {
  id: string;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  phone?: string;
  isActive: boolean;
  roles: RoleDto[];
  locations: LocationDto[];
  createdAt: string;
  updatedAt: string;
}

export interface RoleDto {
  id: string;
  name: string;
  description?: string;
  createdAt: string;
}

export interface LocationDto {
  id: string;
  name: string;
  address: string;
  phone?: string;
  latitude?: number;
  longitude?: number;
  createdAt: string;
  updatedAt: string;
}

export interface CreateUserRequestDto {
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

export interface UpdateUserRequestDto {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  phone?: string;
  roleIds: string[];
  locationIds: string[];
  isActive: boolean;
}