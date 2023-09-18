import React from 'react'
import { useState, useEffect } from 'react'


export default function Filter({
    sort, setSortBy,
    cateName, search, setSearch,
    pageIndex, setPageIndex, totalPages }) {
    //
    const listInputs = ['Mặc định', 'Mới nhất', 'Bán chạy',
        'Khuyến mãi', 'Giá: thấp đến cao', 'Giá: cao đến thấp']

    const handleSelect = (event, select) => {
        setSortBy(select);
    };
    const handleSearch = (e) => {
        setSearch(e.target.value)
    };
    const handlePage = (e, skipPage) => {
        if (skipPage > 0 && skipPage < parseInt(totalPages) + 1) {
            setPageIndex(skipPage)
        }
    }

    return (
        <div className="home-filter">
            <div className="home-filter__page">
                <span className="home-filter__page-num">
                    <a href="/Home" className="myactionlink">Home</a>/
                    <a href="/Home/Store" className="myactionlink">Store</a>
                    <a className="myactionlink">/{cateName}</a>
                </span>
            </div>
            <span className="home-filter__label">Sắp xếp theo</span>
            <div className="select-input">
                <span className="select-input__label">{sort}</span>
                <i className="fa fa-solid fa-caret-down"></i>
                <ul className="select-input__list">
                    {listInputs.map(select => (
                        <li key={select} className="select-input__item">
                            <a className="select-input__link"
                                onClick={(event) => handleSelect(event, select)}>
                                {select}
                            </a>
                        </li>
                    ))}
                </ul>
            </div>
            <form className="dropdown-search show" id="dropdown-search">
                <input onInput={(event) => handleSearch(event)} className="input-validation-error"
                    id="search-box" placeholder="Nhập tên sản phẩm" type="text" value={search} />
                <button type="button" className="btnsearch" id="btnSearch">
                    <i className="fa fa-search"></i>
                </button>
            </form>
            <div className="home-filter__page">
                <span className="home-filter__page-num">
                    <span className="home-filter__page-current">{pageIndex}</span> / {totalPages}
                </span>
                <div className="home-filter__page-control">
                    <a onClick={(event) => handlePage(event, pageIndex - 1)}
                        className={`home-filter__page-btn home-filter__page-btn--left 
                        ${pageIndex == 1 ? 'home-filter__page-btn--disabled' : ''}`}>
                        <i className="home-filter__page-icon fa fa-caret-left"></i>
                    </a>
                    <a onClick={(event) => handlePage(event, pageIndex + 1)}
                        className={`home-filter__page-btn home-filter__page-btn--right 
                        ${pageIndex == totalPages ? 'home-filter__page-btn--disabled' : ''}`}>
                        <i className="home-filter__page-icon fa fa-caret-right"></i>
                    </a>
                </div>
            </div>
        </div>
    )
}
