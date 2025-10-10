import React, { Fragment, useState } from 'react';
import axios from 'axios';

function Registration(){

    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [userName, setUserName] = useState('');
    const [email, setEmail] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const [address, setAddress] = useState('');
    const [password, setPassword] = useState('');

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

    const handleSave = () => {
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
            alert(result.data);
        }).catch((error) => {
            alert(error);
        })
    }

    return (
        <Fragment>
            <div>Registration</div>
            <label>First Name</label>
            <input type="text" id="txtFirstName" placeholder="First Name" onChange={(e)=> handleFirstNameChange(e.target.value)}></input>
            <br></br>

            <label>Last Name</label>
            <input type="text" id="txtLastName" placeholder="Last Name" onChange={(e)=> handleLastNameChange(e.target.value)}></input>
            <br></br>

            <label>User Name</label>
            <input type="text" id="txtLastName" placeholder="User Name" onChange={(e)=> handleUserNameChange(e.target.value)}></input>
            <br></br>

            <label>Email</label>
            <input type="text" id="txtEmail" placeholder="Email Address" onChange={(e)=> handleEmailChange(e.target.value)}></input>
            <br></br>

            <label>Password</label>
            <input type="text" id="txtPassword" placeholder="Enter the Password" onChange={(e)=> handlePasswordChange(e.target.value)}></input>
            <br></br>

            <label>Address</label>
            <input type="text" id="txtAddress" placeholder="Enter Address" onChange={(e)=> handleAddressChange(e.target.value)}></input>
            <br></br>

            <label>Phone Number</label>
            <input type="text" id="txtPhoneNumber" placeholder="Enter Phone number" onChange={(e)=> handlePhoneNumberChange(e.target.value)}></input>
            <br></br>

            <button onClick={() => handleSave()}>Save</button>
        </Fragment>
    )
}

export default Registration;