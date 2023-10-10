function showProgress() {
    $('#progress').show();
}
function hideProgress() {
    $('#progress').hide();
}

function showModalLargel(title, content) {
    var myModal = new bootstrap.Modal(document.getElementById("modal-largel"), {
    });
    $("#modal-title-largel").text(title);
    $("#modal-body-largel").empty();
    $("#modal-body-largel").append(content);
    myModal.show();
}
function showToast(message, statusCode) {
    var backgrounColor;
    document.getElementById('toast').className = 'toast align-items-center text-white border-0 position-fixed top-0 end-0 p-3';
    $("#toastContent").text(message);
    if (statusCode === 200) {
        backgrounColor = "bg-success";
        $("#toast").addClass(backgrounColor);
    } else {
        backgrounColor = "bg-danger";
        $("#toast").addClass(backgrounColor);
    }
    $("#toast").show();
    setTimeout(function () {
        $("#toast").hide();
    }, 2000);
}
function showModalDanger(content) {
    var myModal = new bootstrap.Modal(document.getElementById("modal-danger"), {
    });
    $("#modal-danger-content").text(content);
    myModal.show();
}
var modalDanger = document.getElementById('modal-danger');
if (modalDanger) {
    modalDanger.addEventListener('hidden.bs.modal', function (event) {
        $('#btnDanger').off('click');
    });
}