import { apiClient } from './apiClient';
import type { User, CreateUserRequest, UpdateUserRequest, PaginationParams } from '../types/user';

export const userService = {
  getUsers: async (params?: PaginationParams) => {
    const response = await apiClient.get<{ data: User[]; totalCount: number }>('/users', {
      params
    });
    return response.data;
  },

  getUserById: async (id: string) => {
    const response = await apiClient.get<User>(`/users/${id}`);
    return response.data;
  },

  createUser: async (userData: CreateUserRequest) => {
    const response = await apiClient.post<User>('/users', userData);
    return response.data;
  },

  updateUser: async (id: string, userData: UpdateUserRequest) => {
    const response = await apiClient.put<User>(`/users/${id}`, userData);
    return response.data;
  },

  deleteUser: async (id: string) => {
    await apiClient.delete(`/users/${id}`);
  },

  deactivateUser: async (id: string) => {
    const response = await apiClient.patch<User>(`/users/${id}`, { isActive: false });
    return response.data;
  },

  activateUser: async (id: string) => {
    const response = await apiClient.patch<User>(`/users/${id}`, { isActive: true });
    return response.data;
  },
};