var _benhNhan = null;
$(document).ready(function () {
        
    configDateLongMask();
    configDateDefault();
    configDate();
    $('input[name="NgaySinh"]').on('blur', function () {
        var pattern = /^\d{2}-\d{2}-\d{4}$/;
        if (pattern.test($(this).val())) {
            $('input[name="NamSinh"]').val($(this).val().substring(6));
        } else {
            $(this).val('');
        }
    })
    $('input[name="NamSinh"]').on('blur', function () {
        var date = $('input[name="NgaySinh"]').val();
        var pattern = /^\d{2}-\d{2}-\d{4}$/;
        if (pattern.test(date)) {
            var strDate = date.substring(0, date.length - 4);

            $('input[name="NgaySinh"]').val(strDate + $(this).val());
        }
    });

    var das = [{
        className: 'select.cbTinhCuTru',
        placeholder: "-- Tỉnh cư trú --",
        action: "/DanhMuc/DM_TinhCuTru/getListTinhCuTru"
    },
    {
        className: 'select.cbQuanCuTru',
        placeholder: "-- Quận cư trú --",
        action: "/DanhMuc/DM_QuanCuTru/getListQuanCuTru"
    },
    {
        className: 'select.cbXaCuTru',
        placeholder: "-- Xã cư trú --",
        action: "/DanhMuc/DM_XaCuTru/getListXaCuTru"
        }
        ,
        {
            className: 'select.cbQuocGia',
            placeholder: "-- Quốc gia --",
            action: "/DanhMuc/DM_QuocGia/getListQuocGia"
        }
        ,
        {
            className: 'select.cbNgheNghiep',
            placeholder: "-- Nghề nghiệp --",
            action: "/DanhMuc/DM_NgheNghiep/getListNgheNghiep"
        }
        ,
        {
            className: 'select.cbDanToc',
            placeholder: "-- Dân tộc --",
            action: "/DanhMuc/DM_DanToc/getListDanToc"
        }
    ];
    configCbDataBase(das);

    $('select.cbTinhCuTru').on('change', function () {
        var id = $(this).val();
        $('select.cbQuanCuTru')[0].tomselect.clear();
        $('select.cbQuanCuTru')[0].tomselect.clearOptions();
        $('select.cbXaCuTru')[0].tomselect.clear();
        $('select.cbXaCuTru')[0].tomselect.clearOptions();
        $.ajax({
            method: "post",
            url: '/DanhMuc/DM_QuanCuTru/getListQuanCuTru',
        }).done(function (response) {
            var newOptions = response.filter(function (item) {
                return item.idTinh == id;
            });

            $('select.cbQuanCuTru')[0].tomselect.addOptions(newOptions)
        })
    });
    $('select.cbQuanCuTru').on('change', function () {
        var id = $(this).val();
        $('select.cbXaCuTru')[0].tomselect.clear();
        $('select.cbXaCuTru')[0].tomselect.clearOptions();
        $.ajax({
            method: "post",
            url: '/DanhMuc/DM_XaCuTru/getListXaCuTru',
        }).done(function (response) {
            var newOptions = response.filter(function (item) {
                return item.idQuan == id;
            });

            $('select.cbXaCuTru')[0].tomselect.addOptions(newOptions)
        })
    });

    $('#btnLuuBN').on('click', function () {
        var btn = $(this);
        spinnerBtn(btn);
        var form = document.getElementById('formTTHC');
        if (!form.checkValidity()) {
            form.classList.add('was-validated');
            showBtn(btn, "Tiếp nhận");
        } else {
            var namSinh = parseInt($('input[name="NamSinh"]').val());
            var toDay = new Date();
            if ((toDay.getFullYear() - namSinh) <= 6 && $('input[name="NgaySinh"]').val() == '') {
                showToast('Bệnh nhân dưới dưới 6 tuổi, vui lòng nhập ngày tháng năm sinh!', 400);
                showBtn(btn, "Lưu");
                return;
            }
            form.classList.remove('was-validated');
            var formData = $('#formTTHC').serialize();
            $.ajax({
                url: '/QuanLy/QL_TiepNhanBenhNhan/LuuBenhNhan',
                method: 'POST',
                data: formData,
                success: function (result) {
                    showBtn(btn, "Tiếp nhận");
                    showToast(result.message, result.statusCode);
                    if (result.statusCode == 200) {
                        _benhNhan = result.data
                        $('#formTTHC').find('input[name="Id"]').val(_benhNhan.id);
                        $('#formTTHC').find('input[name="MaBn"]').val(_benhNhan.maBn);
                    }
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
    });
    $('#formSearch').on('submit', function (event) {
        event.preventDefault();
        searchBenhNhan();
    });

    $(document).on('click', '.btn-active', function () {
        var id = $(this).val();
        showModalDanger('Bạn có muốn thực hiện thao tác!');
        $('#btnDanger').on('click', function () {
            if (id) {
                $.ajax({
                    url: '/QuanLy/QL_TiepNhanBenhNhan/kichHoat',
                    method: 'POST',
                    data: {
                        id: id
                    },
                    success: function (result) {
                        showToast(result.message, result.statusCode);
                    },
                    error: function (error) {
                        console.error(error);
                    }
                });
            }
        })
    });
    $('#txtNgay').on('dp.change', function () {
        searchBenhNhan();
    });
    $(document).on('click', '.btn-remove', function () {
        var id = $(this).val();
        var tr = $(this).closest('tr');
        showModalDanger('Bạn có muốn thực hiện thao tác!');
        $('#btnDanger').on('click', function () {
            $.ajax({
                url: '/QuanLy/QL_TiepNhanBenhNhan/remove', // Đường dẫn đến action xử lý form
                method: 'POST',
                data: "id=" + id,
                success: function (response) {
                    showToast(response.message, response.statusCode);
                    if (response.statusCode == 200) {
                        tr.remove();
                    }
                },
                error: function (xhr, status, error) {
                    console.error(error);
                }
            });
        })
    });
    $(document).on('click', '.btn-unactive', function () {
        var id = $(this).val();
        var tr = $(this).closest('tr');
        showModalDanger('Bạn có muốn thực hiện thao tác!');
        $('#btnDanger').on('click', function () {
            $.ajax({
                url: '/QuanLy/QL_TiepNhanBenhNhan/huykichhoat', // Đường dẫn đến action xử lý form
                method: 'POST',
                data: "id=" + id,
                success: function (response) {
                    showToast(response.message, response.statusCode);
                    if (response.statusCode == 200) {
                        tr.remove();
                    }
                },
                error: function (xhr, status, error) {
                    console.error(error);
                }
            });
        })
    });

    $('#btnReloadDSTN').on('click', function () {
        $.ajax({
            url: '/QuanLy/QL_TiepNhanBenhNhan/reloadTableDSTN',
            method: 'POST',
            success: function (result) {
                renderTableDstn(result);
            },
            error: function (error) {
                console.error(error);
            }
        });
    });
    $(document).on('click', '.btn-edit', function () {
        var id = $(this).val();
        if (id) {
            $.ajax({
                url: '/QuanLy/QL_TiepNhanBenhNhan/getBenhNhan',
                method: 'POST',
                data: {
                    id: id
                },
                success: function (result) {
                    _benhNhan = result;

                    appendDataBenhNhan(_benhNhan);

                    var tabLapPhieu = document.getElementById('tabstiepnhan');
                    var tab = new bootstrap.Tab(tabLapPhieu);
                    tab.show();
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
    });

    $('#btnXoaTrang').on('click', function () {
        var form = $('#formTTHC');
        form.find('input[name="Id"]').val('0');
        form.find('input[name="MaBn"]').val('');
        form.find('input[name="TenBn"]').val('');
        form.find('input[name="NgaySinh"]').val('');
        form.find('input[name="NamSinh"]').val('');
        form.find('input[name="DiaChi"]').val('');
        form.find('input[name="DienThoai"]').val('');
        form.find('input[name="Email"]').val('');
        form.find('input[name="SoCccd"]').val('');
        form.find('input[name="NgayCap"]').val('');
        form.find('input[name="NoiCap"]').val('');
        form.find('input[name="NgayDen"]').val(getDateNow());
        form.find('textarea[name="GhiChu"]').val('');

        form.find('select[name="Idgt"]').val(1);
        form.find('select[name="Idqg"]')[0].tomselect.clear();
        form.find('select[name="Idtinh"]')[0].tomselect.clear();
        form.find('select[name="Idquan"]')[0].tomselect.clear();
        form.find('select[name="Idquan"]')[0].tomselect.clearOptions();
        form.find('select[name="Idpx"]')[0].tomselect.clear();
        form.find('select[name="Idpx"]')[0].tomselect.clearOptions();

        form.find('select[name="Iddt"]')[0].tomselect.clear();
        form.find('select[name="Idnn"]')[0].tomselect.clear();
        _benhNhan = null;
    })
});
function searchBenhNhan() {
    $.ajax({
        url: '/QuanLy/QL_TiepNhanBenhNhan/timKiemBenhNhan',
        method: 'POST',
        data: {
            ma: $('#txtMa').val(),
            ten: $('#txtTen').val(),
            sdt: $('#txtSdt').val(),
            ngay: $('#txtNgay').val(),
        },
        success: function (result) {
            renderTableDs(result);
        },
        error: function (error) {
            console.error(error);
        }
    });
}
function renderTableDs(datas) {
    $('#tbody-table-DS').empty();
    datas.forEach(function (d) {
        $('#tbody-table-DS').append(`<tr data-id="${d.id}">
                                                    <td class="text-center MaBn">${d.maBn}</td>
                                                    <td class="text-start TenBn">${d.tenBn}</td>
                                                    <td class="text-start NgaySinh">${formatDay(d.ngaySinh)}</td>
                                                    <td class="text-center NamSinh">${toEmpty(d.namSinh)}</td>
                                                    <td class="text-center Idgt">${(d.idgtNavigation == null ? "" : d.idgtNavigation.tenGioiTinh)}</td>
                                                    <td class="text-start DiaChi">${toEmpty(d.diaChi)}</td>
                                                    <td class="text-center Idtinh">${(d.idtinhNavigation == null ? "" : d.idtinhNavigation.tenTinh)}</td>
                                                    <td class="text-start Idquan">${(d.idquanNavigation == null ? "" : d.idquanNavigation.tenQuan)}</td>
                                                    <td class="text-start Idpx">${(d.idpxNavigation == null ? "" : d.idpxNavigation.tenXa)}</td>
                                                    <td class="text-start DienThoai">${toEmpty(d.dienThoai)}</td>
                                                    <td class="text-start Email">${toEmpty(d.email)}</td>
                                                    <td class="text-start Iddt">${(d.iddtNavigation == null ? "" : d.iddtNavigation.tenDanToc)}</td>
                                                    <td class="text-start Idnn">${(d.idnnNavigation == null ? "" : d.idnnNavigation.tenNgheNghiep)}</td>
                                                    <td class="text-start Idqg">${(d.idqgNavigation == null ? "" : d.idqgNavigation.tenQuocGia)}</td>
                                                    <td class="text-start SoCccd">${toEmpty(d.soCccd)}</td>
                                                    <td class="text-start NgayCap">${formatDay(d.ngayCap)}</td>
                                                    <td class="text-start GhiChu">${(d.ghiChu == null ? "" : d.ghiChu)}</td>
                                                    <td class="text-start NgayDen">${(formatDay(d.ngayDen == null ? null : d.ngayDen))}</td>
                                                    <td class="text-center">
                                                        <div class="btn-group" role="group" aria-label="Basic outlined example">
                                                            <button class="btn btn-icon bg-green-lt btn-active" value="${d.id}" data-bs-toggle="tooltip" data-bs-placement="left" title="Kích hoạt">
                                                                <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-brand-supabase" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                                                                    <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
                                                                    <path d="M4 14h8v7l8 -11h-8v-7z"></path>
                                                                </svg>
                                                            </button>
                                                            <button class="btn btn-icon bg-azure-lt btn-edit" value="${d.id}" data-bs-toggle="tooltip" data-bs-placement="left" title="Sửa">
                                                                <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-edit" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                                                                    <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
                                                                    <path d="M7 7h-1a2 2 0 0 0 -2 2v9a2 2 0 0 0 2 2h9a2 2 0 0 0 2 -2v-1"></path>
                                                                    <path d="M20.385 6.585a2.1 2.1 0 0 0 -2.97 -2.97l-8.415 8.385v3h3l8.385 -8.415z"></path>
                                                                    <path d="M16 5l3 3"></path>
                                                                </svg>
                                                            </button>
                                                            <button class="btn btn-icon bg-dark-lt btn-remove" value="${d.id}" data-bs-toggle="tooltip" data-bs-placement="right" title="Thay đổi trạng thái">
                                                                <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-trash" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                                                                    <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
                                                                    <path d="M4 7l16 0"></path>
                                                                    <path d="M10 11l0 6"></path>
                                                                    <path d="M14 11l0 6"></path>
                                                                    <path d="M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12"></path>
                                                                    <path d="M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3"></path>
                                                                </svg>
                                                            </button>
                                                        </div>
                                                    </td>
                                                </tr>`)
    })
}
function renderTableDstn(datas) {
    $('#tbody-table-TN').empty();
    datas.forEach(function (d) {
        $('#tbody-table-TN').append(`<tr data-id="${d.id}">
                                                    <td class="text-center MaBn">${d.maBn}</td>
                                                    <td class="text-start TenBn">${d.tenBn}</td>
                                                    <td class="text-start NgaySinh">${formatDay(d.ngaySinh)}</td>
                                                    <td class="text-center NamSinh">${toEmpty(d.namSinh)}</td>
                                                    <td class="text-center Idgt">${(d.idgtNavigation == null ? "" : d.idgtNavigation.tenGioiTinh)}</td>
                                                    <td class="text-start DiaChi">${toEmpty(d.diaChi)}</td>
                                                    <td class="text-center Idtinh">${(d.idtinhNavigation == null ? "" : d.idtinhNavigation.tenTinh)}</td>
                                                    <td class="text-start Idquan">${(d.idquanNavigation == null ? "" : d.idquanNavigation.tenQuan)}</td>
                                                    <td class="text-start Idpx">${(d.idpxNavigation == null ? "" : d.idpxNavigation.tenXa)}</td>
                                                    <td class="text-start DienThoai">${toEmpty(d.dienThoai)}</td>
                                                    <td class="text-start Email">${toEmpty(d.email)}</td>
                                                    <td class="text-start Iddt">${(d.iddtNavigation == null ? "" : d.iddtNavigation.tenDanToc)}</td>
                                                    <td class="text-start Idnn">${(d.idnnNavigation == null ? "" : d.idnnNavigation.tenNgheNghiep)}</td>
                                                    <td class="text-start Idqg">${(d.idqgNavigation == null ? "" : d.idqgNavigation.tenQuocGia)}</td>
                                                    <td class="text-start SoCccd">${toEmpty(d.soCccd)}</td>
                                                    <td class="text-start NgayCap">${formatDay(d.ngayCap)}</td>
                                                    <td class="text-start GhiChu">${(d.ghiChu == null ? "" : d.ghiChu)}</td>
                                                    <td class="text-start NgayDen">${(formatDay(d.ngayDen == null ? null : d.ngayDen))}</td>
                                                    <td class="text-center">
                                                        <div class="btn-group" role="group" aria-label="Basic outlined example">
                                                            <button class="btn btn-icon bg-red-lt btn-unactive" value="${d.id}" data-bs-toggle="tooltip" data-bs-placement="left" title="Huỷ kích hoạt">
                                                                <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-circuit-switch-open" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                                                                    <path stroke="none" d="M0 0h24v24H0z" fill="none"></path>
                                                                    <path d="M2 12h2"></path>
                                                                    <path d="M20 12h2"></path>
                                                                    <path d="M6 12m-2 0a2 2 0 1 0 4 0a2 2 0 1 0 -4 0"></path>
                                                                    <path d="M18 12m-2 0a2 2 0 1 0 4 0a2 2 0 1 0 -4 0"></path>
                                                                    <path d="M7.5 10.5l7.5 -5.5"></path>
                                                                </svg>
                                                            </button>
                                                        </div>
                                                    </td>
                                                </tr>`)
    })
}
function appendDataBenhNhan(bn) {
    var form = $('#formTTHC');
    form.find('input[name="Id"]').val(bn.id);
    form.find('input[name="MaBn"]').val(bn.maBn);
    form.find('input[name="TenBn"]').val(bn.tenBn);
    form.find('input[name="NgaySinh"]').val(formatDay(bn.ngaySinh));
    form.find('input[name="NamSinh"]').val(bn.namSinh);
    form.find('input[name="DiaChi"]').val(bn.diaChi);
    form.find('input[name="DienThoai"]').val(bn.dienThoai);
    form.find('input[name="Email"]').val(bn.email);
    form.find('input[name="SoCccd"]').val(bn.soCccd);
    form.find('input[name="NgayCap"]').val(formatDay(bn.ngayCap));
    form.find('input[name="NoiCap"]').val(bn.noiCap);
    form.find('input[name="NgayDen"]').val(formatDay(bn.ngayDen));
    form.find('textarea[name="GhiChu"]').val(bn.ghiChu);

    form.find('select[name="Idgt"]').val(bn.idgt);
    form.find('select[name="Idqg"]')[0].tomselect.setValue(bn.idqg);

    form.find('select[name="Idtinh"]')[0].tomselect.setValue(bn.idtinh);
    setTimeout(function () {
        form.find('select[name="Idquan"]')[0].tomselect.setValue(bn.idquan);
        setTimeout(function () {
            form.find('select[name="Idpx"]')[0].tomselect.setValue(bn.idpx);
        }, 150)
    }, 100);
    
    form.find('select[name="Iddt"]')[0].tomselect.setValue(bn.iddt);
    form.find('select[name="Idnn"]')[0].tomselect.setValue(bn.idnn);
}