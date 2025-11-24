import type { UserDto, LocationDto } from './auth';

export interface OrderDto {
  id: string;
  orderNumber: string;
  customerName: string;
  customerPhone: string;
  customerAddress: string;
  locationId: string;
  status: OrderStatusDto;
  courier?: UserDto;
  assembler?: UserDto;
  totalAmount: number;
  deliveryFee: number;
  comment?: string;
  items: OrderItemDto[];
  createdAt: string;
  updatedAt: string;
  deliveryTime?: string;
  completedAt?: string;
}

export interface OrderItemDto {
  id: string;
  product: ProductDto;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  status: OrderItemStatusDto;
  preparedBy?: UserDto;
  preparedAt?: string;
  assembledBy?: UserDto;
  assembledAt?: string;
}

export interface OrderStatusDto {
  id: string;
  name: string;
  displayName: string;
  description?: string;
  sortOrder: number;
  isActive: boolean;
}

export interface OrderItemStatusDto {
  id: string;
  name: string;
  displayName: string;
  description?: string;
  sortOrder: number;
  isActive: boolean;
}

export interface ProductDto {
  id: string;
  name: string;
  description?: string;
  price: number;
  categoryId: string;
  locationId: string;
  imageUrl?: string;
  preparationTime?: number;
  isAvailable: boolean;
  isOnStopList: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateOrderRequestDto {
  customerName: string;
  customerPhone: string;
  customerAddress: string;
  locationId: string;
  deliveryFee: number;
  comment?: string;
  deliveryTime?: string;
  items: CreateOrderItemRequestDto[];
}

export interface CreateOrderItemRequestDto {
  productId: string;
  quantity: number;
}

export interface UpdateOrderItemStatusRequestDto {
  statusId: string;
  note?: string;
}

export interface AssignCourierRequestDto {
  courierId: string;
}

export interface UpdateOrderStatusRequestDto {
  statusId: string;
  note?: string;
}