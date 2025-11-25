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
  ListItem
} from '@mui/material';
import { useAuth } from '../../contexts/AuthContext';
import type { OrderDto, OrderItemDto } from '../../types/order';
import { apiClient } from '../../services/apiClient';

const PackingOrdersPage: React.FC = () => {
  const { user } = useAuth();
  const [orders, setOrders] = useState<OrderDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Get user's location if available
  const locationId = user?.locations[0]?.id;

  useEffect(() => {
    if (!locationId) {
      setError('У вас не назначено ни одной торговой точки');
      setLoading(false);
      return;
    }

    const fetchPackingOrders = async () => {
      try {
        setLoading(true);
        const response = await apiClient.get<OrderDto[]>(`/orders/packing?locationId=${locationId}`);
        setOrders(response.data);
      } catch (err: any) {
        setError(err.response?.data?.message || 'Ошибка при загрузке заказов для упаковки');
      } finally {
        setLoading(false);
      }
    };

    fetchPackingOrders();
    
    // Set up polling to refresh orders every 30 seconds
    const intervalId = setInterval(fetchPackingOrders, 30000);
    
    return () => clearInterval(intervalId);
  }, [locationId]);

  const handleMarkOrderPacked = async (orderId: string) => {
    try {
      await apiClient.patch(`/orders/${orderId}/packed`);
      
      // Update the order in the local state
      setOrders(prevOrders => 
        prevOrders.map(order => 
          order.id === orderId 
            ? { ...order, status: { ...order.status, name: "assembled", displayName: "Собран" } } 
            : order
        )
      );
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка при обновлении статуса заказа');
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
        <Typography variant="h6" align="center">Загрузка заказов для упаковки...</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        Заказы для упаковки
      </Typography>
      
      {orders.length === 0 ? (
        <Alert severity="info">Нет заказов для упаковки</Alert>
      ) : (
        <Grid container spacing={3}>
          {orders.map(order => (
            <Grid key={order.id} size={{ xs: 12, md: 6, lg: 4 }}>
              <Card>
                <CardContent>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <Typography variant="h6">Заказ #{order.orderNumber}</Typography>
                    <Chip 
                      label={order.status.displayName} 
                      color={order.status.name === 'ready' ? 'primary' : 'secondary'} 
                    />
                  </Box>
                  
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                    Клиент: {order.customerName}
                  </Typography>
                  
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
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
                        <Chip 
                          label={item.status.displayName} 
                          color={item.status.name === 'prepared' ? 'success' : 'default'} 
                          size="small"
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
                    Создан: {new Date(order.createdAt).toLocaleString('ru-RU')}
                  </Typography>
                  <Button 
                    variant="contained" 
                    color="primary"
                    onClick={() => handleMarkOrderPacked(order.id)}
                    disabled={order.status.name !== 'ready'}
                  >
                    Заказ собран
                  </Button>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}
    </Container>
  );
};

export default PackingOrdersPage;