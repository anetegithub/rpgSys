/// <reference path="jquery-1.7.2.js" />
/// <reference path="knockout-2.1.0.js" />

function ToDoViewModel() {
    var self = this;

    function MessageItem(root, id,text,master,system) {
        var self = this,
            updating = false;

        self.id = id;
        self.text = ko.observable(text);
        self.master = ko.observable(master);
        self.system = ko.observable(system);

        self.remove = function () {
            root.sendDelete(self);
        };

        self.update = function (title, finished) {
            updating = true;
            self.title(title);
            self.finished(finished);
            updating = false;
        };

        self.finished.subscribe(function () {
            if (!updating) {
                root.sendUpdate(self);
            }
        });
    };

    self.addItemTitle = ko.observable("");
    self.items = ko.observableArray();

    self.add = function (id, title, finished) {
        self.items.push(new MessageItem(self, id, title, finished));
    };

    self.sendCreate = function () {
        $.ajax({
            url: "/api/chat",
            data: { 'Title': self.addItemTitle(), 'Finished': false },
            type: "POST"
        });

        self.addItemTitle("");
    };
};

$(function () {
    var viewModel = new ToDoViewModel(),
        hub = $.connection.msg;

    ko.applyBindings(viewModel);

    hub.addItem = function (item) {
        viewModel.add(item.ID, item.Title, item.Finished);
    };

    $.connection.hub.start();

    $.get("/api/chat", function (items) {
        $.each(items, function (idx, item) {
            viewModel.add(item.ID, item.Title, item.Finished);
        });
    }, "json");
});
