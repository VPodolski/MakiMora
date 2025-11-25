import React, { useEffect, useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Checkbox,
  ListItemText,
  OutlinedInput,
  Chip,
  Box
} from '@mui/material';
import { UserDto, RoleDto, LocationDto } from '../../types/user';

interface UserFormProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: any) => Promise<void>;
  user?: UserDto | null;
  roles: RoleDto[];
  locations: LocationDto[];
}

const UserForm: React.FC<UserFormProps> = ({ open, onClose, onSubmit, user, roles, locations }) => {
  const [formData, setFormData] = useState<any>({
    username: '',
    email: '',
    password: '',
    firstName: '',
    lastName: '',
    phone: '',
    roleIds: [],
    locationIds: [],
    isActive: true
  });
  
  const [isEditing, setIsEditing] = useState(false);

  useEffect(() => {
    if (user) {
      setFormData({
        username: user.username,
        email: user.email,
        firstName: user.firstName,
        lastName: user.lastName,
        phone: user.phone || '',
        roleIds: user.roles.map((r: RoleDto) => r.id),
        locationIds: user.locations.map((l: LocationDto) => l.id),
        isActive: user.isActive,
        password: '' // Don't set existing password
      });
      setIsEditing(true);
    } else {
      setFormData({
        username: '',
        email: '',
        password: '',
        firstName: '',
        lastName: '',
        phone: '',
        roleIds: [],
        locationIds: [],
        isActive: true
      });
      setIsEditing(false);
    }
  }, [user]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleRoleChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    const {
      target: { value },
    } = event;
    setFormData(prev => ({
      ...prev,
      roleIds: typeof value === 'string' ? value.split(',') : value,
    }));
  };

  const handleLocationChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    const {
      target: { value },
    } = event;
    setFormData(prev => ({
      ...prev,
      locationIds: typeof value === 'string' ? value.split(',') : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await onSubmit(formData);
  };

  const handleClose = () => {
    onClose();
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <form onSubmit={handleSubmit}>
        <DialogTitle>
          {isEditing ? 'Редактировать пользователя' : 'Создать нового пользователя'}
        </DialogTitle>
        <DialogContent>
          <Box sx={{ mt: 2 }}>
            <TextField
              margin="dense"
              name="username"
              label="Имя пользователя"
              type="text"
              fullWidth
              variant="outlined"
              value={formData.username}
              onChange={handleChange}
              required
            />
            <TextField
              margin="dense"
              name="email"
              label="Email"
              type="email"
              fullWidth
              variant="outlined"
              value={formData.email}
              onChange={handleChange}
              required
              sx={{ mt: 1 }}
            />
            {!isEditing && (
              <TextField
                margin="dense"
                name="password"
                label="Пароль"
                type="password"
                fullWidth
                variant="outlined"
                value={formData.password}
                onChange={handleChange}
                required
                sx={{ mt: 1 }}
              />
            )}
            <TextField
              margin="dense"
              name="firstName"
              label="Имя"
              type="text"
              fullWidth
              variant="outlined"
              value={formData.firstName}
              onChange={handleChange}
              required
              sx={{ mt: 1 }}
            />
            <TextField
              margin="dense"
              name="lastName"
              label="Фамилия"
              type="text"
              fullWidth
              variant="outlined"
              value={formData.lastName}
              onChange={handleChange}
              required
              sx={{ mt: 1 }}
            />
            <TextField
              margin="dense"
              name="phone"
              label="Телефон"
              type="tel"
              fullWidth
              variant="outlined"
              value={formData.phone}
              onChange={handleChange}
              sx={{ mt: 1 }}
            />
            <FormControl fullWidth sx={{ mt: 1 }}>
              <InputLabel>Роли</InputLabel>
              <Select
                multiple
                name="roleIds"
                value={formData.roleIds}
                onChange={handleRoleChange}
                input={<OutlinedInput label="Роли" />}
                renderValue={(selected: any) => (
                  <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                    {selected.map((value: string) => (
                      <Chip
                        key={value}
                        label={roles.find((r: RoleDto) => r.id === value)?.name}
                        size="small"
                      />
                    ))}
                  </Box>
                )}
              >
                {roles.map((role: RoleDto) => (
                  <MenuItem key={role.id} value={role.id}>
                    <Checkbox checked={formData.roleIds.indexOf(role.id) > -1} />
                    <ListItemText primary={role.name} />
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            <FormControl fullWidth sx={{ mt: 1 }}>
              <InputLabel>Локации</InputLabel>
              <Select
                multiple
                name="locationIds"
                value={formData.locationIds}
                onChange={handleLocationChange}
                input={<OutlinedInput label="Локации" />}
                renderValue={(selected: any) => (
                  <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                    {selected.map((value: string) => (
                      <Chip
                        key={value}
                        label={locations.find((l: LocationDto) => l.id === value)?.name}
                        size="small"
                      />
                    ))}
                  </Box>
                )}
              >
                {locations.map((location: LocationDto) => (
                  <MenuItem key={location.id} value={location.id}>
                    <Checkbox checked={formData.locationIds.indexOf(location.id) > -1} />
                    <ListItemText primary={location.name} />
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            <Box sx={{ mt: 2 }}>
              <label>
                <input
                  type="checkbox"
                  name="isActive"
                  checked={formData.isActive}
                  onChange={(e) => setFormData(prev => ({ ...prev, isActive: (e.target as HTMLInputElement).checked }))}
                />
                Активный
              </label>
            </Box>
          </Box>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose}>Отмена</Button>
          <Button type="submit" variant="contained">
            {isEditing ? 'Обновить' : 'Создать'}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  );
};

export default UserForm;