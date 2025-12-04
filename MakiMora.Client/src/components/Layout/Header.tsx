import React from 'react';
import { Link } from 'react-router-dom';
import { ShoppingBagIcon } from '@heroicons/react/24/outline';
import { useCart } from '../../hooks/useCart';

const Header: React.FC = () => {
  const { state } = useCart();
  
  const totalItems = state.items.reduce((count, item) => count + item.quantity, 0);

  return (
    <header className="bg-white shadow-md sticky top-0 z-10">
      <div className="container mx-auto px-4 py-4 flex justify-between items-center">
        <Link to="/" className="text-2xl font-bold text-orange-600">
          MakiMora
        </Link>
        <nav>
          <ul className="flex space-x-6">
            <li>
              <Link to="/" className="text-gray-700 hover:text-orange-600 transition-colors">
                Меню
              </Link>
            </li>
            <li>
              <Link to="/about" className="text-gray-700 hover:text-orange-600 transition-colors">
                О нас
              </Link>
            </li>
            <li>
              <Link to="/contacts" className="text-gray-700 hover:text-orange-600 transition-colors">
                Контакты
              </Link>
            </li>
            <li>
              <Link 
                to="/cart" 
                className="relative text-gray-700 hover:text-orange-600 transition-colors flex items-center"
              >
                <ShoppingBagIcon className="h-6 w-6" />
                {totalItems > 0 && (
                  <span className="absolute -top-2 -right-2 bg-red-500 text-white rounded-full h-5 w-5 flex items-center justify-center text-xs">
                    {totalItems}
                  </span>
                )}
              </Link>
            </li>
          </ul>
        </nav>
      </div>
    </header>
  );
};

export default Header;