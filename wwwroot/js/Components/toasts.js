import React from 'react'

export default function Toast(){
    const [state, setState] = React.useState();

    return (
        <div id="toast" class="success">
            <div class="toast toast--success">
                <div class="toast-icon toast-icon--success">
                    <i class="fa fa-check-circle"></i>
                </div>
                <div class="toast-body">
                    <h3 class="toast-title">SUCCESS</h3>
                    <p class="toast-msg">đã thêm sản phẩm vào giỏ hàng.</p>
                </div>
                <div class="toast-close">
                    <i class="fa fa-times"></i>
                </div>
            </div>
        </div>
    )
}