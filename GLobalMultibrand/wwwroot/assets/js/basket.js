//$(document).on('click', '#add-product-to-cart', function () {

//    var productId = $(this).parent().children().val();
//    console.log(productId)
//    $.ajax({
//        type: "POST",
//        url: '/Basket/AddToBasket',
//        data: { productId: productId },
//        success: function () {
//        },
//    });

//})


//$(document).on('change', '#product-quality-from-icon', function () {
//    var productId = $(this).parent().children().val();
//    var count = $(this).val();

//    $.ajax({
//        type: "POST",
//        url: '/Basket/ChangeProductQuality',
//        data: { productId: productId, count: count },
//        success: function () {
//        },
//    });


//});
//document.addEventListener("DOMContentLoaded", function () {
//});

//$(document).ready(function () {
//    $(document).on('click', '.add-product-to-cart', function (e) {
//        console.log($(this));
//    })
//});

 const addToCartBtns = document.querySelectorAll(".add-product-to-cart");
console.log(addToCartBtns);

function addToCart(ev) {
    console.log("is working")
    const productId = ev.target.getAttribute("data-id");

    fetch(`/basket/AddToBasket?productId=${productId}`).then(response => {
        console.log(response)
    })

}
addToCartBtns.forEach(addToCartBtn => addToCartBtn.addEventListener('click',addToCart))

