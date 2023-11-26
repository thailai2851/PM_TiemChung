document.addEventListener("DOMContentLoaded", function () {
    datas = [
        {
            className: ".cbHangHoa",
            placeholder: "-- Vaccine --",
            action: "HH_NhapKho/getListVaccine",
        },
        {
            className: ".cbNhaCungCap",
            placeholder: "-- Nơi cung Cấp --",
            action: "HH_NhapKho/getListNhaCungCap",
        },
        {
            className: ".cbHangHoa1",
            placeholder: "-- VacXin --",
            action: "/HangHoa/HH_NhapKho/getListVaccine",
        },
        {
            className: ".cbNhaCungCap1",
            placeholder: "-- Nơi cung Cấp --",
            action: "HH_NhapKho/getListNhaCungCap",
        }

    ];
    configCbDataBase(datas);
    $('#tBody-ThemChiTietPhieuNhap').on('change', 'select[name="IdhangHoa"]', function () {
        var tr = $(this).closest('tr');
        var idVC = $(this).val();
        getDVT(idVC, tr);
        var tr = $(this).closest('tr');
        if (tr.is(":last-child")) {
            addRowChiTietNhapKho();
        }
    });
    function getDVT(idVC, tr) {
        $.ajax({
            url: '/HangHoa/HH_NhapKho/getDVT', // Đường dẫn đến action xử lý form
            method: 'POST',
            data: {
                idVC: idVC,
            },
            success: function (response) {
                tr.find('input[name="tenDvt"]').val(response);
            }
        });
    }
    //$(document).on('input', 'input[name="DonGiaNhap"], input[name="SoLuongNhap"]', function () {
    //    console.log(23423);
    //    var tongTienHang = 0;
    //    var tongTienCK = 0;
    //    var tongTienThue = 0;
    //    var tongTra = 0;
    //    $('input[name="ThanhTien"]').each(function () {

    //        var tr = $(this).closest('tr');
    //        var SlxGia = parseFloat($(this).inputmask('unmaskedvalue'));

    //        if (SlxGia) {
    //            tongTienHang += parseFloat(SlxGia);
    //        } 
    //    })

    //    $('#tongTienHang').val(tongTienHang);
    //    $('#tongTra').val(tongTienHang - tongTienCK + tongTienThue);
    //});
    // thay đổi thành tiền khi giá nhập thay đổi
    $(document).on('input', 'input[name="DonGiaNhap"]', function () {
        var tr = $(this).closest('tr');
        var donGiaNhap = $(this).inputmask('unmaskedvalue');
        var soLuongNhap = tr.find('input[name="SoLuongNhap"]').inputmask('unmaskedvalue');

        if (soLuongNhap != "") {
            tr.find('input[name="ThanhTien"]').val(soLuongNhap * donGiaNhap);
        }
        updateThongSoQuanLy(tr, donGiaNhap, soLuongNhap);

    });
    $('#btnTaoPhieuNhapKho').on('click', function () {
        luuPhieuNhap(true);
    })
    $(document).on('click', '.remove-phieuNhapCt', function () {
        var tr = $(this).closest('tr');

        if (tr.is(":last-child")) {
            if (tr.is(":last-child") && tr.is(":first-child")) {
                var stt = tr.index() + 1;
                // xóa trắng các input
                tr.find('input').val('');
                tr.find('input.stt').val(stt);
                if (tr.find('select[name="SoLo"]')[0].tomselect) {
                    tr.find('select[name="SoLo"]')[0].tomselect.clear();
                    tr.find('select[name="SoLo"]')[0].tomselect.clearOptions();
                }
            }
            return;
        } else {
            var idCt = tr.find('input[name="Id"]').val();
            tr.find('input[name="SoLuongNhap"]').val('0').trigger('keyup');
            if (idCt > 0) {
                danhSachCtBiXoa.push(idCt);
            }
            if (!tr.is(":last-child")) {
                tr.find('select[name="IdhangHoa"]')[0].tomselect.destroy();
            }
            tr.remove();
            $("#tBody-ThemChiTietPhieuNhap tr").each(function () {
                $(this).find('input.stt').val($(this).index() + 1);
            })

        }
    });
    $('#btnXoaTrang').on('click', function () {
        xoaTrangPhieuNhapKho(); 
        $('#btnTaoPhieuNhapKho').show();
        $('#IdnhaCungCap')[0].tomselect.enable();
        $('#formThemPhieuNhapKho input').prop('readonly', false);
        $('#SoPhieu').prop('readonly', true);
        $('#GhiChuPhieuNhap').prop('readonly', false);
    });
    configDateShortMask();
    formatNumber();
    configDateDefault();
    configDateTimeDefault();
    configDateLongMask();
});
function updateThongSoQuanLy(tr, donGiaNhap, soLuongNhap) {
    console.log(donGiaNhap, soLuongNhap);

    // thay đổi số lượng tồn
    if (donGiaNhap != "" && soLuongNhap != "") {
        console.log(23423);
        var tongTienHang = 0;
        var tongTienCK = 0;
        var tongTienThue = 0;
        var tongTra = 0;
        $('input[name="ThanhTien"]').each(function () {

            var tr = $(this).closest('tr');
            var SlxGia = parseFloat($(this).inputmask('unmaskedvalue'));

            if (SlxGia) {
                tongTienHang += parseFloat(SlxGia);
            }
        })

        $('#tongTienHang').val(tongTienHang);
        $('#tongTra').val(tongTienHang - tongTienCK + tongTienThue);

    }
}
function addRowChiTietNhapKho() {


var newRow = $(`<tr id="moi">
                <td class="first-td-column text-center ps-0 td-sticky" style="width:32px;z-index:2;">
                    <input autocomplete="off" type="text" class="form-control form-table text-center stt" readonly value="${GanSTT()}" />
                    <input type="hidden" name="Id" value="0" />
                </td>
                <td class="td-sticky md-sticky" style="left: 33px;background-color: #fff !important; z-index:2">
                    <select name="IdhangHoa" class="form-select form-table cbHangHoa" style="position:relative;width:400px;">
                    </select>
                </td>
                <td style="width:55px;">
                    <input autocomplete="off" type="text" class="form-control form-table formatted-number-float" name="SoLuongNhap" />
                </td>
                <td style="width:60px;">
                    <input autocomplete="off" type="text" class="form-control form-table" readonly name="tenDvt" tabindex="-1" />
                </td>
                <td style="width:80px;">
                    <input autocomplete="off" t type="text" class="form-control form-table formatted-number-float" value="" name="DonGiaNhap" />
                </td>
                <td style="width:100px;">
                    <input autocomplete="off" type="text" class="form-control form-table formatted-number-float" value="" name="ThanhTien" />
                </td>
                <td style="width:40px; display : none">
                    <input autocomplete="off" type="text" class="form-control form-table formatted-number-float" value="" name="ChiecKhau" />
                </td>
                <td style="width:40px; display : none">
                    <input autocomplete="off" type="text" class="form-control form-table formatted-number-float" value="" name="ThueVat" />
                </td>
                <td style="width:90px;">
                    <input autocomplete="off" type="text" class="form-control form-table input-date-long-mask" name="NgaySanXuat" />
                </td>
                <td style="width:90px;">
                    <input autocomplete="off" type="text" class="form-control form-table input-date-long-mask" name="HanDung" />
                <td style="width: 200px">
                    <textarea autocomplete="off" rows="1" class="form-control form-table" name="GhiChuHangNhap"></textarea>
                </td>
                                                        
                <td class="text-center last-td-column pe-0" style="width:40px;">
                    <button type="button" class="btn btn-icon btn-sm text-red remove-phieuNhapCt">
                        <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-x" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                            <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
                            <path d="M18 6l-12 12"></path>
                            <path d="M6 6l12 12"></path>
                        </svg>
                    </button>
                </td>
            </tr>
`);

    $("#tBody-ThemChiTietPhieuNhap").append(newRow);
    configChoRowThemChiTietNhapKho(newRow);
    formatNumber();
    configDateShortMask();
    configDateLongMask();

};
function GanSTT() {
    var stt = $('#tBody-ThemChiTietPhieuNhap tr').length;

    return Number(Number(stt) + 1);
}
function configChoRowThemChiTietNhapKho(tr) {
    console.log(tr);
    $.ajax({
        dataType: "json",
        method: "post",
        url: '/HangHoa/HH_NhapKho/getListVaccine',
    }).done(function (response) {

                var s = tr.find('select[name="IdhangHoa"]');
                var mySelect = new TomSelect(s, {
                    selectOnTab: true,
                    loadingClass: "Đang tìm kiếm...",
                    maxOptions: 50,
                    valueField: 'id',
                    labelField: 'ten',
                    placeholder: "-- Vaccine --",
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

                $('.cbHangHoa').next().children('div.ts-control').on('click', function () {
                    mySelect.open();
                });
    });
};

function luuPhieuNhap() {
    var tableData = [];
    var dataPhieuNhapMaster = new FormData();
    var rows = $('#tBody-ThemChiTietPhieuNhap tr:not(:last-child)');
    if (rows.length == 0) {
        showToast("Vui lòng thêm thông tin phiếu nhập", 500);
        return;
    }
    rows.each(function () {
        var row = $(this);
        var rowData = {};
        rowData.Idvaccine = row.find('select[name="IdhangHoa"]').val();
        rowData.SoLuong = row.find('input[name="SoLuongNhap"]').val();
        rowData.DonGia = row.find('input[name="DonGiaNhap"]').val();
        rowData.Cktm = row.find('input[name="ChiecKhau"]').val();
        rowData.Thue = row.find('input[name="ThueVat"]').val();

        rowData.Nsx = row.find('input[name="NgaySanXuat"]').val();
        rowData.Hsd = row.find('input[name="HanDung"]').val();
        rowData.GhiChu = row.find('textarea[name="GhiChuHangNhap"]').val();

        tableData.push(rowData);
    });
    var idNCC = $('#IdnhaCungCap').val();
    if (idNCC == '') {
        showToast("Vui lòng chọn nhà cung cấp", 500);
        return;
    }
    var ngayNhap = $('#NgayGioNhap').val();
    var soHD = $('#SoHoaDon').val();
    var ngayHD = $('#NgayHoaDon').val();

    var ghiChu = $('#GhiChuPhieuNhap').val();
    dataPhieuNhapMaster.append("GhiChu", ghiChu);
    dataPhieuNhapMaster.append("NgayNhap", ngayNhap);
    dataPhieuNhapMaster.append("Idncc", idNCC);
    dataPhieuNhapMaster.append("SoHd", soHD);
    dataPhieuNhapMaster.append("NgayHd", ngayHD);


    var data = {
        PhieuNhap: queryStringToData(dataPhieuNhapMaster),
        ChiTietPhieuNhap: tableData
    };
    console.log(data);
    $.ajax({
        url: '/NhapKho/ThemPhieuNhap', // Đường dẫn đến action xử lý form
        method: 'POST',
        data: JSON.stringify(data),
        contentType: "application/json",
        success: function (response) {
            if (response.statusCode == 200) {
                xoaTrangPhieuNhapKho();



            }
            showToast(response.message, response.statusCode);
        }
    });
}
function emptyThemChiTietPhieuNhap() {
    $("#tBody-ThemChiTietPhieuNhap tr").each(function () {
        $(this).find("select").each(function () {
            if ($(this)[0].tomselect) {
                $(this)[0].tomselect.destroy();
            }
        })
    });
    $("#tBody-ThemChiTietPhieuNhap").empty();
}
function xoaTrangPhieuNhapKho() {
    $('#Id').val('0');
    emptyThemChiTietPhieuNhap();

    $("#tBody-DsPhieu").empty();
    $("#tBody-BaoCaoChiTiet").empty();
    $("#tBody-BaoCaoTongHop").empty();

    addRowChiTietNhapKho();
    clearForm("formThemPhieuNhapKho");
    $("#formThemPhieuNhapKho").removeClass('was-validated');

    $('#tongTienHang').val("0");
    $('#tongTienCK').val("0");
    $('#tongTienThue').val("0");
    $('#tongTra').val("0");
}
/////////////////////////////////////////// tab xem 
var _fromDay = null;
var _toDay = null;
var _soPhieu = null;
function getDSXKNL() {
    //for (var i = 1; i <= 7; i++) {
    //    addRowTableXKNL(i);
    //}
    _fromDay = $('input[name="TuNgay"]').val();
    _toDay = $('input[name="DenNgay"]').val();
    _soPhieu = $('input[name="SoPhieu"]').val();

    $.ajax({
        url: '/NhapKho/LichSuNhap', // Đường dẫn đến action xử lý form
        method: 'POST',
        data: {
            TuNgay: _fromDay,
            DenNgay: _toDay,
            maPhieu: _soPhieu,
            idVC: $('select.cbHangHoa1').val(),
            inNCC: $('select.cbNhaCungCap1').val(),
            soHD: $('input[name="SoHD"]').val()
        },
        success: function (response) {
            $('#tBody-DsPhieu').empty();
            $('#TienThanhToanTabLS').val('');
            response.forEach(function (data, i) {
                addRowTableXKNL(data, i);
            });
            /*TinhTongTienPhieuNhap();*/
            //if (response.statusCode == 200) {
            //    $('#tbodyChiTietPhieuNhap').empty();
            //    $('#TienThanhToan').val('');

            //}
            //showToast(response.message, response.statusCode);
        }
    });
}
function addRowTableXKNL(data, i) {
    var newRow = ` <tr class="accordion-toggle collapsed" id="c-2474${i}" data-bs-toggle="collapse" data-parent="#c-2474${i}" href="#collap-2474${i}" aria-expanded="false">
                                    <td>${i+1}</td>
                                <td>${data.soPx}</td>
                                <td>${formatDay(data.ngayTao)}</td>
                                <td>${data.nhaCungCap}</td>
                                <td>${data.soLuongHH} </td>
                                <td> <input readonly autocomplete="off" type="text" class="w-100 form-control form-table formatted-number-float" style="width:55px;" value=${data.tongTien} id="tongTienXuat" name="tongTienXuat"/></td>
                                <td>
                                    <button class="btn btn-sm dropdown-toggle more-horizontal" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        <span class="text-muted sr-only">Thao Tác</span>
                                    </button>
                                    <div class="dropdown-menu dropdown-menu-right">
                                        <a class="dropdown-item" href="#">In</a>
                                    </div>
                                </td>
                            </tr>
                            
                            <tr id="collap-2474${i}" class="in p-3 bg-light collapse" style="">
                                <td colspan="8">
                            `;
    data.chiTietPhieuNhap.forEach(function (data, index) {
        var i = index + 1;
        newRow += `<dl class="row mb-0 mt-1">
                                        <dt class="col-sm-1">${i}</dt>
                                        <dd class="col-sm-1">${data.idhhNavigation.maVaccine}</dd>
                                        <dt class="col-sm-3">${data.idhhNavigation.tenVaccine}</dt>
                                        <dt class="col-sm-1">${data.dvt}</dt>
                                        <dd class="col-sm-1">${data.soLuong}</dd>
                                        <dt class="col-sm-2"><input readonly autocomplete="off" type="text" class="w-100 form-control form-table formatted-number-float" style="width:55px;" value=${data.gia}/></dt>
                                        <dd class="col-sm-2"><input readonly autocomplete="off" type="text" class="w-100 form-control form-table formatted-number-float" style="width:55px;" value=${data.soLuong * data.gia}/></dd>
                                    </dl>
                                `;
    });
    newRow += `
    </td>
    </tr>`;
    $('#tBody-DsPhieu').append(newRow);
    formatNumber();
}

