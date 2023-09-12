$(document).ready(function () {
    // About slide
    document.getElementById('next').onclick = function () {
        let lists = document.querySelectorAll('.item');
        document.getElementById('slide').appendChild(lists[0]);
    }
    document.getElementById('prev').onclick = function () {
        let lists = document.querySelectorAll('.item');
        document.getElementById('slide').prepend(lists[lists.length - 1]);
    }
    $('.cn_num').each(function () {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 4000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });


    // Model
    var imgSliders = document.querySelectorAll('.release__thumb li img');
    var colors = ['rgb(101 104 99)', '#968f8f', 'rgb(132 180 202)'];
    for (var i = 0; i < imgSliders.length; ++i) {
        (function (index) { // Sử dụng closure để lưu trữ giá trị của i
            imgSliders[index].onclick = function (e) {
                imgSlider(e.target.src);
                changeCircle(colors[index]);
            }
        })(i); // Truyền giá trị của i vào hàm closure
    }

    function imgSlider(anything) {
        document.querySelector('.fashion').src = anything;
    }
    function changeCircle(color) {
        const circle = document.querySelector('.release__circle');
        circle.style.background = color;
    }

    // Send mail
    $('#sendmail').click(function () {
        if (Validate()) {
            var name = $('#name').val();
            var email = $('#email').val();
            var messenger = $('#messenger').val();
            $.ajax({
                url: '/Home/Contact',
                type: 'POST',
                data: {
                    name: name,
                    email: email,
                    messenger: messenger
                },
                success: function (res) {
                    alert("Đã gửi yêu cầu thành công");
                    resetInput();
                    $('#content').html(res.content);
                }
            });
        }
    });
    function Validate() {
        var check = false;
        debugger;
        var name = $('#name').val();
        var email = $('#email').val();
        var messenger = $('#messenger').val();
        if (name === '') {
            $('#name').next().html('Bạn chưa nhập họ và tên');
            check = false;
        }
        else {
            $('#name').next().html('');
            check = true;
        }
        if (email === '') {
            $('#email').next().html('Bạn chưa nhập email liên lạc');
            check = false;
        }
        else {
            $('#email').next().html('');
            check = true;
        }
        if (messenger === '') {
            $('#messenger').next().html('Bạn chưa nhập nội dung');
            check = false;
        }
        else {
            $('#messenger').next().html('');
            check = true;
        }
        return check;
    }
    function resetInput() {
        $('#name').val('');
        $('#email').val('');
        $('#messenger').val('');
    }
})