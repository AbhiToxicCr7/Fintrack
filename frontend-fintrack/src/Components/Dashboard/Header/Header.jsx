import React, { useEffect, useState } from 'react';
import {BsJustify, BsSearch,BsBellFill,BsFillEnvelopeFill,BsPersonCircle} from 'react-icons/bs';

function Header(){
    return (
        <header className='dashboard-header'>
            <div className='menu-icon'>
                <BsJustify className='icon'/>
            </div>
            <div className='header-left'>
                <BsSearch className='icon'></BsSearch>
            </div>
            <div className='header-right'>
                <BsBellFill className='icon'></BsBellFill>
                <BsFillEnvelopeFill className='icon'></BsFillEnvelopeFill>
                <BsPersonCircle className='icon'></BsPersonCircle>
            </div>
        </header>
        // <div>Header</div>
    )
}

export default Header