import React, { useEffect, useState } from 'react';
import { apiClient } from '../services/apiClient';
import type { Product, Category } from '../types';
import { useCart } from '../store/cartStore';

const Catalog: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [products, setProducts] = useState<Record<string, Product[]>>({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const addToCart = useCart(state => state.addItem);

  useEffect(() => {
    const fetchCatalogData = async () => {
      try {
        setLoading(true);
        
        // Fetch all categories
        const categoriesResponse = await apiClient.get<Category[]>('/categories');
        setCategories(categoriesResponse.data);

        // Fetch all products
        const productsResponse = await apiClient.get<Product[]>('/products');
        const productsByCategory: Record<string, Product[]> = {};

        // Group products by category
        productsResponse.data.forEach(product => {
          if (!productsByCategory[product.categoryId]) {
            productsByCategory[product.categoryId] = [];
          }
          productsByCategory[product.categoryId].push(product);
        });

        setProducts(productsByCategory);
      } catch (err: any) {
        setError(err.response?.data?.message || 'Ошибка загрузки каталога');
        console.error('Error fetching catalog:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchCatalogData();
  }, []);

  const handleAddToCart = (product: Product) => {
    addToCart(product, 1);
  };

  if (loading) {
    return (
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="flex justify-center items-center h-96">
          <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-orange-500"></div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative" role="alert">
          <strong className="font-bold">Ошибка! </strong>
          <span className="block sm:inline">{error}</span>
        </div>
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <h1 className="text-3xl font-bold text-center mb-8 text-gray-900">Наши суши и роллы</h1>
      
      {categories.map(category => (
        <div key={category.id} className="mb-12">
          <h2 className="text-2xl font-bold mb-6 text-orange-600">{category.name}</h2>
          
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
            {products[category.id]?.map(product => (
              <div key={product.id} className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow duration-300 flex flex-col h-full">
                {product.imageUrl ? (
                  <img 
                    src={product.imageUrl} 
                    alt={product.name} 
                    className="w-full h-48 object-cover"
                  />
                ) : (
                  <div className="w-full h-48 bg-gray-200 flex items-center justify-center">
                    <span className="text-gray-500">Изображение отсутствует</span>
                  </div>
                )}
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
                      onClick={() => handleAddToCart(product)}
                      disabled={!product.isAvailable || product.isOnStopList}
                    >
                      В корзину
                    </button>
                  </div>
                  {(!product.isAvailable || product.isOnStopList) ? (
                    <span className="inline-block px-2 py-1 text-xs font-semibold text-red-800 bg-red-100 rounded-full mt-2">
                      {product.isOnStopList ? "На стоп-листе" : "Недоступно"}
                    </span>
                  ) : (
                    <span className="inline-block px-2 py-1 text-xs font-semibold text-green-800 bg-green-100 rounded-full mt-2">
                      В наличии
                    </span>
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
      ))}
    </div>
  );
};

export default Catalog;