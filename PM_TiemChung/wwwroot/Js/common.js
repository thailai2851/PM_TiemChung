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
function formatNumberWithElement(inputs) {
    inputs.each(function () {
        var value = $(this).val();
        if (value !== "0") {
            $(this).inputmask({
                alias: "numeric",
                groupSeparator: ",",
                autoGroup: true,
                digits: 0,
                allowMinus: false,
                placeholder: '0',
                digitsOptional: false,
                // Định dạng đặc biệt nếu giá trị là 0
                onBeforeMask: function (value, opts) {
                    if (value === "0") {
                        return "0\\";
                    }
                    return value;
                },
            });
        }
    });
}
// format lại số lúc nhập thành dạng 100,000,000.00
function formatNumberFloatWithElement(inputs) {
    inputs.each(function () {
        var min = $(this).attr('min');
        var input = $(this).inputmask({
            alias: "numeric",
            radixPoint: ".",
            groupSeparator: ",",
            autoGroup: true,
            digits: 2,
            digitsOptional: true,
            allowMinus: false,
            prefix: "",
            min: min ? parseFloat(min) : 0
        });
        input.on("blur", function () {
            $(this).trigger('keyup');
        });
    });
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
                        return '<div class="d-flex"><span>' + escape(item.ten) + '</span></div>';
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
function showDropdownMenu(select, dropdownElement) {
    var dropdown = $(dropdownElement.dropdown);
    // set chiều rộng cho dropdown menu theo select
    var ts_wapper = select.next();
    $('#dropdow-show').css('width', ts_wapper.outerWidth(true) + 'px');
    var td = select.parent();
    // lấy kiểu position của thẻ chứa nó
    var parentPosition = td.css("position");

    // lấy vị trí theo chiều dọc và tính toán vị trí cho thẻ dropdown
    const screenHeight = $(window).height();
    const offset = select.position();
    var scrollTop = $(window).scrollTop();
    var relativeTopWindow = offset.top - scrollTop;

    const dropdownHeight = dropdown.outerHeight(true) + relativeTopWindow;
    const height = ts_wapper.outerHeight(true);

    var tableScrollTop = select.closest('.table-responsive').scrollTop();
    var relativeTop = offset.top - tableScrollTop;

    if (parentPosition === "sticky") {
        if (dropdownHeight + height < screenHeight + tableScrollTop) {
            $('#dropdow-show').css('top', relativeTop + height);
            $('#dropdow-show').css('left', ts_wapper.offset().left);
        }
        else {
            $('#dropdow-show').css('top', relativeTop - dropdown.outerHeight(true));
            $('#dropdow-show').css('left', ts_wapper.offset().left);
        }
    } else {
        var tableScrollLeft = select.closest('.table-responsive').scrollLeft();
        var relativeLeft = offset.left - tableScrollLeft;

        if (dropdownHeight + height < screenHeight + tableScrollTop) {
            $('#dropdow-show').css('top', relativeTop + height);
            $('#dropdow-show').css('left', relativeLeft);
        }
        else {
            $('#dropdow-show').css('top', relativeTop - dropdown.outerHeight(true));
            $('#dropdow-show').css('left', relativeLeft);
        }
    }
}
function toEmpty(data) {
    if (data == null || data == undefined) {
        return "";
    } else {
        return data;
    }
}
function spinnerBtn(btn) {
    btn.prop('disabled', true);
    btn.html(`<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>`);
}
function showBtn(btn, text) {
    btn.prop('disabled', false);
    btn.html(text);
}
function getDataFromTr(tr) {
    var formData = {};
    tr.find('input, select, textarea').each(function () {
        if (this.name) {
            if (this.type === 'checkbox') {
                formData[this.name] = this.checked;
            } else {
                formData[this.name] = this.value ?? null;
            }
        }
    });

    // Chuyển đối tượng formData thành chuỗi serialized
    return formData;
}
function configDateLongMask() {
    $('.input-date-long-mask').inputmask({ alias: "datetime", inputFormat: 'dd-mm-yyyy', placeholder: '__-__-____' });
}
function configDateLongMaskWithElement(e) {
    e.inputmask({ alias: "datetime", inputFormat: 'dd-mm-yyyy', placeholder: '__-__-____' });
}
function configDateDefault() {
    var today = new Date();
    $('.input-date-default').datetimepicker({
        locale: 'vi',
        useStrict: true,
        defaultDate: today,
        format: "DD-MM-yyyy",
        extraFormats: ["DD-MM-yyyy", "DD/MM/yyyy", "yyyy"],
        icons: {
            date: "ti ti-calendar",
            up: "ti ti-chevron-up",
            down: "ti ti-chevron-down",
            previous: 'ti ti-chevron-left',
            next: 'ti ti-chevron-right',
            time: "ti ti-alarm"
        },
        keyBinds: {
            left: null,
            right: null,
        }
    });
}
function configDate() {
    $('.input-date').datetimepicker({
        locale: 'vi',
        format: "DD-MM-yyyy",
        useStrict: true,
        widgetPositioning: {
            horizontal: 'auto',
            vertical: 'bottom'
        },
        extraFormats: ["DD-MM-yyyy", "DD/MM/yyyy", "yyyy"],
        icons: {
            date: "ti ti-calendar",
            up: "ti ti-chevron-up",
            down: "ti ti-chevron-down",
            previous: 'ti ti-chevron-left',
            next: 'ti ti-chevron-right',
            time: "ti ti-alarm"
        },
        keyBinds: {
            left: null,
            right: null,
        }
    });
}
function getDateTimeNow() {
    // Lấy ngày giờ hiện tại
    var currentDate = new Date();

    // Lấy các thành phần ngày, tháng, năm, giờ và phút
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1; // Tháng bắt đầu từ 0, cần +1 để đúng
    var year = currentDate.getFullYear();
    var hours = currentDate.getHours();
    var minutes = currentDate.getMinutes();

    // Chuyển đổi thành định dạng "dd-MM-yyyy HH:mm"
    return ("0" + day).slice(-2) + "-" + ("0" + month).slice(-2) + "-" + year + " " + ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2);
}

function getDateNow() {
    // Lấy ngày giờ hiện tại
    var currentDate = new Date();

    // Lấy các thành phần ngày, tháng và năm
    var day = currentDate.getDate();
    var month = currentDate.getMonth() + 1; // Ghi chú: Tháng trong JavaScript bắt đầu từ 0
    var year = currentDate.getFullYear();

    // Định dạng chuỗi ngày tháng
    return (day < 10 ? '0' : '') + day + '-' + (month < 10 ? '0' : '') + month + '-' + year;
}