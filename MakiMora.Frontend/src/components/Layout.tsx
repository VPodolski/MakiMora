import React, { type ReactNode } from 'react';
import { AppBar, Toolbar, Typography, Container, Box, IconButton, Menu, MenuItem, Tooltip, Avatar } from '@mui/material';
import { useAuth } from '../contexts/AuthContext';
import { Link, useNavigate } from 'react-router-dom';

interface LayoutProps {
  children: ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
  };

  const handleProfileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    // Handle profile menu opening if needed
  };

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            <Link to="/dashboard" style={{ textDecoration: 'none', color: 'inherit' }}>
              MakiMora Backoffice
            </Link>
          </Typography>
          
          {user && (
            <Box sx={{ display: 'flex', alignItems: 'center' }}>
              <Typography variant="body1" sx={{ mr: 2 }}>
                {user.firstName} {user.lastName}
              </Typography>
              
              <Tooltip title="Account settings">
                <IconButton onClick={handleProfileMenuOpen} sx={{ p: 0 }}>
                  <Avatar sx={{ width: 32, height: 32 }}>
                    {user.firstName.charAt(0)}{user.lastName.charAt(0)}
                  </Avatar>
                </IconButton>
              </Tooltip>
              
              <Box sx={{ ml: 2 }}>
                <IconButton color="inherit" onClick={handleLogout}>
                  <Typography variant="button">Выход</Typography>
                </IconButton>
              </Box>
            </Box>
          )}
        </Toolbar>
      </AppBar>
      
      <Container component="main" sx={{ mt: 4, mb: 4, flexGrow: 1 }}>
        {children}
      </Container>
      
      <Box component="footer" sx={{ py: 3, px: 2, mt: 'auto', backgroundColor: 'background.paper' }}>
        <Container maxWidth="lg">
          <Typography variant="body2" color="text.secondary" align="center">
            {'Copyright © MakiMora '}
            {new Date().getFullYear()}
            {'.'}
          </Typography>
        </Container>
      </Box>
    </Box>
  );
};

export default Layout;