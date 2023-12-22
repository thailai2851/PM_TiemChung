var idModel;
var dataResponse = [];
$(document).ready(function () {
    $('#txtSearch').keypress(function (event) {
        // Kiểm tra nếu phím nhấn là Enter (keyCode 13)
        if (event.keyCode === 13) {
            searchWithKeyword();
        }
    });
    $('#formUpdate').submit(function (event) {
        event.preventDefault(); // Ngăn chặn hành vi mặc định của form submit
        var form = document.getElementById('formUpdate');
        if (!form.checkValidity()) {
            event.preventDefault();
            event.stopPropagation();
            form.classList.add('was-validated');
        } else {
            var formData = $(this).serialize(); // Lấy dữ liệu từ form

            $.ajax({
                url: '/DanhMuc/DM_NhanVien/update', // Đường dẫn đến action xử lý form
                method: 'POST',
                data: formData,
                success: function (response) {
                    showToast(response.message, response.statusCode);
                    if (response.statusCode == 200) {
                        var data = response.data;
                        var tr = getRowTable(data);
                        if ($("tr[data-id=" + data.id + "]").length) {
                            $("tr[data-id=" + data.id + "]").replaceWith(tr);
                        } else {
                            $("tr[data-id=" + data.id + "]").append(tr);
                        }
                        showColumnFromSesion();
                        $('#modal-largel').modal('hide');
                    }
                },
                error: function (xhr, status, error) {
                    // Xử lý lỗi (nếu có) khi gửi form
                    console.error(error);
                }
            });
        }
    });
    $(document).on('click', '#btnDanger', function () {
        $.ajax({
            type: "post",
            url: "/DanhMuc/DM_NhanVien/changeActive",
            data: "id=" + idModel,
            success: function (response) {
                if (response.statusCode == 200) {
                    $("tr[data-id=" + idModel + "]").remove();
                }
                showToast(response.message, response.statusCode);
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
})
function changeActive(id) {
    idModel = id;
    showModalDanger('Bạn có muốn thao tác?');
}
function getRowTable(data) {
    return `<tr data-id="${data.id}">
    <td class="text-center MaNhanVien">${data.maNhanVien == null ? "" : data.maNhanVien}</td>
    <td class="text-start TenNhanVien">${data.tenNhanVien == null ? "" : data.tenNhanVien}</td>
    <td class="text-center NgaySinh">${data.ngaySinh == null ? "" : formatDay(data.ngaySinh)}</td>
    <td class="text-start Idgt">${data.idgtNavigation == null ? "" : data.idgtNavigation.tenGioiTinh}</td>
    <td class="text-start DiaChi">${data.diaChi == null ? "" : data.diaChi}</td>
    <td class="text-start QueQuan">${data.queQuan == null ? "" : data.queQuan}</td>
    <td class="text-start DienThoai">${data.dienThoai == null ? "" : data.dienThoai}</td>
    <td class="text-center Mabhxh">${data.mabhxh == null ? "" : data.mabhxh}</td>
    <td class="text-center Macchn">${data.macchn == null ? "" : data.macchn}</td>
    <td class="text-center Ngaycapcchn">${data.ngaycapcchn == null ? "" : formatDay(data.ngaycapcchn)}</td>
    <td class="text-start Noicapcchn">${data.noicapcchn == null ? "" : data.noicapcchn}</td>
    <td class="text-center last-td-column">
        <div class="btn-group" role="group" aria-label="Basic outlined example">
            <button onclick="showModal(${data.id})" class="btn btn-icon bg-azure-lt" data-bs-toggle="tooltip" data-bs-placement="left" title="Sửa">
                        <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-edit" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
                            <path d="M7 7h-1a2 2 0 0 0 -2 2v9a2 2 0 0 0 2 2h9a2 2 0 0 0 2 -2v-1"></path>
                            <path d="M20.385 6.585a2.1 2.1 0 0 0 -2.97 -2.97l-8.415 8.385v3h3l8.385 -8.415z"></path>
                            <path d="M16 5l3 3"></path>
                        </svg>
    
                    </button>
                    <button class="btn btn-icon bg-dark-lt" onclick="changeActive(${data.id})" data-bs-toggle="tooltip" data-bs-placement="right" title="Thay đổi trạng thái">
                    ${data.active ? '<svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-trash" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">< path stroke = "none" d = "M0 0h24v24H0z" fill = "none" ></path ><path d="M4 7l16 0"></path><path d="M10 11l0 6"></path><path d="M14 11l0 6"></path><path d="M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12"></path><path d="M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3"></path></svg > ' :
            '<svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-arrow-back-up" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">< path stroke = "none" d = "M0 0h24v24H0z" fill = "none" ></path ><path d="M9 14l-4 -4l4 -4"></path><path d="M5 10h11a4 4 0 1 1 0 8h-1"></path></svg > '
        }
                </button>
        </div>
    </td>
    </tr>`;
}
function updateDataOfTable(datas) {
    $("#tbody-table").empty();

    datas.map((data) =>
        $("#tbody-table").append(getRowTable(data))
    );
    showColumnFromSesion();
    hideProgress();
}
function updatePagi(prePage, nextPage, pageNumber) {

    var pagi = `<ul class="pagination m-2 justify-content-end" id="pagi">
      <li class="page-item ${prePage == 0 ? "disabled" : ""
        }"><button onclick="changePage(1)" class="page-link"><svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-chevrons-left" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
        <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
        <path d="M11 7l-5 5l5 5"></path>
        <path d="M17 7l-5 5l5 5"></path>
     </svg></button></li>
      <li class="page-item ${prePage == 0 ? "disabled" : ""}">
        <button class="page-link" tabindex="-1" aria-disabled="true" onclick="${prePage == 0 ? "" : "changePage(" + prePage + ")"
        }">
          <!-- Download SVG icon from http://tabler-icons.io/i/chevron-left -->
          <svg xmlns="http://www.w3.org/2000/svg" class="icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
            <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
            <path d="M15 6l-6 6l6 6"></path>
          </svg>
        </button>
      </li>
      ${prePage != 0
            ? "<li class='page-item'><button class='page-link' onclick='changePage(" +
            prePage +
            ")'>" +
            prePage +
            "</button></li>"
            : ""
        }
      <li class="page-item active"><button class="page-link">${prePage + 1
        }</button></li>
      ${(nextPage !== 0 ? "<li class='page-item'><button class='page-link' onclick='changePage(" + nextPage + ")'>" + nextPage + "</button></li>" : "")
        }
      <li class="page-item ${nextPage == 0 ? "disabled" : ""}">
        <button class="page-link" onclick="${nextPage == 0 ? "" : "changePage(" + (pageNumber + 1) + ")"
        }"">
          <!-- Download SVG icon from http://tabler-icons.io/i/chevron-right -->
          <svg xmlns="http://www.w3.org/2000/svg" class="icon" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"></path><path d="M9 6l6 6l-6 6"></path></svg></button></li>
      <li class="page-item  ${nextPage == 0 ? "disabled" : ""
        }"><button class="page-link" onclick="changePage(-1)"><svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-chevrons-right" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
        <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
        <path d="M7 7l5 5l-5 5"></path>
        <path d="M13 7l5 5l-5 5"></path>
     </svg></button></li>
    </ul>`;
    $("#pagi").replaceWith(pagi);
}
function changePage(pageNumber) {
    showProgress();

    $.ajax({
        type: "post",
        url: "/DanhMuc/DM_NhanVien/changePage",
        data: "pageNumber=" + pageNumber,
        success: function (response) {
            hideProgress();
            updateDataOfTable(response.result);
            updatePagi(response.prePage, response.nextPage, pageNumber);
        },
        error: function (error) {
            console.log(error);
        },
    });
}

// hiển thị các column trong table được show
$(".show-column").on("change", function () {
    var check = $(this).is(":checked");
    if (check) {
        $("." + $(this).val()).show();
    } else {
        $("." + $(this).val()).hide();
    }
    updateSessionColumnnShow($(this).val(), $(this).is(":checked"));
});

//lấy dữ liệu các column được phép show từ session
function getSessionColumnShow() {
    // Tạo tên session từ path của URL
    var sessionName = window.location.pathname;

    // Kiểm tra nếu session đã tồn tại
    if (sessionStorage.getItem(sessionName)) {
        // Trả về giá trị của session
        return sessionStorage.getItem(sessionName);
    } else {
        // Tạo mới session với giá trị mặc định
        var defaultValue = [
            {
                columnName: "MaNhanVien",
                value: true,
            },
            {
                columnName: "TenNhanVien",
                value: true,
            },

            {
                columnName: "NgaySinh",
                value: true,
            },
            {
                columnName: "Idgt",
                value: true,
            },
            {
                columnName: "DiaChi",
                value: true,
            },
            {
                columnName: "QueQuan",
                value: true,
            },
            {
                columnName: "DienThoai",
                value: true,
            },
            {
                columnName: "Mabhxh",
                value: true,
            },
            {
                columnName: "Macchn",
                value: true,
            },

            {
                columnName: "Ngaycapcchn",
                value: true,
            },
            {
                columnName: "Noicapcchn",
                value: true,
            }
        ];
        sessionStorage.setItem(sessionName, JSON.stringify(defaultValue));

        // Trả về giá trị mặc định
        return JSON.stringify(defaultValue);
    }
}
// update session khi người dùng check
function updateSessionColumnnShow(columnName, value) {
    var data = getSessionColumnShow();

    var sessionValue = JSON.parse(data);

    // Duyệt qua mảng session
    $.each(sessionValue, function (index, element) {
        // Kiểm tra nếu columnName = true
        if (element.columnName == columnName) {
            element.value = value;
        }
    });
    // Tạo tên session từ path của URL
    var sessionName = window.location.pathname;
    sessionStorage.setItem(sessionName, JSON.stringify(sessionValue));
}

//hàm hiển thị các column từ session
function showColumnFromSesion() {
    var data = getSessionColumnShow();
    var sessionValue = JSON.parse(data);
    // Duyệt qua mảng session
    $.each(sessionValue, function (index, element) {
        // Kiểm tra nếu columnName = true
        if (element.value === true) {
            // Hiển thị phần tử có columnName = true
            $("." + element.columnName).show();
        } else {
            $("." + element.columnName).hide();
        }
        // Tìm các phần tử input có class "show-column" và thuộc tính "data-column" trùng khớp với tên cột
        var $input = $('.show-column[value="' + element.columnName + '"]');

        // Thiết lập thuộc tính "checked" của phần tử input dựa trên giá trị
        $input.prop("checked", element.value);
    });
}
function scrollTable() {
    var table = document.getElementById('tableDM');
    var scrollPercentage = (table.scrollTop + table.clientHeight) / table.scrollHeight;
    var numRow = $('#tbody-table tr').length;
    var len = dataResponse.length;
    // Kiểm tra nếu người dùng kéo xuống 80% chiều rộng của bảng
    if (scrollPercentage > 0.8 && len > 100 && numRow != len) {
        var ds = dataResponse.slice(numRow, numRow + 100);
        ds.map((data) =>
            $("#tbody-table").append(getRowTable(data))
        );
        showColumnFromSesion();
    }
    if (numRow == len) dataResponse = [];
}

$(document).ready(function () {
    showColumnFromSesion();
});

function searchWithKeyword() {
    showProgress();
    var key = $('#txtSearch').val();
    if (key == "") {
        $.ajax({
            type: "post",
            url: "/DanhMuc/DM_NhanVien/api/getModelsWithNumberPage",
            success: function (response) {
                dataResponse = [];
                hideProgress();
                updateDataOfTable(response.result);
                updatePagi(response.prePage, response.nextPage, 1);
                $('#pagi').show();
            },
            error: function (error) {
                console.log(error);
            }
        });

    } else {
        $.ajax({
            type: "post",
            url: "/DanhMuc/DM_NhanVien/api/searchWithKeyword",
            data: "key=" + key + "&active=" + $('select[name="Active"]').val(),
            success: function (response) {

                hideProgress();
                if (response.length > 100) {
                    updateDataOfTable(response.slice(0, 99));
                    dataResponse = response;
                } else {
                    updateDataOfTable(response);
                }
                $('#pagi').hide();
            },
            error: function (error) {
                console.log(error);
            }
        });

    }
}
function showModal(id) {
    $.ajax({
        type: "post",
        url: "/DanhMuc/DM_NhanVien/showModal",
        data: "id=" + id,
        success: function (response) {
            showModalLargel(response.title, response.view);
        },
        error: function (error) {
            console.log(error);
        }
    });
}