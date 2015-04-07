var Play = null;
var user = JSON.parse($.cookie("user"));

$(function () {
    if (user.GameId != 0) {
        $.getJSON('../api/game/byid?Id=' + user.GameId)
        .done(function (data) {
            if (data.Id != 0) {
                if (data.IsActive) {
                    Play = PlayManager();
                    Play.Init();
                    Play.Label = "Приключение: " + data.Scenario.Title;

                    Play.Players.Add(data.Master);
                    Play.Players.Show(data.Heroes);

                    Play.Events.gameid = data.Id;
                    //Play.Events.updateevent=UPDATE OTHER CLIENTS
                    Play.Events.event = data.Event;

                    Play.Locations.gameid = data.Id;
                    //Play.Locations.updatelocation=UPDATE OTHER CLIENTS
                    Play.Locations.location = data.Location;
                    

                    if (data.Master.Id == user.HeroId) {
                        $('#gameOffBtn').html('Закончить приключение');
                        $('#gameOffBtn').click(Lobby.MagicBlock.ExitBtn.OnClick);
                        $('#gameOffBtn').css('display', 'inline');
                        Play.End.Init(true);
                    } else {
                        $('#gameOffBtn').click(Lobby.MagicBlock.ExitBtn.OnClick);
                        $('#gameOffBtn').css('display', 'inline');
                        Play.End.Init(false);
                    }
                }
            }
        });
    }
});

function PlayManager() {
    var pm = new Object();

    pm.Players = PlayersList();
    pm.Npcs = NpcList();
    pm.Events = EventMgr();
    pm.Locations = LocationMgr();

    pm.Container = ContainerManager();

    pm.Info = Inforamtion();
    pm.Stat = Stats();
    pm.Skills = Skills();
    pm.Inventory = Inventory();

    pm.Chat = ChatManager();
    pm.End = EndButton();

    pm.IsMaster=false;

    pm.Label = "Unknown";
    pm.watch('Label', function (id, oldval, newval) {
        $('#gameName').html(newval);
        return newval;
    })

    pm.LabelHero = "Unknown";
    pm.watch('LabelHero', function (id, oldval, newval) {
        $('#userLogin').html(newval);
        return newval;
    })

    pm.SendMessage = function (GameId, HeroId, HeroImg, HeroMsg) { };

    pm.Init = function () {
        $.getJSON('../api/hero/infostate?hid=' + user.HeroId)
               .done(function (data) {                   
                   pm.LabelHero = "Герой: " + data.Name;
               });
        pm.Chat.Init();        
        $('#infoBtn').click(pm.Info.Show);
        $('#statBtn').click(pm.Stat.Show);
        $('#skillBtn').click(pm.Skills.Show);
        $('#stuffBtn').click(pm.Inventory.Show);
    }

    return pm;
};

