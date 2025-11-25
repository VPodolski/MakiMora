import React, { useState, useEffect } from 'react';
import { 
  Container, 
  Typography, 
  Box, 
  Button, 
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Alert
} from '@mui/material';
import { Edit as EditIcon, Delete as DeleteIcon, Add as AddIcon } from '@mui/icons-material';
import { useAuth } from '../../contexts/AuthContext';
import { UserDto } from '../../types/user';
import { apiClient } from '../../services/apiClient';
import UserForm from '../../components/UserManagement/UserForm';

const UserManagementPage: React.FC = () => {
  const { user: currentUser } = useAuth();
  const [users, setUsers] = useState<UserDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [openForm, setOpenForm] = useState(false);
  const [editingUser, setEditingUser] = useState<UserDto | null>(null);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        setLoading(true);
        const response = await apiClient.get<UserDto[]>('/users');
        setUsers(response.data);
      } catch (err: any) {
        setError(err.response?.data?.message || 'Ошибка при загрузке пользователей');
      } finally {
        setLoading(false);
      }
    };

    fetchUsers();
  }, []);

  const handleOpenForm = () => {
    setEditingUser(null);
    setOpenForm(true);
  };

  const handleEdit = (user: UserDto) => {
    setEditingUser(user);
    setOpenForm(true);
  };

  const handleDelete = async (id: string) => {
    if (window.confirm('Вы уверены, что хотите удалить этого пользователя?')) {
      try {
        await apiClient.delete(`/users/${id}`);
        setUsers(users.filter(user => user.id !== id));
      } catch (err: any) {
        setError(err.response?.data?.message || 'Ошибка при удалении пользователя');
      }
    }
  };

  const handleSubmit = async (userData: any) => {
    try {
      if (editingUser) {
        const response = await apiClient.put<UserDto>(`/users/${editingUser.id}`, userData);
        setUsers(users.map(u => u.id === editingUser.id ? response.data : u));
      } else {
        const response = await apiClient.post<UserDto>('/users', userData);
        setUsers([...users, response.data]);
      }
      setOpenForm(false);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка при сохранении пользователя');
    }
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setEditingUser(null);
  };

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">{error}</Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4">Управление пользователями</Typography>
        <Button 
          variant="contained" 
          startIcon={<AddIcon />}
          onClick={handleOpenForm}
        >
          Добавить пользователя
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Имя пользователя</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Имя</TableCell>
              <TableCell>Фамилия</TableCell>
              <TableCell>Телефон</TableCell>
              <TableCell>Роли</TableCell>
              <TableCell>Локации</TableCell>
              <TableCell>Активный</TableCell>
              <TableCell>Действия</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {users.map((user) => (
              <TableRow key={user.id}>
                <TableCell>{user.id}</TableCell>
                <TableCell>{user.username}</TableCell>
                <TableCell>{user.email}</TableCell>
                <TableCell>{user.firstName}</TableCell>
                <TableCell>{user.lastName}</TableCell>
                <TableCell>{user.phone || '-'}</TableCell>
                <TableCell>{user.roles.map(r => r.name).join(', ')}</TableCell>
                <TableCell>{user.locations.map(l => l.name).join(', ')}</TableCell>
                <TableCell>{user.isActive ? 'Да' : 'Нет'}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleEdit(user)}>
                    <EditIcon />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(user.id)}>
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <UserForm
        open={openForm}
        onClose={handleCloseForm}
        onSubmit={handleSubmit}
        user={editingUser}
      />
    </Container>
  );
};

export default UserManagementPage;