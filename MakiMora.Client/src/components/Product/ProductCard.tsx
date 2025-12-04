import React from 'react';
import type { ProductDTO } from '../../DTOs';

interface ProductCardProps {
  product: ProductDTO;
 onAddToCart: () => void;
  onViewDetails?: () => void;
}

const ProductCard: React.FC<ProductCardProps> = ({ product, onAddToCart, onViewDetails }) => {
  return (
    <div className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow duration-300">
      <div className="p-4 flex">
        <div className="bg-gray-200 border-2 border-dashed rounded-xl w-32 h-32 flex items-center justify-center">
          {product.imageUrl ? (
            <img 
              src={product.imageUrl} 
              alt={product.name} 
              className="w-full h-full object-cover rounded-md"
            />
          ) : (
            <span className="text-gray-500 text-xs text-center px-2">Изображение отсутствует</span>
          )}
        </div>
        
        <div className="ml-4 flex-1">
          <h3 className="text-lg font-semibold text-gray-900 line-clamp-1">{product.name}</h3>
          <p className="mt-1 text-sm text-gray-500 line-clamp-2">{product.description}</p>
          
          <div className="mt-4 flex items-center justify-between">
            <span className="text-lg font-bold text-gray-900">{product.price} ₽</span>
            <span className="text-sm text-gray-500">{product.weight} г</span>
          </div>
          
          <div className="mt-4 flex space-x-2">
            <button
              onClick={onViewDetails}
              className="flex-1 bg-white border border-orange-600 text-orange-600 hover:bg-orange-50 py-2 px-4 rounded-md transition duration-300"
            >
              Подробнее
            </button>
            <button
              onClick={onAddToCart}
              className="flex-1 bg-orange-600 hover:bg-orange-700 text-white py-2 px-4 rounded-md transition duration-300 flex items-center justify-center"
            >
              <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 mr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
              </svg>
              В корзину
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductCard;