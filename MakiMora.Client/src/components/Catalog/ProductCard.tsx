import React from 'react';
import { useCart } from '../../contexts/CartContext';
import type { ProductDTO } from '../../DTOs';

interface ProductCardProps {
  product: ProductDTO;
}

const ProductCard: React.FC<ProductCardProps> = ({ product }) => {
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
    <div className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow duration-300 flex flex-col h-full">
      <div className="h-48 bg-gray-200 flex items-center justify-center">
        {product.imageUrl ? (
          <img 
            src={product.imageUrl} 
            alt={product.name} 
            className="w-full h-full object-cover"
          />
        ) : (
          <div className="w-full h-full flex items-center justify-center text-gray-500">
            Изображение отсутствует
          </div>
        )}
      </div>
      <div className="p-4 flex-grow flex flex-col">
        <h3 className="text-lg font-semibold mb-2 text-gray-900">{product.name}</h3>
        <p className="text-gray-600 mb-4 flex-grow">{product.description}</p>
        <div className="flex justify-between items-center mt-auto">
          <span className="text-lg font-bold text-orange-600">{product.price} ₽</span>
          <button 
            className={`px-4 py-2 rounded text-white font-medium ${
              (!product.isAvailable || product.isOnStopList) 
                ? 'bg-gray-400 cursor-not-allowed' 
                : 'bg-orange-500 hover:bg-orange-600'
            }`}
            onClick={handleAddToCart}
            disabled={!product.isAvailable || product.isOnStopList}
          >
            В корзину
          </button>
        </div>
        <div className="mt-2">
          {!product.isAvailable ? (
            <span className="inline-block px-2 py-1 text-xs font-semibold text-red-800 bg-red-100 rounded-full">
              Недоступно
            </span>
          ) : product.isOnStopList ? (
            <span className="inline-block px-2 py-1 text-xs font-semibold text-red-800 bg-red-100 rounded-full">
              На стоп-листе
            </span>
          ) : (
            <span className="inline-block px-2 py-1 text-xs font-semibold text-green-800 bg-green-100 rounded-full">
              В наличии
            </span>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProductCard;