import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './UserDetails.css';
import { useNavigate } from 'react-router-dom'; 

export const UserDetails = () => {
    const navigate = useNavigate();
    const [profile, setProfile] = useState(null);
    const token = localStorage.getItem('authToken');
    const loggedUserID = localStorage.getItem('loggedinUserID');

    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [phoneNumber, setPhoneNumber] = useState("");
    const [address, setAddress] = useState("");
    const [userName, setUserName] = useState("");

    const handleFirstNameChange = (value) => {
        setFirstName(value);
    };

    const handleLastNameChange = (value) => {
        setLastName(value);
    };

    const handleEmailChange = (value) =>{
        setEmail(value);
    }

    const handlePhoneNumberChange = (value) => {
        setPhoneNumber(value);
    }

    const handleAddressChange = (value) => {
        setAddress(value);
    }

    const handleUserNameChange = (value) => {
        setUserName(value);
    }

    const [profession, setProfession] = useState("");
    const [jobTitle, setJobTitle] = useState("");
    const [annualSalary, setAnnualSalary] = useState("");

    const handleProfessionChange = (value) => {
        setProfession(value);
    }

    const handleJobTitleChange = (value) => {
        setJobTitle(value);
    }

    const handleAnnualSalaryChange = (value) => {
        setAnnualSalary(value);
    }


    const handleUserDetails = () => {
        const url = "https://localhost:44389/api/UserDetail/Add";

        const data = {
            UserId : loggedUserID,
            Profession : profession,
            JobTitle : jobTitle,
            AnnualSalary : annualSalary
        }

        axios.post(url, data,{
            headers: {
                Authorization: `Bearer ${token}`
            }
        });
        navigate('/Dashboard');
    }

    useEffect(()=>{
        if(!token) return;

        axios.get('https://localhost:44389/api/User/GetProfile',{
            headers: { 
                Authorization: `Bearer ${token}` 
            }
        })
        .then(res => {
            const data = res.data;
            setProfile(data);
            // Initialize editable first name from whichever casing the API returns
            setFirstName(data.Firstname || "");
            setLastName(data.Lastname || "");
            setEmail(data.Email || "");
            setPhoneNumber(data.PhoneNumber || "");
            setAddress(data.Address || "");
            setUserName(data.UserName || "");
        })
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
        <div className="userDetail-container">
            <h1>Hi, Welcome to Fintrack!!</h1>
            <h2>Personal Details</h2>
            <div className="personal-inputs">
                <div className="input1">
                    <input
                        type="text"
                        placeholder="First Name"
                        value={firstName}
                        onChange={(e)=> handleFirstNameChange(e.target.value)}
                    />
                    <input
                        type="text"
                        placeholder="Last Name"
                        value={lastName}
                        onChange={(e)=> handleLastNameChange(e.target.value)}
                    />
                    <input
                        type="text"
                        placeholder="Email"
                        value={email}
                        onChange={(e)=> handleEmailChange(e.target.value)}
                    />
                    <input
                        type="text"
                        placeholder="PhoneNumber"
                        value={phoneNumber}
                        onChange={(e)=> handlePhoneNumberChange(e.target.value)}
                    />
                </div>
                <div className="input2">
                    <input
                        type="text"
                        placeholder="Address"
                        value={address}
                        onChange={(e)=> handleAddressChange(e.target.value)}
                    />
                    <input
                        type="text"
                        placeholder="User Name"
                        value={userName}
                        onChange={(e)=> handleUserNameChange(e.target.value)}
                    />
                </div>
            </div>
            <h2>Financial Details</h2>
            <div className="financial-inputs">
                <div className="input1">
                    <input
                        type="text"
                        placeholder="Profession"
                        value={profession}
                        onChange={(e)=> handleProfessionChange(e.target.value)}
                    />
                    <input
                        type="text"
                        placeholder="Job Title"
                        value={jobTitle}
                        onChange={(e)=> handleJobTitleChange(e.target.value)}
                    />
                    <input
                        type="text"
                        placeholder="AnnualSalary"
                        value={annualSalary}
                        onChange={(e)=> handleAnnualSalaryChange(e.target.value)}
                    />
                </div>
            </div>
            <div className="button-container-userDetail">
                <button className="submit" onClick={()=> handleUserDetails()}>Save and Proceed</button>
            </div>
        </div>
    );
};