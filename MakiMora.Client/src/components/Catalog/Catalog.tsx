import React, { useState, useEffect } from 'react';
import type { ProductDTO, CategoryDTO } from '../../DTOs';
import ProductCard from '../Product/ProductCard';
import { useCart } from '../../contexts/CartContext';
import { useNavigate } from 'react-router-dom';

const Catalog: React.FC = () => {
  const { dispatch } = useCart();
  const navigate = useNavigate();
  const [categories, setCategories] = useState<CategoryDTO[]>([]);
  const [products, setProducts] = useState<ProductDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedCategory, setSelectedCategory] = useState<string | null>(null);

  // Моковые данные для демонстрации
  useEffect(() => {
    // В реальном приложении здесь будет вызов API
    const mockCategories: CategoryDTO[] = [
      {
        id: '1',
        name: 'Роллы',
        description: 'Традиционные и авторские роллы',
        isActive: true
      },
      {
        id: '2',
        name: 'Сеты',
        description: 'Комплекты суши и роллов',
        isActive: true
      },
      {
        id: '3',
        name: 'Суши',
        description: 'Классические суши из свежайших ингредиентов',
        isActive: true
      },
      {
        id: '4',
        name: 'Напитки',
        description: 'Саке, зеленый чай и другие напитки',
        isActive: true
      },
      {
        id: '5',
        name: 'Супы и салаты',
        description: 'Дополнения к основному меню',
        isActive: true
      }
    ];

    const mockProducts: ProductDTO[] = [
      {
        id: '1',
        name: 'Филадельфия угорь',
        description: 'Угорь, сыр сливочный, огурец, кунжут, соус унаги',
        price: 550,
        weight: 20,
        categoryId: '1',
        categoryName: 'Роллы',
        imageUrl: null,
        isAvailable: true
      },
      {
        id: '2',
        name: 'Калифорния с лососем',
        description: 'Лосось, авокадо, огурец, икра масаго, кунжут',
        price: 480,
        weight: 200,
        categoryId: '1',
        categoryName: 'Роллы',
        imageUrl: null,
        isAvailable: true
      },
      {
        id: '3',
        name: 'Дракон',
        description: 'Угри, авокадо, огурец, кунжут, соус унаги',
        price: 620,
        weight: 250,
        categoryId: '1',
        categoryName: 'Роллы',
        imageUrl: null,
        isAvailable: true
      },
      {
        id: '4',
        name: 'Сет "Маки-тян"',
        description: 'Ассорти из 8 видов суши и роллов',
        price: 1200,
        weight: 650,
        categoryId: '2',
        categoryName: 'Сеты',
        imageUrl: null,
        isAvailable: true
      },
      {
        id: '5',
        name: 'Сет "Сакура"',
        description: 'Классический сет для двоих',
        price: 1800,
        weight: 900,
        categoryId: '2',
        categoryName: 'Сеты',
        imageUrl: null,
        isAvailable: true
      },
      {
        id: '6',
        name: 'Филадельфия классика',
        description: 'Лосось, сыр сливочный, огурец, кунжут',
        price: 450,
        weight: 210,
        categoryId: '1',
        categoryName: 'Роллы',
        imageUrl: null,
        isAvailable: true
      }
    ];

    setCategories(mockCategories);
    setProducts(mockProducts);
    setLoading(false);
  }, []);

  const filteredProducts = selectedCategory
    ? products.filter(product => product.categoryId === selectedCategory)
    : products;

  const addToCart = (product: ProductDTO) => {
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

  const viewProductDetails = (productId: string) => {
    navigate(`/product/${productId}`);
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

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 mb-6">Меню</h1>
        
        <div className="flex overflow-x-auto pb-4 mb-6 scrollbar-hide">
          <div className="flex space-x-2">
            <button
              onClick={() => setSelectedCategory(null)}
              className={`px-4 py-2 rounded-full whitespace-nowrap ${
                selectedCategory === null
                  ? 'bg-orange-600 text-white'
                  : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
              }`}
            >
              Все
            </button>
            {categories.map(category => (
              <button
                key={category.id}
                onClick={() => setSelectedCategory(category.id)}
                className={`px-4 py-2 rounded-full whitespace-nowrap ${
                  selectedCategory === category.id
                    ? 'bg-orange-600 text-white'
                    : 'bg-gray-200 text-gray-700 hover:bg-gray-300'
                }`}
              >
                {category.name}
              </button>
            ))}
          </div>
        </div>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {filteredProducts.map(product => (
          <ProductCard
          key={product.id}
          product={product}
          onAddToCart={() => addToCart(product)}
          onViewDetails={() => viewProductDetails(product.id)}
        />
        ))}
      </div>

      {filteredProducts.length === 0 && (
        <div className="text-center py-12">
          <svg 
            className="mx-auto h-12 w-12 text-gray-400" 
            fill="none" 
            viewBox="0 0 24 24" 
            stroke="currentColor"
          >
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 0 0118 0z" />
          </svg>
          <h3 className="mt-2 text-lg font-medium text-gray-900">Товары не найдены</h3>
          <p className="mt-1 text-gray-500">В выбранной категории пока нет товаров</p>
        </div>
      )}
    </div>
  );
};

export default Catalog;