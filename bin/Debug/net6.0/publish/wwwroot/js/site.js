/*   ProductDetail Quantity   */

function increaseValue() {
    var value = parseInt(document.getElementById('number45').value, 10);
    value = isNaN(value) ? 1 : value;
    value++;
    value > 99 ? value = 99 : '';
    document.getElementById('number45').value = value;
}

function decreaseValue() {
    var value = parseInt(document.getElementById('number45').value, 10);
    value = isNaN(value) ? 1 : value;
    value--;
    value < 1 ? value = 1 : '';
    document.getElementById('number45').value = value;
}

//Category Chosen
function myFunction(element) {
    document.getElementById("Category12").value = element;
}

function myFunctionEx(element) {
    document.getElementById("Category13").value = element;
}

$(function () {
    $('#f_search input').keydown(function (e) {

        var searchTerm = $("#search4This").val();

        if (searchTerm == "" && Event.keyCode === 13) {
            e.preventDefault();
            return false;
        }
    });
})


//Scroll to top button
let mybutton = document.getElementById("btn-back-to-top");

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () {
    scrollFunction();
};

function scrollFunction() {
    if (
        document.body.scrollTop > 200 ||
        document.documentElement.scrollTop > 200
    ) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}
// When the user clicks on the button, scroll to the top of the document
mybutton.addEventListener("click", backToTop);

function backToTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

const pageAccessedByReload = (
    (PerformanceNavigationTiming && PerformanceNavigationTiming.TYPE === 1) ||
    window.performance
        .getEntriesByType('navigation')
        .map((nav) => nav.type)
        .includes('reload')
);

document.addEventListener("DOMContentLoaded", function () {
    var lazyloadImages = document.querySelectorAll("img.lazy");
    var lazyloadThrottleTimeout;

    function lazyload() {
        if (lazyloadThrottleTimeout) {
            clearTimeout(lazyloadThrottleTimeout);
        }

        lazyloadThrottleTimeout = setTimeout(function () {
            lazyloadImages.forEach(function (img) {
                if ($(window).trigger('scroll')) {
                    img.src = img.dataset.src;
                    img.classList.remove('lazy');
                }
                if (pageAccessedByReload == true) {
                    img.src = img.dataset.src;
                    img.classList.remove('lazy');
                }
            });
            if (lazyloadImages.length == 0) {
                document.removeEventListener("scroll", lazyload);
                window.removeEventListener("resize", lazyload);
                window.removeEventListener("orientationChange", lazyload);
            }
        }, 20);
    }

    document.addEventListener("scroll", lazyload);
    window.addEventListener("resize", lazyload);
    window.addEventListener("orientationChange", lazyload);
});

function hamDropdown() {
    document.querySelector(".noidung_dropdown").classList.toggle("hienThi");
}

setTimeout(function() { $("#cartpopup2").fadeOut(); }, 2500);