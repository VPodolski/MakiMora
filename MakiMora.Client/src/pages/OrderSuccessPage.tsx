import React from 'react';

const OrderSuccessPage: React.FC = () => {
  return (
    <div className="container mx-auto px-4 py-8">
      <div className="max-w-3xl mx-auto text-center">
        <div className="inline-flex items-center justify-center w-16 h-16 rounded-full bg-green-100 mb-6">
          <svg className="w-8 h-8 text-green-600" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M5 13l4 4L19 7"></path>
          </svg>
        </div>
        
        <h1 className="text-3xl font-bold text-gray-900 mb-4">Заказ оформлен!</h1>
        <p className="text-lg text-gray-600 mb-6">
          Спасибо за ваш заказ. Номер вашего заказа: <span className="font-semibold">#ORD-2025-12345</span>
        </p>
        
        <div className="bg-white shadow rounded-lg p-6 mb-8 text-left">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">Детали заказа</h2>
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <h3 className="text-sm font-medium text-gray-500">Имя</h3>
              <p className="text-gray-900">Иван Иванов</p>
            </div>
            
            <div>
              <h3 className="text-sm font-medium text-gray-500">Телефон</h3>
              <p className="text-gray-900">+7 (999) 123-45-67</p>
            </div>
            
            <div>
              <h3 className="text-sm font-medium text-gray-500">Email</h3>
              <p className="text-gray-900">ivan@example.com</p>
            </div>
            
            <div>
              <h3 className="text-sm font-medium text-gray-500">Адрес доставки</h3>
              <p className="text-gray-900">ул. Примерная, д. 10, кв. 5</p>
            </div>
            
            <div className="md:col-span-2">
              <h3 className="text-sm font-medium text-gray-500">Время доставки</h3>
              <p className="text-gray-900">Как можно скорее</p>
            </div>
          </div>
        </div>
        
        <div className="mb-8">
          <h2 className="text-xl font-semibold text-gray-900 mb-4">Состав заказа</h2>
          
          <div className="bg-white shadow overflow-hidden sm:rounded-md">
            <ul className="divide-y divide-gray-200">
              <li>
                <div className="px-4 py-4 sm:px-6 flex justify-between items-center">
                  <div className="flex items-center">
                    <div className="flex-shrink-0 w-16 h-16 rounded-md overflow-hidden">
                      <div className="w-full h-full bg-gray-200 border-2 border-dashed rounded-md" />
                    </div>
                    <div className="ml-4">
                      <h3 className="text-lg font-medium text-gray-900">Филадельфия угорь</h3>
                      <p className="text-gray-500">1 × 550.00 ₽</p>
                    </div>
                  </div>
                  <p className="text-lg font-medium text-gray-900">550.00 ₽</p>
                </div>
              </li>
              <li>
                <div className="px-4 py-4 sm:px-6 flex justify-between items-center">
                  <div className="flex items-center">
                    <div className="flex-shrink-0 w-16 h-16 rounded-md overflow-hidden">
                      <div className="w-full h-full bg-gray-200 border-2 border-dashed rounded-md" />
                    </div>
                    <div className="ml-4">
                      <h3 className="text-lg font-medium text-gray-900">Калифорния с лососем</h3>
                      <p className="text-gray-500">2 × 480.00 ₽</p>
                    </div>
                  </div>
                  <p className="text-lg font-medium text-gray-900">960.00 ₽</p>
                </div>
              </li>
              <li>
                <div className="px-4 py-4 sm:px-6 flex justify-between items-center">
                  <div className="flex items-center">
                    <div className="flex-shrink-0 w-16 h-16 rounded-md overflow-hidden">
                      <div className="w-full h-full bg-gray-200 border-2 border-dashed rounded-md" />
                    </div>
                    <div className="ml-4">
                      <h3 className="text-lg font-medium text-gray-900">Суп мисо</h3>
                      <p className="text-gray-500">1 × 220.00 ₽</p>
                    </div>
                  </div>
                  <p className="text-lg font-medium text-gray-900">220.00 ₽</p>
                </div>
              </li>
            </ul>
          </div>
        </div>
        
        <div className="bg-white shadow rounded-lg p-6">
          <div className="flex justify-between items-center mb-2">
            <span className="text-gray-600">Товары</span>
            <span className="text-gray-900 font-medium">1 730.00 ₽</span>
          </div>
          <div className="flex justify-between items-center mb-2">
            <span className="text-gray-600">Доставка</span>
            <span className="text-gray-900 font-medium">200.00 ₽</span>
          </div>
          <div className="flex justify-between items-center pt-4 border-t border-gray-200">
            <span className="text-lg font-semibold text-gray-900">Итого</span>
            <span className="text-lg font-semibold text-gray-900">1 930.00 ₽</span>
          </div>
        </div>
        
        <div className="mt-8">
          <a 
            href="/" 
            className="inline-flex items-center px-6 py-3 border border-transparent text-base font-medium rounded-md shadow-sm text-white bg-orange-600 hover:bg-orange-700"
          >
            Продолжить покупки
          </a>
        </div>
      </div>
    </div>
  );
};

export default OrderSuccessPage;