function ChatManager() {
    var cm = new Object();
    cm.Send = function () { };
    cm.SendCustom = function () { };
    cm.Init = function () {
        $('#send_btn').click(cm.Send);
        cm.ShowLog();
    }
    cm.ShowLog = function () {
        $.getJSON('../api/game/chat?GameId=' + user.GameId + "&Limit=30").done(function (data) {
            for (var i = 0; i < data.length; i++) {
                cm.Add(data[i]);
            }
        });
    };
    cm.Add = function (Msg) {
        if (Msg.GameMessageType.Id == 2) {
            Msg.Avatar = "img/fable.png";
            Msg.Name = "<label style='color:black'>Рассказчик</label>";
            Msg.Text = "<label style='color:black'>" + Msg.Text + "</label>";
            Msg.Stamp = "<label style='color:black'>" + Msg.Stamp + "</label>";
        }
        if (Msg.GameMessageType.Id == 3) {
            Msg.Avatar = 'img/npc.jpg';
            Msg.Text = "<strong>" + Msg.Text + "</strong>";
            Msg.Stamp = "<strong>" + Msg.Stamp + "</strong>";
        }
        if (Msg.GameMessageType.Id == 4) {
            Msg.Avatar = 'img/event.png';
            Msg.Name = "<label style='color:#045FB4'>Событие: " + Msg.Name + "</label>";
            Msg.Text = "<label style='color:#045FB4'>" + Msg.Text + "</label>";
            Msg.Stamp = "<label style='color:#045FB4'>" + Msg.Stamp + "</label>";
        }
        if (Msg.GameMessageType.Id == 5) {
            Msg.Avatar = 'img/location.png';
            Msg.Name = "<label style='color:green'> Локация: " + Msg.Name + "</label>";
            Msg.Text = "<label style='color:green'>" + Msg.Text + "</label>";
            Msg.Stamp = "<label style='color:green'>" + Msg.Stamp + "</label>";
        }
        if (Msg.GameMessageType.Id == 6) {
            Msg.Avatar = 'img/unknown.png';
            Msg.Name = "<label style='color:red'>Сервер</label>";
            Msg.Text = "<label style='color:red'>" + Msg.Text + "</label>";
            Msg.Stamp = "<label style='color:red'>" + Msg.Stamp + "</label>";
        }

        var html = "<li class='left clearfix'>";
        html += "<span class='chat-img pull-left'>";
        html += "<img src='" + Msg.Avatar + "' height='50px' width='50px' class='img-circle'/></span>";
        html += "<div class='chat-body'><strong>" + Msg.Name + "</strong><small class='pull-right text-muted'><i class='fa fa-clock-o fa-fw'></i>" + Msg.Stamp + "</small>";
        html += "<p>" + Msg.Text + "</p>";
        html += "</div></li>";
        $('#chatBox').html($('#chatBox').html() + html);
        $("#chatWrapper").animate({ scrollTop: $('#chatWrapper')[0].scrollHeight }, 50);
    }

    return cm;
}

function PlayersList() {
    var pl = new Object();

    pl.Container = $('#gameparty');
    pl.Html=function(){
        return pl.Container.html();
    };

    pl.Show = function (Heroes) {
        for (var i = 0; i < Heroes.length; i++) {
            pl.Add(Heroes[i]);
        };
    };
    pl.Add = function (Hero) {
        pl.Container.html(pl.Html() + "<span heroId='" + Hero.Id +
            "' class='list-group-item'><span><img src=\"" + Hero.Avatar +
            "\" height='40px' width='40px' class='img-circle'></span>&nbsp;<span class='h2-corporative'>" + Hero.Name + "</span></span>");
    };
    pl.Remove = function (Hero) {
        for (var i = 0; i < pl.Container[0].children.length; i++) {
            if (parseInt($(pl.Container[0].children[i]).attr('heroid')) == Hero.Id) {
                $(pl.Container[0].children[i]).attr('heroid').remove();
            }
        };
    };

    return pl;
};

function NpcList() {
    var pl = new Object();

    pl.Container = $('#npcparty');
    pl.Html = function () {
        return pl.Container.html();
    };

    pl.Show = function (Npcs) {
        for (var i = 0; i < Npcs.length; i++) {
            pl.Add(Npcs[i]);
        };
    };
    pl.Add = function (Npc) {
        pl.Container.html(pl.Html() + "<span npcid='" + Npc.Id +
            "' class='list-group-item'><span><img src='img/npc.jpg'" +
            "\" height='40px' width='40px' class='img-circle'></span>&nbsp;<span class='h2-corporative'>" + Npc.Name + "</span></span>");
    };
    pl.Remove = function (Npc) {
        for (var i = 0; i < pl.Container[0].children.length; i++) {
            if (parseInt($(pl.Container[0].children[i]).attr('npcid')) == Npc.Id) {
                $(pl.Container[0].children[i]).attr('npcid').remove();
            }
        };
    };

    return pl;
};

