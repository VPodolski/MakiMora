import React, { useState } from 'react';
import { useCart } from '../store/cartStore';
import { apiClient } from '../services/apiClient';
import type { CreateOrderRequest } from '../types';

const Checkout: React.FC = () => {
  const [formData, setFormData] = useState({
    customerName: '',
    customerPhone: '',
    customerAddress: '',
    comment: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);
  
  const cartItems = useCart(state => state.items);
  const getTotalPrice = useCart(state => state.getTotalPrice);
  const clearCart = useCart(state => state.clearCart);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      // Calculate delivery fee based on location or other factors
      const deliveryFee = 200; // This could be calculated based on distance or other factors
      
      // Prepare order data
      const orderData: CreateOrderRequest = {
        customerName: formData.customerName,
        customerPhone: formData.customerPhone,
        customerAddress: formData.customerAddress,
        locationId: '11111111-1111-1111-1111-11111111', // Default location, should come from context or selection
        deliveryFee: deliveryFee,
        comment: formData.comment,
        items: cartItems.map(item => ({
          productId: item.productId,
          quantity: item.quantity
        }))
      };

      // Send order to backend
      await apiClient.post('/orders', orderData);

      // Clear cart after successful order
      clearCart();
      setSuccess(true);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка при оформлении заказа');
      console.error('Error creating order:', err);
    } finally {
      setLoading(false);
    }
  };

  if (success) {
    return (
      <div className="max-w-2xl mx-auto p-6 bg-white rounded-lg shadow-md">
        <div className="text-center py-12">
          <div className="mx-auto flex items-center justify-center h-12 w-12 rounded-full bg-green-100">
            <svg 
              className="h-6 w-6 text-green-600" 
              fill="none" 
              viewBox="0 0 24 24" 
              stroke="currentColor"
            >
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
            </svg>
          </div>
          <h3 className="mt-5 text-lg font-medium text-gray-900">Заказ оформлен!</h3>
          <p className="mt-2 text-gray-500">
            Ваш заказ успешно создан. Номер заказа будет отправлен на ваш телефон.
          </p>
          <div className="mt-6">
            <a 
              href="/" 
              className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-orange-600 hover:bg-orange-700 focus:outline-none"
            >
              Продолжить покупки
            </a>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto p-6 bg-white rounded-lg shadow-md">
      <h1 className="text-2xl font-bold mb-6 text-gray-900">Оформление заказа</h1>
      
      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4" role="alert">
          <strong className="font-bold">Ошибка! </strong>
          <span className="block sm:inline">{error}</span>
        </div>
      )}
      
      <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
        <div>
          <h2 className="text-lg font-medium mb-4">Информация о доставке</h2>
          <form onSubmit={handleSubmit}>
            <div className="mb-4">
              <label htmlFor="customerName" className="block text-sm font-medium text-gray-700 mb-1">
                Имя *
              </label>
              <input
                type="text"
                id="customerName"
                name="customerName"
                value={formData.customerName}
                onChange={handleChange}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-orange-500 focus:border-orange-500"
              />
            </div>
            
            <div className="mb-4">
              <label htmlFor="customerPhone" className="block text-sm font-medium text-gray-700 mb-1">
                Телефон *
              </label>
              <input
                type="tel"
                id="customerPhone"
                name="customerPhone"
                value={formData.customerPhone}
                onChange={handleChange}
                required
                className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-orange-500 focus:border-orange-500"
              />
            </div>
            
            <div className="mb-4">
              <label htmlFor="customerAddress" className="block text-sm font-medium text-gray-700 mb-1">
                Адрес доставки *
              </label>
              <textarea
                id="customerAddress"
                name="customerAddress"
                value={formData.customerAddress}
                onChange={handleChange}
                required
                rows={3}
                className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-orange-500 focus:border-orange-500"
              ></textarea>
            </div>
            
            <div className="mb-6">
              <label htmlFor="comment" className="block text-sm font-medium text-gray-700 mb-1">
                Комментарий к заказу
              </label>
              <textarea
                id="comment"
                name="comment"
                value={formData.comment}
                onChange={handleChange}
                rows={3}
                placeholder="Особые пожелания, этаж, домофон и т.д."
                className="w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-orange-500 focus:border-orange-500"
              ></textarea>
            </div>
            
            <button
              type="submit"
              disabled={loading || cartItems.length === 0}
              className="w-full flex justify-center py-3 px-4 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-orange-600 hover:bg-orange-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-orange-500 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {loading ? (
                <span className="flex items-center">
                  <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                  </svg>
                  Обработка...
                </span>
              ) : (
                `Оформить заказ на ${(getTotalPrice() + 200).toFixed(2)} ₽`
              )}
            </button>
          </form>
        </div>
        
        <div>
          <h2 className="text-lg font-medium mb-4">Ваш заказ</h2>
          <div className="border border-gray-200 rounded-md p-4">
            <ul className="divide-y divide-gray-200">
              {cartItems.map(item => (
                <li key={item.id} className="py-3 flex justify-between items-center">
                  <div>
                    <h3 className="text-sm font-medium text-gray-900">{item.productName}</h3>
                    <p className="text-sm text-gray-500">{item.quantity} × {item.productPrice.toFixed(2)} ₽</p>
                  </div>
                  <p className="text-sm font-medium text-gray-900">{(item.productPrice * item.quantity).toFixed(2)} ₽</p>
                </li>
              ))}
            </ul>
            
            <div className="mt-6 border-t border-gray-200 pt-4">
              <div className="flex justify-between text-base font-medium text-gray-900 mb-1">
                <p>Товары</p>
                <p>{getTotalPrice().toFixed(2)} ₽</p>
              </div>
              
              <div className="flex justify-between text-base font-medium text-gray-900 mb-1">
                <p>Доставка</p>
                <p>200.00 ₽</p>
              </div>
              
              <div className="flex justify-between text-lg font-bold text-gray-900 mt-4 pt-4 border-t border-gray-200">
                <p>Итого</p>
                <p>{(getTotalPrice() + 200).toFixed(2)} ₽</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Checkout;