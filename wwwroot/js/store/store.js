import React from 'react'
import ReactDOM from 'react-dom'
import { BrowserRouter } from 'react-router-dom'

import { FilterProvider } from './store.Context'
import Categories from './Components/category'
import Filter from './Components/filter'
import GetData from './Components/data'

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


