import React from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

interface ProtectedRouteProps {
  children: React.ReactElement;
  allowedRoles?: string[];
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, allowedRoles }) => {
  const { isAuthenticated, user } = useAuth();
  const location = useLocation();

  // Check if user has required role
  const hasRequiredRole = () => {
    if (!allowedRoles || !user) return true;
    
    const userRoles = user.roles.map(role => role.name.toLowerCase());
    return allowedRoles.some(role => userRoles.includes(role.toLowerCase()));
  };

  // If not authenticated, redirect to login
  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  // If authenticated but doesn't have required role, redirect to dashboard
  if (!hasRequiredRole()) {
    return <Navigate to="/dashboard" replace />;
  }

  return children;
};

export default ProtectedRoute;