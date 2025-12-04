import React from 'react';
import type { ProductDTO } from '../DTOs';
import { useCart } from '../contexts/CartContext';

interface ProductDetailProps {
  product: ProductDTO;
  onBack: () => void;
}

const ProductDetail: React.FC<ProductDetailProps> = ({ product, onBack }) => {
  const { dispatch } = useCart();

  const handleAddToCart = () => {
    dispatch({
      type: 'ADD_ITEM',
      payload: {
        id: product.id,
        productName: product.name,
        productPrice: product.price,
        quantity: 1,
        image: product.imageUrl || undefined
      }
    });
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <button
        onClick={onBack}
        className="flex items-center text-orange-600 hover:text-orange-800 mb-6"
      >
        <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 mr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M10 19l-7-7m0 0l7-7m-7 7h18" />
        </svg>
        Назад к меню
      </button>

      <div className="bg-white rounded-lg shadow-lg overflow-hidden">
        <div className="md:flex">
          <div className="md:shrink-0 p-8 flex items-center justify-center bg-gray-100">
            {product.imageUrl ? (
              <img 
                src={product.imageUrl} 
                alt={product.name} 
                className="w-full max-w-md object-cover rounded-md"
              />
            ) : (
              <div className="w-full max-w-md h-64 bg-gray-200 border-2 border-dashed rounded-md flex items-center justify-center text-gray-500">
                Изображение отсутствует
              </div>
            )}
          </div>
          
          <div className="p-8 flex-1">
            <div className="flex justify-between items-start">
              <div>
                <h1 className="text-3xl font-bold text-gray-900 mb-2">{product.name}</h1>
                <p className="text-2xl font-bold text-orange-600 mb-4">{product.price} ₽</p>
              </div>
              
              <div className="bg-gray-200 text-gray-800 px-3 py-1 rounded-full text-sm font-medium">
                {product.weight} г
              </div>
            </div>
            
            <p className="text-gray-600 mb-6">{product.description}</p>
            
            <div className="mb-6">
              {!product.isAvailable ? (
                <span className="inline-block px-3 py-1 text-sm font-semibold text-red-80 bg-red-100 rounded-full">
                  Недоступно
                </span>
              ) : product.isOnStopList ? (
                <span className="inline-block px-3 py-1 text-sm font-semibold text-red-800 bg-red-100 rounded-full">
                  На стоп-листе
                </span>
              ) : (
                <span className="inline-block px-3 py-1 text-sm font-semibold text-green-800 bg-green-100 rounded-full">
                  В наличии
                </span>
              )}
            </div>
            
            <div className="flex space-x-4">
              <button
                onClick={handleAddToCart}
                disabled={!product.isAvailable || product.isOnStopList}
                className={`flex-1 py-3 px-6 rounded-md text-white font-medium ${
                  (!product.isAvailable || product.isOnStopList) 
                    ? 'bg-gray-400 cursor-not-allowed' 
                    : 'bg-orange-600 hover:bg-orange-700'
                }`}
              >
                Добавить в корзину
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductDetail;