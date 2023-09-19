import React from 'react'
import Select from './filter.select.js'
import Search from './filter.search.js'
import Button from './filter.button.js'
export default function Filter(props) {

    return (
        <div className="home-filter">
            <div className="home-filter__page">
                <span className="home-filter__page-num">
                    <a href="/Home" className="myactionlink">Home</a>/
                    <a href="/Home/Store" className="myactionlink">Store</a>
                    <a className="myactionlink">/{props.cateName}</a>
                </span>
            </div>
            <Select
                sort={props.sort}
                setSortBy={props.setSortBy}
            />
            <Search
                setSearch={props.setSearch}
                search={props.search}
            />
            <Button
                pageIndex={props.pageIndex}
                setPageIndex={props.setPageIndex}
                totalPages={props.totalPages}
            />
        </div>
    )
}
