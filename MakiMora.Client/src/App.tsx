import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { CartProvider } from './contexts/CartContext';
import Layout from './components/Layout/Layout';
import HomePage from './pages/HomePage';
import CartPage from './pages/CartPage';
import CheckoutPage from './pages/CheckoutPage';
import OrderSuccessPage from './pages/OrderSuccessPage';
import ProductDetailPage from './pages/ProductDetailPage';

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <CartProvider>
        <Router>
          <Layout>
            <Routes>
              <Route path="/" element={<HomePage />} />
              <Route path="/catalog" element={<HomePage />} />
              <Route path="/cart" element={<CartPage />} />
              <Route path="/checkout" element={<CheckoutPage />} />
              <Route path="/order-success" element={<OrderSuccessPage />} />
              <Route path="/product/:id" element={<ProductDetailPage />} />
              <Route path="/about" element={<div className="container mx-auto px-4 py-8"><h1 className="text-3xl font-bold mb-6">О нас</h1><p>Информация о компании MakiMora</p></div>} />
              <Route path="/contacts" element={<div className="container mx-auto px-4 py-8"><h1 className="text-3xl font-bold mb-6">Контакты</h1><p>Контактная информация для связи</p></div>} />
            </Routes>
          </Layout>
        </Router>
      </CartProvider>
    </QueryClientProvider>
  );
}

export default App;
