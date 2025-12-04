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

export interface CreateProductRequestDto {
  name: string;
  description?: string;
  price: number;
  categoryId: string;
  locationId: string;
  imageUrl?: string;
  preparationTime?: number;
  isAvailable: boolean;
}

export interface UpdateProductRequestDto {
  name: string;
  description?: string;
  price: number;
  categoryId: string;
  imageUrl?: string;
  preparationTime?: number;
  isAvailable: boolean;
}

export interface UpdateProductAvailabilityRequestDto {
  isAvailable: boolean;
}

export interface UpdateProductStopListStatusRequestDto {
  isOnStopList: boolean;
}