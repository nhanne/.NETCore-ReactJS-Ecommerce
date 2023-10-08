import React from 'react'

function Toast({ state }) {
    return (
        <div id="toast" className={`${state ? 'success' : 'failed'}`}>
            <div className={`toast ${state ? 'toast--success' : 'toast--failed'}`}>
                <div className={`toast-icon ${state ? 'toast-icon--success' : 'toast-icon--failed'}`}>
                    <i className={`fa ${state ? 'fa-check-circle' : 'fa-exclamation'}`}></i>
                </div>
                <div className="toast-body">
                    <h3 className="toast-title">{state ? 'SUCCESS' : 'FAILED'}</h3>
                    <p className="toast-msg">{state ? 'đã thêm sản phẩm vào giỏ hàng.' : 'sản phẩm hiện đang hết hàng.'} </p>
                </div>
                <div className="toast-close">
                    <i className="fa fa-times"></i>
                </div>
            </div>
        </div>
    )
}
export default Toast