function EventMgr() {
    var evmngr = new Object();
    evmngr.gameid = 0;
    evmngr.updateevent = function (gameid) { };
    evmngr.event = EventObject;
    evmngr.watch('event', function (id, oldval, newval) {
        $('#currentEvent').html(newval.Title);
        evmngr.updateevent(evmngr.gameid);
        return newval;
    })
    return evmngr;
}

function EventObject() {
    var eo = new Object();
    eo.Id = 0;
    eo.Title = "Unknown";
    eo.Description = "Nothing";
    return eo;
}

function LocationMgr() {
    var evmngr = new Object();
    evmngr.gameid = 0;
    evmngr.updatelocation = function (gameid) { };
    evmngr.location = LocationObject;
    evmngr.watch('location', function (id, oldval, newval) {
        $('#currentLocation').html(newval.Description);
        evmngr.updatelocation(evmngr.gameid);
        return newval;
    })
    return evmngr;
}

function LocationObject() {
    var eo = new Object();
    eo.Id = 0;
    eo.Specification = "Unknown";
    eo.Description = "Nothing";
    eo.Map = "";
    return eo;
}

function Rolls() {
    var rlmngr = new Object();

    return rlmngr;
};

function EndButton() {
    var o = new Object();
    o.Sync=function(GameId){};
    o.Init = function (IsMaster) {
        if (IsMaster) {            
            $('#gameOffBtn').css('display', 'inline');
            $('#gameOffBtn').click(function () {
                $.getJSON('../api/game/delete?GameId=' + user.GameId)
               .done(function (data) {
                   if (data == "true") {
                       var user = JSON.parse($.cookie("user"));
                       var gameid = user.GameId;
                       user.GameId = 0;
                       $.cookie('user', JSON.stringify(user), { path: '/site/' });
                       NewLobby.SyncGameDelete(gameid);
                       window.location.replace('profile');
                   } else
                       alert('Невозможно закончить приключение!');
               });
            });
        } else {
            $('#gameOffBtn').css('display', 'inline');
            $('#gameOffBtn').click(function () {
                $.getJSON('../api/game/exit?GameId=' + user.GameId + "&UserId=" + user.Id)
                .done(function (data) {
                    if (data == "true") {
                        var user = JSON.parse($.cookie("user"));
                        var gameid = user.GameId;
                        user.GameId = 0;
                        $.cookie('user', JSON.stringify(user), { path: '/site/' });
                        Lobby.SyncHeroExit(gameid, user.HeroId);
                        window.location.reload();
                    } else
                        alert('Невозможно покинуть партию!');
                });
            });
        }
    }
    return o;
}

function Inforamtion() {
    var o = new Object();

    o.Show = function () {
        $.getJSON('../api/hero/infostate?hid=' + user.HeroId).done(function (data) {
            var innerhtml = "";
            innerhtml+="<div class='row text-center'><div class='col-lg-6'><h2>Информация</h2><div class='table-responsive'><table class='table table-bordered table-hover'><tbody>";
            innerhtml+="<tr><td>Имя</td><td>"+data.Name+"</td></tr>";
            innerhtml+="<tr><td>Уровень</td><td>"+data.Level+"</td></tr>";
            innerhtml += "<tr><td>Опыт</td><td>" + data.Expirience + "</td></tr>";
            innerhtml+="<tr><td>Класс</td><td>"+data.Class.Value+"</td></tr>";
            innerhtml+="<tr><td>Раса</td><td>"+data.Race.Value+"</td></tr>";
            innerhtml+="<tr><td>Божество</td><td>"+data.God+"</td></tr>";
            innerhtml+="<tr><td>Особенности</td><td>В разработке</td></tr>";
            innerhtml+="</tbody></table></div></div><div class='col-lg-6'><h2>Внешность</h2><div class='table-responsive'><table class='table table-bordered table-hover'><tbody>";
            innerhtml+="<tr><td>Пол</td><td>"+data.Sex.Value+"</td></tr>";
            innerhtml+="<tr><td>Возраст</td><td>"+data.Age+"</td></tr>";
            innerhtml+="<tr><td>Рост</td><td>"+data.Height.Value+"</td></tr>";
            innerhtml+="<tr><td>Вес</td><td>"+data.Weight+"</td></tr>";
            innerhtml+="<tr><td>Цвет глаз</td><td>"+data.Eyes+"</td></tr>";
            innerhtml+="<tr><td>Цвет волос</td><td>"+data.Hair+"</td></tr>";
            innerhtml+="<tr><td>Цвет кожи</td><td>"+data.Skin+"</td></tr>";
            innerhtml += "</tbody></table></div></div></div>";
            Play.Container.Show('fa-info',"Описание", 'default', innerhtml);
        });
    };

    return o;
}

