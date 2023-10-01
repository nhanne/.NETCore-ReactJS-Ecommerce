import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter } from 'react-router-dom'

import { FilterProvider } from './storeContext.js'
import Categories from './Components/category.js'
import Filter from './Components/filter.js'
import GetData from './Components/data.js'

function App() {
    return (
        <BrowserRouter>
            <FilterProvider>
                <div className="grid app__content">
                    <div className="store__heading ">
                        <Filter />
                    </div>
                    <div className="grid__row store__body">
                        <Categories />
                        <GetData />
                    </div>
                </div>
            </FilterProvider>
        </BrowserRouter>
    )
}

ReactDOM.render(<App />, document.querySelector('#store'))


