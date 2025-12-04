// User-related DTOs
export interface UserLoginDTO {
  email: string;
  password: string;
}

export interface UserRegisterDTO {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phone: string;
}

export interface UserProfileDTO {
  id: string;
  firstName: string;
 lastName: string;
  email: string;
  phone: string;
  createdAt: string;
}

// Product-related DTOs
export interface ProductDTO {
  id: string;
  name: string;
  description: string;
 price: number;
  weight: number;
  categoryId: string;
  categoryName: string;
  imageUrl: string | null;
  isAvailable: boolean;
  isOnStopList?: boolean;
}

export interface ProductCreateDTO {
  name: string;
  description: string;
  price: number;
  weight: number;
  categoryId: string;
 imageUrl?: string;
}

export interface ProductUpdateDTO {
  id: string;
  name: string;
  description: string;
  price: number;
  weight: number;
  categoryId: string;
  imageUrl?: string;
  isAvailable: boolean;
}

// Category-related DTOs
export interface CategoryDTO {
  id: string;
  name: string;
  description: string;
  isActive: boolean;
}

export interface CategoryCreateDTO {
  name: string;
  description: string;
}

export interface CategoryUpdateDTO {
  id: string;
  name: string;
  description: string;
  isActive: boolean;
}

// Order-related DTOs
export interface OrderDTO {
  id: string;
  userId: string;
  customerName: string;
  customerPhone: string;
  customerEmail: string;
  deliveryAddress: string;
  orderStatus: string;
  totalAmount: number;
  createdAt: string;
  updatedAt: string;
  orderItems: OrderItemDTO[];
}

export interface OrderCreateDTO {
  customerName: string;
  customerPhone: string;
  customerEmail: string;
  deliveryAddress: string;
  deliveryTime: string;
  specialInstructions: string;
  orderItems: OrderItemCreateDTO[];
  totalAmount: number;
}

export interface OrderUpdateDTO {
  id: string;
  orderStatus: string;
}

export interface OrderItemDTO {
  id: string;
  orderId: string;
  productId: string;
 productName: string;
 productPrice: number;
  quantity: number;
  orderItemStatus: string;
}

export interface OrderItemCreateDTO {
  productId: string;
  quantity: number;
  price: number;
}

export interface CreateOrderRequest {
  customerName: string;
  customerPhone: string;
  customerAddress: string;
  locationId: string;
  deliveryFee: number;
  comment: string;
  items: OrderItemCreateDTO[];
}

export interface OrderItemUpdateDTO {
  id: string;
  orderItemStatus: string;
}

// Cart-related DTOs
export interface CartItem {
  id: string;
  productName: string;
  productPrice: number;
  quantity: number;
  image?: string;
}

// Cart state DTOs
export interface CartState {
  items: CartItem[];
}

export interface CartAction {
 type: 'ADD_ITEM' | 'REMOVE_ITEM' | 'UPDATE_QUANTITY' | 'CLEAR_CART';
  payload?: {
    id: string;
    productName?: string;
    productPrice?: number;
    quantity?: number;
    image?: string;
  };
}