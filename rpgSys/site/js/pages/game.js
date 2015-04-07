$(function () {

    $('#userAvatar').attr('src', user.Avatar);
    $('#lastTime').html("Последняя авторизация: " + user.Stamp);

    Lobby = GameLobby();

    //Sync
    var synclobby = $.connection.lobby;

    synclobby.client.gamedelete  = function (GameId) {
        var user = JSON.parse($.cookie("user"));
        if (user.GameId == GameId && GameId != 0) {
            user.GameId = 0;
            $.cookie('user', JSON.stringify(user), { path: '/site/' });
            alert('Приключение отменено!');
            window.location.replace('profile');
        }
    };
    synclobby.client.gamestart = function (GameId) {
        var user = JSON.parse($.cookie("user"));
        if (user.GameId = GameId && GameId != 0)
            window.location.reload();
    };
    synclobby.client.listupdate = function (GameId) {
        var user = JSON.parse($.cookie("user"));
        if (user.GameId == GameId && GameId != 0) {
            Lobby.MagicBlock.ShowList();
        }
    }
    synclobby.client.syncheroexit = function (GameId, Hero) {
        var user = JSON.parse($.cookie("user"));
        if (user.GameId == GameId && GameId != 0) {
            Play.Players.Remove(Hero);
        }
    };
    synclobby.client.syncgamedelete = function (GameId) {
        var user = JSON.parse($.cookie("user"));
        if (user.GameId == GameId && GameId != 0) {
            user.GameId = 0;
            $.cookie('user', JSON.stringify(user), { path: '/site/' });
            alert('Приключение закончено!');
            window.location.replace('profile');
        }
    };

    var gamehub = $.connection.game;

    gamehub.client.addherotolist = function (Hero) {
        Play.Players.Add(Hero);
    };
    gamehub.client.removefromlist = function (Hero) {
        Play.Players.Remove(Hero);
    };
    gamehub.client.addgamemsg = function (Msg) {
        Play.Chat.Add(Msg);
    }


    $.connection.hub.start().done(function () {
        Lobby.SyncUpdate = function (GameId) {
            synclobby.server.listupdate(GameId);
        };
        Lobby.SyncStart = function (GameId) {
            synclobby.server.gamestart(GameId);
        };
        Lobby.SyncDelete = function (GameId) {
            synclobby.server.gamedelete(GameId);
        };
        Lobby.SyncHeroExit = function (GameId, HeroId) {
            synclobby.server.syncheroexit(GameId, HeroId);
        };
        Lobby.SyncGameDelete = function (GameId) {
            synclobby.server.syncgamedelete(GameId);
        };

        $('#send_btn').click(function () {
            if ($('#textFoSend').val() != '') {
                if ($('#textFoSend').val().replace(/\s/g, '').length) {
                    gamehub.server.sendmsg(user.GameId, user.Login, user.Avatar, "1", $('#textFoSend').val());
                    $('#textFoSend').val('');
                }
            }
        });
    });    
    
    Lobby.IsState();
});
var user = JSON.parse($.cookie("user"));
var Lobby = null;

