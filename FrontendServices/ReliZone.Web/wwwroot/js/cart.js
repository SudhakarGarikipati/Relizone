function addToCart(itemid, unitprice, quatity) {
    $.ajax({
        type: "Get",
        contentType: "application/json; charset=utf-8",
        url: '/Cart/AddToCart/' + itemid + "/" + unitprice + "/" + quatity,
        success: function (response) {
            if (response != undefined && response.status == "success") {
                // Update the cart count or display a success message
                // Optionally, you can update the cart count displayed on the page
                var counter = response.count;
                $("#cartCounter").text();
            }
        },
        error: function (result) { }
    });
}

function deleteItem(id, cartId) {
    if (id > 0) {
        $.ajax({
            type: "Get",
            url: '/Cart/DeleteItem/' + cartId + "/" + id,
            success: function (response) {
                if (response != undefined && response > 0) {
                    location.reload(); // Reload the page to reflect changes)
                } 
            }
        });
    }
}

function updateQuantity(id, currentQuantity, quantity, cartId) {
    if ((currentQuantity >= 1 && quantity == 1) || (currentQuantity > 1 && quantity == -1)) {
        $.ajax({
            type: 'GET',
            url: '/Cart/UpdateQuantity/' + cartId + "/" + id + "/" + quantity,            
            success: function (response) {
                if (response > 0) {
                    location.reload();
                }
            }
        });
    }
}

$(function () {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/Cart/GetCartCount',
        success: function (data) {
            $("#cartCounter").text(data);
        },
        error: function (result) {
        },
    });
});