import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { ShoppingBagIcon } from '@heroicons/react/24/outline';
import CartModal from '../Cart/CartModal';
import { useCart } from '../../contexts/CartContext';

const Layout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [cartOpen, setCartOpen] = useState(false);
  const { state } = useCart();

  const toggleCart = () => {
    setCartOpen(!cartOpen);
  };

  const totalItems = state.items.reduce((count: number, item: any) => count + item.quantity, 0);

  return (
    <div className="flex flex-col min-h-screen bg-gray-50">
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
                <button 
                  onClick={toggleCart}
                  className="relative text-gray-700 hover:text-orange-600 transition-colors"
                >
                  <ShoppingBagIcon className="h-6 w-6" />
                  {totalItems > 0 && (
                    <span className="absolute -top-2 -right-2 bg-red-500 text-white rounded-full h-5 w-5 flex items-center justify-center text-xs">
                      {totalItems}
                    </span>
                  )}
                </button>
              </li>
            </ul>
          </nav>
        </div>
      </header>

      <main className="flex-grow">
        {children}
        <CartModal isOpen={cartOpen} onClose={toggleCart} />
      </main>

      <footer className="bg-gray-800 text-white py-6 mt-12">
        <div className="container mx-auto px-4 text-center">
          <p>&copy; {new Date().getFullYear()} MakiMora. Все права защищены.</p>
          <p className="mt-2 text-gray-400">Доставка свежих суши и роллов</p>
        </div>
      </footer>
    </div>
  );
};

export default Layout;