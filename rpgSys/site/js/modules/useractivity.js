$(document).ready(function () {
    var html = "<div class='row'><div class='col-lg-12'><div class='panel panel-default'><div class='panel-heading'><h3 class='panel-title'>";                            
    $.getJSON('../api/UserActivity?UserId=' + getCookie("user_id"))
        .done(function (data) {
            html += "<i class='fa fa-clock-o fa-fw'></i> Ваши последние события</h3></div>";
            for (var i = 0; i < data.Activityes.length; i++) {
                html += "<a href='#' class='list-group-item'><span class='badge'>" + data.Activityes[i].Stamp + "</span><i class='fa fa-fw " + data.Activityes[i].Icon + "'></i> " + data.Activityes[i].Info + "</a>";
            }
            html += "</div></div></div>";
            $('#uaContainer').html(html);
        })
        .fail(function (jqXHR, textStatus, err) {
            html += "<i class='fa fa-exclamation-triangle fa-fw'></i> Ошибка при загрузке активности пользователя!</h3></div></div></div></div>"
            $('#uaContainer').html(html);
        });
    return false;
});