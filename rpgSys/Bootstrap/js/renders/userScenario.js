$("#prevPage").click(function () {
    setPage(parseInt($("#pageControl").val()) - 1,true);
});
$("#nextPage").click(function () {
    setPage(parseInt($("#pageControl").val()) + 1,false);
});

function setPage(page, back) {
    if (page == 1) {
        $("#prevPage").css("visibility", "hidden");
    } else if (page == 2) {
        if (!checkFirstPagevalues())
            return;
        $("#prevPage").css("visibility", "visible");
    } else if (page == 3) {
        if (!validLocationPage())
            return;
    } else if (page == 5) {
        if (!checkEventsCount())
            return;
        $("#nextPage").css("visibility", "hidden");
        $("#sendPage").css("visibility", "visible");
    }
    if (back) {
        if ($("#sendPage").css("visibility") == "visible") {
            $("#sendPage").css("visibility", "hidden");
        }
        if ($("#nextPage").css("visibility") == "hidden") {
            $("#nextPage").css("visibility", "visible");
        }
        pageP = "#page" + (parseInt(page) + 1);
        liP = "#Lipage" + (parseInt(page) + 1);
        pageN = "#page" + page;
        liN = "#Lipage" + page;

        $(liP).css("visibility", "hidden");

    } else {
        pageP = "#page" + (parseInt(page) - 1);
        liP = "#Lipage" + (parseInt(page) - 1);
        pageN = "#page" + page;
        liN = "#Lipage" + page;

        $(liN).css("visibility", "visible");
    }

    $(pageN).css("display", "block");
    $(pageP).css("display", "none");

    $(liP).removeClass("active");
    $(liN).addClass("active");

    $("#pageControl").val(page);
}

$('#addLocation').click(function () {
    if ($('#locationName').val() != "" && $('#locationInfo').val() != "" && $('#locationMaster').val() != "") {
        if ($('#locationName').val().length <= 70) {
            var id = currentIdLoc();
            var name = $('#locationName').val();
            var info = $('#locationInfo').val();
            var info_cut = $('#locationInfo').val().substr(0, 25) + "...";
            var master = $('#locationMaster').val();
            var html = "<div class='list-group-item' id='locationId" + id + "'><button type='button' class='close' onclick=\"$('#locationId" + id + "').remove();\">&times;</button><h4 class='list-group-item-heading'>" + name + "</h4> <p class='list-group-item-text'>" + info_cut + "</p>";
            var src = "";
            if ($('#preview').attr('src') != undefined) {
                src = "src='" + $('#preview').attr('src') + "'";
            }
            html += "<img " + src + "/>";
            html += "<input type='hidden' value='" + info + "' />";
            html += "<input type='hidden' value='" + master + "' /></div>";
            $('#locationList').html($('#locationList').html() + html);
        }
        else
        {
            $('#errContainer').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> Название локации слишком длинное!</div>");
        }
    } else {
        $('#errContainer').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> Все поля должны быть заполнены!</div>");
    }
})

function currentIdLoc(){
    return $('#locationList').children().length;
}

function currentIdChr() {
    return $('#characterList').children().length;
}

function currentIdcharasuka() {
    return $('#charas').children().length;
}

function currentIdEvn() {
    return $('#eventList').children().length;
}

function currentIdReward() {
    return $('#rewardPROTECTList').children().length;
}

