import React from 'react'
export default function Select(props) {

    const listInputs = ['Mặc định', 'Mới nhất', 'Bán chạy',
        'Khuyến mãi', 'Giá: thấp đến cao', 'Giá: cao đến thấp']
    return (
        <>
            <span className="home-filter__label">Sắp xếp theo</span>
            <div className="select-input">
                <span className="select-input__label">{props.sort}</span>
                <i className="fa fa-solid fa-caret-down"></i>
                <ul className="select-input__list">
                    {listInputs.map(select => (
                        <li key={select} className="select-input__item">
                            <a  className="select-input__link"
                                onClick={() => props.setSortBy(select)}
                            >
                                {select}
                            </a>
                        </li>
                    ))}
                </ul>
            </div>
        </>
    )
}