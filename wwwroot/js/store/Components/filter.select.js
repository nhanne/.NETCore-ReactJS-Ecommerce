import React from 'react'
import { FilterContext } from '../store.Context'

export default function Select() {
    const context = React.useContext(FilterContext)

    const listInputs = ['Mặc định', 'Mới nhất', 'Bán chạy',
        'Khuyến mãi', 'Giá: thấp đến cao', 'Giá: cao đến thấp']

    return (
        <>
            <span className="home-filter__label">Sắp xếp theo</span>
            <div className="select-input">
                <span className="select-input__label">{context.sort}</span>
                <i className="fa fa-solid fa-caret-down"></i>
                <ul className="select-input__list">
                    {listInputs.map(select => (
                        <li key={select} className="select-input__item">
                            <a  className="select-input__link"
                                onClick={() => context.setSort(select)}
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