function renderCharacteristics() {
    var html = "";
    var text = new Array("<hr/>", "Уровень: ", "<hr/>", "Телосложение: ", "Тренированность: ", "Интеллект: ", "Мудрость: ", "Харизма: ", "Судьба: ", "<hr/>", "Стойкость: ", "Реакция: ", "Воля: ", "<hr/>", "Здоровье: ", "Регенерация: ", "<hr/>", "Класс защиты: ", "Природная защита: ", "Физическая защита: ", "Магическая защита: ", "<hr/>", "Инициатива: ", "Скорость: ", "<hr/>", "Шанс крит.: ", "Бонус крит.: ", "Атака: ");
    var ids = new Array(" ", "lvl", " ", "con", "fit", "int", "wis", "cha", "fte", " ", "frt", "rfl", "wil", " ", "hit", "reg", " ", "cld", "ndf", "pdf", "mdf", " ", "ini", "spd", " ", "crt", "brt", "atk");
    for (var i = 0; i < text.length; i++) {
        if (text[i] == "<hr/>") {
            html += text[i];
        } else {
            html += "<div class='form-group'><label for='conText' class='col-sm-3 control-label' id='"+ids[i]+"Labbel'>" + text[i] + " </label><div class='col-sm-6'><span class='picker' selectr='" + ids[i] + "'></span></div></div>";
        }
    }
    $('#charContainer').html(html);
}

$('#addCharacter').click(function () {
    if ($('#nameText').val() != "" && $('#infoText').val() != "" && $('#masterText').val() != "") {
        if ($('#nameText').val().length <= 40) {
            var id = currentIdLoc();
            var name = $('#nameText').val();
            var info = $('#infoText').val();
            var info_cut = $('#infoText').val().substr(0, 200) + "...";
            var master = $('#masterText').val();
            //рисуем блок
            var html = "<div class='list-group-item' id='charId" + id + "'><button type='button' class='close' onclick=\"$('#charId" + id + "').remove();\">&times;</button><h3 class='list-group-item-heading text-center'>" + name + "</h3> <p class='list-group-item-text text-center'>" + info_cut + "</p>";
            //рисуем таблицу
            html += "<table class='table table-bordered table-hover'><tbody>";
            //заполняем таблицу
            html += "<tr><td>Уровень</td><td>" + $('#lvlValue').val() + "</td></tr>";
            html += "<tr><td>Здоровье</td><td>" + $('#hitValue').val() + "</td></tr>";
            html += "<tr><td>Класс защиты</td><td>" + $('#cldValue').val() + "</td></tr>";
            html += "<tr><td>Защита</td><td>" + (parseInt($('#ndfValue').val()) + parseInt($('#pdfValue').val()) + parseInt($('#mdfValue').val())).toString() + "</td></tr>";
            html += "<tr><td>Атака</td><td>" + $('#atkValue').val() + "</td></tr>";
            html += "<tr><td>Урон</td><td>" + $('#dmg1Value').val() + "d" + $('#dmg2Value').val() + "</td></tr>";
            html += "<tr><td>Инициатива</td><td>" + $('#iniValue').val() + "</td></tr>";
            html += "<tr><td>Скорость</td><td>" + $('#spdValue').val() + "</td></tr>";
            html += "</tbody></table>";

            //заполняем инфу для отправки на сервер            
            html += "<input type='hidden' value='" + master + "' />";
            html += "<input type='hidden' value='" + info + "' />";
            var ids = new Array("lvl", "con", "fit", "int", "wis", "cha", "fte", "frt", "rfl", "wil", "hit", "reg", "cld", "ndf", "pdf", "mdf", "ini", "spd", "crt", "brt", "atk", "dmg1", "dmg2");
            for (var i = 0; i < ids.length; i++) {
                var selector = '#' + ids[i] + 'Value';
                var selectorComment = '#' + ids[i] + 'Labbel';
                html += "<input type='hidden' comment='" + $(selectorComment)[0].innerText + "' value='" + $(selector).val() + "' />";
            }
            html += "</div>";
            //вставляшки
            $('#characterList').html($('#characterList').html() + html);
        }
        else
        {
            $('#errContainer').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> Имя персонажа слишком длинное!</div>");
        }
    } else {
        $('#errContainer').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> Все поля должны быть заполнены!</div>");
    }
})

