import React from 'react';
import { useCart } from '../store/cartStore';
import type { CartItem } from '../store/cartStore';

interface CartProps {
  isOpen: boolean;
  onClose: () => void;
}

const Cart: React.FC<CartProps> = ({ isOpen, onClose }) => {
  const items = useCart(state => state.items);
  const updateQuantity = useCart(state => state.updateQuantity);
  const removeItem = useCart(state => state.removeItem);
  const clearCart = useCart(state => state.clearCart);
  const getTotalPrice = useCart(state => state.getTotalPrice);
  const getTotalItems = useCart(state => state.getTotalItems);

  const handleRemoveItem = (id: string) => {
    removeItem(id);
  };

  const handleIncrement = (id: string) => {
    const item = items.find(i => i.id === id);
    if (item) {
      updateQuantity(id, item.quantity + 1);
    }
  };

  const handleDecrement = (id: string) => {
    const item = items.find(i => i.id === id);
    if (item) {
      updateQuantity(id, item.quantity - 1);
    }
  };

  const handleCheckout = () => {
    // Navigate to checkout page
    window.location.href = '/checkout';
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 overflow-hidden">
      <div className="absolute inset-0 bg-black bg-opacity-50" onClick={onClose}></div>
      
      <div className="absolute inset-y-0 right-0 max-w-full flex">
        <div className="relative w-screen max-w-md">
          <div className="h-full flex flex-col bg-white shadow-xl">
            <div className="flex-1 overflow-y-auto py-6 px-4 sm:px-6">
              <div className="flex items-start justify-between">
                <h2 className="text-lg font-medium text-gray-900">Корзина</h2>
                <button
                  onClick={onClose}
                  className="ml-3 h-7 w-7 flex items-center justify-center rounded-md text-gray-400 hover:text-gray-500 hover:bg-gray-100"
                >
                  <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" className="h-6 w-6">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                  </svg>
                </button>
              </div>

              <div className="mt-8">
                <div className="flow-root">
                  {items.length === 0 ? (
                    <div className="text-center py-12">
                      <svg 
                        className="mx-auto h-12 w-12 text-gray-400" 
                        fill="none" 
                        viewBox="0 0 24 24" 
                        stroke="currentColor"
                      >
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                      </svg>
                      <h3 className="mt-2 text-sm font-medium text-gray-900">Корзина пуста</h3>
                      <p className="mt-1 text-sm text-gray-500">Добавьте блюда в корзину, чтобы сделать заказ</p>
                    </div>
                  ) : (
                    <ul className="-my-6 divide-y divide-gray-200">
                      {items.map((item: CartItem) => (
                        <li key={item.id} className="py-6 flex">
                          <div className="flex-shrink-0 w-24 h-24 rounded-md overflow-hidden">
                            {item.image ? (
                              <img
                                src={item.image}
                                alt={item.productName}
                                className="w-full h-full object-cover object-center"
                              />
                            ) : (
                              <div className="w-full h-full bg-gray-200 border-2 border-dashed rounded-md" />
                            )}
                          </div>

                          <div className="ml-4 flex-1 flex flex-col">
                            <div>
                              <div className="flex justify-between text-base font-medium text-gray-900">
                                <h3>{item.productName}</h3>
                                <p className="ml-4">{(item.productPrice * item.quantity).toFixed(2)} ₽</p>
                              </div>
                              <p className="mt-1 text-sm text-gray-500">{item.productPrice} ₽/шт</p>
                            </div>
                            <div className="flex-1 flex items-end justify-between text-sm">
                              <div className="flex items-center">
                                <button
                                  onClick={() => handleDecrement(item.id)}
                                  className="text-gray-500 hover:text-gray-600"
                                >
                                  <svg className="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                                    <path fillRule="evenodd" d="M5 10a1 1 0 011-1h8a1 1 0 110 2H6a1 1 0 01-1-1z" clipRule="evenodd" />
                                  </svg>
                                </button>
                                
                                <span className="mx-2 text-gray-700">{item.quantity}</span>
                                
                                <button
                                  onClick={() => handleIncrement(item.id)}
                                  className="text-gray-500 hover:text-gray-600"
                                >
                                  <svg className="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                                    <path fillRule="evenodd" d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z" clipRule="evenodd" />
                                  </svg>
                                </button>
                              </div>

                              <button
                                onClick={() => handleRemoveItem(item.id)}
                                type="button"
                                className="font-medium text-red-600 hover:text-red-500"
                              >
                                Удалить
                              </button>
                            </div>
                          </div>
                        </li>
                      ))}
                    </ul>
                  )}
                </div>
              </div>
            </div>

            {items.length > 0 && (
              <div className="border-t border-gray-200 py-6 px-4 sm:px-6">
                <div className="flex justify-between text-base font-medium text-gray-900">
                  <p>Итого</p>
                  <p>{getTotalPrice().toFixed(2)} ₽</p>
                </div>
                
                <div className="mt-6">
                  <button
                    onClick={handleCheckout}
                    className="w-full flex justify-center items-center px-6 py-3 border border-transparent rounded-md shadow-sm text-base font-medium text-white bg-orange-600 hover:bg-orange-700"
                  >
                    Оформить заказ
                  </button>
                </div>
                
                <div className="mt-6 flex justify-center text-sm text-gray-500">
                  <p>
                    Или{' '}
                    <button
                      type="button"
                      onClick={onClose}
                      className="text-orange-600 font-medium hover:text-orange-500"
                    >
                      продолжить покупки<span aria-hidden="true"> &rarr;</span>
                    </button>
                  </p>
                </div>
                
                <div className="mt-6">
                  <button
                    onClick={clearCart}
                    className="w-full flex justify-center items-center px-6 py-3 border border-transparent rounded-md shadow-sm text-base font-medium text-white bg-red-600 hover:bg-red-700"
                  >
                    Очистить корзину
                  </button>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Cart;