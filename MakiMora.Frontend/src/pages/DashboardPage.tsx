import React from 'react';
import { useAuth } from '../contexts/AuthContext';
import {
  Container,
  Typography,
  Box,
  Grid,
  Card,
  CardContent,
  List,
  ListItem,
  ListItemText
} from '@mui/material';

const DashboardPage: React.FC = () => {
  const { user } = useAuth();

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Grid container spacing={3}>
        <Grid item xs={12}>
          <Typography variant="h4" gutterBottom>
            Добро пожаловать, {user?.firstName} {user?.lastName}!
          </Typography>
          <Typography variant="subtitle1" color="text.secondary">
            Ваша роль: {user?.roles.map(r => r.name).join(', ')}
          </Typography>
        </Grid>
        
        {/* Cards for different functionalities based on role */}
        {user?.roles.some(r => ['manager', 'hr'].includes(r.name.toLowerCase())) && (
          <>
            <Grid item xs={12} md={4} key="users-management">
              <Card>
                <CardContent>
                  <Typography variant="h6" gutterBottom>
                    Управление пользователями
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Просмотр, создание и редактирование сотрудников
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            
            <Grid item xs={12} md={4} key="menu-management">
              <Card>
                <CardContent>
                  <Typography variant="h6" gutterBottom>
                    Управление меню
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Добавление и редактирование товаров в меню
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            
            <Grid item xs={12} md={4} key="location-management">
              <Card>
                <CardContent>
                  <Typography variant="h6" gutterBottom>
                    Управление точками
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Просмотр и управление торговыми точками
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          </>
        )}
        
        {user?.roles.some(r => ['sushi_chef', 'packer', 'courier'].includes(r.name.toLowerCase())) && (
          <>
            <Grid item xs={12} md={4} key="my-orders">
              <Card>
                <CardContent>
                  <Typography variant="h6" gutterBottom>
                    Мои заказы
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Просмотр заказов, назначенных вам
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          </>
        )}
        
        {user?.roles.some(r => ['sushi_chef'].includes(r.name.toLowerCase())) && (
          <Grid item xs={12} md={4} key="preparation-orders">
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Заказы на приготовление
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Просмотр заказов, ожидающих приготовления
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        )}
        
        {user?.roles.some(r => ['packer'].includes(r.name.toLowerCase())) && (
          <Grid item xs={12} md={4} key="packing-orders">
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Заказы на упаковку
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Просмотр заказов, готовых к упаковке
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        )}
        
        {user?.roles.some(r => ['courier'].includes(r.name.toLowerCase())) && (
          <Grid item xs={12} md={4} key="delivery-orders">
            <Card>
              <CardContent>
                <Typography variant="h6" gutterBottom>
                  Заказы на доставку
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Просмотр и выбор заказов для доставки
                </Typography>
              </CardContent>
            </Card>
          </Grid>
        )}
        
        <Grid item xs={12} md={4} key="recent-activity">
          <Card>
            <CardContent>
              <Typography variant="h6" gutterBottom>
                Недавняя активность
              </Typography>
              <List dense>
                <ListItem>
                  <ListItemText primary="Новый заказ #ORD20251124001" secondary="Создан 5 минут назад" />
                </ListItem>
                <ListItem>
                  <ListItemText primary="Заказ #ORD20251124002" secondary="Готов к сборке" />
                </ListItem>
                <ListItem>
                  <ListItemText primary="Заказ #ORD20251124003" secondary="Доставлен" />
                </ListItem>
              </List>
            </CardContent>
          </Card>
        </Grid>
      </Grid>
    </Container>
  );
};

export default DashboardPage;