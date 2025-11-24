import { create } from 'zustand';
import type { LoginRequest, LoginResponse, AuthState } from '../types';
import { apiClient } from '../services/apiClient';

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  token: null,
  isAuthenticated: false,

  login: async (credentials: LoginRequest) => {
    try {
      const response = await apiClient.post<LoginResponse>('/auth/login', credentials);
      const { accessToken, user } = response.data;

      // Store token in localStorage
      localStorage.setItem('token', accessToken);
      
      set({
        user,
        token: accessToken,
        isAuthenticated: true
      });
    } catch (error: any) {
      console.error('Login failed:', error.response?.data || error.message);
      throw new Error(error.response?.data?.message || 'Login failed');
    }
  },

  logout: () => {
    // Remove token from localStorage
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    
    set({
      user: null,
      token: null,
      isAuthenticated: false
    });
  },

  initializeAuth: async () => {
    const token = localStorage.getItem('token');
    if (token) {
      try {
        // In a real app, you might want to validate the token with an API call
        // For now, we'll just set the stored token
        set({ token, isAuthenticated: true });
      } catch (error) {
        console.error('Token validation failed:', error);
        localStorage.removeItem('token');
        set({ token: null, isAuthenticated: false });
      }
    }
  }
}));