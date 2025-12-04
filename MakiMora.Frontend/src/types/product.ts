// No need to import Category and Location since they are not used in this file

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

export interface UpdateProductAvailabilityRequest {
  isAvailable: boolean;
}

export interface UpdateProductStopListStatusRequest {
  isOnStopList: boolean;
}