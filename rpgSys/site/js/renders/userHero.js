$(document).ready(function () {
    var cookie = getCookie("hero_id")
    if (cookie == '0') {
        $('#new').css("display", "block");
        reloadSkills();
    }
    else {
        $('#old').css("display", "block");
        $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=0&UserId=' + getCookie("user_id"))
            .done(function (data) {
                var html = "";
                for (var i = 0; i < data.length; i++) {
                    html += "<tr info-attr='" + data[i].Info + "' id='info" + i.toString() + "' oncontextmenu=\"ShowInfo('info" + i.toString() + "','fiInfoContainer1')\">";
                    html += "<td>" + data[i].Name + "</td>";
                    html += "<td>" + data[i].Value + "</td>";
                    html += "</tr>"
                }
                $('#table1').html(html);
            });
        $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=1&UserId=' + getCookie("user_id"))
            .done(function (data) {
                var html = "";
                for (var i = 0; i < data.length; i++) {
                    var name = data[i].Name.split(' ');
                    html += "<tr info-attr='" + data[i].Info + "' id='char" + i.toString() + "' oncontextmenu=\"ShowInfo('char" + i.toString() + "','fiInfoContainer1')\">";
                    html += "<td>" + name[0] + "</td>";
                    html += "<td>" + name[1] + "</td>";
                    html += "<td>" + data[i].Value + "</td>";
                    html += "</tr>"
                }
                $('#table2').html(html);
            });
        $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=2&UserId=' + getCookie("user_id"))
           .done(function (data) {
               var html = "";
               for (var i = 0; i < data.length; i++) {
                   html += "<tr info-attr='" + data[i].Info + "' id='profi" + i.toString() + "' oncontextmenu=\"ShowInfo('profi" + i.toString() + "','fiInfoContainer1')\">";
                   html += "<td>" + data[i].Name + "</td>";
                   html += "<td>" + data[i].Value + "</td>";
                   html += "</tr>"
               }
               $('#table3').html(html);
           });

        $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=3&UserId=' + getCookie("user_id"))
           .done(function (data) {
               var html = "";
               for (var i = 0; i < data.length; i++) {
                   html += "<tr info-attr='" + data[i].Info + "' id='health" + i.toString() + "' oncontextmenu=\"ShowInfo('health" + i.toString() + "','fiInfoContainer2')\">";
                   html += "<td>" + data[i].Name + "</td>";
                   html += "<td>" + data[i].Value + "</td>";
                   html += "</tr>"
               }
               $('#table5').html(html);
           });
        $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=4&UserId=' + getCookie("user_id"))
           .done(function (data) {
               var html = "";
               for (var i = 0; i < data.length; i++) {
                   html += "<tr info-attr='" + data[i].Info + "' id='defence" + i.toString() + "' oncontextmenu=\"ShowInfo('defence" + i.toString() + "','fiInfoContainer2')\">";
                   html += "<td>" + data[i].Name + "</td>";
                   html += "<td>" + data[i].Value + "</td>";
                   html += "</tr>"
               }
               $('#table6').html(html);
           });
        $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=5&UserId=' + getCookie("user_id"))
           .done(function (data) {
               var html = "";
               for (var i = 0; i < data.length; i++) {
                   html += "<tr info-attr='" + data[i].Info + "' id='attack" + i.toString() + "' oncontextmenu=\"ShowInfo('attack" + i.toString() + "','fiInfoContainer2')\">";
                   html += "<td>" + data[i].Name + "</td>";
                   html += "<td>" + data[i].Value + "</td>";
                   html += "</tr>"
               }
               $('#table7').html(html);
           });
        $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=6&UserId=' + getCookie("user_id"))
           .done(function (data) {
               var html = "";
               for (var i = 0; i < data.length; i++) {
                   html += "<tr info-attr='" + data[i].Info + "' id='initi" + i.toString() + "' oncontextmenu=\"ShowInfo('initi" + i.toString() + "','fiInfoContainer2')\">";
                   html += "<td>" + data[i].Name + "</td>";
                   html += "<td>" + data[i].Value + "</td>";
                   html += "</tr>"
               }
               $('#table8').html(html);
           });

        $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=7&UserId=' + getCookie("user_id"))
       .done(function (data) {
           var html = "";
           for (var i = 0; i < data.length; i++) {
               var name = data[i].Name.split('~');
               html += "<tr info-attr='" + data[i].Info + "' id='mater" + i.toString() + "' oncontextmenu=\"ShowInfo('mater" + i.toString() + "','fiInfoContainer3')\">";
               html += "<td>" + name[0] + "</td>";
               html += "<td>" + name[1] + "</td>";
               html += "<td>" + data[i].Value + "</td>";
               html += "<td>" + data[i].Bonus + "</td>";
               html += "</tr>"
           }
           $('#table9').html(html);
       });
        $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=8&UserId=' + getCookie("user_id"))
       .done(function (data) {
           var html = "";
           for (var i = 0; i < data.length; i++) {
               var name = data[i].Name.split('~');
               html += "<tr info-attr='" + data[i].Info + "' id='mental" + i.toString() + "' oncontextmenu=\"ShowInfo('mental" + i.toString() + "','fiInfoContainer3')\">";
               html += "<td>" + name[0] + "</td>";
               html += "<td>" + name[1] + "</td>";
               html += "<td>" + data[i].Value + "</td>";
               html += "<td>" + data[i].Bonus + "</td>";
               html += "</tr>"
           }
           $('#table10').html(html);
       });
        $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=9&UserId=' + getCookie("user_id"))
       .done(function (data) {
           var html = "";
           for (var i = 0; i < data.length; i++) {
               var name = data[i].Name.split('~');
               html += "<tr info-attr='" + data[i].Info + "' id='clsss" + i.toString() + "' oncontextmenu=\"ShowInfo('clsss" + i.toString() + "','fiInfoContainer3')\">";
               html += "<td>" + name[0] + "</td>";
               html += "<td>" + name[1] + "</td>";
               html += "<td>" + data[i].Value + "</td>";
               html += "<td>" + data[i].Bonus + "</td>";
               html += "</tr>"
           }
           $('#table11').html(html);
       });
    }
});

