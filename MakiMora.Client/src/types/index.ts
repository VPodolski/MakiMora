export interface User {
  id: string;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  phone?: string;
  isActive: boolean;
  roles: Role[];
  locations: Location[];
  createdAt: string;
  updatedAt: string;
}

export interface Role {
  id: string;
  name: string;
  description?: string;
  createdAt: string;
}

export interface Location {
  id: string;
  name: string;
  address: string;
  phone?: string;
  latitude?: number;
  longitude?: number;
  createdAt: string;
  updatedAt: string;
}

export interface Category {
  id: string;
  name: string;
  description?: string;
  locationId: string;
  isActive: boolean;
  sortOrder: number;
  createdAt: string;
  updatedAt: string;
}

export interface Product {
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

export interface OrderItem {
  id: string;
  product: Product;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  status: OrderItemStatus;
  preparedBy?: User;
  preparedAt?: string;
  assembledBy?: User;
  assembledAt?: string;
}

export interface OrderItemStatus {
  id: string;
  name: string;
  displayName: string;
  description?: string;
  sortOrder: number;
  isActive: boolean;
}

export interface OrderStatus {
  id: string;
  name: string;
  displayName: string;
  description?: string;
  sortOrder: number;
  isActive: boolean;
}

export interface Order {
  id: string;
  orderNumber: string;
  customerName: string;
  customerPhone: string;
  customerAddress: string;
  locationId: string;
  status: OrderStatus;
  courier?: User;
  assembler?: User;
  totalAmount: number;
  deliveryFee: number;
  comment?: string;
  items: OrderItem[];
  createdAt: string;
  updatedAt: string;
  deliveryTime?: string;
  completedAt?: string;
}

export interface CreateOrderRequest {
  customerName: string;
  customerPhone: string;
  customerAddress: string;
  locationId: string;
  deliveryFee: number;
  comment?: string;
  deliveryTime?: string;
  items: CreateOrderItemRequest[];
}

export interface CreateOrderItemRequest {
  productId: string;
  quantity: number;
}

export interface UpdateOrderStatusRequest {
  statusId: string;
  note?: string;
}

export interface UpdateOrderItemStatusRequest {
  statusId: string;
  note?: string;
}

export interface AssignCourierRequest {
  courierId: string;
}

export interface CreateProductRequest {
  name: string;
  description?: string;
  price: number;
  categoryId: string;
  locationId: string;
  imageUrl?: string;
  preparationTime?: number;
  isAvailable: boolean;
}

export interface UpdateProductRequest {
  name: string;
  description?: string;
  price: number;
  categoryId: string;
  imageUrl?: string;
  preparationTime?: number;
  isAvailable: boolean;
}

export interface CreateCategoryRequest {
  name: string;
  description?: string;
  locationId: string;
  isActive: boolean;
  sortOrder: number;
}

export interface UpdateCategoryRequest {
  name: string;
  description?: string;
  locationId: string;
  isActive: boolean;
  sortOrder: number;
}

export interface CreateLocationRequest {
  name: string;
  address: string;
  phone?: string;
  latitude?: number;
  longitude?: number;
}

export interface UpdateLocationRequest {
  name: string;
  address: string;
  phone?: string;
  latitude?: number;
  longitude?: number;
}

export interface InventorySupply {
  id: string;
  locationId: string;
  supplierName: string;
  supplyDate: string;
  expectedDate: string;
  status: string; // pending, delivered, cancelled
  totalCost?: number;
  managerId: string;
  createdAt: string;
  deliveredAt?: string;
  items: InventorySupplyItem[];
}

export interface InventorySupplyItem {
  id: string;
  supplyId: string;
  productId?: string;
  productName: string;
  quantity: number;
  unitCost: number;
  totalCost: number;
}

export interface CourierEarning {
  id: string;
  courierId: string;
  orderId: string;
  amount: number;
  earningType: string; // delivery_fee, bonus, penalty
  date: string;
  createdAt: string;
}

export interface CreateInventorySupplyRequest {
  locationId: string;
  supplierName: string;
  supplyDate: string;
  expectedDate: string;
  managerId: string;
  items: CreateInventorySupplyItemRequest[];
}

export interface CreateInventorySupplyItemRequest {
  productId?: string;
  productName: string;
  quantity: number;
  unitCost: number;
}

export interface UpdateInventorySupplyRequest {
  supplierName: string;
  supplyDate: string;
  expectedDate: string;
}

export interface UpdateSupplyStatusRequest {
  status: string;
}

export interface CreateCourierEarningRequest {
  courierId: string;
  orderId: string;
  amount: number;
  earningType: string;
  date: string;
}

export interface CreateUserRequest {
  username: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phone?: string;
  roleIds: string[];
  locationIds: string[];
  isActive: boolean;
}

export interface UpdateUserRequest {
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  phone?: string;
  roleIds: string[];
  locationIds: string[];
  isActive: boolean;
}