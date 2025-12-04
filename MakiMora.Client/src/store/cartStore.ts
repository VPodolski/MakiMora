import { create } from 'zustand';
import type { Product } from '../types';

export interface CartItem {
  id: string;
  productId: string;
  productName: string;
  productPrice: number;
  quantity: number;
  image?: string;
}

interface CartState {
  items: CartItem[];
  addItem: (product: Product, quantity: number) => void;
  updateQuantity: (id: string, quantity: number) => void;
  removeItem: (id: string) => void;
  clearCart: () => void;
  getTotalItems: () => number;
  getTotalPrice: () => number;
}

export const useCartStore = create<CartState>((set, get) => ({
  items: [],
  
  addItem: (product, quantity = 1) => {
    const existingItem = get().items.find(item => item.productId === product.id);
    
    if (existingItem) {
      set(state => ({
        items: state.items.map(item =>
          item.productId === product.id
            ? { ...item, quantity: item.quantity + quantity }
            : item
        ),
      }));
    } else {
      const newItem: CartItem = {
        id: `${product.id}-${Date.now()}`,
        productId: product.id,
        productName: product.name,
        productPrice: product.price,
        quantity,
        image: product.imageUrl
      };
      
      set(state => ({
        items: [...state.items, newItem]
      }));
    }
  },
  
  updateQuantity: (id, quantity) => {
    if (quantity <= 0) {
      get().removeItem(id);
      return;
    }
    
    set(state => ({
      items: state.items.map(item =>
        item.id === id ? { ...item, quantity } : item
      ),
    }));
  },
  
  removeItem: (id) => {
    set(state => ({
      items: state.items.filter(item => item.id !== id),
    }));
  },
  
  clearCart: () => {
    set({ items: [] });
  },
  
  getTotalItems: () => {
    return get().items.reduce((sum, item) => sum + item.quantity, 0);
  },
  
  getTotalPrice: () => {
    return get().items.reduce((sum, item) => sum + (item.productPrice * item.quantity), 0);
  },
}));

// Save cart to localStorage whenever it changes
useCartStore.subscribe((state) => {
  localStorage.setItem('cart', JSON.stringify(state.items));
});

// Load cart from localStorage on initialization
const savedCart = localStorage.getItem('cart');
if (savedCart) {
  try {
    const parsedCart = JSON.parse(savedCart);
    // We can't directly update the store state from here, so we'll need to handle this differently
    // This would typically be done in a provider component
  } catch (e) {
    console.error('Failed to parse cart from localStorage', e);
  }
}