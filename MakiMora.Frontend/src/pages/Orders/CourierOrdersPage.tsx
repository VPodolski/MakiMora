import React, { useState, useEffect } from 'react';
import { 
  Container, 
  Typography, 
  Box, 
  Card, 
  CardContent, 
  CardActions, 
  Button, 
  Grid, 
  ListItemText, 
  Divider,
  Chip,
  Alert,
  List,
  ListItem,
  Paper,
  LinearProgress
} from '@mui/material';
import { useAuth } from '../../contexts/AuthContext';
import type { OrderDto } from '../../types/order';
import { apiClient } from '../../services/apiClient';

const CourierOrdersPage: React.FC = () => {
  const { user } = useAuth();
  const [orders, setOrders] = useState<OrderDto[]>([]);
  const [assignedOrders, setAssignedOrders] = useState<OrderDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [todayEarnings, setTodayEarnings] = useState<number>(0);

  // Get user's location if available
  const locationId = user?.locations[0]?.id;

  useEffect(() => {
    if (!locationId) {
      setError('У вас не назначено ни одной торговой точки');
      setLoading(false);
      return;
    }

    const fetchCourierOrders = async () => {
      try {
        setLoading(true);
        
        // Fetch orders available for pickup
        const availableResponse = await apiClient.get<OrderDto[]>(`/orders/available-for-pickup?locationId=${locationId}`);
        
        // Fetch orders assigned to current courier
        const assignedResponse = await apiClient.get<OrderDto[]>(`/orders/by-courier/${user?.id}?status=picked_up`);
        
        setOrders(availableResponse.data);
        setAssignedOrders(assignedResponse.data);
      } catch (err: any) {
        setError(err.response?.data?.message || 'Ошибка при загрузке заказов для доставки');
      } finally {
        setLoading(false);
      }
    };

    const fetchTodayEarnings = async () => {
      try {
        const today = new Date();
        const startOfDay = new Date(today.setHours(0, 0, 0, 0));
        const endOfDay = new Date(today.setHours(23, 59, 999));
        
        const response = await apiClient.get<number>(`/courier-earnings/total?courierId=${user?.id}&startDate=${startOfDay.toISOString()}&endDate=${endOfDay.toISOString()}`);
        setTodayEarnings(response.data);
      } catch (err: any) {
        // Don't show error for earnings calculation as it's not critical
        console.error('Error fetching earnings:', err);
      }
    };

    fetchCourierOrders();
    fetchTodayEarnings();
    
    // Set up polling to refresh orders every 30 seconds
    const intervalId = setInterval(() => {
      fetchCourierOrders();
      fetchTodayEarnings();
    }, 30000);
    
    return () => clearInterval(intervalId);
  }, [locationId, user?.id]);

  const handleAssignOrder = async (orderId: string) => {
    try {
      await apiClient.patch(`/orders/${orderId}/assign-courier`, {
        courierId: user?.id
      });
      
      // Move order from available to assigned
      setOrders(prevOrders => prevOrders.filter(order => order.id !== orderId));
      setAssignedOrders(prevAssigned => [...prevAssigned, ...orders.filter(order => order.id === orderId)]);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка при назначении заказа');
    }
  };

  const handleMarkOrderDelivered = async (orderId: string) => {
    try {
      await apiClient.patch(`/orders/${orderId}/delivered`);
      
      // Move order from assigned to delivered
      setAssignedOrders(prevAssigned => prevAssigned.filter(order => order.id !== orderId));
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка при обновлении статуса доставки');
    }
  };

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <Alert severity="error">{error}</Alert>
      </Container>
    );
  }

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ mt: 4 }}>
        <LinearProgress />
        <Typography variant="h6" align="center" sx={{ mt: 2 }}>Загрузка заказов для доставки...</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        Заказы для доставки
      </Typography>
      
      {/* Statistics Panel */}
      <Paper elevation={3} sx={{ p: 2, mb: 3 }}>
        <Typography variant="h6" gutterBottom>
          Статистика за сегодня
        </Typography>
        <Box sx={{ display: 'flex', gap: 3 }}>
          <Box>
            <Typography variant="body2" color="text.secondary">Заработок</Typography>
            <Typography variant="h5" color="primary">{todayEarnings.toFixed(2)} ₽</Typography>
          </Box>
          <Box>
            <Typography variant="body2" color="text.secondary">Доставлено заказов</Typography>
            <Typography variant="h5">{assignedOrders.filter(o => o.status.name === 'delivered').length}</Typography>
          </Box>
        </Box>
      </Paper>
      
      {/* Available Orders for Pickup */}
      <Typography variant="h5" gutterBottom sx={{ mt: 3 }}>
        Доступные для забора
      </Typography>
      
      {orders.length === 0 ? (
        <Alert severity="info">Нет заказов, готовых к забору</Alert>
      ) : (
        <Grid container spacing={3} sx={{ mb: 4 }}>
          {orders.map(order => (
            <Grid key={order.id} size={{ xs: 12, md: 6, lg: 4 }}>
              <Card>
                <CardContent>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <Typography variant="h6">Заказ #{order.orderNumber}</Typography>
                    <Chip 
                      label={order.status.displayName} 
                      color="primary" 
                    />
                  </Box>
                  
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                    Клиент: {order.customerName}
                  </Typography>
                  
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                    Телефон: {order.customerPhone}
                  </Typography>
                  
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    Адрес: {order.customerAddress}
                  </Typography>
                  
                  <Divider sx={{ my: 1 }} />
                  
                  <Typography variant="subtitle2" gutterBottom>
                    Блюда:
                  </Typography>
                  
                  <List dense>
                    {order.items.map(item => (
                      <ListItem key={item.id} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                        <ListItemText 
                          primary={`${item.product.name} x${item.quantity}`} 
                          secondary={`Цена: ${(item.unitPrice * item.quantity).toFixed(2)} руб.`}
                        />
                      </ListItem>
                    ))}
                  </List>
                  
                  <Typography variant="subtitle2" sx={{ mt: 2 }}>
                    Общая сумма: {order.totalAmount.toFixed(2)} руб.
                  </Typography>
                </CardContent>
                <CardActions sx={{ justifyContent: 'flex-end' }}>
                  <Button 
                    variant="contained" 
                    color="primary"
                    onClick={() => handleAssignOrder(order.id)}
                  >
                    Взять в доставку
                  </Button>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}
      
      {/* Assigned Orders for Delivery */}
      <Typography variant="h5" gutterBottom>
        Мои заказы в доставке
      </Typography>
      
      {assignedOrders.length === 0 ? (
        <Alert severity="info">У вас нет заказов в доставке</Alert>
      ) : (
        <Grid container spacing={3}>
          {assignedOrders.map(order => (
            <Grid key={order.id} size={{ xs: 12, md: 6, lg: 4 }}>
              <Card>
                <CardContent>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <Typography variant="h6">Заказ #{order.orderNumber}</Typography>
                    <Chip 
                      label={order.status.displayName} 
                      color={order.status.name === 'picked_up' ? 'warning' : 'success'} 
                    />
                  </Box>
                  
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                    Клиент: {order.customerName}
                  </Typography>
                  
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                    Телефон: {order.customerPhone}
                  </Typography>
                  
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    Адрес: {order.customerAddress}
                  </Typography>
                  
                  <Divider sx={{ my: 1 }} />
                  
                  <Typography variant="subtitle2" gutterBottom>
                    Блюда:
                  </Typography>
                  
                  <List dense>
                    {order.items.map(item => (
                      <ListItem key={item.id} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                        <ListItemText 
                          primary={`${item.product.name} x${item.quantity}`} 
                          secondary={`Цена: ${(item.unitPrice * item.quantity).toFixed(2)} руб.`}
                        />
                      </ListItem>
                    ))}
                  </List>
                  
                  <Typography variant="subtitle2" sx={{ mt: 2 }}>
                    Общая сумма: {order.totalAmount.toFixed(2)} руб.
                  </Typography>
                </CardContent>
                <CardActions sx={{ justifyContent: 'space-between' }}>
                  <Typography variant="caption" color="text.secondary">
                    Назначен: {new Date(order.updatedAt).toLocaleString('ru-RU')}
                  </Typography>
                  {order.status.name === 'picked_up' && (
                    <Button 
                      variant="contained" 
                      color="success"
                      onClick={() => handleMarkOrderDelivered(order.id)}
                    >
                      Заказ доставлен
                    </Button>
                  )}
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}
    </Container>
  );
};

export default CourierOrdersPage;