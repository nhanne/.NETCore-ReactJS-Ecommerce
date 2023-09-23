import React from 'react'
import { Link } from 'react-router-dom'
import Pagination from './data.pagination.js'
export default function Products({ products }) {

    return (
        <>
            <ul id="products" className="row">
                {products.map(product => (
                    <li key={product.p.id} className='product-item'>
                        <div className="product-top">
                            <Link className="product-thumb" to="./Product"><img
                                src={`/Images/sp/${product.p.picture}`} />
                            </Link>
                            <Link className="buynow"
                                title="Order"
                                to="./Product" >
                                Xem ngay
                            </Link>
                        </div>
                        <div className="product-info">
                            <a className="product-cat"> {product.cateName}</a>
                            <a className="product-name">{product.p.name}</a>
                            <div className="product-price">
                                <span> </span>
                                <span className="product--unitPrice">{product.p.unitPrice.toLocaleString()} VND
                                    <span className="product--currentPrice"></span>
                                </span>
                            </div>

                        </div>
                    </li>
                ))}
            </ul>
            <Pagination />
        </>

    )
}