$(document).ready(function () {
    $.getJSON('../api/server')
        .done(function (data) {
            if (data.ServerMessage != "") {
                $('#ServerMessage').html("<ol class='breadcrumb'> <li class='active'><i class='fa fa-cogs'></i>&nbsp;" + data.ServerMessage + "</li></ol>");
            }
            if (data.MessageOfTheDay != "") {
                var msg=data.MessageOfTheDay.split('~');
                $('#MessageOfTheDay').html("<div class='alert alert-" + msg[0] + " alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i>&nbsp;" + msg[1] + "</div>");
            }
            $('#ServerModule').html(data.Modules);
            $('#ServerScenario').html(data.Scenarios);
            $('#ServerItem').html(data.Tickets);
            $('#ServerCharacter').html(data.Characters);
        })
        .fail(function (jqXHR, textStatus, err) {
            if (data.MessageOfTheDay != undefined) {
                var msg = data.MessageOfTheDay.split('~');
                $('#MessageOfTheDay').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i>Ошибка на сервере!</div>");
            }
        });
});