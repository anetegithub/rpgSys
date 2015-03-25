$(function () {
    var user = JSON.parse($.cookie("user"));
    $('#userAvatar').attr('src', user.Avatar);
    $('#lastTime').html("Последняя авторизация: " + user.Stamp);
    $.getJSON('../api/server')
                .done(function (data) {
                    $('#serverName').html(data.Name);
                });
    $.getJSON('../api/scenario/list')
                .done(function (data) {
                    $('#scenarioCount').html(data.length + " Сценариев");
                });
    $.getJSON('../api/module/list')
            .done(function (data) {
                $('#moduleCount').html(data.length + " Модулей");
            });
    $.getJSON('../api/hero/list')
            .done(function (data) {
                $('#heroCount').html(data.length + " Персонажей");
            });
    $.getJSON('../api/bug/list')
            .done(function (data) {
                $('#bugCount').html(data.length + " Багов");
            });
});

function showScenarInfo() {
    $.getJSON('../api/scenario/list')
                .done(function (data) {
                    var html = "";
                    html += "<div class='panel panel-back noti-box'>";
                    html += "<button type='button' class='close' style='padding-right:2%;' data-dismiss='alert' aria-hidden='true'>×</button>";
                    html += "<span class='icon-box bg-color-corporative'>";
                    html += "	<i class='fa fa-comments'></i>";
                    html += "</span>";
                    html += "<div class='text-box'>";
                    html += "	<p class='main-text'> Сценарии</p>";
                    html += "	<hr>";
                    html += "	<p class='text-muted'>";
                    html += "		<div class='panel panel-primary'>";
                    html += "			<div class='list-group'>";
                    for (var i = 0; i < data.length; i++) {
                        html += "				<span class='list-group-item'>";
                        var color = "";
                        if (data[i].Badge == "Активен")
                            color = "bg-color-green";
                        else if (data[i].Badge == "Не активен")
                            color = "bg-color-red";
                        html += "				<span class='badge " + color + "'>" + data[i].Badge + "</span>";
                        html += "				<i class='fa fa-fw fa-comments'></i>&nbsp;" + data[i].Text;
                        html += "				</span>";
                    };
                    html += "			</div>";
                    html += "		</div>";
                    html += "	</p>";
                    html += "</div>";
                    html += "</div>";
                    $('#info').html(html);
                });
};

function showModuleInfo() {
    $.getJSON('../api/module/list')
                .done(function (data) {
                    var html = "";
                    html += "<div class='panel panel-back noti-box'>";
                    html += "<button type='button' class='close' style='padding-right:2%;' data-dismiss='alert' aria-hidden='true'>×</button>";
                    html += "<span class='icon-box bg-color-blue'>";
                    html += "	<i class='fa fa-tasks'></i>";
                    html += "</span>";
                    html += "<div class='text-box'>";
                    html += "	<p class='main-text'> Модули</p>";
                    html += "	<hr>";
                    html += "	<p class='text-muted'>";
                    html += "		<div class='panel panel-info'>";
                    html += "			<div class='list-group'>";
                    for (var i = 0; i < data.length; i++) {
                        html += "				<span class='list-group-item'>";
                        var color = "";
                        if (data[i].Badge == "Подключен")
                            color = "bg-color-green";
                        else if (data[i].Badge == "Отключен")
                            color = "bg-color-red";
                        html += "				<span class='badge " + color + "'>" + data[i].Badge + "</span>";
                        html += "				<i class='fa fa-fw fa-tasks'></i>&nbsp;" + data[i].Text;
                        html += "				</span>";
                    };
                    html += "			</div>";
                    html += "		</div>";
                    html += "	</p>";
                    html += "</div>";
                    html += "</div>";
                    $('#info').html(html);
                });
};

function showHeroesInfo() {
    $.getJSON('../api/hero/list')
                .done(function (data) {
                    var html = "";
                    html += "<div class='panel panel-back noti-box'>";
                    html += "<button type='button' class='close' style='padding-right:2%;' data-dismiss='alert' aria-hidden='true'>×</button>";
                    html += "<span class='icon-box bg-color-green'>";
                    html += "	<i class='fa fa-users'></i>";
                    html += "</span>";
                    html += "<div class='text-box'>";
                    html += "	<p class='main-text'> Персонажи</p>";
                    html += "	<hr>";
                    html += "	<p class='text-muted'>";
                    html += "		<div class='panel panel-success'>";
                    html += "			<div class='list-group'>";
                    for (var i = 0; i < data.length; i++) {
                        html += "				<span class='list-group-item'>";
                        var color = "";
                        if (data[i].Badge == "Активен")
                            color = "bg-color-green";
                        else if (data[i].Badge == "Не активен")
                            color = "bg-color-red";
                        html += "				<span class='badge " + color + "'>" + data[i].Badge + "</span>";
                        html += "				<i class='fa fa-fw fa-users'></i>&nbsp;" + data[i].Text;
                        html += "				</span>";
                    };
                    html += "			</div>";
                    html += "		</div>";
                    html += "	</p>";
                    html += "</div>";
                    html += "</div>";
                    $('#info').html(html);
                });
};

function showTicketsInfo() {
    $.getJSON('../api/bug/list')
                .done(function (data) {
                    var html = "";
                    html += "<div class='panel panel-back noti-box'>";
                    html += "<button type='button' class='close' style='padding-right:2%;' data-dismiss='alert' aria-hidden='true'>×</button>";
                    html += "<span class='icon-box bg-color-red'>";
                    html += "	<i class='fa fa-gavel'></i>";
                    html += "</span>";
                    html += "<div class='text-box'>";
                    html += "	<p class='main-text'> Баги</p>";
                    html += "	<hr>";
                    html += "	<p class='text-muted'>";
                    html += "		<div class='panel panel-danger'>";
                    html += "			<div class='list-group'>";
                    for (var i = 0; i < data.length; i++) {
                        html += "				<span class='list-group-item'>";
                        var color = "";
                        if (data[i].Badge == "Закрыт")
                            color = "bg-color-green";
                        else if (data[i].Badge == "Открыт")
                            color = "bg-color-red";
                        html += "				<span class='badge " + color + "'>" + data[i].Badge + "</span>";
                        html += "				<i class='fa fa-fw fa-gavel'></i>&nbsp;" + data[i].Text;
                        html += "				</span>";
                    };
                    html += "			</div>";
                    html += "		</div>";
                    html += "	</p>";
                    html += "</div>";
                    html += "</div>";
                    $('#info').html(html);
                });
};