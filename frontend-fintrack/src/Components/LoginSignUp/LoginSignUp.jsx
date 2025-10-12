import React, { useState } from 'react'
import axios from 'axios';
import './LoginSignUp.css'
import user_icon from '../Assets/person.png'
import email_icon from '../Assets/email.png'
import pass_icon from '../Assets/password.png'
import { useNavigate } from 'react-router-dom'; 

export const LoginSignUp = () => {

  const navigate = useNavigate();
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [userName, setUserName] = useState('');
  const [email, setEmail] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [address, setAddress] = useState('');
  const [password, setPassword] = useState('');
  const [action, setAction] = useState("Login");
  const [profile, setProfile] = useState(null);
  //var userID= null;

  const handleFirstNameChange = (value) => {
        setFirstName(value);
    }

  const handleLastNameChange = (value) => {
        setLastName(value);
    }

  const handleUserNameChange = (value) => {
        setUserName(value);
    }

  const handleEmailChange = (value) =>{
        setEmail(value);
    }

  const handlePhoneNumberChange = (value) => {
        setPhoneNumber(value);
    }

  const handleAddressChange = (value) => {
        setAddress(value);
    }

  const handlePasswordChange = (value) => {
        setPassword(value);
    }

  const handleRegister = () => {
        const data = {
            FirstName : firstName,
            LastName : lastName,
            UserName : userName,
            Email : email,
            PhoneNumber : phoneNumber,
            Address : address,
            Password : password,
            IsActive : true
        }
        var url = "https://localhost:44389/api/User/Register";
        axios.post(url, data).then((result) => {
            //userID = result.Data.userId; 
            alert(result.data);
        }).catch((error) => {
            alert(error);
        })
    }

  const handleLogin = async ()=>{
        const url = "https://localhost:44389/api/Auth/Login";

        const data = {
            UserName : userName,
            Password : password,
            ClientId : "Client1"
            //UserId : userID
        } 

        try {
            const response = await axios.post(url, data);
            // Assuming token is in response.data.token or response.data (adjust as per your API)
            const token = response.data.Token || response.data;
            const userID = response.data.UserID || response.data;
            // Call GetProfile API with JWT token
            localStorage.setItem('authToken',token);
            localStorage.setItem('loggedinUserID',userID);
            navigate('/UserDetails');
        } catch (error) {
            alert("Login failed or unable to fetch profile.");
        }

    }

    // if(profile){
    //     return (
    //                <div className="dashboard">
    //                 <h2>Dashboard</h2>
    //                 <p>Email: {profile.Email}</p>
    //                 <p>First Name: {profile.Firstname}</p>
    //                 <p>Last Name: {profile.Lastname}</p>
    //               </div>
    //             );
    // }

  return (
    <div className="container">
        <div className="header">
          <div className="text">{action === "Login" ? "Login" : "Sign Up"}</div>
          <div className="underline"></div>
          <div className="submit-container">
          <div
            className={`submit ${action === "Sign Up" ? "active" : "inactive"}`}
            onClick={()=> setAction("Sign Up")}
          >
            Sign Up
          </div>
          <div
            className={`submit ${action === "Login" ? "active" : "inactive"}`}
            onClick={()=> setAction("Login")}
          >
            Login
          </div>
        </div>
        </div>
        <div className="inputs">
          {action === "Sign Up" && (
            <div className="input">
              <img src={user_icon} alt="" />
              <input type="text" placeholder="First Name" onChange={(e)=> handleFirstNameChange(e.target.value)}/>
            </div>
          )}
          {action === "Sign Up" && (
            <div className="input">
              <img src={user_icon} alt="" />
              <input type="text" placeholder="Last Name" onChange={(e)=> handleLastNameChange(e.target.value)}/>
            </div>
          )}
            <div className="input">
              <img src={user_icon} alt="" />
              <input type="text" placeholder="User Name" onChange={(e)=> handleUserNameChange(e.target.value)}/>
            </div>
          {action === "Sign Up" && (
            <div className="input">
              <img src={user_icon} alt="" />
              <input type="text" placeholder="Phone Number" onChange={(e)=> handlePhoneNumberChange(e.target.value)}/>
            </div>
          )}
          {action === "Sign Up" && (
            <div className="input">
              <img src={user_icon} alt="" />
              <input type="text" placeholder="Address" onChange={(e)=> handleAddressChange(e.target.value)}/>
            </div>
          )}
          {action === "Sign Up" && (
            <div className="input">
            <img src={email_icon} alt="" />
            <input type="email" placeholder="Email" onChange={(e)=> handleEmailChange(e.target.value)}/>
          </div>
          )}
          <div className="input">
            <img src={pass_icon} alt="" />
            <input type="password" placeholder="Password" onChange={(e)=> handlePasswordChange(e.target.value)}/>
          </div>
        </div>
        {action === "Login" && (
          <div className="forgot-password">
          Forgot password? <span>Click Here!</span>
          </div>
        )}
        <div className="button-container">
          {action === "Sign Up" && (
            <button className="register-submit" onClick={()=> handleRegister()}>Submit</button>
          )}
          {action === "Login" && (
            <button className="login-submit" onClick={()=> handleLogin()}>Enter</button>
          )}
        </div>
    </div>
  )
}
