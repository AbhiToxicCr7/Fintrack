import logo from './logo.svg';
import './App.css';
import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { LoginSignUp } from './Components/LoginSignUp/LoginSignUp';
import { Dashboard } from './Components/Dashboard/Dashboard';
import { UserDetails } from './Components/UserDetails/UserDetails';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<LoginSignUp />} />
        <Route path="/UserDetails" element={<UserDetails />} />
        <Route path="/Dashboard" element={<Dashboard />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
