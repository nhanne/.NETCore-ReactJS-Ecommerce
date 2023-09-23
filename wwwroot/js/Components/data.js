import React from 'react'
import { Routes, Route } from 'react-router-dom'

import { useContext, memo} from 'react'
import { FilterContext } from '../storeContext.js'


import Products from './data.products.js'
import Product from './data.product'

 function GetData() {
    const context = useContext(FilterContext)

    return (
        <div className="grid__column-10" id="release">
            <Routes>
                <Route path="/Home/Store" element={<Products products={context.products}/>} />
                <Route path="/Home/Store/Product" element={<Product /> } />
            </Routes>
        </div>
    )
}
export default memo(GetData)
