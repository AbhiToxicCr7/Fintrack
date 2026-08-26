import React, { useEffect, useState } from 'react';
import {BsFillArchiveFill} from 'react-icons/bs';
import { BarChart, Bar, Rectangle, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, LineChart, Line } from 'recharts';
import { TfiAngleDoubleDown, TfiAngleDoubleUp  } from "react-icons/tfi";
import { GoAlertFill } from "react-icons/go";
import { CiFaceSmile } from "react-icons/ci";
import { useQuery } from '@tanstack/react-query';
import axios from 'axios';
import { getUserDetails } from "../../../Services/UserDetailService.js";
import { FetchExpenseService } from "../../../Services/UserExpenseService.js";
import ExpenseIncomeBarChart from "../../Charts/ExpenseIncomeBarChart";
import Expenses from '../Expenses/Expenses.jsx';
import ExpensesChart from "../../Charts/ExpensePieChart";


function Home(){

   const userFirstName = localStorage.getItem('userFirstName');
   const loggedinUserID = localStorage.getItem('loggedinUserID');
   const token = localStorage.getItem('authToken');
   const [transactions, setTransactions] = useState([]);

   const getTransactions = () => {
           const url = `https://localhost:44389/api/Dashboard/GetDashboardData?userId=${loggedinUserID}`;

           axios.get(url, { headers: { Authorization: `Bearer ${token}` } })
               .then((r)=>{
                   setTransactions(r.data.RecentTransactions || []);
               })
               .catch(()=>{
                   // fallback to older endpoint if present
               });
       }

    useEffect(()=>{
            getTransactions();
            // eslint-disable-next-line react-hooks/exhaustive-deps
        },[]);

   const {
    data: userDetails,
    isLoading,
    error
   } = useQuery({
    queryKey: ['userDetails', loggedinUserID],
    queryFn: ()=> getUserDetails(loggedinUserID, token)
   })

   const {
    data: userExpenses
   } = useQuery({
    queryKey: ['userExpenses', loggedinUserID],
    queryFn: ()=> FetchExpenseService(loggedinUserID, token)
   })

   const incomeVsExpenseData = userExpenses?.MonthWiseIncomeExpenseData || [];
   const expenseByCategory  = userExpenses?.ExpenseByCategory || {};

    const chartData = incomeVsExpenseData.map(item => ({
        name: `${item.MonthName} ${item.Year}`,
        income: item.TotalIncomeAmount,
        expense: item.TotalExpenseAmount
    }));

    const pieChartData = Object.entries(expenseByCategory).map(([key, value]) => ({
        name: key,
        value: value
    }));

   const savings = userDetails?.MonthlySalary - userExpenses?.TotalExpenseAmount;
   const savingsRate =
    ((savings / userDetails?.MonthlySalary) * 100).toFixed(2);

    return (
        <main className='main-container'>
            {/* <div className='main-title'>
                <h3>Welcome {userFirstName}</h3>
            </div> */}

            <div className='main-cards'>
                <div className='card'>
                    <div className='card-inner'>
                        <h3>INCOME</h3>
                        <TfiAngleDoubleUp className='card_icon'></TfiAngleDoubleUp>
                    </div>
                    <h1>{userDetails?.MonthlySalary} INR</h1>
                </div>
                <div className='card'>
                    <div className='card-inner'>
                        <h3>EXPENSE</h3>
                        <TfiAngleDoubleDown className='card_icon'></TfiAngleDoubleDown>
                    </div>
                    <h1>{userExpenses?.TotalExpenseAmount} INR</h1>
                </div>
                <div className='card'>
                    <div className='card-inner'>
                        <h3>SAVINGS</h3>
                        <CiFaceSmile className='card_icon'></CiFaceSmile>
                    </div>
                    <h1>{savings} INR</h1>
                </div>
                <div className='card'>
                    <div className='card-inner'>
                        <h3>SAVINGS RATE</h3>
                        <CiFaceSmile className='card_icon'></CiFaceSmile>
                    </div>
                    <h1>{savingsRate}%</h1>
                </div>
            </div>
            <div className='charts'>
                <ResponsiveContainer width="100%" height="100%">
                  <ExpenseIncomeBarChart data={chartData}></ExpenseIncomeBarChart>   
                </ResponsiveContainer>

                <ExpensesChart data={pieChartData} responsive></ExpensesChart>
            </div>
            <div className='recent-trasaction-container'>
                <table className='transaction-table'>
                    <thead>
                            <tr>
                                <th>Amount</th>
                                <th>Note</th>
                                <th>Category</th>
                                <th>Date</th>
                                <th>TransactionType</th>
                            </tr>
                    </thead>
                    <tbody>
                            {transactions.map((item)=> (
                                <tr key={item.Id || item.id}>
                                    <td>{item.Amount}</td>
                                    <td>{item.Note}</td>
                                    <td>{item.Category}</td>
                                    <td>{item.Date ? (item.Date.split('T')[0]) : ''}</td>
                                    <td>{item.TransactionType}</td>
                                </tr>
                            ))}
                    </tbody>
                </table>
            </div>
        </main>
    )
}

export default Home