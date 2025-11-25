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

const KitchenOrdersPage: React.FC = () => {
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

    const fetchKitchenOrders = async () => {
      try {
        setLoading(true);
        const response = await apiClient.get<OrderDto[]>(`/orders/kitchen?locationId=${locationId}`);
        setOrders(response.data);
      } catch (err: any) {
        setError(err.response?.data?.message || 'Ошибка при загрузке заказов');
      } finally {
        setLoading(false);
      }
    };

    fetchKitchenOrders();
    
    // Set up polling to refresh orders every 30 seconds
    const intervalId = setInterval(fetchKitchenOrders, 30000);
    
    return () => clearInterval(intervalId);
  }, [locationId]);

  const handleMarkItemReady = async (orderItem: OrderItemDto) => {
    try {
      // Find the "prepared" status ID
      const preparedStatus = await findStatusIdByName("prepared");
      
      if (!preparedStatus) {
        setError('Статус "готово" не найден');
        return;
      }

      await apiClient.patch(`/orders/items/${orderItem.id}/status`, {
        statusId: preparedStatus
      });

      // Update the order in the local state
      setOrders(prevOrders => 
        prevOrders.map(order => {
          if (order.items.some(item => item.id === orderItem.id)) {
            return {
              ...order,
              items: order.items.map(item => 
                item.id === orderItem.id 
                  ? { ...item, status: { ...item.status, name: "prepared", displayName: "Готово" } } 
                  : item
              )
            };
          }
          return order;
        })
      );
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка при обновлении статуса блюда');
    }
  };

  const findStatusIdByName = async (statusName: string): Promise<string | null> => {
    // In a real implementation, this would call an API endpoint to get status IDs
    // For now, we'll return mock IDs
    const statusIds: Record<string, string> = {
      "pending": "pending-status-id",
      "preparing": "preparing-status-id",
      "prepared": "prepared-status-id",
      "assembled": "assembled-status-id",
      "delivered": "delivered-status-id",
      "cancelled": "cancelled-status-id"
    };
    
    return statusIds[statusName] || null;
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
        <Typography variant="h6" align="center">Загрузка заказов...</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom>
        Заказы для кухни
      </Typography>
      
      {orders.length === 0 ? (
        <Alert severity="info">Нет заказов для приготовления</Alert>
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
                      color={order.status.name === 'pending' ? 'primary' : 'secondary'} 
                    />
                  </Box>
                  
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                    Клиент: {order.customerName}
                  </Typography>
                  
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    Телефон: {order.customerPhone}
                  </Typography>
                  
                  <Divider sx={{ my: 1 }} />
                  
                  <Typography variant="subtitle2" gutterBottom>
                    Блюда:
                  </Typography>
                  
                  <List dense>
                    {order.items
                      .filter(item => item.status.name === 'pending' || item.status.name === 'preparing')
                      .map(item => (
                        <ListItem key={item.id} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                          <ListItemText 
                            primary={`${item.product.name} x${item.quantity}`} 
                            secondary={`Цена: ${(item.unitPrice * item.quantity).toFixed(2)} руб.`}
                          />
                          <Button 
                            variant="contained" 
                            size="small"
                            onClick={() => handleMarkItemReady(item)}
                            disabled={item.status.name !== 'pending' && item.status.name !== 'preparing'}
                          >
                            Готово
                          </Button>
                        </ListItem>
                      ))
                    }
                  </List>
                </CardContent>
                <CardActions sx={{ justifyContent: 'flex-end' }}>
                  <Typography variant="caption" color="text.secondary">
                    Создан: {new Date(order.createdAt).toLocaleString('ru-RU')}
                  </Typography>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}
    </Container>
  );
};

export default KitchenOrdersPage;