function Stats() {
    var o = new Object();
    o.Show = function () {
        $.getJSON('../api/hero/stats?hid=' + user.HeroId).done(function (data) {
            var innerhtml = "";
            innerhtml += "<div class='row text-center'><div class='col-lg-6'><h2>Характеристики</h2><div class='table-responsive'><table class='table table-bordered table-hover'><tbody>";
            if (data[1] != null) {
                for (var i = 0; i < data[1].length;i++){
                    innerhtml += '<tr><td>' + data[1][i].CharacteristicName.Value + '</td>';
                    innerhtml += '<td>' + data[1][i].DIX.Value + '</td>';
                    innerhtml += '<td>' + data[1][i].Value + '</td></tr>';
                }
            }
            innerhtml += "</tbody></table></div></div><div class='col-lg-6'><h2>Способности</h2><div class='table-responsive'><table class='table table-bordered table-hover'><tbody>";
            if (data[0] != null) {
                for (var i = 0; i < data[0].length; i++) {
                    innerhtml += '<tr><td>' + data[0][i].AbilityName.Value + '</td>';
                    innerhtml += '<td>' + data[0][i].Value + '</td>';
                }
            }
            innerhtml += "</tbody></table></div></div></div>";
            Play.Container.Show('fa-ellipsis-v', "Показатели", 'default', innerhtml);
        });
    };
    return o;
}

function Skills() {
    var o = new Object();
    o.Show = function () {
        $.getJSON('../api/hero/skills?hid=' + user.HeroId).done(function (data) {
            var innerhtml = "";
            innerhtml += "<div class='row text-center'><div class='col-lg-4'><h2>Телесные</h2><div class='table-responsive'><table class='table table-bordered table-hover'><tbody>";
            for (var i = 0; i < 12; i++) {
                innerhtml += '<tr><td>' + data[i].SkillName.Value + '</td>';
                innerhtml += '<td>' + data[i].DIX.Value + '</td>';
                innerhtml += '<td>' + data[i].Value + '</td></tr>';
            }
            innerhtml += "</tbody></table></div></div><div class='col-lg-4'><h2>Ментальные</h2><div class='table-responsive'><table class='table table-bordered table-hover'><tbody>";
            for (var i = 12; i < 24; i++) {
                innerhtml += '<tr><td>' + data[i].SkillName.Value + '</td>';
                innerhtml += '<td>' + data[i].DIX.Value + '</td>';
                innerhtml += '<td>' + data[i].Value + '</td></tr>';
            }
            innerhtml += "</tbody></table></div></div><div class='col-lg-4'><h2>Классовые</h2><div class='table-responsive'><table class='table table-bordered table-hover'><tbody>";
            for (var i = 24; i < 36; i++) {
                innerhtml += '<tr><td>' + data[i].SkillName.Value + '</td>';
                innerhtml += '<td>' + data[i].DIX.Value + '</td>';
                innerhtml += '<td>' + data[i].Value + '</td></tr>';
            }            
            innerhtml += "</tbody></table></div></div></div>";
            Play.Container.Show('fa-magic', "Навыки", 'default', innerhtml);
        });
    };
    return o;
}

