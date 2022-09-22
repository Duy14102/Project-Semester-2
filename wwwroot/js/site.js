/*   ProductDetail Quantity   */

function increaseValue() {
    var value = parseInt(document.getElementById('number').value, 10);
    value = isNaN(value) ? 0 : value;
    value++;
    document.getElementById('number').value = value;
}

function decreaseValue() {
    var value = parseInt(document.getElementById('number').value, 10);
    value = isNaN(value) ? 0 : value;
    value < 1 ? value = 1 : '';
    value--;
    document.getElementById('number').value = value;
}

//Category Chosen
function myFunction(element) {
    document.getElementById("Category12").value = element;
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