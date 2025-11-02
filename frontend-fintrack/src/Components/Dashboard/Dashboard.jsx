import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './Dashboard.css';

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
        <div className="dashboard" style={{ padding: 24 }}>
            <h2>Dashboard</h2>
            <p>Email: {profile.Email}</p>
            <p>First Name: {profile.Firstname}</p>
            <p>Last Name: {profile.Lastname}</p>
        </div>
    );
};