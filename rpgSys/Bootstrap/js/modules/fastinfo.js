var mouseX, mouseY;
$(document).mousemove(function (e) {
    mouseX = e.pageX;
    mouseY = e.pageY;
}).mouseover();


function ShowInfo(id, container) {
    id = "#" + id;
    container = "#" + container;
    $('.fastinfo-menu').empty();
    $('.fastinfo-menu').html("<li><i class='fa fa-fw fa-info'></i>Инфо</li>");
    $(".fastinfo-menu li").click(function () {
        $(container).html("<div class='row'><div class='col-lg-12'><div class='alert alert-info alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> " + $(id).attr("info-attr") + "</div></div></div>");
        $(".fastinfo-menu").hide(100);
    });


    $(".fastinfo-menu").finish().toggle(100).
   css({
       top: mouseY + "px",
       left: mouseX + "px"
   });
}

$(document).bind("mousedown", function (e) {
    if (!$(e.target).parents(".fastinfo-menu").length > 0) {
        $(".fastinfo-menu").hide(100);
    }
});

$(document).bind("contextmenu", function (event) {
    event.preventDefault();
});

function SetFastInfoItems(items) {
    var html = "<ul class='fastinfo-menu'></ul>";
    $('#fiContainer').html(html);
}

$(document).ready(function () {
    SetFastInfoItems(new Array("Инфо"));
});