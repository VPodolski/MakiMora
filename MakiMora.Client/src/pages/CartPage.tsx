import React from 'react';
import { useCart } from '../contexts/CartContext';

const CartPage: React.FC = () => {
  const { state, dispatch } = useCart();

  const handleIncrement = (id: string) => {
    const item = state.items.find(i => i.id === id);
    if (item) {
      dispatch({ type: 'UPDATE_QUANTITY', payload: { id, quantity: item.quantity + 1 } });
    }
  };

  const handleDecrement = (id: string) => {
    const item = state.items.find(i => i.id === id);
    if (item && item.quantity > 1) {
      dispatch({ type: 'UPDATE_QUANTITY', payload: { id, quantity: item.quantity - 1 } });
    } else if (item && item.quantity === 1) {
      dispatch({ type: 'REMOVE_ITEM', payload: { id } });
    }
  };

  const handleRemove = (id: string) => {
    dispatch({ type: 'REMOVE_ITEM', payload: { id } });
  };

  const handleClearCart = () => {
    dispatch({ type: 'CLEAR_CART' });
  };

  const subtotal = state.items.reduce((sum, item) => sum + (item.productPrice * item.quantity), 0);
  const deliveryFee = 200;
  const total = subtotal + deliveryFee;

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-2xl font-bold mb-6 text-gray-900">Корзина</h1>
      
      {state.items.length === 0 ? (
        <div className="text-center py-12">
          <svg 
            className="mx-auto h-12 w-12 text-gray-400" 
            fill="none" 
            viewBox="0 0 24 24" 
            stroke="currentColor"
          >
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
          </svg>
          <h3 className="mt-2 text-lg font-medium text-gray-900">Корзина пуста</h3>
          <p className="mt-1 text-gray-500">Добавьте блюда в корзину, чтобы сделать заказ</p>
          <div className="mt-6">
            <a 
              href="/" 
              className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-orange-600 hover:bg-orange-700"
            >
              Перейти к меню
            </a>
          </div>
        </div>
      ) : (
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          <div className="lg:col-span-2">
            <div className="bg-white shadow overflow-hidden sm:rounded-lg">
              <ul className="divide-y divide-gray-200">
                {state.items.map(item => (
                  <li key={item.id} className="py-6 px-4 sm:px-6">
                    <div className="flex items-center">
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

                      <div className="ml-4 flex-grow">
                        <div className="flex justify-between">
                          <div>
                            <h3 className="text-lg font-medium text-gray-900">{item.productName}</h3>
                            <p className="mt-1 text-sm text-gray-500">{item.productPrice.toFixed(2)} ₽/шт</p>
                          </div>
                          <p className="text-lg font-medium text-gray-900">{(item.productPrice * item.quantity).toFixed(2)} ₽</p>
                        </div>

                        <div className="mt-4 flex items-center justify-between">
                          <div className="flex items-center">
                            <button
                              onClick={() => handleDecrement(item.id)}
                              className="text-gray-500 hover:text-gray-600"
                            >
                              <svg className="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                                <path fillRule="evenodd" d="M5 10a1 1 0 011-1h8a1 1 0 110 2H6a1 1 0 01-1-1z" clipRule="evenodd" />
                              </svg>
                            </button>
                            
                            <span className="mx-3 text-gray-700">{item.quantity}</span>
                            
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
                            onClick={() => handleRemove(item.id)}
                            type="button"
                            className="text-red-600 hover:text-red-800"
                          >
                            <svg className="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
                              <path fillRule="evenodd" d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z" clipRule="evenodd" />
                            </svg>
                          </button>
                        </div>
                      </div>
                    </div>
                  </li>
                ))}
              </ul>
            </div>
            
            <div className="mt-6">
              <button
                onClick={handleClearCart}
                className="px-4 py-2 text-sm font-medium text-white bg-red-600 hover:bg-red-700 rounded-md"
              >
                Очистить корзину
              </button>
            </div>
          </div>
          
          <div>
            <div className="bg-white shadow sm:rounded-lg sticky top-4">
              <div className="px-4 py-5 sm:p-6">
                <h3 className="text-lg font-medium text-gray-900">Итого</h3>
                
                <div className="mt-4 space-y-4">
                  <div className="flex justify-between">
                    <p className="text-base font-medium text-gray-700">Товары</p>
                    <p className="text-base font-medium text-gray-900">{subtotal.toFixed(2)} ₽</p>
                  </div>
                  
                  <div className="flex justify-between">
                    <p className="text-base font-medium text-gray-700">Доставка</p>
                    <p className="text-base font-medium text-gray-900">{deliveryFee} ₽</p>
                  </div>
                  
                  <div className="flex justify-between border-t border-gray-200 pt-4">
                    <p className="text-lg font-medium text-gray-900">Итого</p>
                    <p className="text-lg font-medium text-gray-900">{total.toFixed(2)} ₽</p>
                  </div>
                </div>
                
                <div className="mt-6">
                  <a
                    href="/checkout"
                    className="flex justify-center items-center px-6 py-3 border border-transparent text-base font-medium rounded-md shadow-sm text-white bg-orange-600 hover:bg-orange-700"
                  >
                    Перейти к оформлению
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default CartPage;