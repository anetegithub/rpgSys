$(document).ready(function () {
    $(".tile").map(function () {        
        $(this).html("<div class='col-lg-" + $(this).attr('cols') + " " + $(this).attr('color') + " " +
            " text-center tile-container' onclick=\"window.location.replace('" + $(this).attr('href') +
            "');\"'><div class='text-center tile-container-icon'><i class='fa " + $(this).attr('icon') +
            " fa-5x'></i></div><div class='text-left' id='" + $(this).attr('pushId') + "' > " + $(this).attr('text') + "</div></div>");
    });
});