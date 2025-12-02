import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './Dashboard.css';
import Header from './Header/Header';
import Home from './Home/Home.jsx';
import Sidebar from './Sidebar/Sidebar.jsx';

export const Dashboard = () => {
    const [profile, setProfile] = useState(null);
    const token = localStorage.getItem('authToken');
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
            <Header></Header>
            <Sidebar></Sidebar>
            <Home></Home>
        </div>
    );
};