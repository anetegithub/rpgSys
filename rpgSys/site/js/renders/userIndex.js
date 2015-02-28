$(document).ready(function () {
    $.post('../api/users/profile', { '': getCookie('user_id') }).done(function (data) {
        $('#rendPr').html('Пользователь: ' + getCookie('pl_name') + "&nbsp;&nbsp;<i class='fa fa-location-arrow'></i>");        
        if (data[0] == '0') {
            $('#rendCh').html(" Создайте своего уникального персонажа!&nbsp;&nbsp;<i class='fa fa-location-arrow'></i>");
        } else {
            $('#rendCh').html(" Персонаж :" + data[0] + "&nbsp;&nbsp;<i class='fa fa-location-arrow'></i>");
        }
        if (data[1] == '0') {
            $('#rendGm').html(" Нажмите что бы начать новый сценарий&nbsp;&nbsp;<i class='fa fa-location-arrow'></i>");
        } else {
            $('#rendGm').html(" Сценарий '" + data[1] + "'&nbsp;&nbsp;<i class='fa fa-location-arrow'></i>");
        }
        $('#rendSc').html(" Новый сценарий&nbsp;&nbsp;<i class='fa fa-location-arrow'></i>");
        $('#rendSr').html(" Сервер&nbsp;&nbsp;<i class='fa fa-location-arrow'></i>");
    });
});