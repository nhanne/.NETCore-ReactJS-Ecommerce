import React from 'react'
import { useContext } from 'react'
import { FilterContext } from '../storeContext.js'
export default function Search() {
    const context = useContext(FilterContext)

    return (
        <form className="dropdown-search show" id="dropdown-search">
            <input
                onInput={(e) => context.setSearch(e.target.value)}
                className="input-validation-error"
                id="search-box"
                placeholder="Nhập tên sản phẩm"
                type="text" value={context.search}
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