function GameLobby() {
    var NewLobby = new Object();
    NewLobby.Main = MainBlock();
    NewLobby.Main.Init();

    NewLobby.Main.Change = function (newval) {
        $('#showNextBlock').removeClass('bg-color-corporative');
        NewLobby.Scenario.Ain("");
        NewLobby.Role.Visible(false);
        NewLobby.MagicBlock.Visible(false);
        NewLobby.Scenario.Visible(true);
        if (newval == 0) {
            NewLobby.Scenario.ShowTemplates();
            NewLobby.Role.Label = "Мастер";
        } else {
            NewLobby.Scenario.ShowGames();
            NewLobby.Role.Label = "Герой";
        }
    };

    NewLobby.Scenario = ScenarioBlock();
    NewLobby.Scenario.Change = function (newval) {
        NewLobby.Role.Visible(true);
        if ($('#showNextBlock').attr('class').indexOf('bg-color-corporative') > -1) {
            $('#showNextBlock').removeClass('bg-color-corporative');
            NewLobby.MagicBlock.Visible(false);
        }
        
    };

    NewLobby.Role = RoleBlock();
    NewLobby.Role.OnClick = function () {
        if (NewLobby.Main.Value == 0) {
            $('#showNextBlock').addClass('bg-color-corporative');
            NewLobby.MagicBlock.ShowFakeList();
            NewLobby.MagicBlock.Visible(true);
            NewLobby.MagicBlock.Label = "Создать приключение";
            NewLobby.MagicBlock.CreateBtn.Visible(true);
            NewLobby.MagicBlock.CreateBtn.Enabled(true);
            NewLobby.MagicBlock.StartBtn.Visible(true);
            NewLobby.MagicBlock.DeleteBtn.Visible(true);
            NewLobby.MagicBlock.ConnectBtn.Visible(false);
            NewLobby.MagicBlock.ExitBtn.Visible(false);
        } else {
            $('#showNextBlock').addClass('bg-color-corporative');
            NewLobby.MagicBlock.GameId = NewLobby.Scenario.SelectedScenario.Id;
            NewLobby.MagicBlock.ShowList();
            NewLobby.MagicBlock.Visible(true);
            NewLobby.MagicBlock.Label = "Присоеденились";
            NewLobby.MagicBlock.ConnectBtn.Visible(true);
            NewLobby.MagicBlock.ConnectBtn.Enabled(true);
            NewLobby.MagicBlock.CreateBtn.Visible(false);
            NewLobby.MagicBlock.StartBtn.Visible(false);
            NewLobby.MagicBlock.DeleteBtn.Visible(false);
            NewLobby.MagicBlock.ExitBtn.Visible(false);

        }
    };
    NewLobby.Role.Init();

    NewLobby.MagicBlock = MagicBlock();
    NewLobby.MagicBlock.Init();    
    NewLobby.SyncUpdate = function (GameId) { };
    NewLobby.SyncHeroExit = function (GameId, HeroId) { };
    NewLobby.SyncStart = function (GameId) { };
    NewLobby.SyncDelete = function (GameId) { };
    NewLobby.SyncGameDelete = function (GameId) { };
    NewLobby.MagicBlock.ConnectBtn.OnClick = function () {
        var user = JSON.parse($.cookie("user"));
        $.getJSON('../api/game/connect?GameId=' + NewLobby.Scenario.SelectedScenario.Id + "&UserId=" + user.Id)
            .done(function (data) {
                if (data == "true") {                    
                    var user = JSON.parse($.cookie("user"));
                    user.GameId = NewLobby.Scenario.SelectedScenario.Id;
                    $.cookie('user', JSON.stringify(user), { path: '/site/' });
                    NewLobby.SyncUpdate(NewLobby.Scenario.SelectedScenario.Id);
                    window.location.reload();
                } else if (data == "NoHero") {
                    alert('У вас должен быть персонаж!');
                    window.location.replace('hero');
                } else
                    NewLobby.MagicBlock.Break();
            });
    };
    NewLobby.MagicBlock.ConnectBtn.Init();
    NewLobby.MagicBlock.CreateBtn.OnClick = function () {
        var user = JSON.parse($.cookie("user"));
        var Game = NewGame();
        Game.Master.UserId = user.Id;
        Game.Scenario.Id = NewLobby.Scenario.SelectedScenario.Id;
        if (user.HeroId != 0) {
            $.post('../api/game/create', { '': JSON.stringify(Game) }).done(function (data) {
                if (data.Id != 0) {
                    var user = JSON.parse($.cookie("user"));
                    user.GameId = data.Id;
                    $.cookie('user', JSON.stringify(user), { path: '/site/' });
                    NewLobby.MagicBlock.Label = "Запустить приключение"
                    NewLobby.MagicBlock.CreateBtn.Enabled(false);
                    NewLobby.MagicBlock.DeleteBtn.Enabled(true);
                    NewLobby.MagicBlock.StartBtn.Enabled(true);
                    window.location.reload();
                }
                else
                    NewLobby.MagicBlock.Break();
            });
        } else {
            alert('У вас должен быть персонаж!');
            window.location.replace('hero');
        }
    };
    NewLobby.MagicBlock.CreateBtn.Init();
    NewLobby.MagicBlock.StartBtn.OnClick = function () {
        var user = JSON.parse($.cookie("user"));
        $.getJSON('../api/game/start?GameId=' + user.GameId)
            .done(function (data) {
                if (data == "true") {
                    NewLobby.SyncStart(user.GameId);
                    window.location.reload();
                }
                else
                    NewLobby.MagicBlock.Break();
            });
    };
    NewLobby.MagicBlock.StartBtn.Init();
    NewLobby.MagicBlock.DeleteBtn.OnClick = function () {
        var user = JSON.parse($.cookie("user"));
        $.getJSON('../api/game/delete?GameId=' + user.GameId)
            .done(function (data) {
                if (data == "true") {                    
                    var user = JSON.parse($.cookie("user"));
                    NewLobby.SyncDelete(user.GameId);
                    user.GameId = 0;
                    $.cookie('user', JSON.stringify(user), { path: '/site/' });                    
                    window.location.replace('profile');
                }
                else
                    NewLobby.MagicBlock.Break();
            });
    };
    NewLobby.MagicBlock.DeleteBtn.Init();
    NewLobby.MagicBlock.ExitBtn.OnClick = function () {
        var user = JSON.parse($.cookie("user"));
        $.getJSON('../api/game/exit?GameId=' + user.GameId + "&UserId=" + user.Id)
            .done(function (data) {
                if (data == "true") {
                    var user = JSON.parse($.cookie("user"));
                    var gameid = user.GameId;
                    user.GameId = 0;
                    $.cookie('user', JSON.stringify(user), { path: '/site/' });
                    NewLobby.SyncUpdate(gameid);
                    window.location.reload();
                } else if (data == "NoHero") {
                    alert('У вас должен быть персонаж!');
                    window.location.replace('hero');
                } else
                    NewLobby.MagicBlock.Break();
            });
    }
    NewLobby.MagicBlock.ExitBtn.Init();
        

    NewLobby.IsState = function () {
        var user = JSON.parse($.cookie("user"));
        if (user.GameId != 0) {
            $.getJSON('../api/game/byid?Id=' + user.GameId)
            .done(function (data) {
                if (data.Id != 0) {
                    if (!data.IsActive) {
                        if (data.Master.Id == user.HeroId) {
                            $('#newPt0btn0').css('cursor', 'cross');
                            $('#newPt0btn1').css('cursor', 'cross');
                            $('#newPt0btn0').unbind("click");
                            $('#newPt0btn1').unbind("click");
                            NewLobby.Scenario.ShowTemplates();
                            NewLobby.Scenario.Visible(true);
                            NewLobby.Scenario.OnShow = function () {
                                NewLobby.Scenario.SelectThisItem('#' + data.Scenario.Id + 'ScListItem');
                                NewLobby.Scenario.Ain(data.Scenario.Fable);
                                NewLobby.Scenario.DisableList();
                            };
                            NewLobby.Role.Visible(true);
                            $('#showNextBlock').unbind("click");
                            $('#showNextBlock').css('cursor', 'cross');

                            $('#showNextBlock').addClass('bg-color-corporative');
                            NewLobby.MagicBlock.Visible(true);
                            NewLobby.MagicBlock.GameId = data.Id;
                            NewLobby.MagicBlock.Label = "Запустить игру";                            
                            NewLobby.MagicBlock.StartBtn.Visible(true);
                            NewLobby.MagicBlock.StartBtn.Enabled(true);
                            NewLobby.MagicBlock.DeleteBtn.Visible(true);
                            NewLobby.MagicBlock.DeleteBtn.Enabled(true);
                            NewLobby.MagicBlock.ShowList();

                            //WAIT UNTILL PLAYERS ADDED

                            $('html, body').animate({ scrollTop: $(document).height() });
                        } else {
                            $('#newPt0btn0').css('cursor', 'cross');
                            $('#newPt0btn1').css('cursor', 'cross');
                            $('#newPt0btn0').unbind("click");
                            $('#newPt0btn1').unbind("click");
                            NewLobby.Scenario.ShowGames();
                            NewLobby.Scenario.Visible(true);
                            NewLobby.Scenario.OnShow = function () {
                                NewLobby.Scenario.SelectThisItem('#' + data.Id + 'ScListItem');
                                NewLobby.Scenario.Ain(data.Scenario.Recomendation);
                                NewLobby.Scenario.DisableList();
                            };
                            NewLobby.Role.Visible(true);
                            $('#showNextBlock').unbind("click");
                            $('#showNextBlock').css('cursor', 'cross');

                            $('#showNextBlock').addClass('bg-color-corporative');
                            NewLobby.MagicBlock.Visible(true);
                            NewLobby.MagicBlock.GameId = data.Id;
                            NewLobby.MagicBlock.Label = "Присоеденились";
                            NewLobby.MagicBlock.ExitBtn.Visible(true);
                            NewLobby.MagicBlock.ExitBtn.Enabled(true);
                            NewLobby.MagicBlock.ShowList();
                        }
                    } else {
                        $('#new').css('display', 'none');
                        $('#old').css('display', 'block');
                    }
                }
            });
        }
    };

    return NewLobby;
}