$('#addReward').click(function () {
    if ($('#rewardName').val() != "") {
        if ($('#rewardName').val().length <= 100) {
            var id = currentIdReward();
            var name = $('#rewardName').val();
            var type = $('#rewardChar').val();
            var ifs = "";
            if (type != "Всем") {
                ifs = $('#rewardCharAdditional').val();
            }
            var rare = $('#rewardRare').val();
            var add = $('#rewardAdditinalSpecials').val();
            var add_cut = $('#rewardAdditinalSpecials').val().substr(0, 20) + "...";

            //render

            var html = "<div class='list-group-item' id='rewardId" + id + "'>";
            html += "<button type='button' class='close' onclick=\"$('#rewardId" + id + "').remove();\">&times;</button>";
            html += "<h2 class='list-group-item-heading text-center'> " + type + "</h2>";
            html += "<p class='list-group-item-text text-center'>" + name + "</p>";
            html += "<p class='list-group-item-text text-center'>" + add_cut + "</p>";
            html += "<input type='hidden' value='" + add + "' />";
            html += "<input type='hidden' value='" + rare + "' />";
            html += "<div class='panel panel-default'>";
            html += "<div class='panel-heading'>Характеристики</div>";
            html += "<div class='panel-body'>";
            html += "<ul class='list-group' id='charas_rewardId" + id + "'>";
            //items render
            for (var i = 0; i < $('#charas').children().length; i++) {
                var c = $('#charas').children()[i].innerText.split(' ');
                var v = c[0].replace('×', '');
                var n = c[1];
                html += "<li class='list-group-item'><span class='badge'>" + v + "</span>" + n + "</li>";
                html += "<input type='hidden' value='" + n + ':' + v + "' />";
            }

            //render
            html += "</ul>";
            html += "</div>";
            html += "</div>";
            html += "</div>";

            //вставляшки
            $('#rewardPROTECTList').html($('#rewardPROTECTList').html() + html);
        }
        else {
            $('#errContainer').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> Название награды слишком длинное!</div>");
        }
    } else {
        $('#errContainer').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> Награда не может быть без названия!</div>");
    }
});

$('#addEvent').click(function () {
    if ($('#eventName').val() != "" && $('#eventInfo').val() != "") {
        if ($('#eventName').val().length <= 100) {
            var id = currentIdEvn();
            var name = $('#eventName').val();
            var info = $('#eventInfo').val();
            //рисуем блок
            var html = "<div class='list-group-item' id='charId" + id + "'><button type='button' class='close' onclick=\"$('#charId" + id + "').remove();\">&times;</button><h2 class='list-group-item-heading text-center'>" + name + "</h2>";
            //заполняем инфу для отправки на сервер            
            html += "<input type='hidden' value='" + info + "' /></div>";
            //вставляшки
            $('#eventList').html($('#eventList').html() + html);
        }
        else {
            $('#errContainer').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> Название события слишком длинное!</div>");
        }
    } else {
        $('#errContainer').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> Все поля должны быть заполнены!</div>");
    }
})

$('#addCharact').click(function () {
    var id = currentIdcharasuka();
    var c = $('#rewardCharact').val();
    var v = $('#chrValue').val();
    $('#charas').html($('#charas').html() + "<li id='rewrd" + id + "' class='list-group-item'><button type='button' class='close' onclick=\"$('#rewrd" + id + "').remove();\">&times;</button><span class='badge'>" + v + "</span> " + c + "</li>");
});

function eventInfo() {
    var info = "События - это стадии прохождения сценария. Благодаря событиям персонажи продвигаются по сюжету и доводят дела до конца. При создании событий надо быть уверенным что события логически объяснимы и понятны всем игрокам. События определяют стадии сценария, благодаря событиям вы начинаете и завершаете сценарий. Для того что бы использовать событие мастером, достаточно выбрать его в списке событий, затем уточнить локацию и выбрать персонажей.";
    $('#errContainer').html("<div class='alert alert-info alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> " + info + "</div>");
}

function checkFirstPagevalues() {
    if ($('#scenarioName').val() == "" || $('#scenarioText').val() == "" || $('#scenarioTextMaster').val() == "") {
        $('#errContainer').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> Все поля должны быть заполнены!</div>");
        return false;
    }
    return true;
}

