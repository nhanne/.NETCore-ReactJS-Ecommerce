import React from 'react'
import ReactDOM from 'react-dom' 
import {useState} from 'react'
import Categories from './Components/category.js'
import Filter from './Components/filter.js'
import GetData from './Components/data.js'

function App() {
    const [searchTerm, setSearch] = useState('');
    const [sortBy, setSortBy] = useState('Mặc định');
    const [cateName, setCateName] = useState('');
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState();
   
    return (
        <div className="grid app__content">
            <div className="store__heading ">
                <Filter
                    sort={sortBy}
                    setSortBy={setSortBy}
                    cateName={cateName}
                    search={searchTerm}
                    setSearch={setSearch}
                    pageIndex={pageIndex}
                    setPageIndex={setPageIndex}
                    totalPages={totalPages}
                />
            </div>
            <div className="grid__row store__body">
                <Categories
                    setCateName={setCateName}
                />
                <GetData
                    cate={cateName}
                    sort={sortBy}
                    search={searchTerm}
                    page={pageIndex}
                    setPage={setPageIndex}
                    totalPages={totalPages}
                    setTotalPages={setTotalPages}
                />
            </div>
        </div>

    )
}


ReactDOM.render(<App />, document.querySelector('#main'))


