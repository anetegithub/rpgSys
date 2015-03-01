$('#log-btn').click(function () {
    if ($('#userPsw1').val() == $('#userPsw2').val()) {
        var data = {
            "Login": $('#userName').val(),
            "Password": $('#userPsw1').val(),
            "Email": $('#userEmail').val()
        };

        $.ajax({
            url: '../api/users/create',
            type: 'PUT',
            data: { '': JSON.stringify(data) },
            success: function (result) {
                if (result == "True") {
                    success('Вы успешно зарегистрировались!');
                }
                else {
                    fail(result);
                }
            }
        });

        //$.post('../api/users/create', { '': JSON.stringify(data) })
        //    .done(function (data) {
              
        //    });
    } else {
        fail('Пароли не совпадают!');
    }
});

function fail(text) {
    $('#msg').css('color', 'red');
    $('#msg').html("( " + text + " )");
}

function success(text) {
    $('#msg').css('color', 'green');
    $('#msg').html("( " + text + " )");
}