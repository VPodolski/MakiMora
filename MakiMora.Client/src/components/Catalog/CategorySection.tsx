import React from 'react';
import type { ProductDTO, CategoryDTO } from '../../DTOs';
import ProductCard from './ProductCard';

interface CategorySectionProps {
  category: CategoryDTO;
 products: ProductDTO[];
}

const CategorySection: React.FC<CategorySectionProps> = ({ category, products }) => {
  return (
    <section className="mb-12">
      <h2 className="text-2xl font-bold mb-6 text-orange-600">{category.name}</h2>
      
      {products.length === 0 ? (
        <div className="text-center py-8 text-gray-500">
          В данной категории пока нет товаров
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
          {products.map(product => (
            <ProductCard key={product.id} product={product} />
          ))}
        </div>
      )}
    </section>
  );
};

export default CategorySection;