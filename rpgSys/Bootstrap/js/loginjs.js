$(function () {
    UserIdiot();
    var textfieldn = $("input[name=user]");
    var textfieldp = $("input[name=psw]");
    $('button[type="submit"]').click(function (e) {
        e.preventDefault();
        //little validation just to check username
        if (textfieldn.val() != "" && textfieldp.val()!="") {
            GetUserByName(textfieldn.val(), textfieldp.val());       
        } else {
            //remove success mesage replaced with error message
            $("#output").removeClass(' alert alert-success');
            $("#output").addClass("alert alert-danger animated fadeInUp").html("Заполните все поля!");
        }
        //console.log(textfield.val());

    });
});

function GetUserByName(name, password) {
    $.getJSON('api/users?name=' + name + "&psw=" + password)
        .done(function (data) {
            if (data.Password == password) {

                setCookie("pl_name", data.Login, { path: '/Area/' });
                setCookie("user_id", data.Id, { path: '/Area/' });
                setCookie("hero_id", data.HeroId, { path: '/Area/' });
                setCookie("auth_key", data.Auth, { path: '/Area/' });

                var textfieldn = $("input[name=user]");
                var textfieldp = $("input[name=psw]");

                //$("body").scrollTo("#output");
                $("#output").addClass("alert alert-success animated fadeInUp").html("Добро пожаловать " + "<span style='text-transform:uppercase'>" + textfieldn.val() + "</span> !");
                $("#output").removeClass(' alert-danger');
                $("input").css({
                    "height": "0",
                    "padding": "0",
                    "margin": "0",
                    "opacity": "0"
                });
                //change button text 
                $('button[type="submit"]').html("Продолжить")
                .removeClass("btn-info")
                .addClass("btn-default").click(function () {
                    window.location.replace('/Area/UserIndex.html')
                });
                $('#hMsg').css("color", "red");
                $('#hMsg').css("font-weight", "bold");
                $('#hMsg').html("Пожалуйста, не забывайте что это альфа версия продукта!");
            }
        })
        .fail(function (jqXHR, textStatus, err) {
            $("#output").removeClass(' alert alert-success');
            $("#output").addClass("alert alert-danger animated fadeInUp").html("Такого пользователя не существует!");
        });
    return false;
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

function UserIdiot() {
    if (getUrlVars()["error"] == "0") {
        $("#output").removeClass(' alert alert-success');
        $("#output").addClass("alert alert-danger animated fadeInUp").html("Вы не авторизованы!");
    }
}