$(function () {
    $('#check-password-button').on('click', function () {
        var one = $('#password1').val();
        var two = $('#password2').val();
        if (one == two) {
            $('#check-password-message').text('  Match!');
        }
        else {
            $('#check-password-message').text('   Try again!');
        }
    });

    $('#like-button').on('click', function () {
        $.get("/image/likeimage", function (foobar) {
            $('#like-button').prop('disabled');
        });
        
    });
});
