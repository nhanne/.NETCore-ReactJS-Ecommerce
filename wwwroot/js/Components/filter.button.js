import React from 'react'
import { useContext } from 'react'
import { FilterContext } from '../storeContext.js'
export default function Button() {
    const context = useContext(FilterContext)

    const handlePage = (e, skipPage) => {
        if (skipPage > 0 && skipPage < parseInt(context.totalPages.current) + 1) {
            context.setPage(skipPage)
        }
    }
    return (
        <div className="home-filter__page">
            <span className="home-filter__page-num">
                <span className="home-filter__page-current">{context.page}</span> / {context.totalPages.current}
            </span>
            <div className="home-filter__page-control">
                <a onClick={(event) => handlePage(event, context.page - 1)}
                    className={`home-filter__page-btn home-filter__page-btn--left ${context.page == 1 ? 'home-filter__page-btn--disabled' : ''}`}
                >
                    <i className="home-filter__page-icon fa fa-caret-left"></i>
                </a>
                <a onClick={(event) => handlePage(event, context.page + 1)}
                    className={`home-filter__page-btn home-filter__page-btn--right ${context.page == context.totalPages.current ? 'home-filter__page-btn--disabled' : ''}`}
                >
                    <i className="home-filter__page-icon fa fa-caret-right"></i>
                </a>
            </div>
        </div>
    )
}