$(function () {

    var user = JSON.parse($.cookie("user"));
    $('#userAvatar').attr('src', user.Avatar);
    $('#lastTime').html("Последняя авторизация: " + user.Stamp);

    if (user.HeroId != 0) {
        $('#old').css('display', 'block');
        old_render();
    } else {
        $('#new').css('display', 'block');
        new_render();
    }
});

function old_render() {
    Hero = new Object();
    var user = JSON.parse($.cookie("user"));
    $.getJSON('../api/hero?UserId=' + user.Id)
                 .done(function (data) {
                     Hero = data;
                     $('#oldName1').html(Hero.Name);
                     $('#oldName2').html(Hero.Name);
                     $('#oldLevel').html(Hero.Level);
                     $('#oldExp1').html(Hero.Expirience);
                     $('#oldCLass').html(Hero.Class.Value);
                     $('#oldRace').html(Hero.Race.Value);
                     $('#oldGod').html(Hero.God);
                     $('#oldSpecial').html('В разработке');

                     $('#oldSex').html(Hero.Sex.Value);
                     $('#oldAge').html(Hero.Age);
                     $('#oldHeight').html(Hero.Height.Value);
                     $('#oldWeight').html(Hero.Weight);
                     $('#oldEyes').html(Hero.Eyes);
                     $('#oldHair').html(Hero.Hair);
                     $('#oldSkin').html(Hero.Skin);

                     var html = '';
                     for (var i = 0; i < Hero.Characteristics.length; i++) {
                         html += '<tr><td>' + Hero.Characteristics[i].CharacteristicName.Value + '</td>';
                         html += '<td>' + Hero.Characteristics[i].DIX.Value + '</td>';
                         html += '<td>' + Hero.Characteristics[i].Value + '</td></tr>';
                     }
                     $('#oldchar').html(html);

                     for (var i = 0; i < Hero.Abilities.length; i++) {
                         var selector = '#spas' + i.toString();
                         var s0 = selector + 'n';
                         var s1 = selector + 'v';
                         $(s0).html(Hero.Abilities[i].AbilityName.Value);
                         $(s1).html(Hero.Abilities[i].Value);
                     }

                     $('#hp').html(Hero.HealthState.MaximumHitPoints);
                     $('#ahp').html(Hero.HealthState.AdditionalHitPoints);
                     $('#reg').html(Hero.HealthState.Regeneration);
                     $('#des').html(Hero.HealthState.Desease.Value);
                     $('#intox').html(Hero.HealthState.Intoxication.Value);
                     $('#charm').html(Hero.HealthState.Charm.Value);

                     $('#defclass').html(Hero.DefenceState.DefenceClass);
                     $('#defence').html(Hero.DefenceState.Defence);
                     $('#natdef').html(Hero.DefenceState.NaturalDefence);
                     $('#armdef').html(Hero.DefenceState.ArmorDefence);
                     $('#magdef').html(Hero.DefenceState.MagicDefence);
                     $('#bondef').html(Hero.DefenceState.DefenceBonus);

                     $('#atk').html(Hero.AttackState.Attack);
                     $('#bonatk').html(Hero.AttackState.FitAttack);
                     $('#dmg').html(Hero.AttackState.MinimalDamage + '-' + Hero.AttackState.MaximalDamage);
                     $('#dmgweapon').html(Hero.AttackState.WeaponMinimalDamage + '-' + Hero.AttackState.WeaponMaximalDamage);
                     $('#critchance').html(Hero.AttackState.CritChance);
                     $('#critbonus').html(Hero.AttackState.CritBonus);

                     $('#iniz').html(Hero.CommonState.Initiative);
                     $('#inizh').html(Hero.CommonState.InitiativeSize);
                     $('#inizw').html(Hero.CommonState.InitiativeWisdom);
                     $('#inizm').html(Hero.CommonState.InitiativeMagic);
                     $('#spd').html(Hero.CommonState.Speed);
                     $('#spdf').html(Hero.CommonState.SpeedFit);

                     var html = '';
                     for (var i = 0; i < 12; i++) {
                         html += '<tr><td>' + Hero.Skills[i].SkillName.Value + '</td>';
                         html += '<td>' + Hero.Skills[i].DIX.Value + '</td>';
                         html += '<td>' + Hero.Skills[i].Value + '</td></tr>';
                     }
                     $('#table9').html(html);
                     var html = '';
                     for (var i = 12; i < 24; i++) {
                         html += '<tr><td>' + Hero.Skills[i].SkillName.Value + '</td>';
                         html += '<td>' + Hero.Skills[i].DIX.Value + '</td>';
                         html += '<td>' + Hero.Skills[i].Value + '</td></tr>';
                     }
                     $('#table10').html(html);
                     var html = '';
                     for (var i = 24; i < 36; i++) {
                         html += '<tr><td>' + Hero.Skills[i].SkillName.Value + '</td>';
                         html += '<td>' + Hero.Skills[i].DIX.Value + '</td>';
                         html += '<td>' + Hero.Skills[i].Value + '</td></tr>';
                     }
                     $('#table11').html(html);
                 });   
}

