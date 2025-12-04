import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import type { ProductDTO } from '../DTOs';
import ProductDetail from '../components/ProductDetail';

const ProductDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [product, setProduct] = useState<ProductDTO | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // В реальном приложении здесь будет вызов API для получения продукта по ID
  useEffect(() => {
    // Моковые данные для демонстрации
    const mockProduct: ProductDTO = {
      id: id || '',
      name: 'Филадельфия угорь',
      description: 'Угорь, сыр сливочный, огурец, кунжут, соус унаги',
      price: 550,
      weight: 200,
      categoryId: '1',
      categoryName: 'Роллы',
      imageUrl: null,
      isAvailable: true,
      isOnStopList: false
    };

    // Имитация задержки загрузки
    setTimeout(() => {
      setProduct(mockProduct);
      setLoading(false);
    }, 500);
  }, [id]);

 const handleBack = () => {
    navigate('/catalog');
  };

  if (loading) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="flex justify-center items-center h-64">
          <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-orange-600"></div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative" role="alert">
          <strong className="font-bold">Ошибка! </strong>
          <span className="block sm:inline">{error}</span>
        </div>
      </div>
    );
  }

  if (!product) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="text-center py-12">
          <svg 
            className="mx-auto h-12 w-12 text-gray-400" 
            fill="none" 
            viewBox="0 0 24 24" 
            stroke="currentColor"
          >
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 0 0118 0z" />
          </svg>
          <h3 className="mt-2 text-lg font-medium text-gray-900">Товар не найден</h3>
          <p className="mt-1 text-gray-50">Запрашиваемый товар не существует или был удален</p>
          <div className="mt-6">
            <button
              onClick={() => navigate('/catalog')}
              className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md shadow-sm text-white bg-orange-600 hover:bg-orange-700"
            >
              Перейти к меню
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <ProductDetail product={product} onBack={handleBack} />
 );
};

export default ProductDetailPage;