$(function () {
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
                setCookie("pl_name", data.Login, { path: '/site/' });
                setCookie("user_id", data.Id, { path: '/site/' });
                setCookie("hero_id", data.HeroId, { path: '/site/' });
                setCookie("auth_key", data.Auth, { path: '/site/' });
                window.location.replace('profile');
            }
        }).fail(function (jqXHR, textStatus, err) {
            fail('Такого пользователя не существует!');
        });
});

function fail(text) {
    $('#msg').css('color', 'red');
    $('#msg').html("( "+text+" )");
}

// возвращает cookie с именем name, если есть, если нет, то undefined
function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
      "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

// устанавливает cookie c именем name и значением value
// options - объект с свойствами cookie (expires, path, domain, secure)
function setCookie(name, value, options) {
    options = options || {};

    var expires = options.expires;

    if (typeof expires == "number" && expires) {
        var d = new Date();
        d.setTime(d.getTime() + expires * 1000);
        expires = options.expires = d;
    }
    if (expires && expires.toUTCString) {
        options.expires = expires.toUTCString();
    }

    value = encodeURIComponent(value);

    var updatedCookie = name + "=" + value;

    for (var propName in options) {
        updatedCookie += "; " + propName;
        var propValue = options[propName];
        if (propValue !== true) {
            updatedCookie += "=" + propValue;
        }
    }

    document.cookie = updatedCookie;
}

// удаляет cookie с именем name
function deleteCookie(name) {
    setCookie(name, "", { expires: -1 })
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