function Inventory() {
    var o = new Object();
    var f= parseInt
    o.Show = function () {
        $.getJSON('../api/stuff/inventory?hid=' + user.HeroId).done(function (data) {
            var innerhtml = "";
            innerhtml += "<div class='row text-center'><div class='col-lg-12'>";
            for (var i = 0; i < data.length; i++) {
                innerhtml += "<div class='table-responsive'><table class='table table-bordered table-hover'><tbody>";
                innerhtml += '<tr><td><h2>' + data[i].Name + '</h2></td></tr>';                

                if (data[i].Characteristics != null) {
                    var characteristics = "";
                    for (var i = 0; i < data[i].Characteristics.length; i++) {
                        characteristics += data[i].Characteristics[i].CharacteristicName.Value + " " + data[i].Characteristics[i].Value + ' | ';
                    }
                    innerhtml += '<tr><td> Характеристики:' + characteristics + '</td></tr>';
                }
                if (data[i].Abilities != null) {
                    var abilities = "";
                    for (var i = 0; i < data[i].Abilities.length; i++) {
                        characteristics += data[i].Abilities[i].AbilityName.Value + " " + data[i].Abilities[i].Value + ' | ';
                    }
                    innerhtml += '<tr><td> Способности:' + abilities + '</td></tr>';
                }
                if (data[i].HealthState != null) {
                    var hstate = "";

                    if (data[i].HealthState.CurrentHitPoints != 0)
                        hstate += " Исцеление " + data[i].HealthState.CurrentHitPoints;

                    if (data[i].HealthState.MaximumHitPoints != 0)
                        hstate += " Макс. Здоровье " + data[i].HealthState.MaximumHitPoints;

                    if (data[i].HealthState.AdditionalHitPoints != 0)
                        hstate += " Доп. Здоровье " + data[i].HealthState.AdditionalHitPoints;

                    if (data[i].HealthState.Regeneration != 0)
                        hstate += " Регенерация " + data[i].HealthState.Regeneration;

                    if (data[i].HealthState.Desease.Id != 1)
                        hstate += data[i].HealthState.Desease.Value;

                    if (data[i].HealthState.Intoxication.Id != 1)
                        hstate += data[i].HealthState.Intoxication.Value;

                    if (data[i].HealthState.Charm.Id != 1)
                        hstate += data[i].HealthState.Charm.Value;

                    innerhtml += '<tr><td> Показ. здор.:' + hstate + '</td></tr>';
                }
                if (data[i].DefenceState != null) {
                    var hstate = "";

                    if (data[i].DefenceState.Defence != 0)
                        hstate += " Защита " + data[i].DefenceState.Defence;

                    if (data[i].DefenceState.NaturalDefence != 0)
                        hstate += " Нат. защита " + data[i].DefenceState.NaturalDefence;

                    if (data[i].DefenceState.ArmorDefence != 0)
                        hstate += " Физ. защита " + data[i].DefenceState.ArmorDefence;

                    if (data[i].DefenceState.MagicDefence != 0)
                        hstate += " Маг. защита " + data[i].DefenceState.MagicDefence;

                    if (data[i].DefenceState.DefenceBonus != 0)
                        hstate += " Бонус защиты " + data[i].DefenceState.DefenceBonus;

                    if (data[i].DefenceState.DefenceClass != 0)
                        hstate += " Класс защиты " + data[i].DefenceState.DefenceClass;

                    innerhtml += '<tr><td> Показ. защиты:' + hstate + '</td></tr>';
                }
                if (data[i].AttackState != null) {
                    var hstate = "";

                    if (data[i].AttackState.Attack != 0)
                        hstate += " Атака " + data[i].AttackState.Attack;

                    if (data[i].AttackState.FitAttack != 0)
                        hstate += " Доп. атака " + data[i].AttackState.FitAttack;

                    if (data[i].AttackState.WeaponMinimalDamage != 0 || data[i].AttackState.WeaponMaximalDamage != 0)
                        hstate += " Урон " + data[i].AttackState.WeaponMinimalDamage + "-" + data[i].AttackState.WeaponMaximalDamage;

                    if (data[i].AttackState.CritChance != 0)
                        hstate += " Крит. шанс " + data[i].AttackState.CritChance;

                    if (data[i].AttackState.CritBonus != 0)
                        hstate += " Крит. бонус " + data[i].AttackState.CritBonus;

                    innerhtml += '<tr><td> Показ. атаки:' + hstate + '</td></tr>';
                }
                if (data[i].CommonState != null) {
                    var hstate = "";

                    if (data[i].CommonState.Initiative != 0)
                        hstate += " Инициатива " + data[i].CommonState.Initiative;

                    if (data[i].CommonState.InitiativeSize != 0)
                        hstate += " Иниц. размера " + data[i].CommonState.InitiativeSize;

                    if (data[i].CommonState.InitiativeWisdom != 0)
                        hstate += " Иниц. мудрости " + data[i].CommonState.InitiativeWisdom;

                    if (data[i].CommonState.InitiativeMagic != 0)
                        hstate += " Иниц. магич. " + data[i].CommonState.InitiativeMagic;

                    if (data[i].CommonState.Speed != 0)
                        hstate += " Скорость " + data[i].CommonState.Speed;

                    if (data[i].CommonState.SpeedFit != 0)
                        hstate += " Доп. скорость " + data[i].CommonState.SpeedFit;

                    innerhtml += '<tr><td> Показ. общие:' + hstate + '</td></tr>';
                }
                if (data[i].Skills != null) {
                    var hstate = "";

                    for (var i = 0; i < data[i].Skills.length; i++) {
                        hstate += data[i].Skills[i].SkillName.Value + " " + data[i].Skills[i].Value + " | ";
                    }

                    innerhtml += '<tr><td> Навыки:' + hstate + '</td></tr>';
                }
                var addithtml = "";
                if (data[i].Class != null) {
                    addithtml += "Класс : " + data[i].Class.Value + " | ";
                }
                if (data[i].Race != null) {
                    addithtml += "Раса : " + data[i].Race.Value + " | ";
                }
                if (data[i].Height != null) {
                    addithtml += "Рост : " + data[i].Height.Value + " | ";
                }
                if (data[i].Sex != null) {
                    addithtml += "Пол : " + data[i].Sex.Value + " | ";
                }
                if (addithtml != "")
                    innerhtml += '<tr><td> Дополнительно:' + hstate + '</td></tr>';

                if (data[i].IsOnHero)
                    innerhtml += "<tr><td>На герое</tr></td>";
                else
                    innerhtml += "<tr><td>В рюкзаке</tr></td>";

                innerhtml += "</tbody></table></div>";
                innerhtml += "<br/>";
            }
            innerhtml += "</div></div>";
            Play.Container.Show('fa-list-alt', "Инвентарь", 'default', innerhtml);
        });
    };
    return o;
}

function ContainerManager() {
    var o = new Object();

    o.Show = function (Icon, Title, ColorBorder, Inner) {
        var html = "";
        html += "<div class='panel panel-back noti-box'>";
        html += "<button type='button' class='close' style='padding-right:2%;' data-dismiss='alert' aria-hidden='true'>×</button>";
        html += "<span class='icon-box bg-color-corporative'>";
        html += "	<i class='fa " + Icon + "'></i>";
        html += "</span>";
        html += "<div class='text-box'>";
        html += "	<p class='main-text'> " + Title + "</p>";
        html += "	<hr>";
        html += "	<p class='text-muted'>";
        html += "		<div class='panel panel-" + ColorBorder + "'>";
        html += Inner;
        html += "		</div>";
        html += "	</p>";
        html += "</div>";
        html += "</div>";
        $('#info').html(html);
    };

    return o;
}