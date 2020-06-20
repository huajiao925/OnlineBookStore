// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $("#search").keyup(function (e) {
        e.preventDefault();
        if (e.keyCode === 13) {
            var textEnter = $("#search").val();
            console.log(textEnter);
            $(".product-info").each(function () {
                var id = $(this).find("h4").text();
                if (id.toLowerCase().indexOf(textEnter.toLowerCase()) < 0) {
                    $(this).hide();
                } else {
                    $(this).show();
                }
            })
        }
    });
    $("#search").keydown(function (e) {
        if (e.keyCode === 8) {
            location.reload();
        }
    });
});