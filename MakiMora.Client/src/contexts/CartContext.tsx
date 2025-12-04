import React, { createContext, useContext, useReducer } from 'react';
import type { ReactNode } from 'react';
import type { CartItem, CartState, CartAction } from '../DTOs';

// Используем типы из DTOs

export const CartContext = createContext<{
  state: CartState;
  dispatch: React.Dispatch<CartAction>;
} | undefined>(undefined);

const cartReducer = (state: CartState, action: CartAction): CartState => {
  switch (action.type) {
    case 'ADD_ITEM':
      const existingItem = state.items.find(item => item.id === action.payload?.id);
      if (existingItem) {
        return {
          items: state.items.map(item =>
            item.id === action.payload?.id
              ? { ...item, quantity: item.quantity + 1 }
              : item
          ),
        };
      } else {
        const newItem: CartItem = {
          id: action.payload?.id || '',
          productName: action.payload?.productName || '',
          productPrice: action.payload?.productPrice || 0,
          quantity: action.payload?.quantity || 1,
          image: action.payload?.image,
        };
        return {
          items: [...state.items, newItem],
        };
      }
    case 'REMOVE_ITEM':
      return {
        items: state.items.filter(item => item.id !== action.payload?.id),
      };
    case 'UPDATE_QUANTITY':
      if (action.payload?.quantity && action.payload.quantity <= 0) {
        return {
          items: state.items.filter(item => item.id !== action.payload?.id),
        };
      }
      return {
        items: state.items.map(item =>
          item.id === action.payload?.id
            ? { ...item, quantity: action.payload?.quantity || item.quantity }
            : item
        ),
      };
    case 'CLEAR_CART':
      return { items: [] };
    default:
      return state;
  }
};

const loadCartFromLocalStorage = (): CartState => {
  try {
    const serializedCart = localStorage.getItem('cart');
    if (serializedCart === null) {
      return { items: [] };
    }
    return JSON.parse(serializedCart);
  } catch (err) {
    console.error('Could not load cart from localStorage', err);
    return { items: [] };
  }
};

const saveCartToLocalStorage = (cartState: CartState) => {
  try {
    const serializedCart = JSON.stringify(cartState);
    localStorage.setItem('cart', serializedCart);
  } catch (err) {
    console.error('Could not save cart to localStorage', err);
  }
};

export const CartProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [state, dispatch] = useReducer(cartReducer, loadCartFromLocalStorage());

  // Save to localStorage whenever state changes
  React.useEffect(() => {
    saveCartToLocalStorage(state);
  }, [state]);

  return (
    <CartContext.Provider value={{ state, dispatch }}>
      {children}
    </CartContext.Provider>
  );
};

export const useCart = () => {
  const context = useContext(CartContext);
  if (context === undefined) {
    throw new Error('useCart must be used within a CartProvider');
  }
  return context;
};