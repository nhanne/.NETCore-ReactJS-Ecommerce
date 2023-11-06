import React from 'react'
import axios from 'axios';
import { useParams, Link } from 'react-router-dom'
import clsx from 'clsx'
import styles from '/wwwroot/css/module/product.module.css'
import Toast from './toast'

function Product() {

    const { Id } = useParams();
    // Color & Size
    const sizes = React.useRef();
    const colors = React.useRef();
    const [checkedSize, setCheckedSize] = React.useState();
    const [checkedColor, setCheckedColor] = React.useState();
    // Quantity & Product
    const [quantity, setQuantity] = React.useState();
    const [product, setProduct] = React.useState();

    const [showToast, setShowToast] = React.useState(false);

    React.useEffect(() => {
        fetch(`/Home/Product?Id=${Id}`)
            .then(res => res.json())
            .then(response => {
                sizes.current = response.sizes;
                colors.current = response.colors;
                setProduct(response.product[0]);
            });
    }, [Id])
    const handleAddToCart = () => {
        if (checkedSize && checkedColor) {
            axios.post('/Cart/AddToCart', null, {
                params: {
                    productId: parseInt(Id),
                    colorId: checkedColor,
                    sizeId: checkedSize,
                }
            })
                .then((response) => {
                    (response.data) ? setShowToast(true) : setShowToast(false);
                    setTimeout(() => {
                        setShowToast(false);
                    }, 3000);
                })
        }
        else {
            alert("Vui lòng chọn phân loại sản phẩm")
        }
    }
    if (checkedSize && checkedColor) {
        axios.get('/Home/getStock', {
            params: {
                productId: Id,
                colorId: checkedColor,
                sizeId: checkedSize,
            }
        })
            .then((response) => {
                setQuantity(response.data.quantity)
            })
    }

    return (
        <div className={clsx("grid__row", styles.detail)}>
            {product !== undefined ? (
                <>
                    <div className={clsx("grid__column-6", styles.detail__picture)}>
                        <img src={`/Images/sp/${product.image}`} alt="product" />
                    </div>
                    <div className={clsx("grid__column-6", styles.detail__info)}>
                        <div className={styles.detail__info_heading}>
                            <span className={styles.home_filter__page_num}>
                                <Link to="/Home/Store" className={styles.myactionlink}>Store</Link>/ {product.category}
                            </span>
                            <h1 className={styles.detail__info_name}>
                                {product.name}
                            </h1>
                            <div className={styles.detail__info_price}>
                                {product.sale > 0 && (
                                    <span className={styles.detail__info_price__cost}>
                                        {product.costPrice.toLocaleString()}
                                        <span className={styles.detail__info_price__symbol}>đ</span>
                                    </span>
                                )}
                                <span className={styles.detail__info_price__unit}>
                                    {product.unitPrice.toLocaleString()}
                                    <span className={styles.detail__info_price__symbol}>đ</span>
                                </span>
                            </div>
                        </div>
                        <div className={styles.detail__info_size}>
                            <span className={styles.detail__info_size_label}>Size</span>
                            {sizes.current.map(size => (
                                <label key={size.id} className={checkedSize === size.id ? styles.active : ''}>
                                    {size.name}
                                    <input
                                        checked={checkedSize === size.id}
                                        onChange={() => setCheckedSize(size.id)}
                                        type="radio"
                                        required />
                                </label>
                            ))}
                        </div>
                        <div className={styles.detail__info_color}>
                            <span className={styles.detail__info_color_label}>Color</span>
                            {colors.current.map(color => (
                                <label key={color.id} className={checkedColor === color.id ? styles.active : ''}>
                                    {color.name}
                                    <input
                                        checked={checkedColor === color.id}
                                        onChange={() => setCheckedColor(color.id)}
                                        type="radio"
                                        required />
                                </label>
                            ))}
                        </div>
                        {checkedSize && checkedColor && (
                            <span id="qtyinStock">
                                <span id="qtyinStock">Có sẵn {quantity} sản phẩm
                                </span>
                            </span>
                        )}

                        <button
                            className={clsx(styles.btn, styles.detail__info_button_addtoCart)}
                            onClick={() => handleAddToCart()}
                        >
                            Thêm vào giỏ hàng
                        </button>
                    </div>
                    {showToast && <Toast state={showToast} />}
                </>
            ) : (
                <span className="validate">Sản phẩm hiện đang hết hàng, vui lòng quay lại sau.</span>
            )
            }
        </div >
    )
}
export default React.memo(Product)