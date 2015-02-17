
//$(function () {
//    function outingDataViewModel() {
//        var self = this;
//        self.hub = $.connection.msg;
//    }
//    var vm = new outingDataViewModel();

//    $.connection.hub.start();

//    $.get("/api/chat", function (items) {
//        $.each(items, function (idx, item) {
//            $('#chat').html($('#chat').html() + item.text);
//        });
//    }, "json");
//});

//function newmessage() {
    //$.ajax({
    //    url: "/api/chat",
    //    data: { 'Text': 'New text', 'Master': false, 'System': false },
    //    type: "POST",
    //    done: update()
    //});
//}

//function update() {
//    $('#chat').html('');
//    $.get("/api/chat", function (items) {
//        $.each(items, function (idx, item) {
//            $('#chat').html($('#chat').html() + item.text);
//        });
//    }, "json");
//}

$(function () {
    // Declare a proxy to reference the hub. 
    var chat = $.connection.msg;

    // Create a function that the hub can call to broadcast messages.
    chat.client.additem = function (name, message) {
        $.ajax({
            url: "/api/chat",
            data: { 'Text': 'New text', 'Master': false, 'System': false },
            type: "POST"
        });
    };

    // Start the connection.
    $.connection.hub.start().done(function () {
        $('#send_btn').click(function () {            
            chat.server.message('n','m');
        });
    });
});