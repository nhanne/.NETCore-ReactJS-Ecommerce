
// ===========================  navbar-collapse =========================== 
var navbar = document.getElementById('navbar');
var mobileMenu = document.getElementById('mobile-menu');
var navbarHeight = navbar.clientHeight;
mobileMenu.onclick = function() {
    var isClosed = navbar.clientHeight === navbarHeight;
    if (isClosed) {
        navbar.style.height = 'auto';
        $('#navbar').addClass('sticky');
    } else {
        navbar.style.height = null;
        $('#navbar').removeClass('sticky');
    }
}
// ===========================  sidebar-collapse =========================== 
function openNav() {
    document.getElementById("mySidebar").style.width = "50%";
    document.getElementById("main").style.marginLeft = "50%";
}

function closeNav() {
    document.getElementById("mySidebar").style.width = "0";
    document.getElementById("main").style.marginLeft = "0";
}
// ===========================  navbar-search =========================== 

function navSearch() {
    document.getElementById("dropdown-search").classList.toggle("show");
}
window.onscroll = () => {
    document.getElementById("dropdown-search").classList.remove('show');
}


$(document).ready(function () {
    var prevScrollpos = window.pageYOffset;
    window.onscroll = function () {
        var currentScrollPos = window.pageYOffset;
        if (prevScrollpos > currentScrollPos) {
            document.getElementById("navbar").style.top = "0";
        } else {
            document.getElementById("navbar").style.top = "-50px";
        }
        prevScrollpos = currentScrollPos;
    }
    $(window).scroll(function() {
        if ($(this).scrollTop()) {
            $('#navbar').addClass('sticky');
            $('#backtop').fadeIn();

        } else {
            $('#navbar').removeClass('sticky');
            $('#backtop').fadeOut();
        }
    });
    $('#backtop').click(function() {
        $('html,body').animate({
            scrollTop: 0
        }, 500);
    });
   
   
});
// ===========================  nav-link ===========================
function about() {
    window.location = '/Home#about';
}
function contact() {
    window.location = '/Home#contact';
}

