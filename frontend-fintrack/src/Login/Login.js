import React, { Fragment, useState } from 'react';
import axios from 'axios';

function Login(){

    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [profile, setProfile] = useState(null);

    const handleUserNameChange = (value) => {
        setUserName(value);
    }

    const handlePassword = (value)=> {
        setPassword(value);
    }

    const handleLogin = async ()=>{
        const url = "https://localhost:44389/api/Auth/Login";

        const data = {
            UserName : userName,
            Password : password,
            ClientId : "Client1"
        }

        // axios.post(url, data)
        // .then((response) => {
        //     alert(response);
        // })
        // .catch((error) => {
        //     alert(error);
        // });    
        
        try {
            const response = await axios.post(url, data);
            // Assuming token is in response.data.token or response.data (adjust as per your API)
            const token = response.data.token || response.data;
            // Call GetProfile API with JWT token
            const profileResponse = await axios.get(
                "https://localhost:44389/api/User/GetProfile",
                {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                }
            );
            setProfile(profileResponse.data);
        } catch (error) {
            alert("Login failed or unable to fetch profile.");
        }
    }

    // Show Dashboard if profile is loaded
    if (profile) {
        return (
            <Fragment>
                <h2>Dashboard</h2>
                <div>Email: {profile.Email}</div>
                <div>First Name: {profile.Firstname}</div>
                <div>Last Name: {profile.Lastname}</div>
                <div>Role: {profile.Role}</div>
            </Fragment>
        );
    }

    return (
        <Fragment>
            <div>Login</div>
            {/* <label>Name</label>
            <input type="text" placeholder="Enter Name" onChange={(e)=>{handleNameChange(e.target.value)}}></input>
            <br></br> */}

            <label>User Name</label>
            <input type="text" placeholder="User Name" onChange={(e)=>{handleUserNameChange(e.target.value)}}></input>
            <br></br>

            <label>Password</label>
            <input type="text" placeholder="Enter Password" onChange={(e)=>{handlePassword(e.target.value)}}></input>
            <br></br>

            <button onClick={()=>{handleLogin()}}>Login</button>

        </Fragment>
        
    );
}

export default Login;