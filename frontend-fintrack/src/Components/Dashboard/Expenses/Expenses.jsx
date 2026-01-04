import { TfiAngleDoubleDown, TfiAngleDoubleUp  } from "react-icons/tfi";
import { GoAlertFill } from "react-icons/go";
import React, { useState } from 'react'

function Expenses(){
    const [action, setAction] = useState("Null");
    const [category, setCategory] = useState("");
    const [date, setDate] = useState("");
    const categories = ["Food","Transport","Housing","Utilities","Healthcare","Education","Entertainment","Clothing","Savings","Investment","Miscellaneous"]

    const handleAmountChange = ()=>{

    }
    const handleCategoryChange = (value)=>{
        setCategory(value);
    }
    const handleDateChange = (value)=>{
        setDate(value);
    }
    const handleNoteChange = ()=>{
        
    }
    const handleSave = ()=>{
        
    }
    const handleCancel = ()=>{
        
    }

    return (
        <main className='main-expense-container'>
            <div className='top-header'>
                <h3>EXPENSES</h3>
                <h2>Track and manage your daily spending</h2>
            </div>
            <div className='exp-cards'>
                <div className='exp-card'>
                    <div className='exp-card-inner'>
                        <h3>TOTAL EXPENSES(This Month)</h3>
                        <TfiAngleDoubleDown className='card_icon'></TfiAngleDoubleDown>
                    </div>
                    <h1>45,000 INR</h1>
                </div>
                <div className='exp-card'>
                    <div className='exp-card-inner'>
                        <h3>TOP SPENDING CATEGORY</h3>
                        <TfiAngleDoubleUp className='card_icon'></TfiAngleDoubleUp>
                    </div>
                    <h1>HOUSING</h1>
                </div>
                <div className='exp-card'>
                    <div className='exp-card-inner'>
                        <h3>REMAINING BALANCE</h3>
                        <GoAlertFill className='card_icon'></GoAlertFill>
                    </div>
                    <h1>25000</h1>
                </div>
            </div>
            <div className="exp-add-button-container">
                <button className="add-expense" onClick={()=> setAction("add")}>Add Expense</button>
            </div>
            <div className="expense-inputs">
                {action === "add" &&(
                    <div className="input1">
                    <input
                        type="text"
                        placeholder="Amount"
                        onChange={(e)=> handleAmountChange(e.target.value)}
                    />
                    <select
                        value={category}
                        onChange={(e)=> handleCategoryChange(e.target.value)}
                        >
                        <option value="">Select Category</option>
                        {categories.map((c) =>(
                            <option key={c} value={c}>{c}</option>
                        ))}
                    </select>
                    <input
                        type="date"
                        placeholder="Date"
                        onChange={(e)=> handleDateChange(e.target.value)}
                    />
                    <input
                        type="text"
                        placeholder="Notes"
                        onChange={(e)=> handleNoteChange(e.target.value)}
                    />
                </div>
                )}
            </div>
            <div className="exp-save-button-container">
                <button className="save" onClick={()=> handleSave()}>Save</button>
                <button className="cancel" onClick={()=> handleCancel()}>Cancel</button>
            </div>          
        </main>
    )
}

export default Expenses