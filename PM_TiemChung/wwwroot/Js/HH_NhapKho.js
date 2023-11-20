﻿document.addEventListener("DOMContentLoaded", function () {
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
    $(document).on('input', 'input[name="ThanhTienTon"], input[name="ChiecKhau"], input[name="ThueVat"]', function () {
        console.log(23423);
        var tongTienHang = 0;
        var tongTienCK = 0;
        var tongTienThue = 0;
        var tongTra = 0;
        $('input[name="ThanhTien"]').each(function () {

            var tr = $(this).closest('tr');
            var SlxGia = parseFloat($(this).inputmask('unmaskedvalue'));

        if (SlxGia) {
            console.log("if");
                tongTienHang += parseFloat(SlxGia);
            }

            var chiecKhau = parseFloat(tr.find('input[name="ChiecKhau"]').inputmask('unmaskedvalue'));
            var tienChiecKhau = 0;
            if (chiecKhau && SlxGia) {
                tienChiecKhau = ((chiecKhau * SlxGia) / 100);
                tongTienCK += tienChiecKhau;
            }

            var thueVat = parseFloat(tr.find('input[name="ThueVat"]').inputmask('unmaskedvalue'));
            var tienThue = 0;
            if (thueVat && SlxGia) {
                tienThue = (((SlxGia - tienChiecKhau) * thueVat) / 100);
                tongTienThue += tienThue;
            }   
        })

        $('#tongTienHang').val(tongTienHang);
        $('#tongTienCK').val(tongTienCK);
        $('#tongTienThue').val(tongTienThue);
        $('#tongTra').val(tongTienHang - tongTienCK + tongTienThue);
    });
    // thay đổi thành tiền khi giá nhập thay đổi
    $(document).on('keyup', 'input[name="DonGiaNhap"]', function () {
        var tr = $(this).closest('tr');
        var donGiaNhap = $(this).inputmask('unmaskedvalue');
        var soLuongNhap = tr.find('input[name="SoLuongNhap"]').inputmask('unmaskedvalue');

        if (soLuongNhap != "") {
            tr.find('input[name="ThanhTien"]').val(soLuongNhap * donGiaNhap);
        }
        console.log(donGiaNhap);
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
});
function updateThongSoQuanLy(tr, donGiaNhap, soLuongNhap) {
    console.log(donGiaNhap, soLuongNhap);

    // thay đổi số lượng tồn
    if (donGiaNhap != "" && soLuongNhap != "") {
        var soLuongTon = soLuongNhap;
        var donGiaTon = (donGiaNhap * soLuongNhap) / soLuongTon;
        console.log(soLuongTon, donGiaTon,donGiaTon * soLuongTon);
        tr.find('input[name="SoLuongTon"]').val(soLuongTon);
        tr.find('input[name="DonGiaTon"]').val(donGiaTon);
        tr.find('input[name="ThanhTienTon"]').val(donGiaTon * soLuongTon).trigger('input');

    }
}
function addRowChiTietNhapKho() {


var newRow = $(`<tr id="moi">
                <td class="first-td-column text-center ps-0 td-sticky">
                    <input autocomplete="off" type="text" class="form-control form-table text-center stt" readonly value="${GanSTT()}" style="width:32px;z-index:2;" />
                    <input type="hidden" name="Id" value="0" />
                </td>
                <td class="td-sticky md-sticky" style="left: 33px;background-color: #fff !important; z-index:2">
                    <select name="IdhangHoa" class="form-select form-table cbHangHoa" style="position:relative;width:400px;">
                    </select>
                </td>
                <td>
                    <input autocomplete="off" type="text" class="form-control form-table formatted-number-float" style="width:55px;" name="SoLuongNhap" />
                </td>
                <td>
                    <input autocomplete="off" type="text" class="form-control form-table" style="width:60px;" readonly name="tenDvt" tabindex="-1" />
                </td>
                <td>
                    <input autocomplete="off" t type="text" class="form-control form-table formatted-number-float" style="width:80px;" value="" name="DonGiaNhap" />
                </td>
                <td>
                    <input autocomplete="off" type="text" class="form-control form-table formatted-number-float" style="width:100px;" value="" name="ThanhTien" />
                </td>
                <td>
                    <input autocomplete="off" type="text" class="form-control form-table formatted-number-float" style="width:40px;" value="" name="ChiecKhau" />
                </td>
                <td>
                    <input autocomplete="off" type="text" class="form-control form-table formatted-number-float" value="" style="width:40px;" name="ThueVat" />
                </td>
                <td>
                    <input autocomplete="off" type="text" class="form-control form-table input-date-short-mask" style="width:90px;" name="NgaySanXuat" />
                </td>
                <td>
                    <input autocomplete="off" type="text" class="form-control form-table input-date-short-mask" style="width:90px;" name="HanDung" />
                <td>
                    <textarea autocomplete="off" rows="1" class="form-control form-table" style="width: 200px" name="GhiChuHangNhap"></textarea>
                </td>
                                                        
                <td class="text-center last-td-column pe-0">
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

