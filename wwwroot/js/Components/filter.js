import React from 'react'
import Select from './filter.select.js'
import Search from './filter.search.js'
import Button from './filter.button.js'
import { useContext } from 'react'
import { FilterContext } from '../storeContext.js'

export default function Filter(category) {
    const context = useContext(FilterContext)

    return (
        <div className="home-filter">
            <div className="home-filter__page">
                <span className="home-filter__page-num">
                    <a href="/Home" className="myactionlink">Home</a>/
                    <a href="/Home/Store" className="myactionlink">Store</a>
                    <a className="myactionlink">/{context.category}</a>
                </span>
            </div>
            <Select />
            <Search />
            <Button />
        </div>
    )
}
