import React from 'react'
import { Routes, Route } from 'react-router-dom'
import {memo} from 'react'

import Products from './data.products.js'
import Product from './data.product'

 function GetData() {

    return (
        <div className="grid__column-10" id="release">
            <Routes>
                <Route path="/Home/Store" element={<Products />} />
                <Route path="/Home/Store/Product/:Id" element={<Product /> } />
            </Routes>
        </div>
    )
}
export default memo(GetData)
