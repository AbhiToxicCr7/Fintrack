import logo from './logo.svg';
import './App.css';
import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { LoginSignUp } from './Components/LoginSignUp/LoginSignUp';
import { Dashboard } from './Components/Dashboard/Dashboard';
//import Registration from './Registration/Registration';
//import Login from './Login/Login';

function App() {
  return (
    // <div className="App">
    //   <LoginSignUp />
    // </div>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<LoginSignUp />} />
        <Route path="/dashboard" element={<Dashboard />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
