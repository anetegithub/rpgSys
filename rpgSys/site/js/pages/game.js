$(function () {
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
        NewLobby.Role.Visible(false);
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
        alert('yep');
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
    mb.ConnectBtn = null;
    mb.CreateBtn = null;
    mb.StartBtn = null;
    mb.DeleteBtn = null;
    mb.Init = function () {
        mb.ConnectBtn = Button('#btnConnect');
        mb.ConnectBtn.OnClick = function () {

        };
    };
    

    return mb;
}

function Button(Selector) {
    var btn = new Object();

    btn.Selector = Selector;
    btn.Visible = function (bool) {
        if (bool) {
            $(this.Selector).css('display', 'block');
            return true;
        } else {
            $(this.Selector).css('display', 'none');
        }
    };
    btn.Disable = function (bool) {
        if (bool) {
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
                        html += "<span class='list-group-item' style='cursor:pointer' onclick='Lobby.Scenario.SelectedScenario=ScenarioMiniObject(" + data[i].Id + ",\"" + data[i].Text + "\");'>";
                        html += "<i class='fa fa-fw fa-comments'></i>&nbsp;";
                        html += data[i].Text + "</span>";
                    }
                    $('#scenarioList').html(html);
                    sb.Label = "Доступные сценарии";
                });
    };
    sb.ShowGames = function () {
        $.getJSON('../api/game/list')
                .done(function (data) {
                    var html = "";
                    for (var i = 0; i < data.length; i++) {
                        html += "<span class='list-group-item' style='cursor:pointer' onclick='Lobby.Scenario.SelectedScenario=ScenarioMiniObject(" + data[i].Id + ",\"" + data[i].Text + "\");'>";
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

    return rb;
}

function ScenarioMiniObject(Id,Name){
    var cmo=new Object();
    cmo.Id=Id;
    cmo.Name = Name;
    return cmo;
}

//constructor
function PageController(IsNew) {

    var pc = new Object();

    pc.IsNew = false;
    pc.Game = null;
    pc.HeroList = NewHeroList();

    pc.Initialize = function () {
        var user = JSON.parse($.cookie("user"));
        if (user.GameId != 0) {
            if (this.Game.Active) {
                $('#old').css('display', 'block');
            }
            else {
                $('#new').css('display', 'block');
                IsNew = true;
                if (this.Game.Scenario != null)
                    this.Part1();
            }
        } else {
            $('#new').css('display', 'block');
            IsNew = true;
        }
    };

    pc.Part1 = function () {
        $('#newPart1').css('display', 'block');
        $('#scenarioList').html("<span class='list-group-item'><i class='fa fa-fw fa-comments'></i>&nbsp;" + pc.Game.Scenario.Title + "</span>");
        this.Part2();
    };

    pc.Part2 = function () {
        $('#newPart2').css('display', 'block');
        $('#scenarioList').html("<span class='list-group-item'><i class='fa fa-fw fa-user'></i>&nbsp; Мастер</span>");
        this.Part3();
    };

    pc.Part3 = function () {
        $('#newPart3').css('display', 'block');
        $('#newPart3Title').html("Создать и начать");
        this.HeroList.SetList(this.Game.Heroes);
    };

    var user = JSON.parse($.cookie("user"));
    $.getJSON('../api/game/GameId='+user.GameId).done(function (data) {
        pc.Game = data;
        pc.Initialize();
    });
};

function ButtonManager() {
    var BtnMng = new Object();

    BtnMng.Init = function (Game) {
        var user = JSON.parse($.cookie("user"));

        if (user.GameId == 0) {
            $('#btnCreate').css('display', 'block');
            $('#btnStart').css('display', 'block');
            $('#btnDelete').css('display', 'block');

            $('#btnCreate').click(this.Create());

            $('#btnStart').click(function () { });
            $('#btnStart').addClass("disabled");
            $('#btnDelete').click(function () { });
            $('#btnDelete').addClass("disabled");
        } else {

        }
    };

    BtnMng.Create = function () {
        var user = JSON.parse($.cookie("user"));
        var data = NewGame();
        data.Master.Id = user.Id;
        data.Scenario.Id = parseInt($('#newPart1Value'));
        $.post('../api/game/create', { '': JSON.stringify(data) })
            .done(function (Game) {
                if (Game.Id != 0) {
                    $('#btnStart').removeClass("disabled");
                    $('#btnStart').click(BtnMng.Start());

                    $('#btnDelete').removeClass("disabled");
                    $('#btnDelete').click(BtnMng.Delete());
                } else {
                    alert('Произошла ошибка при создании игры, попробуйте позже или отправьте баг-репорт!');
                    window.location.replace('profile');
                }
            });
    }

    BtnMng.Start = function (Game) {
        $.getJSON('../api/game/start?GameId=' + Game.Id).done(function (data) {
            //UPDATE all clients to exit lobby and start game
        });
    }

    BtnMng.Delete = function (Game) {
        $.getJSON('../api/game/delete?GameId=' + Game.Id).done(function (data) {
            //UPDATE all clients to reset lobby and send msg game is deleted
        });
    }

    BtnMng.Connect = function (Game) {
        var user = JSON.parse($.cookie("user"));
        $.getJSON('../api/game/connect?GameId=' + Game.Id + "&UserId=" + user.Id).done(function (data) {
            //UPDATE all clients to reset lobby and send msg game is deleted
        });
    }
}

function NewGame() {
    var Game = new Object();
    Game.Master = { Id: 0 };
    Game.Heroes = [];
    Game.Scenario = { Id: 0 };
    return Game;
}

function NewHeroList(Game) {

    var HeroList = new Object;

    HeroList.SetList = function (Heroes) {
        var html = "";
        for (var i = 0; i < Heroes; i++) {
            html += "<span class='list-group-item'><i class='fa fa-fw fa-" + (Heroes[i].Sex.Id == 0 ? "male" : "female") + "'></i>&nbsp; " + Heroes[i].Name + "</span>";
        }
        $('#newPart3HeroList').html(html);
    };

    HeroList.AddToList = function (Hero) {
        var html = "<span class='list-group-item'><i class='fa fa-fw fa-" + (Hero.Sex.Id == 0 ? "male" : "female") + "'></i>&nbsp; " + Hero.Name + "</span>";
        $('#newPart3HeroList').html($('#newPart3HeroList').html() + html);
    }

    HeroList.DeleteFromList = function (HeroName) {
        //foreach child where name==HeroName remove from this
    }

    return HeroList;
}