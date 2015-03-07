$(function () {

    var user = JSON.parse($.cookie("user"));
    $('#userAvatar').attr('src', user.Avatar);
    $('#lastTime').html("Последняя авторизация: " + user.StampToString);

    if (JSON.parse($.cookie('user')).HeroId != 0) {
        $('#old').css('display', 'block');

        $.getJSON('../api/hero?HeroId=' + user.HeroId)
                .done(function (data) {
                    var Hero = data;
                    $('#oldName2').html(Hero.Name);
                });
    } else {
        $('#new').css('display', 'block');
    }
});