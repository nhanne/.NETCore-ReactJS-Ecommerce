import React from 'react'
export default function Button(props) {

    const handlePage = (e, skipPage) => {
        if (skipPage > 0 && skipPage < parseInt(props.totalPages) + 1) {
            props.setPageIndex(skipPage)
        }
    }
    return (
        <div className="home-filter__page">
            <span className="home-filter__page-num">
                <span className="home-filter__page-current">{props.pageIndex}</span> / {props.totalPages}
            </span>
            <div className="home-filter__page-control">
                <a onClick={(event) => handlePage(event, props.pageIndex - 1)}
                    className={`home-filter__page-btn home-filter__page-btn--left ${props.pageIndex == 1 ? 'home-filter__page-btn--disabled' : ''}`}
                >
                    <i className="home-filter__page-icon fa fa-caret-left"></i>
                </a>
                <a onClick={(event) => handlePage(event, props.pageIndex + 1)}
                    className={`home-filter__page-btn home-filter__page-btn--right ${props.pageIndex == props.totalPages ? 'home-filter__page-btn--disabled' : ''}`}
                >
                    <i className="home-filter__page-icon fa fa-caret-right"></i>
                </a>
            </div>
        </div>
    )
}