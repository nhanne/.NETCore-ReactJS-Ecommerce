function getProduct(Id) {
    fetch(`/Home/Product/${Id}`)
        .then(function (reponse) {
            return reponse.json();
            //JSON.parse: JSON => JavaSripts
        })
        .then(function (stocks) {
            var html = "";
            html += `
                 <div class="grid__row detail">
                    <div class="detail__picture">
                        <img src="/Images/sp/${stocks.product[0].image}" alt="product">
                    </div>
                    <div class="grid__column-6 detail__info">
                        <div class="detail__info-heading">
                            <span class="home-filter__page-num">
                                <span class="myactionlink">Home</span>/
                                <a href="/Home/Store" class="myactionlink">Store</a>/ ${stocks.product[0].category}
                            </span>
                        </div>
                         <h1 class="detail__info-name">${stocks.product[0].name}</h1>
                        <div class="detail__info-price">
                            <span class="detail__info-price--unit">
                                ${stocks.product[0].unitPrice.toLocaleString()}
                                <span class="detail__info-price--symbol">đ</span>
                            </span>
                        </div>
                        <div class="detail__info-size">
                            <span class="detail__info-size-label">Size</span>
            `;
            for (var i = 0; i < stocks.sizes.length; i++) {
                html += `   <label>
                                ${stocks.sizes[i].name}
                                <input type="radio" name="Size" value="${stocks.sizes[i].id}" 
                                id="${stocks.sizes[i].id}" onchange="handleQty()" />
                            </label>
            `};
            html += `  </div>
                           <div class="detail__info-color">
                                <span class="detail__info-color-label">Color</span>
            `;
            for (var i = 0; i < stocks.colors.length; i++) {
                html += `   <label>
                                ${stocks.colors[i].name}
                                <input type="radio" name="Size" value="${stocks.colors[i].id}" 
                                id="${stocks.colors[i].id}" onchange="handleQty()" />
                            </label>
            `};
            html += `  </div>
                            <span id="qtyinStock"><span id="qty"></span> </span>
                            <button class="btn detail__info-button-addtoCart" type="submit" onclick="addToCart()">
                                Thêm vào giỏ hàng
                            </button>
                    </div>
             </div>
            `
            $('#release').html(html);
            loadActive();

        })

        .catch(function (err) {
            console.log(err);
        })
}
function loadActive() {
    $(".detail__info-size label").on('click', function () {
        $(".detail__info-size label").removeClass('active');
        $(this).addClass('active');
    });
    $(".detail__info-color label").on('click', function () {
        $(".detail__info-color label").removeClass('active');
        $(this).addClass('active');
    });
}