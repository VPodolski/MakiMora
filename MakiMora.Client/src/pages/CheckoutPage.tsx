import React, { useState } from 'react';
import { useCart } from '../contexts/CartContext';
import { OrderCreateDTO, OrderItemCreateDTO } from '../DTOs';

const CheckoutPage: React.FC = () => {
  const { state, dispatch } = useCart();
  const [formData, setFormData] = useState({
    customerName: '',
    customerPhone: '',
    customerEmail: '',
    deliveryAddress: '',
    deliveryTime: 'asap',
    specialInstructions: '',
  });
  const [isProcessing, setIsProcessing] = useState(false);

  const subtotal = state.items.reduce((sum, item) => sum + (item.productPrice * item.quantity), 0);
  const deliveryFee = 200;
  const total = subtotal + deliveryFee;

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsProcessing(true);

    try {
      // Подготовка данных для отправки на сервер
      const orderItems: OrderItemCreateDTO[] = state.items.map(item => ({
        productId: item.id,
        quantity: item.quantity,
        price: item.productPrice,
      }));

      const orderData: OrderCreateDTO = {
        customerName: formData.customerName,
        customerPhone: formData.customerPhone,
        customerEmail: formData.customerEmail,
        deliveryAddress: formData.deliveryAddress,
        deliveryTime: formData.deliveryTime === 'asap' ? new Date().toISOString() : formData.deliveryTime,
        specialInstructions: formData.specialInstructions,
        orderItems,
        totalAmount: total,
      };

      // Здесь будет вызов API для создания заказа
      console.log('Отправка заказа:', orderData);

      // Имитация успешной отправки
      await new Promise(resolve => setTimeout(resolve, 1000));

      // Очистка корзины после успешного заказа
      dispatch({ type: 'CLEAR_CART' });

      // Перенаправление на страницу подтверждения заказа
      alert('Заказ успешно оформлен!');
      window.location.href = '/order-confirmation';
    } catch (error) {
      console.error('Ошибка при оформлении заказа:', error);
      alert('Произошла ошибка при оформлении заказа. Пожалуйста, попробуйте еще раз.');
    } finally {
      setIsProcessing(false);
    }
  };

  if (state.items.length === 0) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center py-12">
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
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-2xl font-bold mb-6 text-gray-900">Оформление заказа</h1>
      
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div className="lg:col-span-2">
          <form onSubmit={handleSubmit} className="bg-white shadow sm:rounded-lg p-6">
            <div className="grid grid-cols-1 gap-y-6">
              <div>
                <h2 className="text-lg font-medium text-gray-900">Контактная информация</h2>
                <div className="mt-4 grid grid-cols-1 gap-y-6 sm:grid-cols-2 sm:gap-x-4">
                  <div>
                    <label htmlFor="customerName" className="block text-sm font-medium text-gray-700">
                      Имя <span className="text-red-500">*</span>
                    </label>
                    <input
                      type="text"
                      id="customerName"
                      name="customerName"
                      value={formData.customerName}
                      onChange={handleChange}
                      required
                      className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-orange-500 focus:border-orange-500"
                    />
                  </div>

                  <div>
                    <label htmlFor="customerPhone" className="block text-sm font-medium text-gray-700">
                      Телефон <span className="text-red-500">*</span>
                    </label>
                    <input
                      type="tel"
                      id="customerPhone"
                      name="customerPhone"
                      value={formData.customerPhone}
                      onChange={handleChange}
                      required
                      className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-orange-500 focus:border-orange-500"
                    />
                  </div>

                  <div>
                    <label htmlFor="customerEmail" className="block text-sm font-medium text-gray-700">
                      Email
                    </label>
                    <input
                      type="email"
                      id="customerEmail"
                      name="customerEmail"
                      value={formData.customerEmail}
                      onChange={handleChange}
                      className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-orange-500 focus:border-orange-500"
                    />
                  </div>
                </div>
              </div>

              <div>
                <h2 className="text-lg font-medium text-gray-900">Адрес доставки</h2>
                <div className="mt-4">
                  <label htmlFor="deliveryAddress" className="block text-sm font-medium text-gray-700">
                    Адрес <span className="text-red-500">*</span>
                  </label>
                  <textarea
                    id="deliveryAddress"
                    name="deliveryAddress"
                    rows={3}
                    value={formData.deliveryAddress}
                    onChange={handleChange}
                    required
                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-orange-500 focus:border-orange-500"
                  />
                </div>
              </div>

              <div>
                <h2 className="text-lg font-medium text-gray-900">Время доставки</h2>
                <div className="mt-4">
                  <div className="flex items-center">
                    <input
                      id="deliveryTime-asap"
                      name="deliveryTime"
                      type="radio"
                      value="asap"
                      checked={formData.deliveryTime === 'asap'}
                      onChange={handleChange}
                      className="focus:ring-orange-500 h-4 w-4 text-orange-600 border-gray-300"
                    />
                    <label htmlFor="deliveryTime-asap" className="ml-3 block text-sm font-medium text-gray-700">
                      Как можно скорее
                    </label>
                  </div>
                  <div className="mt-4 flex items-center">
                    <input
                      id="deliveryTime-scheduled"
                      name="deliveryTime"
                      type="radio"
                      value="scheduled"
                      checked={formData.deliveryTime !== 'asap'}
                      onChange={handleChange}
                      className="focus:ring-orange-500 h-4 w-4 text-orange-600 border-gray-300"
                    />
                    <label htmlFor="deliveryTime-scheduled" className="ml-3 block text-sm font-medium text-gray-700">
                      Запланировать доставку
                    </label>
                    {formData.deliveryTime !== 'asap' && (
                      <input
                        type="datetime-local"
                        name="deliveryTime"
                        value={formData.deliveryTime !== 'asap' ? formData.deliveryTime : ''}
                        onChange={handleChange}
                        className="ml-4 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-orange-500 focus:border-orange-500"
                      />
                    )}
                  </div>
                </div>
              </div>

              <div>
                <h2 className="text-lg font-medium text-gray-900">Дополнительные инструкции</h2>
                <div className="mt-4">
                  <label htmlFor="specialInstructions" className="block text-sm font-medium text-gray-700">
                    Особые пожелания
                  </label>
                  <textarea
                    id="specialInstructions"
                    name="specialInstructions"
                    rows={3}
                    value={formData.specialInstructions}
                    onChange={handleChange}
                    placeholder="Например: не звонить, оставить у двери"
                    className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-orange-500 focus:border-orange-500"
                  />
                </div>
              </div>
            </div>

            <div className="mt-8 flex justify-end">
              <button
                type="submit"
                disabled={isProcessing}
                className="ml-3 inline-flex justify-center py-2 px-4 border border-transparent shadow-sm text-sm font-medium rounded-md text-white bg-orange-600 hover:bg-orange-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-orange-500 disabled:opacity-50"
              >
                {isProcessing ? 'Обработка...' : `Оформить заказ на ${total.toFixed(2)} ₽`}
              </button>
            </div>
          </form>
        </div>

        <div>
          <div className="bg-white shadow sm:rounded-lg sticky top-4">
            <div className="px-4 py-5 sm:p-6">
              <h3 className="text-lg font-medium text-gray-900">Ваш заказ</h3>
              
              <div className="mt-4 space-y-4">
                <div className="flow-root">
                  <ul className="-my-4 divide-y divide-gray-200">
                    {state.items.map(item => (
                      <li key={item.id} className="flex py-4">
                        <div className="flex-shrink-0 w-16 h-16 rounded-md overflow-hidden">
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
                            <p className="mt-1 text-sm text-gray-500">{item.quantity} × {item.productPrice.toFixed(2)} ₽</p>
                          </div>
                        </div>
                      </li>
                    ))}
                  </ul>
                </div>

                <div className="border-t border-gray-200 pt-4 space-y-3">
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
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CheckoutPage;