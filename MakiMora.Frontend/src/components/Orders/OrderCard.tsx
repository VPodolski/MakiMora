import React from 'react';
import { Card, CardContent, CardActions, Typography, Button, Box, Chip, Divider, List, ListItem, ListItemText } from '@mui/material';
import type { OrderDto } from '../../types/order';

interface OrderCardProps {
  order: OrderDto;
  onStatusChange?: (orderId: string, statusId: string) => void;
  onAssignCourier?: (orderId: string, courierId: string) => void;
  onMarkDelivered?: (orderId: string) => void;
  allowedActions?: string[]; // 'update-status', 'assign-courier', 'mark-delivered'
}

const OrderCard: React.FC<OrderCardProps> = ({ 
  order, 
  onStatusChange, 
  onAssignCourier, 
  onMarkDelivered, 
  allowedActions = [] 
}) => {
  const canUpdateStatus = allowedActions.includes('update-status');
  const canAssignCourier = allowedActions.includes('assign-courier');
  const canMarkDelivered = allowedActions.includes('mark-delivered');
  
  return (
    <Card sx={{ mb: 3 }}>
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
          <Typography variant="h6">Заказ #{order.orderNumber}</Typography>
          <Chip 
            label={order.status.displayName} 
            color={order.status.name === 'delivered' ? 'success' :
                   order.status.name === 'cancelled' ? 'error' :
                   order.status.name === 'pending' || order.status.name === 'preparing' ? 'primary' :
                   order.status.name === 'ready' || order.status.name === 'assembled' ? 'warning' : 'default'} 
            size="small"
          />
        </Box>
        
        <Box sx={{ display: 'flex', gap: 2, mb: 2 }}>
          <Box>
            <Typography variant="body2" color="text.secondary">Клиент:</Typography>
            <Typography>{order.customerName}</Typography>
          </Box>
          <Box>
            <Typography variant="body2" color="text.secondary">Телефон:</Typography>
            <Typography>{order.customerPhone}</Typography>
          </Box>
        </Box>
        
        <Box sx={{ mb: 2 }}>
          <Typography variant="body2" color="text.secondary">Адрес:</Typography>
          <Typography>{order.customerAddress}</Typography>
        </Box>
        
        <Divider sx={{ my: 2 }} />
        
        <Typography variant="subtitle2" gutterBottom>Состав заказа:</Typography>
        <List dense>
          {order.items.map(item => (
            <ListItem key={item.id} sx={{ pl: 0 }}>
              <ListItemText 
                primary={`${item.product.name} x${item.quantity}`}
                secondary={`Цена: ${(item.unitPrice * item.quantity).toFixed(2)} руб.`}
              />
              <Chip 
                label={item.status.displayName} 
                size="small"
                color={item.status.name === 'prepared' ? 'success' : 
                       item.status.name === 'pending' ? 'default' : 
                       item.status.name === 'preparing' ? 'primary' : 
                       item.status.name === 'assembled' ? 'warning' : 'default'} 
              />
            </ListItem>
          ))}
        </List>
        
        <Box sx={{ mt: 2, display: 'flex', justifyContent: 'space-between' }}>
          <Typography variant="body1">Итого: {order.totalAmount.toFixed(2)} руб.</Typography>
          {order.deliveryFee > 0 && (
            <Typography variant="body2" color="text.secondary">Доставка: {order.deliveryFee.toFixed(2)} руб.</Typography>
          )}
        </Box>
      </CardContent>
      
      <CardActions sx={{ justifyContent: 'space-between' }}>
        <Box>
          <Typography variant="caption" color="text.secondary">
            Создан: {new Date(order.createdAt).toLocaleString('ru-RU')}
          </Typography>
          {order.completedAt && (
            <Typography variant="caption" color="text.secondary" sx={{ ml: 2 }}>
              Завершен: {new Date(order.completedAt).toLocaleString('ru-RU')}
            </Typography>
          )}
        </Box>
        
        <Box sx={{ display: 'flex', gap: 1 }}>
          {canUpdateStatus && (
            <Button 
              variant="outlined" 
              size="small"
              onClick={() => onStatusChange?.(order.id, '')}
            >
              Обновить статус
            </Button>
          )}
          
          {canAssignCourier && (
            <Button 
              variant="contained" 
              size="small"
              onClick={() => onAssignCourier?.(order.id, '')}
            >
              Назначить курьера
            </Button>
          )}
          
          {canMarkDelivered && (
            <Button 
              variant="contained" 
              color="success" 
              size="small"
              onClick={() => onMarkDelivered?.(order.id)}
            >
              Отметить доставленным
            </Button>
          )}
        </Box>
      </CardActions>
    </Card>
  );
};

export default OrderCard;