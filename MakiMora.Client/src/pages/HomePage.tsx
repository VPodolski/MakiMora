import React from 'react';
import Catalog from '../components/Catalog/Catalog';

const HomePage: React.FC = () => {
  return (
    <div className="min-h-screen bg-gray-50">
      <div className="bg-white">
        <div className="container mx-auto px-4 py-8">
          <div className="text-center mb-12">
            <h1 className="text-4xl font-bold text-gray-900 mb-4">Добро пожаловать в MakiMora</h1>
            <p className="text-xl text-gray-600 max-w-2xl mx-auto">
              Лучшие суши и роллы с доставкой по Москве. Свежие ингредиенты,
              изысканные вкусы и быстрая доставка прямо к вашему порогу.
            </p>
          </div>
        </div>
      </div>
      
      <div className="container mx-auto px-4 py-8">
        <Catalog />
      </div>
    </div>
  );
};

export default HomePage;