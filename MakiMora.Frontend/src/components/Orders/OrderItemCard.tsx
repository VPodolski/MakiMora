import React from 'react';
import { Card, CardContent, CardActions, Typography, Button, Box, Chip } from '@mui/material';
import type { OrderItemDto } from '../../types/order';

interface OrderItemCardProps {
  item: OrderItemDto;
  onStatusChange: (itemId: string, statusId: string) => void;
  allowedStatusChanges: boolean;
}

const OrderItemCard: React.FC<OrderItemCardProps> = ({ item, onStatusChange, allowedStatusChanges }) => {
  return (
    <Card sx={{ mb: 2 }}>
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Box>
            <Typography variant="h6">{item.product.name}</Typography>
            <Typography variant="body2" color="text.secondary">
              Количество: {item.quantity}, Цена: {item.unitPrice.toFixed(2)} × {item.quantity} = {item.totalPrice.toFixed(2)} руб.
            </Typography>
          </Box>
          <Chip 
            label={item.status.displayName} 
            color={item.status.name === 'prepared' ? 'success' : 
                   item.status.name === 'pending' ? 'default' : 
                   item.status.name === 'preparing' ? 'primary' : 
                   item.status.name === 'assembled' ? 'secondary' : 'default'} 
            size="small"
          />
        </Box>
      </CardContent>
      {allowedStatusChanges && (
        <CardActions sx={{ justifyContent: 'flex-end' }}>
          <Button 
            variant="contained" 
            size="small"
            onClick={() => onStatusChange(item.id, item.status.id)}
          >
            Отметить как готовое
          </Button>
        </CardActions>
      )}
    </Card>
  );
};

export default OrderItemCard;