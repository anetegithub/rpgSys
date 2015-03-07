$(function () {

    var user = JSON.parse($.cookie("user"));
    $('#userAvatar').attr('src', user.Avatar);
    $('#lastTime').html("Последняя авторизация: " + user.StampToString);

    if (JSON.parse($.cookie('user')).HeroId != 0) {
        $('#old').css('display', 'block');
        $.getJSON('../api/hero?HeroId=' + user.HeroId).done(function (data) {
            old_render(data)
        });
    } else {
        $('#new').css('display', 'block');
    }
});

function old_render(Hero) {    
    $('#oldName2').html(Hero.Name);
    $('#oldLevel').html(Hero.Level);
    $('#oldExp1').html(Hero.Expirience);
    $('#oldCLass').html(Hero.Class);
    $('#oldRace').html(Hero.Race);
    $('#oldGod').html(Hero.God);
    $('#oldSpecial').html('В разработке');

    $('#oldSex').html(Hero.Sex);
    $('#oldAge').html(Hero.Age);
    $('#oldHeight').html(Hero.Height);
    $('#oldWeight').html(Hero.Weight);
    $('#oldEyes').html(Hero.Eyes);
    $('#oldHair').html(Hero.Hair);
    $('#oldSkin').html(Hero.Skin);

    var html = '';
    for (var i = 0; i < Hero.Characteristics.length; i++) {
        html += '<tr><td>' + Hero.Characteristics[i].Name + '</td>';
        html += '<td>' + Hero.Characteristics[i].DIX + '</td>';
        html += '<td>' + Hero.Characteristics[i].Value + '</td></tr>';
    }
    $('#oldchar').html(html);

    for (var i = 0; i < Hero.Abilities.length; i++) {
        var selector = '#spas' + i.toString();
        var s0 = selector + 'n';
        var s1 = selector + 'v';
        $(s0).html(Hero.Abilities[i].Info);
        $(s1).html(Hero.Abilities[i].Value);
    }

    $('#hp').html(Hero.HealthState.MaximumHitPoints);
    $('#ahp').html(Hero.HealthState.AdditionalHitPoints);
    $('#reg').html(Hero.HealthState.Regeneration);
    $('#des').html(Hero.HealthState.Desease);
    $('#intox').html(Hero.HealthState.Intoxication);
    $('#charm').html(Hero.HealthState.Charm);

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
    for (var i = 0; i < Hero.MaterialSkill.length; i++) {
        html += '<tr><td>' + Hero.MaterialSkill[i].Name + '</td>';
        html += '<td>' + Hero.MaterialSkill[i].DIX + '</td>';
        html += '<td>' + Hero.MaterialSkill[i].Value + '</td></tr>';
    }
    $('#table10').html(html);
    var html = '';
    for (var i = 0; i < Hero.MentalSkill.length; i++) {
        html += '<tr><td>' + Hero.MentalSkill[i].Name + '</td>';
        html += '<td>' + Hero.MentalSkill[i].DIX + '</td>';
        html += '<td>' + Hero.MentalSkill[i].Value + '</td></tr>';
    }
    $('#table11').html(html);
    var html = '';
    for (var i = 0; i < Hero.ClassSkill.length; i++) {
        html += '<tr><td>' + Hero.ClassSkill[i].Name + '</td>';
        html += '<td>' + Hero.ClassSkill[i].DIX + '</td>';
        html += '<td>' + Hero.ClassSkill[i].Value + '</td></tr>';
    }
    $('#table9').html(html);

}