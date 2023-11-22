var citis = document.getElementById("city");
var districts = document.getElementById("district");
var wards = document.getElementById("ward");
var selectedCity, selectedDistrict, selectedWard, street, address;
var Parameter = {
    url: "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json",
    method: "GET",
    responseType: "application/json",
};
var promise = axios(Parameter);
promise.then(function (result) {
    renderCity(result.data);
});

function renderCity(data) {
    for (const x of data) {
        citis.options[citis.options.length] = new Option(x.Name, x.Id);
    }
    citis.onchange = function () {

        district.length = 1;
        ward.length = 1;
        if (this.value != "") {
            const result = data.filter(n => n.Id === this.value);
            for (const k of result[0].Districts) {
                district.options[district.options.length] = new Option(k.Name, k.Id);
            }

        }
    };
    district.onchange = function () {
        ward.length = 1;
        const dataCity = data.filter((n) => n.Id === citis.value);
        if (this.value != "") {
            const dataWards = dataCity[0].Districts.filter(n => n.Id === this.value)[0].Wards;

            for (const w of dataWards) {
                wards.options[wards.options.length] = new Option(w.Name, w.Id);
            }
        }

    };
    ward.onchange = function () {
    }
}
function CheckOut() {
    $('#address').val("");
    var streets = document.getElementById("street").value;
    var selectedCity = citis.options[citis.selectedIndex].textContent;
    var selectedDistrict = districts.options[districts.selectedIndex].textContent;
    var selectedWard = wards.options[wards.selectedIndex].textContent;
    var addressText = streets + " " + selectedWard + ", " + selectedDistrict + ", " + selectedCity;
    $('#address').val(addressText);

    var _id = $('#Id').val();
    var _name = $('#full_name').val();
    var _email = $('#email').val();
    var _phone = $('#phone').val();
    var _address = $('#address').val();
    var _paymentId = $('#payment').val();
    var _totalPrice = $('#totalPrice').text();
    var _note = $('#note').val();
    var promoCode = $('#promoCode').val();

    var userModel = {
        Id: _id,
        Name: _name,
        Email: _email,
        PhoneNumber: _phone,
        Address: _address
    }
    var orderModel = {
        TotalPrice: _totalPrice,
        PaymentId: _paymentId,
        Note: _note,
        Address: _address,
        PromoCode: promoCode
    }
    
    if (Validate()) {
        $.ajax({
            url: '/Cart/CheckOut',
            type: 'POST',
            data: {
                userModel,
                orderModel
            },
            success: function (response) {
                window.location.href = response.redirectToUrl;
            },
            error: function () {
                alert("Error: " + textStatus + " " + errorThrown + " " + httpRequest);
            }
        });
    }

}
function Validate() {
    var check = false;
    var name = $('#full_name').val();

    if (name === '') {
        $('#full_name').next().html('Bạn chưa nhập họ và tên');
        check = false;
    }
    else {
        $('#full_name').next().html('');
        check = true;
    }

    var email = $('#email').val();
    if (email === '') {
        $('#email').next().html('Bạn chưa email');
        check = false;
    }
    else {
        $('#email').next().html('');
        check = true;
    }

    var phone = $('#phone').val();
    if (phone === '') {
        $('#phone').next().html('Bạn chưa nhập số điện thoại');
        check = false;
    }
    else {
        $('#phone').next().html('');
        check = true;
    }
    var street = $('#street').val();
    if (street === '') {
        $('#street').next().html('Bạn chưa nhập địa chỉ');
        check = false;
    }
    else {
        $('#street').next().html('');
        check = true;
    }
    var district = $('#phone').val();
    if (district === '') {
        $('#district').next().html('Bạn chưa nhập quận');
        check = false;
    }
    else {
        $('#district').next().html('');
        check = true;
    }
    var city = $('#city').val();
    if (city === '') {
        $('#city').next().html('Bạn chưa nhập thành phố');
        check = false;
    }
    else {
        $('#city').next().html('');
        check = true;
    }
    var ward = $('#ward').val();
    if (ward === '') {
        $('#ward').next().html('Bạn chưa nhập phường/ xã');
        check = false;
    }
    else {
        $('#ward').next().html('');
        check = true;
    }
    return check;
}