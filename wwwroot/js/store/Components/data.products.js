import React from 'react'
import { Link } from 'react-router-dom'
import { FilterContext } from '../store.Context'
import Pagination from './data.pagination.js'

function Products() {
    const context = React.useContext(FilterContext);
    return (
        <>
            {context.products.length > 0 ? (
                <ul id="products" className="row">
                    {context.products.map(product => (
                        <li key={product.p.id} className='product-item'>
                            <div className="product-top">
                                <Link
                                    className="product-thumb"
                                    to={`./Product/${product.p.id}`}
                                >
                                    <img src={`/Images/sp/${product.p.picture}`} />
                                </Link>
                                <Link className="buynow"
                                    title="Order"
                                    to={`./Product/${product.p.id}`}
                                >
                                    Xem ngay
                                </Link>
                            </div>
                            <div className="product-info">
                                <a className="product-cat">{product.cateName}</a>
                                <a className="product-name">{product.p.name}</a>
                                <div className="product-price">
                                    <span> </span>
                                    <span className="product--unitPrice">{product.p.unitPrice.toLocaleString()} VND
                                        <span className="product--currentPrice"></span>
                                    </span>
                                </div>
                                {product.p.sale > 0 && (
                                    <div className="store-product__sale-off">
                                        <span className="store-product__sale-off-percent">{product.p.sale}% </span>
                                        <span className="store-product__sale-off-label">Sale</span>
                                    </div>
                                )}
                            </div>
                        </li>
                    ))}
                </ul>
            ) : (
                <span style={{ display: 'block', padding: '12px 16px' }}>Không tìm thấy sản phẩm
                    <span style={{ fontWeight: 'bold', fontSize: '2rem' }}> {context.search} </span>.
                </span>
            )}
            <Pagination />
        </>

    )
}

export default Products