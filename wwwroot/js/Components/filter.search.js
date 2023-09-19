import React from 'react'
export default function Search(props) {
    return (
        <form className="dropdown-search show" id="dropdown-search">
            <input
                onInput={(e) => props.setSearch(e.target.value)}
                className="input-validation-error"
                id="search-box"
                placeholder="Nhập tên sản phẩm"
                type="text" value={props.search}
            />
            <button
                type="button"
                className="btnsearch"
                id="btnSearch"
            >
                <i className="fa fa-search"></i>
            </button>
        </form>
    )
}