function new_render() {
    $.getJSON('../api/hero/enums').done(function (data) {
        setLstSource(data[0], '#newClass');
        setLstSource(data[1], '#newRace');
        setLstSource(data[2], '#newSex');
        setLstSource(data[3], '#newHeight');
        setRaceAttributes('Средний', 65, 20, 'Один', 'Синий', 'Русый', 'Телесный');
    });

    $.getJSON('../api/skills').success(function (data) {
        var stupid = data.length / 12;
        var array = new Array(stupid);
        for (var i = 0; i < stupid; i++) {
            array[i] = data.splice(0, 12);
        }
        setTableSource(array[0], '#table12');
        setTableSource(array[1], '#table13');
        setTableSource(array[2], '#table14');
        for (var i = 2; i < array.length; i++) {
            localStorage.setItem('skills' + (i - 2).toString(), JSON.stringify(array[i]));
        }
        NumberPickerControl.Init();
    });
}

$('#newClass').change(function () {
    var selector = 'skills' + $(this).find('option:selected')[0].index.toString();
    setTableSource(JSON.parse(localStorage.getItem(selector)), '#table14');
    NumberPickerControl.Init();
});

$('#newRace').change(function () {
    var id = $(this).find('option:selected')[0].index;
    switch (id) {
        case 0: setRaceAttributes('Средний', 65, 20, 'Один', 'Синий', 'Русый', 'Телесный'); break;
        case 1: setRaceAttributes('Низкий', 30, 20, 'Уондала', 'Зелёный', 'Русый', 'Телесный'); break;
        case 2: setRaceAttributes('Низкий', 70, 60, 'Тор', 'Коричневый', 'Черный', 'Серый'); break;
        case 3: setRaceAttributes('Низкий', 75, 80, 'Тор', 'Серый', 'Серый', 'Серый'); break;
        case 4: setRaceAttributes('Высокий', 50, 120, 'Теландрия', 'Зелёный', 'Зелёный', 'Светло-зелёный'); break;
        case 5: setRaceAttributes('Высокий', 45, 120, 'Ангарадх', 'Тёмно-синий', 'Чёрно-синий', 'Светло-синий'); break;
        case 6: setRaceAttributes('Высокий', 50, 120, 'Морраг', 'Красный', 'Черный', 'Тёмный'); break;
        case 7: setRaceAttributes('Высокий', 45, 120, 'Фрея', 'Белый', 'Светлый', 'Светлый'); break;
        case 8: setRaceAttributes('Средний', 70, 40, 'Грах', 'Чёрный', 'Черный', 'Светло-красный'); break;
        case 9: setRaceAttributes('Средний', 80, 35, 'Хортаг', 'Чёрный', 'Чёрный', 'Зелёный'); break;
        case 10: setRaceAttributes('Высокий', 65, 25, 'Гутхай', 'Серебрянный', 'Светло-голубой', 'Светло-голубой'); break;
        case 11: setRaceAttributes('Высокий', 65, 25, 'Гутдул', 'Желтый', 'Русый', 'Песчаный'); break;
        case 12: setRaceAttributes('Высокий', 30, 500, 'Нет', 'Белый', 'Светлый', 'Светлый'); break;
        case 13: setRaceAttributes('Высокий', 90, 500, 'Нет', 'Красный', 'Чёрный', 'Красный'); break;
        case 14: setRaceAttributes('Высокий', 5, 1000, 'Нет', 'Прозрачно-голубой', 'Прозрачно-голубой', 'Прозрачно-голубой'); break;
    }
});

function setRaceAttributes(height, weight, age, god, eye, hair, skin) {
    $('#newHeight').val(height);
    $('#newWeight').val(weight);
    $('#newAge').val(age);
    $('#newGod').val(god);
    $('#newEyes').val(eye);
    $('#newHair').val(hair);
    $('#newSkin').val(skin);
}

