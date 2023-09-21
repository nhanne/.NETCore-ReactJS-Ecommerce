import React from 'react'
import ReactDOM from 'react-dom'
import Categories from './Components/category.js'
import Filter from './Components/filter.js'
import GetData from './Components/data.js'
import { FilterProvider } from './storeContext.js'


function App() {
    return (
        <FilterProvider>
            <div className="grid app__content">
                
                <div className="store__heading ">
                    <Filter />
                </div>

                <div className="grid__row store__body">
                    <Categories/>
                    <GetData />
                </div>

            </div>
        </FilterProvider>
    )
}

ReactDOM.render(<App />, document.querySelector('#main'))


