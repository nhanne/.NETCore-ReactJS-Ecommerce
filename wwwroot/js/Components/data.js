import React from 'react'
import { useState, useEffect } from 'react'

export default function GetData
    ({
        cateName, sortBy, searchTerm,
        pageIndex, setPageIndex,
        totalPages, setTotalPages }) {
    const [products, setProducts] = useState([]);

    useEffect(() => {
        fetch(`/Home/getData?category=${cateName}&&sort=${sortBy}&&search=${searchTerm}&&page=${pageIndex}`)
            .then(res => res.json())
            .then(reponse => {
                setProducts(reponse.products);
                setTotalPages(reponse.totalPages);
            });
    }, [cateName, sortBy, searchTerm, pageIndex, totalPages])

    if (pageIndex > totalPages) {
        setPageIndex(1)
    }
    return (
        <React.Fragment>
            <div className="grid__column-10" id="release">
                <ul id="products" className="row">
                    {products.map(product => (
                        <li key={product.p.id} className='product-item'>
                            <div className="product-top">
                                <a className="product-thumb"><img
                                    src={`/Images/sp/${product.p.picture}`} />
                                </a>
                                <a className="buynow"
                                    title="Order"
                                    href={`/Home/Product/${product.p.id}`} >
                                    Xem ngay
                                </a>
                            </div>
                            <div className="product-info">
                                <a className="product-cat"> {product.cateName}</a>
                                <a className="product-name">{product.p.name}</a>
                                <div className="product-price">
                                    <span> </span>
                                    <span className="product--unitPrice">{product.p.unitPrice.toLocaleString()}
                                        <span className="product--currentPrice">₫</span>
                                    </span>
                                </div>

                            </div>
                        </li>
                    ))}
                </ul>
                <div id="pagination" className="MenuTrang pagination-container">
                    <ul class="pagination">
                        {totalPages > 1 ? (
                            <>
                                {Array.from({ length: totalPages }, (_, i) => (
                                    <li className={`page-item`} key={i}>
                                        <a
                                            className={`page-link ${i + 1 === pageIndex ? 'active' : ''}`}
                                            data-page={i + 1}
                                        >
                                            {i + 1}
                                        </a>
                                    </li>
                                ))}
                            </>
                        ) : (
                            ''
                        )}
                    </ul>
                </div>
            </div>
        </React.Fragment >
    )
}