function setLstSource(data, list) {
    var html = '';
    for (var i = 0; i < data.length; i++) {
        html += '<option class="form-control">' + data[i].Value + '</option>';
    }
    $(list).html(html);
}

function setTableSource(data, table) {
    var html = '';
    $(table).html(html);
    for (var i = 0; i < data.length; i++) {
        html += '<tr><td>' + data[i].SkillName.Value + '</td><td>' + data[i].DIX.Value + "</td><<td class='col-md-4'><span class=\'picker\' selectr=\'" + i.toString() + table.split('#').join('g') + "\'></span></td></tr>";
    }
    $(table).html(html);
}

function randomCharacteristics() {
    $('#con').html(getRandomArbitrary(5, 9));
    $('#fit').html(getRandomArbitrary(2, 7));
    var int = getRandomArbitrary(4, 9);
    $('#int').html(int);
    $('#wis').html(getRandomArbitrary(2, 8));
    $('#cha').html(getRandomArbitrary(1, 6));

    $('#npLimiter').val(int);
}

function getRandomArbitrary(min, max) {
    return Math.ceil(Math.random() * (max - min) + min);
}

function createNewHero() {
    if ($('#newName').val() != "") {
        $.post('../api/hero/create', { '': JSON.stringify(Hero()) }).done(function (data) {
            if (parseInt(data) != 0) {
                var data = JSON.parse($.cookie("user"));
                data.HeroId = parseInt(data);
                $.cookie('user', JSON.stringify(data), { path: '/site/' });
                location.reload();
            }
            else
                alert("Ошибка создания персонажа!");
        })
    }
    else
        alert('Введите имя! ~И ещё несколько проверок~');
};

function RuneString(Value)
{
    var RS=new Object();
    RS.Id=0;
    RS.Value=Value;
    return RS;
}

function Hero() {
    var o = new Object();
    o.UserId= JSON.parse($.cookie("user")).Id;
    o.Level = 1;
    o.Name = $('#newName').val();
    o.Expirience = 0;
    o.Class = RuneString($('#newClass').val());
    o.Race = RuneString($('#newRace').val());
    o.God = $('#newGod').val();
    o.Height = RuneString($('#newHeight').val());
    o.Age = $('#newAge').val();
    o.Sex = RuneString($('#newSex').val());
    o.Weight = $('#newWeight').val();
    o.Eyes = $('#newEyes').val();
    o.Hair = $('#newHair').val();
    o.Skin = $('#newSkin').val();
    o.Characteristics = [
        Characteristic('Телосложение', 'CON', $('#con')[0].outerText),
        Characteristic('Тренированность', 'FIT', $('#fit')[0].outerText),
        Characteristic('Интеллект', 'INT', $('#int')[0].outerText),
        Characteristic('Мудрость', 'WIS', $('#wis')[0].outerText),
        Characteristic('Харизма', 'CHA', $('#cha')[0].outerText)
    ];
    o.Abilities = [];
    o.HealthState = {};
    o.DefenceState = {};
    o.AttackState = {};
    o.CommonState = {};
    o.Skills = SkillsArray('#table12', '#table13', '#table14');
    return o;
};

function Characteristic(name, dix, value) {
    var o = new Object();
    o.CharacteristicName = RuneString(name);
    o.DIX = RuneString(dix);
    o.Value = value;
    return o;
};

function SkillsArray(tableSelector1, tableSelector2, tableSelector3) {
    var o = new Object();
    o = [];
    for (var i = 0; i < 12; i++) {
        o.push({
            SkillName: RuneString($(tableSelector1)[0].children[i].children[0].innerText),
            DIX: RuneString($(tableSelector1)[0].children[i].children[1].innerText),
            Value: $($(tableSelector1)[0].children[i].children[2]).find('input')[1].value
        });
    };

    for (var i = 0; i < 12; i++) {
        o.push({
            SkillName: RuneString($(tableSelector2)[0].children[i].children[0].innerText),
            DIX: RuneString($(tableSelector2)[0].children[i].children[1].innerText),
            Value: $($(tableSelector2)[0].children[i].children[2]).find('input')[1].value
        });
    };

    for (var i = 0; i < 12; i++) {
        o.push({
            SkillName: RuneString($(tableSelector3)[0].children[i].children[0].innerText),
            DIX: RuneString($(tableSelector3)[0].children[i].children[1].innerText),
            Value: $($(tableSelector3)[0].children[i].children[2]).find('input')[1].value
        });
    };

    return o;
};