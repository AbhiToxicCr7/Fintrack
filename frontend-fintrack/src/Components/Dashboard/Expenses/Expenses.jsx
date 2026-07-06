import { TfiAngleDoubleDown, TfiAngleDoubleUp  } from "react-icons/tfi";
import { GoAlertFill } from "react-icons/go";
import React, { useState, useEffect } from 'react'
import './Expenses.css';
import axios from 'axios';

function Expenses(){
    //const [action, setAction] = useState("Null");
    const [amount, setAmount] = useState("");
    const [currency, setCurrency] = useState("");
    const [category, setCategory] = useState("");
    const [date, setDate] = useState("");
    const [note, setNote] = useState("");
    const token = localStorage.getItem('authToken');
    const [totalMonthlyExpense, setTotalMonthyExpense] = useState(0);
    const [expenses, setExpenses] = useState([]);
    const [isEditing, setIsEditing] = useState(false);
    const [editingId, setEditingId] = useState(null);
    const [filterCategory, setFilterCategory] = useState("");
    const [filterDay, setFilterDay] = useState("");
    const [filterMonth, setFilterMonth] = useState("");
    const [filterYear, setFilterYear] = useState("");

    const categories = ["Food","Transport","Housing","Utilities","Healthcare","Education","Entertainment","Clothing","Savings","Investment","Miscellaneous"]

    const loggedUserID = localStorage.getItem('loggedinUserID');

    const handleAmountChange = (value)=>{
        setAmount(value);
    }
    const handleCurrencyChange = (value)=>{
        setCurrency(value);
    }
    const handleCategoryChange = (value)=>{
        setCategory(value);
    }
    const handleDateChange = (value)=>{
        setDate(value);
    }
    const handleNoteChange = (value)=>{
        setNote(value);
    }

    const handleSave = ()=>{
        const data = {
            UserId: loggedUserID,
            Amount: amount,
            Currency: currency,
            Category: category,
            IsActive: true,
            Date: date,
            Note: note
        }

        if (isEditing && editingId) {
            const url = `https://localhost:44389/api/UserExpense/UpdateExpense/${editingId}`;
            axios.put(url, data, { headers: { Authorization: `Bearer ${token}` } })
                .then(()=>{
                    window.alert("Expense updated successfully");
                    setIsEditing(false);
                    setEditingId(null);
                    clearForm();
                    fetchExpenses();
                })
                .catch(()=>{
                    window.alert("There was an error updating the expense");
                });
            return;
        }

        const url = "https://localhost:44389/api/UserExpense/AddExpense";
        axios.post(url, data,{
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
        .then(()=>{
            window.alert("Expense added successfully");
            clearForm();
            fetchExpenses();
        })
        .catch(()=>{
            window.alert("There was an error in processing your request")
        });
    }

    const clearForm = ()=>{
        setAmount("");
        setCurrency("");
        setCategory("");
        setDate("");
        setNote("");
    }

    const clearFilter = () =>{
        setFilterCategory("");
        setFilterDay("");
        setFilterMonth("");
        setFilterYear("");

        const url = `https://localhost:44389/api/UserExpense/GetById?userId=${loggedUserID}`;
        axios.get(url, { headers: { Authorization: `Bearer ${token}` } })
            .then(r=> setExpenses(r.data.Expenses || []))
            .catch(()=>{
                // fallback to older endpoint if present
            });
    } 

    const fetchExpenses = () => {
        const base = `https://localhost:44389/api/UserExpense/GetById`;

        const urlParams = new URLSearchParams();
        urlParams.append('userId', loggedUserID);
        if (filterCategory) urlParams.append('category', filterCategory);
        if (filterDay) urlParams.append('day', filterDay);
        if (filterMonth) urlParams.append('month', filterMonth);
        if (filterYear) urlParams.append('year', filterYear);

        const url = `${base}?${urlParams.toString()}`;
        axios.get(url, { headers: { Authorization: `Bearer ${token}` } })
            .then((r)=>{
                setExpenses(r.data.Expenses || []);
                setTotalMonthyExpense(r.data.TotalExpenseAmount || 0);
            })
            .catch(()=>{
                // fallback to older endpoint if present
            });
    }

    useEffect(()=>{
        fetchExpenses();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    },[]);

    const handleDelete = (id)=>{
        if (!window.confirm('Delete this expense?')) return;
        const url = `https://localhost:44389/api/UserExpense/DeleteExpense/${id}`;
        axios.delete(url, { headers: { Authorization: `Bearer ${token}` } })
            .then(()=>{
                fetchExpenses();
            })
            .catch(()=>{
                window.alert('There was an error deleting the expense');
            });
    }

    const handleEdit = (item)=>{
        setIsEditing(true);
        setEditingId(item.Id || item.id || null);
        setAmount(item.Amount || "");
        setCurrency(item.Currency || "");
        setCategory(item.Category || "");
        // try to normalize date string
        setDate(item.Date ? item.Date.split('T')[0] : "");
        setNote(item.Note || "");
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }

    return (
        <main className='main-expense-container'>
            <div className='top-header'>
                <h3>EXPENSES</h3>
                <h2>Track and manage your spending</h2>
            </div>
            <div className='exp-cards'>
                <div className='exp-card'>
                    <div className='exp-card-inner'>
                        <h3>TOTAL EXPENSES(Current Month)</h3>
                        <TfiAngleDoubleDown className='card_icon'></TfiAngleDoubleDown>
                    </div>
                    <h1>{totalMonthlyExpense} INR</h1>
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
            {/* <div className="exp-add-button-container">
                <button className="add-expense" onClick={()=> setAction("add")}>Add Expense</button>
            </div> */}
            <div className="expense-inputs">
                <div className="input1">
                    <input
                        type="text"
                        placeholder="Amount"
                        value={amount}
                        onChange={(e)=> handleAmountChange(e.target.value)}
                    />
                    <input
                        type="text"
                        placeholder="Currency"
                        value={currency}
                        onChange={(e)=> handleCurrencyChange(e.target.value)}
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
                        value={date}
                        onChange={(e)=> handleDateChange(e.target.value)}
                    />
                    <input
                        type="text"
                        placeholder="Notes"
                        value={note}
                        onChange={(e)=> handleNoteChange(e.target.value)}
                    />
                </div>
            </div>
            <div className="exp-save-button-container">
                <button className="save" onClick={()=> handleSave()}>{isEditing? 'Update' : 'Save'}</button>
                {isEditing && (
                    <button className="cancel" onClick={()=>{ setIsEditing(false); setEditingId(null); clearForm(); }}>Cancel</button>
                )}
                {/* <button className="cancel" onClick={()=> handleCancel()}>Cancel</button> */}
            </div>          
            <div className="expenses-table-container">
                <div className="expenses-filters">
                    <label>
                        Category:
                        <select value={filterCategory} onChange={(e)=> setFilterCategory(e.target.value)}>
                            <option value="">All Categories</option>
                            {categories.map(c=> <option key={c} value={c}>{c}</option>)}
                        </select>
                    </label>
                    <label>
                        Day:
                        <input type="number" min="1" max="31" placeholder="DD" value={filterDay} onChange={(e)=> setFilterDay(e.target.value)} />
                    </label>
                    <label>
                        Month:
                        <input type="number" min="1" max="12" placeholder="MM" value={filterMonth} onChange={(e)=> setFilterMonth(e.target.value)} />
                    </label>
                    <label>
                        Year:
                        <input type="number" min="1900" max="2100" placeholder="YYYY" value={filterYear} onChange={(e)=> setFilterYear(e.target.value)} />
                    </label>
                    <div className="filter-actions">
                        <button className="apply" onClick={fetchExpenses}>Apply</button>
                        <button className="clear" onClick={clearFilter}>Clear</button>
                    </div>
                </div>
                <h3>Saved Expenses</h3>
                {expenses.length === 0 ? (
                    <p>No expenses found.</p>
                ) : (
                    <table className="expenses-table">
                        <thead>
                            <tr>
                                <th>Amount</th>
                                <th>Currency</th>
                                <th>Category</th>
                                <th>Date</th>
                                <th>Note</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {expenses.map((item)=> (
                                <tr key={item.Id || item.id}>
                                    <td>{item.Amount}</td>
                                    <td>{item.Currency}</td>
                                    <td>{item.Category}</td>
                                    <td>{item.Date ? (item.Date.split('T')[0]) : ''}</td>
                                    <td>{item.Note}</td>
                                    <td>
                                        <button className="edit" onClick={()=> handleEdit(item)}>Edit</button>
                                        <button className="delete" onClick={()=> handleDelete(item.Id || item.id)}>Delete</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </div>
        </main>
    )
}

export default Expenses