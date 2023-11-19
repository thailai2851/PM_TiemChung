document.addEventListener("DOMContentLoaded", function () {
    $('#loginForm').on('submit', function (e) {
        var form = document.getElementById('loginForm');
        if (!form.checkValidity()) {
            form.classList.add('was-validated');
        } else {
            var formData = $(this).serialize();
            $('#btnLogin').prop('disabled', true);
            $('#btnLogin').html(`<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>`);

            $.ajax({
                url: '/DangNhap',
                type: 'POST',
                data: formData,
                success: function (response) {
                    if (response.statusCode == 200) {
                        $('#message').remove();
                        $('#divMessage').empty();
                        window.location.href = response.url;
                    } else {
                        $('#divMessage').text(response.message);
                    }
                    $('#btnLogin').prop('disabled', false);
                    $('#btnLogin').html(`Đăng nhập`);
                },
                error: function (error) {
                    console.log(error);
                }
            });
            e.preventDefault();
        }
    })
})