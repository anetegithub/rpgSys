$(function () {
    // hub
    var chat = $.connection.generalchat;

    // create functions
    chat.client.newmsg = function (userName,userAvatar, msgStamp, msgText) {
        var msg = "";
        if ($('#msgc').val() % 2 == 0) {
            msg += "<li class='left clearfix'>";
            msg += "<span class='chat-img pull-left'>";
            msg += "<img src='" + userAvatar + "' alt='" + userName + "' height='50px' width='50px' class='img-circle'/></span>";
            msg += "<div class='chat-body'><strong>" + userName + "</strong><small class='pull-right text-muted'><i class='fa fa-clock-o fa-fw'></i>" + msgStamp.replace('T', ' ').replace(new RegExp('-', 'g'), '.') + "</small>";
            msg += "<p>" + msgText + "</p>";
            msg += "</div></li>";
        } else {
            msg += "<li class='right clearfix'>";
            msg += "<span class='chat-img pull-right'>";
            msg += "<img src='" + userAvatar + "' alt='" + userName + "' height='50px' width='50px' class='img-circle'/></span>";
            msg += "<div class='chat-body'><small class='text-muted'><i class='fa fa-clock-o fa-fw'></i>" + msgStamp.replace('T', ' ').replace(new RegExp('-', 'g'), '.') + "</small><strong class='pull-right'>" + userName + "</strong>";
            msg += "<p>" + msgText + "</p>";
            msg += "</div></li>";
        }
        $('#msgc').val(parseInt($('#msgc').val()) + 1);
        $('#chatBox').html($('#chatBox').html() + msg);
        $("#chatWrapper").animate({ scrollTop: $('#chatWrapper')[0].scrollHeight }, 250);
    };

    // start
    $.connection.hub.start().done(function () {
        $('#btn-chat').click(function () {
            if ($('#btn-input').val() != '') {
                if ($('#btn-input').val().replace(/\s/g, '').length) {
                    chat.server.send(JSON.parse($.cookie('user')).Login, JSON.parse($.cookie('user')).Avatar, $('#btn-input').val());
                    $('#btn-input').val('');
                }
            }
        });
    });

    load_chat();
    load_activity();
    render();
});

$("#btn-input").keyup(function (e) {
    if (e.keyCode == 13) {
        $('#btn-chat').click();
    }
});

function load_chat() {
    $.getJSON('../api/chat')
            .done(function (data) {
                var html = "";
                for (var i = 0; i < data.length; i++) {
                    if (i % 2 == 0) {
                        html += "<li class='left clearfix'>";
                        html += "<span class='chat-img pull-left'>";
                        html += "<img src='" + data[i].UserAvatar + "' alt='" + data[i].UserName + "' height='50px' width='50px' class='img-circle'/></span>";
                        html += "<div class='chat-body'><strong>" + data[i].UserName + "</strong><small class='pull-right text-muted'><i class='fa fa-clock-o fa-fw'></i>" + data[i].Stamp.replace('T', ' ').replace(new RegExp('-', 'g'), '.') + "</small>";
                        html += "<p>" + data[i].Text + "</p>";
                        html += "</div></li>";
                    } else {
                        html += "<li class='right clearfix'>";
                        html += "<span class='chat-img pull-right'>";
                        html += "<img src='" + data[i].UserAvatar + "' alt='" + data[i].UserName + "' height='50px' width='50px' class='img-circle'/></span>";
                        html += "<div class='chat-body'><small class='text-muted'><i class='fa fa-clock-o fa-fw'></i>" + data[i].Stamp.replace('T', ' ').replace(new RegExp('-', 'g'), '.') + "</small><strong class='pull-right'>" + data[i].UserName + "</strong>";
                        html += "<p>" + data[i].Text + "</p>";
                        html += "</div></li>";
                    }
                    $('#msgc').val(parseInt($('#msgc').val()) + 1);

                }
                $('#chatBox').html(html);
                $('#chatWrapper').scrollTop($('#chatWrapper').prop("scrollHeight"));
            });
}

function load_activity() {
    $.getJSON('../api/activity?Id=' + JSON.parse($.cookie('user')).Id)
                .done(function (data) {
                    var html = "";
                    for (var i = 0; i < data.length; i++) {
                        html += "<span class='list-group-item'>";
                        html += "<span class='badge'>" + data[i].Stamp + "</span>";
                        html += "<i class='fa fa-fw " + iconByenum(data[i].Action) + "'></i>&nbsp;";
                        html += " " + data[i].Text + "</span>";
                    }
                    $('#activityList').html(html);
                });
}

function iconByenum(activity_type)
{
    switch(activity_type)
    {
        case "0": return "fa-child";
        case "1": return "fa-trophy";
        case "2": return "fa-compass";
        case "3": return "fa-graduation-cap";
        case "4": return "fa-cogs";
    }
}

function render() {
    var user = JSON.parse($.cookie("user"));
    $('#userAvatar').attr('src', user.Avatar);
    $('#userLogin').html("Добро пожаловать, " + user.Login + "! Рады видеть вас вновь! ");
    if (user.HeroId==0) {
        $('#userHero').html('Создать');
    } else {
        $.getJSON('../api/hero?UserId=' + user.Id)
                 .done(function (data) {
                     $('#userHero').html(data.Name);
                 });
    }
    if (user.GameId == 0) {
        $('#userGame').html('Начать сценарий');
    } else {
        $('#userGame').html('GameControllerNeed');
    }
    $.getJSON('../api/server')
                .done(function (data) {
                    $('#userServer').html(data.Name);
                });
    $('#lastTime').html("Последняя авторизация: " + user.Stamp);
}