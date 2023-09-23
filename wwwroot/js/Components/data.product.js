import React from 'react'
import clsx from 'clsx'
import styles from '/wwwroot/css/module/product.module.css'

function Product({ Id }) {

    const [product, setProduct] = React.useState();
    React.useEffect(() => {
        fetch(`/Home/Product?Id=33`)
            .then(res => res.json())
            .then(reponse => {
                setProduct(reponse.product)
            });
    }, [])
    console.log(product)

    return (
        <div className={clsx("grid__row", styles.detail)}>
            <div className={clsx("grid__column-6", styles.detail__picture)}>
                <img src="/Images/sp/1.jpg" alt="product" />
            </div>
            <div className={clsx("grid__column-6", styles.detail__info)}>
                <div className={styles.detail__info_heading}>
                    <span className={styles.home_filter__page_num}>
                        <a href="/" className={styles.myactionlink}>Home</a>/
                        <a href="/" className={styles.myactionlink}>Store</a>/ @Model.Name
                    </span>
                    <h1 className={styles.detail__info_name}>
                        @Model.Name
                    </h1>
                    <div className={styles.detail__info_price}>
                        <span className={styles.detail__info_price__cost}>
                            @string.Format
                            <span className={styles.detail__info_price__symbol}>đ</span>
                        </span>
                        <span className={styles.detail__info_price__unit}>
                            @string.Format
                            <span className={styles.detail__info_price__symbol}>đ</span>
                        </span>
                    </div>
                </div>
                <div className={styles.detail__info_size}>
                    <span className={styles.detail__info_size_label}>Size</span>

                </div>
                <div className={styles.detail__info_color}>
                    <span className={styles.detail__info_color_label}>Color</span>

                </div>
                <button className={clsx(styles.btn, styles.detail__info_button_addtoCart)}>
                    Thêm vào giỏ hàng
                </button>
            </div>
        </div>
    )
}
export default React.memo(Product)