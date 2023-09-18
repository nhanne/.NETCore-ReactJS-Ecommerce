$(document).ready(function () {
    loadData(1);
    loadCategories();
    loadSearch();

    $('body').on('click', '.pagination a', function (e) {
        e.preventDefault();
        var page = $(this).data('page');
        loadData(page);
    });


})


function loadData(page) {
    const _search = localStorage.getItem('search');
    const _category = localStorage.getItem('cate');
    const _sort = localStorage.getItem('sortName');

    $.ajax({
        url: '/Home/getData',
        type: 'GET',
        data: {
            search: _search,
            sort: _sort,
            category: _category,
            page: page
        },
        success: function (res) {
            var items = res.products;
            var html = `<ul id="products" class="row">`;
            if (items.length > 0) {
                for (let i = 0; i < items.length; ++i) {
                    html += ` <li class='product-item'>
                             <div class='product-top'>
                                <a class='product-thumb' onclick='getProduct(${items[i].p.id})'>
                                    <img class='product-picture' src='/Images/sp/${items[i].p.picture}'>
                                </a>
                                <a class='buynow' href='product/${items[i].p.id}'>
                                    Xem ngay
                                </a>
                            </div>
                            <div class='product-info'>
                                <a class='product-cat'>${items[i].cateName}</a>
                                <a class='product-name'>${items[i].p.name}</a>

                                <div class='product-price'>
                                    ${items[i].p.sale > 0 ? `
                                    <span class='product--costPrice'>${items[i].p.costPrice.toLocaleString()}
                                          <span class='product--currentPrice'>₫</span>
                                    </span> ` : `<span></span>`
                        }
                                    <span class='product--unitPrice'>${items[i].p.unitPrice.toLocaleString()}
                                          <span class='product--currentPrice'>₫</span>
                                    </span>
                                </div>
                               ${items[i].p.sale > 0 ? `
                                <div class='store-product__sale-off'>
                                    <span class='store-product__sale-off-percent'>${items[i].p.sale}%</span>
                                    <span class='store-product__sale-off-label'>sale</span>
                                </div>
                                ` : ''}
                            </div>
                        </li>`;
                }
            }
            else {
                html += `
                <span style="display:block;padding: 12px 16px">Không tìm thấy sản phẩm 
                    <span style="font-weight:bold; font-size: 2rem">${$('#search-box').val()}</span>.
                </span> `
            }
            html += `
                </ul>
                <div id="pagination" class="MenuTrang pagination-container"></div>
            `
            $('#release').html(html);
            renderPagination(res.totalPages, res.currentPage);
            homeFilterButton(res.totalPages, res.currentPage);
        },
        error: function (xhr, status, error) {
            console.error('Lỗi:', error);
        }
    });
}
//Tìm kiếm
function loadSearch() {
    $('body').on('input', '#search-box', function () {
        var search = $('#search-box').val();
        localStorage.setItem('search', search);
        loadData();
    });
    $('body').on('click', '#btnSearch', function () {
        loadData();
    });
}
//Sắp xếp theo danh mục, filter
function loadCategories() {
    sortSelect();
    $.ajax({
        url: '/Home/getCategories',
        type: 'GET',

        success: function (res) {
            if (res.totalItems > 0) {
                var cate = res.categories;
                var html = '';
                html += ` 
                          <a href="javascript:void(0)" class="closebtn" onclick="closeNav()">×</a>  
                          <li class='category-item'>
                            <a  onclick='sortCate("")' class='category-item__link'>New Stuff</a>
                          </li>`
                for (let i = 0; i < res.totalItems; ++i) {
                    html += `
                     
                      <li class='category-item'>
                            <a onclick='sortCate("${cate[i].name}")' class='category-item__link'>${cate[i].name}</a>
                      </li>
                    `
                }
                $('#mySidebar').html(html);
            }
        }
    });
}

function sortCate(nameCate) {
    localStorage.setItem('cate', nameCate);
    const cate = localStorage.getItem('cate');
    if (cate == '') {
        localStorage.removeItem('cate');
    }
    document.querySelectorAll('.myactionlink')[2].innerText = cate;
    $('#search-box').val("")
    localStorage.removeItem('search');
    loadData();
}

function sortSelect() {
    var select = document.querySelectorAll('.select-input__link');
    for (var i = 0; i < select.length; ++i) {
        select[i].onclick = function (e) {
            localStorage.setItem('sortName', e.target.innerText);
            document.querySelector('.select-input__label').innerText = localStorage.getItem('sortName');
            loadData();
        }
    }

}


// Phân trang

function renderPagination(totalPages, currentPage) {
    var paginationHtml = '<ul class="pagination">';
    if (totalPages > 1) {
        for (var i = 1; i <= totalPages; ++i) {
            paginationHtml += `<li class="page-item">
                                <a class="page-link ${i === currentPage ? 'active' : ''}" data-page="${i}">${i}</a>
                               </li>`;
        }
    }
    paginationHtml += '</ul>';

    return $('#pagination').html(paginationHtml);
}


function homeFilterButton(totalPages, currentPage) {
    document.querySelector('.home-filter__page-current').innerText = currentPage + '/' + totalPages;
    // Left
    var pagePrev = parseInt(currentPage) - 1;
    var btnPrev = document.querySelector('.home-filter__page-btn--left');
    if (currentPage == 1) {
        btnPrev.classList.add('home-filter__page-btn--disabled');
    }
    else {
        btnPrev.classList.remove('home-filter__page-btn--disabled');
        btnPrev.onclick = function (e) {
            e.preventDefault();
            loadData(pagePrev);
        }
    }
    // Right
    var pageNext = parseInt(currentPage) + 1;
    var btnNext = document.querySelector('.home-filter__page-btn--right');
    if (pageNext > totalPages) {
        btnNext.classList.add('home-filter__page-btn--disabled');
    }
    else {
        btnNext.classList.remove('home-filter__page-btn--disabled');
        btnNext.onclick = function (e) {
            e.preventDefault();
            loadData(pageNext);
        }
    }
}

