import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './Dashboard.css';
import DashboardHeader from './Header/DashboardHeader.jsx';
import Home from './Home/Home.jsx';
import Sidebar from './Sidebar/Sidebar.jsx';
import Expenses from './Expenses/Expenses.jsx';
import Income from './Income/Income.jsx';
import Settings from '../Settings/Settings.jsx'

export const Dashboard = () => {
    const [profile, setProfile] = useState(null);
    const token = localStorage.getItem('authToken');
    const [activeView, setActiveView] = useState('home'); // 'home' | 'expenses'
    //frontend commit test

    useEffect(()=>{
        if(!token) return;

        axios.get('https://localhost:44389/api/User/GetProfile',{
            headers: { Authorization: `Bearer ${token}` }
        })
        .then(res => setProfile(res.data))
        .catch(() => setProfile({error : 'Failed to load profile'}))
    },[token])

    if(!token){
        return <div>Please login first</div>
    }

    if(!profile){
        return <div>Loading profile...</div>
    }

    if(profile.error){
        return <div>{profile.error}</div>
    }

    const renderActiveView = () => {
    switch (activeView) {
        case "expenses":
            return <Expenses />;

        case "income":
            return <Income />;

        case "dashboard":
            return <Home />;

        case "settings":
            return <Settings/>;

        case "reports":
            return <Home/>;

        default:
            return <Home />;
    }
};

    return (
        <div className="grid-container-dashboard">
           {activeView !== "expenses"}
            <Sidebar onNavigate={setActiveView} />
            {renderActiveView()}
        </div>
    );
};