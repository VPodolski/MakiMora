import React from 'react';
import { useForm, Controller } from 'react-hook-form';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  FormControlLabel,
  Switch,
  Select,
  MenuItem,
  Checkbox,
  ListItemText,
  OutlinedInput,
  Chip,
  Box,
  Stack
} from '@mui/material';
import type { CreateUserRequest, UpdateUserRequest, User, Role, Location } from '../../types/user';

interface UserFormProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (data: CreateUserRequest | UpdateUserRequest) => void;
  user?: User | null;
  roles: Role[];
  locations: Location[];
}

const UserForm: React.FC<UserFormProps> = ({ open, onClose, onSubmit, user, roles, locations }) => {
  const isEditing = !!user;
  
  interface UserFormValues {
    username: string;
    email: string;
    password?: string;
    firstName: string;
    lastName: string;
    phone?: string;
    roleIds: string[];
    locationIds: string[];
    isActive: boolean;
  }
  
  const {
    register,
    handleSubmit,
    reset,
    control,
    formState: { errors }
  } = useForm<UserFormValues>({
    defaultValues: {
      username: user?.username || '',
      email: user?.email || '',
      firstName: user?.firstName || '',
      lastName: user?.lastName || '',
      phone: user?.phone || '',
      roleIds: user?.roles.map((r: Role) => r.id) || [],
      locationIds: user?.locations.map((l: Location) => l.id) || [],
      isActive: user?.isActive || false,
      password: '' // Only for create mode
    }
  });

  const handleClose = () => {
    reset();
    onClose();
  };

  const handleFormSubmit = async (data: UserFormValues) => {
    if (isEditing) {
      const updateData: UpdateUserRequest = {
        username: data.username,
        email: data.email,
        firstName: data.firstName,
        lastName: data.lastName,
        phone: data.phone,
        roleIds: data.roleIds,
        locationIds: data.locationIds,
        isActive: data.isActive
      };
      await onSubmit(updateData);
    } else {
      const createData: CreateUserRequest = {
        username: data.username,
        email: data.email,
        password: data.password || '',
        firstName: data.firstName,
        lastName: data.lastName,
        phone: data.phone,
        roleIds: data.roleIds,
        locationIds: data.locationIds,
        isActive: data.isActive
      };
      await onSubmit(createData);
    }
    
    reset();
    handleClose();
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>
        {isEditing ? 'Редактировать сотрудника' : 'Добавить нового сотрудника'}
      </DialogTitle>
      <form onSubmit={handleSubmit(handleFormSubmit)}>
        <DialogContent>
          <Box sx={{ mt: 1 }}>
            <Stack spacing={2}>
              <TextField
                label="Имя пользователя"
                variant="outlined"
                {...register('username', { required: 'Имя пользователя обязательно' })}
                error={!!errors.username}
                helperText={errors.username?.message}
              />
              <TextField
                label="Email"
                variant="outlined"
                {...register('email', { 
                  required: 'Email обязателен',
                  pattern: {
                    value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                    message: 'Некорректный email адрес'
                  }
                })}
                error={!!errors.email}
                helperText={errors.email?.message}
              />
              {!isEditing && (
                <TextField
                  label="Пароль"
                  type="password"
                  variant="outlined"
                  {...register('password', { 
                    required: !isEditing ? 'Пароль обязателен' : false,
                    minLength: {
                      value: 6,
                      message: 'Пароль должен содержать минимум 6 символов'
                    }
                  })}
                  error={!!errors.password}
                  helperText={errors.password?.message}
                />
              )}
              <Box sx={{ display: 'flex', gap: 2 }}>
                <TextField
                  sx={{ flex: 1 }}
                  label="Имя"
                  variant="outlined"
                  {...register('firstName', { required: 'Имя обязательно' })}
                  error={!!errors.firstName}
                  helperText={errors.firstName?.message}
                />
                <TextField
                  sx={{ flex: 1 }}
                  label="Фамилия"
                  variant="outlined"
                  {...register('lastName', { required: 'Фамилия обязательна' })}
                  error={!!errors.lastName}
                  helperText={errors.lastName?.message}
                />
              </Box>
              <Box sx={{ display: 'flex', gap: 2 }}>
                <TextField
                  sx={{ flex: 1 }}
                  label="Телефон"
                  variant="outlined"
                  {...register('phone')}
                />
                <Controller
                  name="isActive"
                  control={control}
                  render={({ field: { onChange, value } }) => (
                    <FormControlLabel
                      control={
                        <Switch
                          checked={value}
                          onChange={(e, checked) => onChange(checked)}
                        />
                      }
                      label={value ? "Активный" : "Неактивный"}
                    />
                  )}
                />
              </Box>
              <Controller
                name="roleIds"
                control={control}
                render={({ field: { onChange, value } }) => (
                  <Select
                    multiple
                    value={value}
                    onChange={onChange}
                    input={<OutlinedInput />}
                    renderValue={(selected) => (
                      <div>
                        {selected.map((value) => (
                          <Chip
                            key={value}
                            label={roles.find((r: Role) => r.id === value)?.name}
                            size="small"
                          />
                        ))}
                      </div>
                    )}
                  >
                    {roles.map((role: Role) => (
                      <MenuItem key={role.id} value={role.id}>
                        <Checkbox checked={value.includes(role.id)} />
                        <ListItemText primary={role.name} />
                      </MenuItem>
                    ))}
                  </Select>
                )}
              />
              <Controller
                name="locationIds"
                control={control}
                render={({ field: { onChange, value } }) => (
                  <Select
                    multiple
                    value={value}
                    onChange={onChange}
                    input={<OutlinedInput />}
                    renderValue={(selected) => (
                      <div>
                        {selected.map((value) => (
                          <Chip
                            key={value}
                            label={locations.find((l: Location) => l.id === value)?.name}
                            size="small"
                          />
                        ))}
                      </div>
                    )}
                  >
                    {locations.map((location: Location) => (
                      <MenuItem key={location.id} value={location.id}>
                        <Checkbox checked={value.includes(location.id)} />
                        <ListItemText primary={location.name} />
                      </MenuItem>
                    ))}
                  </Select>
                )}
              />
            </Stack>
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