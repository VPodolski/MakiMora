import React from 'react';
import type { CartItem } from '../../store/cartStore';

interface CartItemProps {
  item: CartItem;
  onIncrement: (id: string) => void;
  onDecrement: (id: string) => void;
  onRemove: (id: string) => void;
}

const CartItem: React.FC<CartItemProps> = ({ item, onIncrement, onDecrement, onRemove }) => {
  return (
    <div className="flex items-center justify-between py-4 border-b border-gray-200">
      <div className="flex items-center">
        <div className="w-16 h-16 rounded-md overflow-hidden bg-gray-200 mr-4">
          {item.image ? (
            <img 
              src={item.image} 
              alt={item.productName} 
              className="w-full h-full object-cover"
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-gray-500">
              Нет фото
            </div>
          )}
        </div>
        <div>
          <h3 className="font-medium text-gray-900">{item.productName}</h3>
          <p className="text-sm text-gray-500">{item.productPrice.toFixed(2)} ₽/шт</p>
        </div>
      </div>
      
      <div className="flex items-center">
        <div className="flex items-center border border-gray-300 rounded-md mr-4">
          <button 
            onClick={() => onDecrement(item.id)}
            className="px-3 py-1 text-gray-600 hover:bg-gray-100 rounded-l-md"
          >
            -
          </button>
          <span className="px-3 py-1 text-gray-900">{item.quantity}</span>
          <button 
            onClick={() => onIncrement(item.id)}
            className="px-3 py-1 text-gray-600 hover:bg-gray-100 rounded-r-md"
          >
            +
          </button>
        </div>
        
        <div className="mr-4 text-right">
          <p className="font-medium text-gray-900">{(item.productPrice * item.quantity).toFixed(2)} ₽</p>
        </div>
        
        <button 
          onClick={() => onRemove(item.id)}
          className="text-red-500 hover:text-red-700"
        >
          <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
          </svg>
        </button>
      </div>
    </div>
  );
};

export default CartItem;