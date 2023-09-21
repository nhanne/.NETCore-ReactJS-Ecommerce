import React from 'react'
import Pagination from './data.pagination.js'
import Products from './data.products.js'
import { useContext } from 'react'
import { FilterContext } from '../storeContext.js'

export default function GetData() {
    const context = useContext(FilterContext)

    return (
        <div className="grid__column-10" id="release">
            <Products
                products={context.products}
            />
            <Pagination />
        </div>
    )
}

