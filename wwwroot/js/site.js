/*   ProductDetail Quantity   */

function increaseValue() {
    var value = parseInt(document.getElementById('number45').value, 10);
    value = isNaN(value) ? 1 : value;
    value > 99 ? value = 99 : '';
    value++;
    document.getElementById('number45').value = value;
}

function decreaseValue() {
    var value = parseInt(document.getElementById('number45').value, 10);
    value = isNaN(value) ? 1 : value;
    value < 1 ? value = 1 : '';
    value--;
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