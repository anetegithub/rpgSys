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
                        Pla.IsMaster=true;
                    }
                    //$.connection.hub.start().done(function () {
                        
                    //});
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

    pm.Chat=ChatManager();

    pm.IsMaster=false;

    pm.HeroLoaded = false;
    pm.Hero = function (getset) {
        if (getset = "get") {
            if (!pm.HeroLoaded) {
                $.getJSON('../api/hero?UserId=' + user.Id)
                .done(function (data) {
                    localStorage.setItem('hero', JSON.stringify(data));
                    pm.LabelHero = "Герой: " + JSON.parse(localStorage.getItem('hero')).Name;
                    pm.HeroLoaded = true;
                });
            } else
                return JSON.parse(localStorage.getItem('hero'));
        } else if (getset = "set") {

        };
    };

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
        pm.Hero("get");
        pm.Chat.Init();
    }

    return pm;
};

function ChatManager(){
    var cm = new Object();
    cm.Send = function () { };
    cm.SendCustom = function () { };
    cm.Init = function () {
        $('#send_btn').click(cm.Send);
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