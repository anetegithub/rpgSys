$(document).ready(function () {
    if ($.cookie("user") == null || $.cookie("user") == "null") {
        window.location.replace("login?error=0");
    }
});

function LogOff() {
    $.cookie('user', ' ', { expires: -1, path: '/site/' });
    window.location.replace("login");
}