function reloadSkills() {
    $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=10&UserId=' + getCookie("user_id"))
       .done(function (data) {
           var html = "";
           for (var i = 0; i < data.length; i++) {
               html += "<tr>";
               html += "<td>" + data[i].Name + "</td>";
               html += "<td>" + data[i].Info + "</td>";
               html += "<td class='col-lg-5'><span class='picker' selectr='mat" + data[i].Value + "'></span></td>";
               html += "</tr>"
           }
           $('#table12').html(html);
           npCustomInit();
       });
    $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=11&UserId=' + getCookie("user_id"))
   .done(function (data) {
       var html = "";
       for (var i = 0; i < data.length; i++) {
           var name = data[i].Name.split('~');
           html += "<tr>";
           html += "<td>" + data[i].Name + "</td>";
           html += "<td>" + data[i].Info + "</td>";
           html += "<td class='col-lg-5'><span class='picker' selectr='men" + data[i].Value + "'></span></td>";
           html += "</tr>"
       }
       $('#table13').html(html);
       npCustomInit();
   });
    $.getJSON('../api/Hero?Auth=' + getCookie("auth_key") + '&OperationId=12~Некромант&UserId=' + getCookie("user_id"))
   .done(function (data) {
       var html = "";
       for (var i = 0; i < data.length; i++) {
           html += "<tr>";
           html += "<td>" + data[i].Name + "</td>";
           html += "<td>" + data[i].Info + "</td>";
           html += "<td class='col-lg-5'><span class='picker' selectr='cla" + data[i].Value + "'></span></td>";
           html += "</tr>"
       }
       $('#table14').html(html);
       npCustomInit();
   });
}

$('#dice').click(function () {
    var numbers = new Array(0, 0, 0, 0, 0);
    var max = 10;
    for (var i = 0; i < 5; i++) {
        var pre = 0;
        if (i != 0) {
            if (numbers[i - 1] > parseInt(max / 2)) {
                pre += max - numbers[i - 1];
            } else if (numbers[i - 1] < parseInt(max / 2)) {
                pre -= max - numbers[i - 1]
            }
        }
        numbers[i] = getRandomInt(4, max + pre);
        var sm = 0;
        for (var j = 0; j < 5; j++) {
            sm += numbers[j];
        }
        if (sm * 2 > max) {
            max -= 2;
        }
    }

    $('#con').html(numbers[4] + getRandomInt(2, 4));
    $('#fit').html(numbers[2]);
    $('#int').html(numbers[1]);
    $('#cha').html(numbers[3]);
    $('#wis').html(numbers[0]);

    $('#npLimiter').val(numbers[1]);
    reloadSkills();
});

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

$('#createChr').click(function () {
    var data = gatherData();
    var jqxhr = $.post('../api/hero/create', { '': JSON.stringify(data) }).done(function (data) { setCookie("hero_id", data, { path: '/Area/' }); location.reload(); });
});

function gatherData() {

    //Main
    var data = [];

    //main
    data.push({ "Value": $('#name').val(), "Name": 'Имя' });
    data.push({ "Value": $('#clas').val(), "Name": 'Класс' });
    data.push({ "Value": $('#race').val(), "Name": 'Раса' });
    data.push({ "Value": $('#god').val(), "Name": 'Божество' });
    data.push({ "Value": $('#sex').val(), "Name": 'Пол' });

    //char
    data.push({ "Value": $('#con')[0].innerText, "Name": 'Телосложение' });
    data.push({ "Value": $('#fit')[0].innerText, "Name": 'Тренированность' });
    data.push({ "Value": $('#int')[0].innerText, "Name": 'Интеллект' });
    data.push({ "Value": $('#wis')[0].innerText, "Name": 'Мудрость' });
    data.push({ "Value": $('#cha')[0].innerText, "Name": 'Харизма' });


    for (var i = 0; i < $('#table12').children().length; i++) {
        var name = $('#table12').children()[i].children[0].innerText;
        var value = $('#table12').children()[i].children[2].children[0].children[0].children[2].value
        data.push({ "Value": value, "Name": name });
    }
    for (var i = 0; i < $('#table13').children().length; i++) {
        var name = $('#table13').children()[i].children[0].innerText;
        var value = $('#table13').children()[i].children[2].children[0].children[0].children[2].value
        data.push({ "Value": value, "Name": name });
    }
    for (var i = 0; i < $('#table14').children().length; i++) {
        var name = $('#table14').children()[i].children[0].innerText;
        var value = $('#table14').children()[i].children[2].children[0].children[0].children[2].value
        data.push({ "Value": value, "Name": name });
    }

    return data;
}
