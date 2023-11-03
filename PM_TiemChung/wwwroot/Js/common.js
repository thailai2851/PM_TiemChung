function showProgress() {
    $('#progress').show();
}
function hideProgress() {
    $('#progress').hide();
}

function formatEvenNumber(number) {
    if (number == null) return 0;
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

// 1,000,000.00
function formatOddNumber(number) {
    if (number) {
        if (Number.isInteger(number)) {
            return number.toLocaleString('en-US');
        } else {
            return number.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        }
    } else {
        return "";
    }
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

function formatDay(inputString) {
    if (inputString) {
        var inputDate = new Date(inputString);
        var day = inputDate.getDate();
        if (day < 10) {
            day = '0' + day;
        }
        var month = inputDate.getMonth() + 1;
        if (month < 10) {
            month = '0' + month;
        }
        var year = inputDate.getFullYear();
        return day + '-' + month + '-' + year;
    } else {
        return ""
    }
}
function configCbDataBase(datas) {
    
    datas.forEach(data => {
        $.ajax({
            method: "post",
            url: data.action,
        }).done(function (response) {
            var mySelect = new TomSelect($(data.className), {
                selectOnTab: true,
                loadingClass: "Đang tìm kiếm...",
                maxOptions: 50,
                valueField: 'id',
                labelField: 'ten',
                placeholder: data.placeholder,
                options: response,
                openOnFocus: false,
                searchField: ["ten", "ma"],
                render: {
                    option: function (item, escape) {
                        return '<div class="d-flex"><span style="width: 70%;">' + escape(item.ten) + '</span></div>';
                    },
                    no_results: function (data, escape) {
                        return '<div class="no-results">Không tìm thấy dữ liệu </div>';
                    },
                },
                loadThrottle: 400,
            });
            mySelect.positionDropdown();

            $(data.className).next().children('div.ts-control').on('click', function () {
                mySelect.open();
            });
        })
    })
}