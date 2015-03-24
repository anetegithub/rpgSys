﻿$(function () {
    if (getUrlVars()["error"] == "0") {
        fail('Вы не авторизованы!');
    }
})

$('#log-btn').click(function () {
    var log = $('#log').val();
    var psw = $('#psw').val();
    $.getJSON('../api/users?name=' + log + "&psw=" + psw)
        .done(function (data) {
            if (data.Password == psw) {
                localStorage.setItem('Hero',JSON.stringify(data.Hero));
                data.Hero = null;
                localStorage.setItem('Game',JSON.stringify(data.Game));
                data.Game = null;
                $.cookie('user', JSON.stringify(data), { path: '/site/' });
                $.post('../api/users/update', { '': data.Id.toString() }).done(function () {
                    window.location.replace('profile');
                });                
            }
        }).fail(function (jqXHR, textStatus, err) {
            fail('Такого пользователя не существует!');
        });
});

function fail(text) {
    $('#msg').css('color', 'red');
    $('#msg').html("( " + text + " )");
}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}