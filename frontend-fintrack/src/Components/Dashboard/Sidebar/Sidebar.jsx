import React, { useEffect, useState } from 'react';
import {BsJustify, BsSearch,BsBellFill,BsFillEnvelopeFill,BsPersonCircle,BsCart3} from 'react-icons/bs';
import { AiFillDashboard } from "react-icons/ai";
import { TfiAngleDoubleDown, TfiAngleDoubleUp  } from "react-icons/tfi";
import { HiDocumentChartBar } from "react-icons/hi2";
import { IoSettings } from "react-icons/io5";

function Sidebar(){
    return (
        <aside id='sidebar'>
            <div className='sidebar-title'>
                <h3>FINTRACK</h3>
                <span className='icon clos_icon'></span>
            </div>
            <ul className='sidebar-list'>
                <li className='sidebar-list-item'>
                    <a href="">
                        <AiFillDashboard className='icon'></AiFillDashboard>Dashboard
                    </a>
                </li>
                <li className='sidebar-list-item'>
                    <a href="">
                        <TfiAngleDoubleUp className='icon'></TfiAngleDoubleUp>Incomes
                    </a>
                </li>
                <li className='sidebar-list-item'>
                    <a href="">
                        <TfiAngleDoubleDown className='icon'></TfiAngleDoubleDown>Expenses
                    </a>
                </li>
                <li className='sidebar-list-item'>
                    <a href="">
                        <HiDocumentChartBar className='icon'></HiDocumentChartBar>Reports
                    </a>
                </li>
                <li className='sidebar-list-item'>
                    <a href="">
                        <IoSettings className='icon'></IoSettings>Settings
                    </a>
                </li>
            </ul>
        </aside>
    )
}

export default Sidebar