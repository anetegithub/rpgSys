function update() {
    $('#chat').html('');
    $.get("/api/play", function (items) {
        $.each(items, function (idx, item) {
            $('#chat').html($('#chat').html() +"<label>"+item+"</label><br/>");
        });
    }, "json");
}

$(function () {
    // Declare a proxy to reference the hub. 
    var chat = $.connection.play;

    // Create a function that the hub can call to broadcast messages.
    chat.client.additem = function (name, message) {
        $.ajax({
            url: "/api/play",
            data: { 'Text': 'Volvo: New text', 'Master': false, 'System': false },
            type: "POST"
        });
    };

    chat.client.update = function () {
        update();
    };

    // Start the connection.
    $.connection.hub.start().done(function () {
        $('#send_btn').click(function () {
            chat.server.send('n', $('#textFoRsend').val());
        });
    });

    update();
});