import React from 'react'

export default function Products({ products }) {

    return (
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
                                <span className="product--currentPrice">ð</span>
                            </span>
                        </div>

                    </div>
                </li>
            ))}
        </ul>
    )
}