function MagicBlock() {
    var mb = new Object();

    mb.GameId = 0;
    mb.Selector = '#magicBlock';
    mb.Visible = function (bool) {
        if (bool) {
            $(this.Selector).css('display', 'block');
            return true;
        } else {
            $(this.Selector).css('display', 'none');
        }
    };
    mb.Label = "Unknown";
    mb.watch('Label', function (id, oldval, newval) {
        $('#magicUsersName').html(newval);
        return newval;
    })
    mb.SubLabel="Unknown";
    mb.watch('SubLabel', function (id, oldval, newval) {
        $('#infoUsersmagicBlock').html(newval);
        return newval;
    });
    mb.Start=false;
    mb.watch('Start', function (id, oldval, newval) {
        //здесь надо начать игру
        return newval;
    });
    mb.SubLabelVisible=function(bool){
        if (bool) {
            $('#infoUsersmagicBlock').css('display', 'block');
            return true;
        } else {
            $('#infoUsersmagicBlock').css('display', 'none');
        }
    };
    mb.ShowList = function () {
        $.getJSON('../api/game/heroes?GameId=' + this.GameId)
                .done(function (data) {
                    var html = "";
                    for (var i = 0; i < data.length; i++) {
                        html += "<span class='list-group-item'><i class='fa fa-fw fa-" + (data[i].Id == 1 ? "" : "fe") + "male'></i>&nbsp;" + data[i].Text + "</span>";
                    }
                    $('#magicUsersList').html(html);
                    if (data.length == 0)
                        mb.ConnectBtn.Enabled(false);
                });
    };
    mb.ShowFakeList = function () {
        var user = JSON.parse($.cookie("user"));
        $.getJSON('../api/hero?UserId=' + user.Id)
                 .done(function (data) {
                     $('#magicUsersList').html("<span class='list-group-item'><i class='fa fa-fw fa-" + (data.Id == 0 ? "" : "fe") + "male'></i>&nbsp;" + data.Name + "</span>");
                 });
    };
    mb.ConnectBtn = null;
    mb.CreateBtn = null;
    mb.StartBtn = null;
    mb.DeleteBtn = null;
    mb.ExitBtn = null;
    mb.Break = function () {
        alert("Произошла ошибка! Отправьте баг-репорт!");
        window.location("profile");
    };
    mb.Init = function () {
        this.ConnectBtn = Button('#btnConnect');
        this.CreateBtn = Button('#btnCreate');
        this.StartBtn = Button('#btnStart');
        this.DeleteBtn = Button('#btnDelete');
        this.ExitBtn = Button('#btnExit');
    };

    return mb;
}

