﻿$(function () {
    var user = JSON.parse($.cookie("user"));
    $('#userAvatar').attr('src', user.Avatar);
    $('#lastTime').html("Последняя авторизация: " + user.Stamp);
});

var Lobby = GameLobby();

function GameLobby() {
    var NewLobby = new Object();
    NewLobby.Main = MainBlock();
    NewLobby.Main.Init();

    NewLobby.Main.Change = function (newval) {
        $('#showNextBlock').removeClass('bg-color-corporative');
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
    };    

    NewLobby.Role = RoleBlock();
    NewLobby.Role.OnClick = function () {
        if (NewLobby.Main.Value == 0) {
            $('#showNextBlock').addClass('bg-color-corporative');
            NewLobby.MagicBlock.ShowFakeList();
            NewLobby.MagicBlock.Visible(true);
            NewLobby.MagicBlock.Label = "Создать и запустить приключение";
            NewLobby.MagicBlock.CreateBtn.Visible(true);
            NewLobby.MagicBlock.CreateBtn.Enabled(true);
            NewLobby.MagicBlock.StartBtn.Visible(true);
            NewLobby.MagicBlock.DeleteBtn.Visible(true);
        } else {
            $('#showNextBlock').addClass('bg-color-corporative');
            NewLobby.MagicBlock.GameId = NewLobby.Scenario.SelectedScenario.Id;
            NewLobby.MagicBlock.ShowList();
            NewLobby.MagicBlock.Visible(true);
            NewLobby.MagicBlock.Label = "Присоеденились";
            NewLobby.MagicBlock.ConnectBtn.Visible(true);
            NewLobby.MagicBlock.ConnectBtn.Enabled(true);
        }
    };
    NewLobby.Role.Init();

    NewLobby.MagicBlock = MagicBlock();
    NewLobby.MagicBlock.Init();
    NewLobby.MagicBlock.ConnectBtn.OnClick = function () {
        var user = JSON.parse($.cookie("user"));
        $.getJSON('../api/game/connect?GameId=' + NewLobby.Scenario.SelectedScenario.Id + "&UserId=" + User.Id)
            .done(function (data) {
                if (data == "true") {
                    //WAIT UNTIL GAME STARTED
                    NewLobby.MagicBlock.SubLabel = "Вы присоеденились к игре, ждите старта...";
                }
                else
                    NewLobby.MagicBlock.Break();
            });
    };
    NewLobby.MagicBlock.ConnectBtn.Init();
    NewLobby.MagicBlock.CreateBtn.OnClick = function () {
        var user = JSON.parse($.cookie("user"));
        var Game = NewGame();
        Game.Master.Id = user.Id;
        Game.Scenario.Id = NewLobby.Scenario.SelectedScenario.Id;
        $.post('../api/game/create', { '': JSON.stringify(Game) }).done(function (data) {
            if (data.Id != 0) {
                NewLobby.MagicBlock.CreateBtn.Enabled(false);
                NewLobby.MagicBlock.DeleteBtn.Enabled(true);
                NewLobby.MagicBlock.StartBtn.Enabled(true);
            }
            else
                NewLobby.MagicBlock.Break();
        });
    };
    NewLobby.MagicBlock.CreateBtn.Init();
    NewLobby.MagicBlock.StartBtn.OnClick = function () {
        var user = JSON.parse($.cookie("user"));
        $.getJSON('../api/game/start?GameId=' + NewLobby.Scenario.SelectedScenario.Id)
            .done(function (data) {
                if (data == "true") {                    
                    var user = JSON.parse($.cookie("user"));
                    user.GameId = NewLobby.Scenario.SelectedScenario.Id;
                    $.cookie('user', JSON.stringify(user), { path: '/site/' });
                    //SAY OTHERS GAME WAS STARTED
                    window.location.reload();
                }
                else
                    NewLobby.MagicBlock.Break();
            });
    };
    NewLobby.MagicBlock.StartBtn.Init();
    NewLobby.MagicBlock.DeleteBtn.OnClick = function () {
        var user = JSON.parse($.cookie("user"));
        $.getJSON('../api/game/delete?GameId=' + NewLobby.Scenario.SelectedScenario.Id)
            .done(function (data) {
                if (data == "true") {
                    //SAY OTHERS GAME WAS DELETED
                    window.location.reload();
                }
                else
                    NewLobby.MagicBlock.Break();
            });
    };
    NewLobby.MagicBlock.DeleteBtn.Init();

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
                        html += "<span class='list-group-item'><i class='fa fa-fw fa-" + (data[i].Id == 0 ? "" : "fe") + "male'></i>&nbsp;" + data[i].Text + "</span>";
                    }
                    $('#magicUsersList').html(html);
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
    mb.Break = function () {
        alert("Произошла ошибка! Отправьте баг-репорт!");
        window.location("profile");
    };
    mb.Init = function () {
        this.ConnectBtn = Button('#btnConnect');
        this.CreateBtn = Button('#btnCreate');
        this.StartBtn = Button('#btnStart');
        this.DeleteBtn = Button('#btnDelete')
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
    sb.ShowTemplates = function () {
        $.getJSON('../api/scenario/list')
                .done(function (data) {
                    var html = "";
                    for (var i = 0; i < data.length; i++) {
                        html += "<span id='" + data[i].Id + "ScListItem' class='list-group-item' style='cursor:pointer' onclick='Lobby.Scenario.SelectedScenario=ScenarioMiniObject(" + data[i].Id + ",\"" + data[i].Text + "\"); Lobby.Scenario.SelectThisItem(\"#" + data[i].Id + "ScListItem\")'>";
                        html += "<i class='fa fa-fw fa-comments'></i>&nbsp;";
                        html += data[i].Text + "</span>";
                    }
                    $('#scenarioList').html(html);
                    sb.Label = "Доступные сценарии";
                });
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
                        html += "<span id='" + data[i].Id + "ScListItem' class='list-group-item' style='cursor:pointer' onclick='Lobby.Scenario.SelectedScenario=ScenarioMiniObject(" + data[i].Id + ",\"" + data[i].Text + "\"); Lobby.Scenario.SelectThisItem(\"#" + data[i].Id + "ScListItem\")'>";
                        html += "<i class='fa fa-fw fa-comments'></i>&nbsp;";
                        html += data[i].Text + "</span>";
                    }
                    $('#scenarioList').html(html);
                    sb.Label = "Доступные игры";
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
    Game.Id = 0;
    Game.Master = { Id: 0 };
    Game.Heroes = [];
    Game.Scenario = { Id: 0 };
    return Game;
}