$('#rewardRare').change(function () {
    var str = "";
    $("#rewardRare option:selected").each(function () {
        str += $(this).text() + " ";
    });
    $('#rewardRare').removeClass();
    $('#rewardRare').addClass('form-control');
    switch (str) {
        case "Обычный ": $('#rewardRare').addClass('btn-default'); break;
        case "Магический ": $('#rewardRare').addClass('btn-info'); break;
        case "Волшебный ": $('#rewardRare').addClass('btn-warning'); break;
        case "Артефакт ": $('#rewardRare').addClass('btn-success'); break;
        case "Сломано ": $('#rewardRare').addClass('btn-danger'); break;
    }
}).change();

function checkEventsCount() {
    if (currentIdEvn() == 0) {
        $('#errContainer').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> Сюжет не может развиваться без событий!</div>");
        return false;
    }
    return true;
}

function validLocationPage() {
    if (currentIdLoc() == 0) {
        $('#errContainer').html("<div class='alert alert-danger alert-dismissable'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button><i class='fa fa-info-circle'></i> Сценарий должен содержать хотя бы одну локацию!</div>");
        return false;
    }
    return true;
}

$(document).ready(function () {
    renderCharacteristics();
});

function ReleaseDogs() {
    var data = gatherData();
    var jqxhr = $.post('../api/scenario/create', { '': JSON.stringify(data) }).done(function (data) { ShowResultMessage(data); });
};

function ShowResultMessage(data) {
    for (var i = 0; i < 7; i++) {
        var selctr = '#page' + i;
        $(selctr).css('display', 'none');
        $('#scenarioCreated').css('display', 'block');
        $('#resultOfscenario').html(data);
    }
}

function gatherData() {

    //Main
    var data = {
        "Title": $('#scenarioName').val(),
        "Recomendation": $('#scenarioText').val(),
        "Fable": $('#scenarioTextMaster').val(),
        "Locations": [],
        "Npcs": [],
        "Events": [],
        "Rewards": []
    };

    //Locations
    for (var i = 0; i < $('#locationList').children().length; i++) {
        data['Locations'].push({ "Name": $('#locationList').children()[i].children[1].textContent, "Description": $('#locationList').children()[i].children[4].value, "Specification": $('#locationList').children()[i].children[5].value, "Map": $('#locationList').children()[i].children[3].src });
    }

    //Npcs
    for (var i = 0; i < $('#characterList').children().length; i++) {
        var stats = [];
        for (var j = 6; j < $('#characterList').children()[i].children.length; j++) {
            stats.push({ "Name": $('#characterList').children()[i].children[j].attributes[1].value, "Value": $('#characterList').children()[i].children[j].value });
        }
        data['Npcs'].push({ "Name": $('#characterList').children()[i].children[1].innerText, "View": $('#characterList').children()[i].children[4].value, "Specification": $('#characterList').children()[i].children[5].value, "Stats": stats });
    }

    //Events
    for (var i = 0; i < $('#eventList').children().length; i++) {
        data['Events'].push({ "Title": $('#eventList').children()[0].children[1].innerHTML, "Description": $('#eventList').children()[0].children[2].value });
    }

    //Rewards
    for (var i = 0; i < $('#rewardPROTECTList').children().length; i++) {
        var stats = [];
        for (var j = 1; j < $('#rewardPROTECTList').children()[i].children[6].children[1].children[0].children.length; j += 2) {
            var n = $('#rewardPROTECTList').children()[i].children[6].children[1].children[0].children[j].value.split(':')[0];
            var v = $('#rewardPROTECTList').children()[i].children[6].children[1].children[0].children[j].value.split(':')[1];
            stats.push({ "Name": n, "Value": v });
        }
        data['Rewards'].push({
            "Name": $('#rewardPROTECTList').children()[i].children[2].innerText,
            "Who": $('#rewardPROTECTList').children()[i].children[2].innerText,
            "Rare": $('#rewardPROTECTList').children()[i].children[5].value,
            "Additional": $('#rewardPROTECTList').children()[i].children[4].value, "Characteristics": stats
        });
    }

    return data;
};