function Button(Selector) {
    var btn = new Object();

    btn.Selector = Selector;
    btn.Visible = function (bool) {
        if (bool) {
            $(this.Selector).css('display', 'inline');
            return true;
        } else {
            $(this.Selector).css('display', 'none');
        }
    };
    btn.Enabled = function (bool) {
        if (!bool) {
            $(this.Selector).addClass('disabled');
            return true;
        } else {
            $(this.Selector).removeClass('disabled');
        }
    };
    btn.Label = "Unknown";
    btn.watch('Label', function (id, oldval, newval) {
        $(btn.Selector).html(newval);
        return newval;
    })
    btn.OnClick = function () { };
    btn.Init = function () {
        $(btn.Selector).click(btn.OnClick);
    }

    return btn;
}

function MainBlock() {
    var mb = new Object();
    mb.Value = 0;
    mb.LeftPanel = null;
    mb.RightPanel = null;
    mb.watch('Value', function (id, oldval, newval) {
        mb.Change(newval);
        return newval;
    })
    mb.Change = function () { };
    mb.Init = function () {
        this.LeftPanel = Panel('#newPt0btn0');
        this.RightPanel = Panel('#newPt0btn1');

        mb.LeftPanel.click(function () {
            mb.Value = 0;
        });
        mb.RightPanel.click(function () {
            mb.Value = 1;
        });
    }
    return mb;
}

