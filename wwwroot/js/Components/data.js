import React from 'react'
import { useState, useEffect } from 'react'
import Pagination from './data.pagination.js'
import Products from './data.products.js'

export default function GetData(props) {
    const [products, setProducts] = useState([]);

    useEffect(() => {
        fetch(`/Home/getData?category=${props.cate}&&sort=${props.sort}&&search=${props.search}&&page=${props.page}`)
            .then(res => res.json())
            .then(reponse => {
                setProducts(reponse.products);
                props.setTotalPages(reponse.totalPages);
            });
    }, [props.cate, props.sort, props.search, props.page, props.totalPages])

    if (props.page > props.totalPages) {
        props.setPage(1)
    }

    return (
        <div className="grid__column-10" id="release">
            <Products
                products={products}
            />
            <Pagination
                totalPages={props.totalPages}
                page={props.page}
                setPage={props.setPage}
            />
        </div>
    )
}
