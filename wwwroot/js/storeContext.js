import React from 'react'
import { useState, useEffect, createContext } from 'react'

const FilterContext = createContext()
function FilterProvider({ children }) {
    const [search, setSearch] = useState('');
    const [sort, setSort] = useState('Mặc định');
    const [category, setCategory] = useState('');
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState();

    const [products, setProducts] = useState([]);

    useEffect(() => {
        fetch(`/Home/getData?category=${category}&&sort=${sort}&&search=${search}&&page=${page}`)
            .then(res => res.json())
            .then(reponse => {
                setProducts(reponse.products);
                setTotalPages(reponse.totalPages);
            });
    }, [category, sort, search, page, totalPages])
    if (page > totalPages) {
        setPage(1)
    }

    const value = {
        search, setSearch,
        sort, setSort,
        category, setCategory,
        page, setPage,
        totalPages, setTotalPages,
        products
    }

    return (
        <FilterContext.Provider value={value}>
            {children}
        </FilterContext.Provider>
    )
}
export { FilterContext, FilterProvider }