function Panel(sourceId) {
    return $(sourceId);
}

function ScenarioBlock() {
    var sb = new Object();
    sb.Selector = '#scenarioBlock';
    sb.Ain = function (Label) {
        $('#scenarioRecomendation').html(Label);
    };
    sb.Visible = function (bool) {
        if (bool) {
            $(this.Selector).css('display', 'block');
            return true;
        } else {
            $(this.Selector).css('display', 'none');
        }
    };
    sb.SelectedScenario = ScenarioMiniObject(0, 'Unknown');
    sb.watch('SelectedScenario', function (id, oldval, newval) {
        sb.Change(newval);
        return newval;
    })
    sb.Change = function () { };
    sb.OnShow = function () { };
    sb.ShowTemplates = function () {
        $.getJSON('../api/scenario/list')
                .done(function (data) {
                    var html = "";
                    for (var i = 0; i < data.length; i++) {
                        html += "<span id='" + data[i].Id + "ScListItem' class='list-group-item' ScenarioId='"+data[i].Id+"' style='cursor:pointer' onclick='Lobby.Scenario.SelectedScenario=ScenarioMiniObject(" + data[i].Id + ",\"" + data[i].Text + "\"); Lobby.Scenario.SelectThisItem(\"#" + data[i].Id + "ScListItem\"); Lobby.Scenario.Ain(\""+data[i].Param1+"\");'>";
                        html += "<i class='fa fa-fw fa-comments'></i>&nbsp;";
                        html += data[i].Text + "</span>";
                    }
                    $('#scenarioList').html(html);
                    sb.Label = "Доступные сценарии";
                    sb.OnShow();
                });
    };
    sb.DisableList = function () {
        for (var i = 0; i < $('#scenarioList')[0].children.length; i++) {
            $($('#scenarioList')[0].children[i]).css('cursor', 'cross');
            $($('#scenarioList')[0].children[i]).unbind("click");
            $($('#scenarioList')[0].children[i]).removeAttr("onclick");
        }
    };
    sb.SelectThisItem = function (itemId) {        
        for (var i = 0; i < $('#scenarioList')[0].children.length; i++)
            $($('#scenarioList')[0].children[i]).removeClass('bg-color-corporative');
        $(itemId).addClass('bg-color-corporative');
    };
    sb.ShowGames = function () {
        $.getJSON('../api/game/list')
                .done(function (data) {
                    var html = "";
                    for (var i = 0; i < data.length; i++) {
                        html += "<span id='" + data[i].Id + "ScListItem' class='list-group-item' ScenarioId='" + data[i].Id + "' style='cursor:pointer' onclick='Lobby.Scenario.SelectedScenario=ScenarioMiniObject(" + data[i].Id + ",\"" + data[i].Text + "\"); Lobby.Scenario.SelectThisItem(\"#" + data[i].Id + "ScListItem\"); Lobby.Scenario.Ain(\"" + data[i].Param1 + "\");'>";
                        html += "<i class='fa fa-fw fa-comments'></i>&nbsp;";
                        html += data[i].Text + "</span>";
                    }
                    $('#scenarioList').html(html);
                    sb.Label = "Доступные игры";
                    sb.OnShow();
                });
    };
    sb.Label = "Unknown";
    sb.watch('Label', function (id, oldval, newval) {
        $('#scenarioLabel').html(newval);
        return newval;
    })
    return sb;
}

function RoleBlock() {
    var rb = new Object();

    rb.Selector = '#roleBlock';
    rb.Visible = function (bool) {
        if (bool) {
            $(this.Selector).css('display', 'block');
            return true;
        } else {
            $(this.Selector).css('display', 'none');
        }
    };
    rb.Label = "Unknown";
    rb.watch('Label', function (id, oldval, newval) {
        $('#roleName').html(newval);
        return newval;
    })
    rb.OnClick = function () { };
    rb.Init = function () {
        $('#showNextBlock').click(this.OnClick);
    }

    return rb;
}

function ScenarioMiniObject(Id,Name){
    var cmo=new Object();
    cmo.Id=Id;
    cmo.Name = Name;
    return cmo;
}

function NewGame() {
    var Game = new Object();
    Game.UserId = 0;
    Game.Master = { Id: 0 };
    Game.Heroes = [];
    Game.Scenario = { Id: 0 };
    return Game;
}