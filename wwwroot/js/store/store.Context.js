import React from 'react'
import { useState, useEffect, useRef, createContext } from 'react'

const FilterContext = createContext()
function FilterProvider({ children }) {
    const [search, setSearch] = useState('');
    const [sort, setSort] = useState('Mặc định');
    const [category, setCategory] = useState('');
    const [page, setPage] = useState(1);
    const [products, setProducts] = useState([]);
    const totalPages = useRef();

    useEffect(() => {
        fetch(`/Home/getData?category=${category}&&sort=${sort}&&search=${search}&&page=${page}`)
            .then(res => res.json())
            .then(reponse => {
                if (reponse.products.length > 0) {
                    totalPages.current = reponse.totalPages;
                }
                setProducts(reponse.products);

            });
    }, [category, sort, search, page])

    if (page > totalPages.current) {
        setPage(1)
    }
    
    const value = {
        search, setSearch,
        sort, setSort,
        category, setCategory,
        page, setPage,
        totalPages,
        products
    }

    return (
        <FilterContext.Provider value={value}>
            {children}
        </FilterContext.Provider>
    )
}
export { FilterContext, FilterProvider }