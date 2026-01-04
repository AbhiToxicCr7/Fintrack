import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './Dashboard.css';
import DashboardHeader from './Header/DashboardHeader.jsx';
import Home from './Home/Home.jsx';
import Sidebar from './Sidebar/Sidebar.jsx';
import Expenses from './Expenses/Expenses.jsx';

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

    return (
        <div className="grid-container-dashboard">
            {activeView !== 'expenses' ? <DashboardHeader/> : null}
            {/* <DashboardHeader></DashboardHeader> */}
            <Sidebar onNavigate={setActiveView}></Sidebar>
            {activeView === 'expenses' ? <Expenses /> : <Home />}
        </div>
    );
};