var _idBenhNhan = null;
$(document).ready(function () {
    configDateDefault();
    configDateLongMaskWithElement($('tr .input-date-long-mask'));
    $('#btnReloadDSTN').on('click', function () {
        $.ajax({
            url: '/QuanLy/QL_KhamBenh/reloadTableDSTN',
            method: 'POST',
            success: function (result) {
                renderTableDs(result);
            },
            error: function (error) {
                console.error(error);
            }
        });
    });
    $('#btnReloadDSThuNgan').on('click', function () {
        $.ajax({
            url: '/QuanLy/QL_KhamBenh/reloadDSThuNgan',
            method: 'POST',
            data: "ngay=" + $('#txtNgayDeNghi').val(),
            success: function (result) {
                renderTableDsThuNgan(result);
            },
            error: function (error) {
                console.error(error);
            }
        });
    });
    $('#btnReloadDSTiemChung').on('click', function () {
        $.ajax({
            url: '/QuanLy/QL_KhamBenh/reloadDSTiemChung',
            method: 'POST',
            data: "ngay=" + $('#txtNgayThuNgan').val(),
            success: function (result) {
                renderTableTiemChung(result);
            },
            error: function (error) {
                console.error(error);
            }
        });
    });
    $(document).on('change', 'input[name="DeNghiTiem"]', function () {
        var checked = this.checked;
        var ngayDeNghi = $(this).closest('tr').find('input[name="NgayDeNghiTiem"]');
        if (checked) {
            ngayDeNghi.val(getDateNow());
        } else {
            ngayDeNghi.val('');
        }
    });
    $(document).on('change', 'input.daTiem', function () {
        var checked = this.checked;
        var ngayDeNghi = $(this).closest('tr').find('input[name="NgayTiem"]');
        if (checked) {
            ngayDeNghi.val(getDateNow());
        } else {
            ngayDeNghi.val('');
        }
    });
    $(document).on('change', 'input[name="DaThu"]', function () {
        var checked = this.checked;
        var ngayDeNghi = $(this).closest('tr').find('input[name="NgayThu"]');
        if (checked) {
            ngayDeNghi.val(getDateNow());
        } else {
            ngayDeNghi.val('');
        }
    });
    $(document).on('change', 'input.checkDaTiem', function () {
        var checked = this.checked;
        var tr = $(this).closest('tr');
        var deNghi = tr.find('input[name="DeNghiTiem"]');
        var ngayDeNghiTiem = tr.find('input[name="NgayDeNghiTiem"]');
        var ngayHen = tr.find('input[name="NgayHen"]');
        if (checked) {
            deNghi.val('').prop('disabled', true);
            ngayDeNghiTiem.val('').prop('disabled', true);
            ngayHen.val('').prop('disabled', true);
        } else {
            if ($(this).data('checkdudk')) {
                deNghi.prop('disabled', false);
                ngayDeNghiTiem.prop('disabled', false);
                ngayHen.prop('disabled', false);
            }
        }
    });
    $(document).on('click', '.btn-kham', function () {
        var id = $(this).val();
        showBn(id, true);
    });
    $('#btnReloadBn').click(function () {
        if (_idBenhNhan) {
            $.ajax({
                url: '/QuanLy/QL_KhamBenh/reloadTTLichTiem',
                method: 'POST',
                data: "idBn=" + _idBenhNhan,
                success: function (result) {
                    showToast(result.message, result.statusCode);
                    if (result.statusCode == 200) {
                        showBn(_idBenhNhan, false);
                    }
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
    })
    $('#btnLuu').on('click', function () {
        if (_idBenhNhan) {
            var listLichTiem = [];
            $('#tbody-lichTiem tr').each(function () {
                var lichTiem = getDataFromTr($(this));
                listLichTiem.push(lichTiem);
            })

            $.ajax({
                url: '/QuanLy/QL_KhamBenh/luuLichTiem',
                method: 'POST',
                data: JSON.stringify(listLichTiem),
                contentType: "application/json",
                success: function (result) {
                    showToast(result.message, result.statusCode);
                    var datas = result.data;
                    var inputIds = [];
                    if (result.statusCode == 200) {
                        $('#tbody-lichTiem tr').each(function () {
                            var inputId = $(this).find('input[name="Id"]');
                            if (inputId.val() == 0) {
                                inputIds.push(inputId);
                            }
                        });
                        inputIds.forEach(function (item, index) {
                            item.val(datas[index]);
                        })
                    }
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
    });

    $(document).on('click', '.btn-save-thungan', function () {
        showModalDanger('Bạn có muốn thực hiện thao tác!');
        var id = $(this).val();
        var btn = $(this);
        var tr = $(this).closest('tr');
        $('#btnDanger').on('click', function () {
            var ngayThu = tr.find('input[name="NgayThu"]');
            var daThu = tr.find('input[name="DaThu"]');
            if (id && ngayThu.val() && daThu.prop('checked')) {
                $.ajax({
                    url: '/QuanLy/QL_KhamBenh/luuThuNgan',
                    method: 'POST',
                    data: {
                        id: id,
                        ngayThu: ngayThu.val()
                    },
                    success: function (result) {
                        showToast(result.message, result.statusCode);
                        if (result.statusCode == 200) {
                            daThu.prop('disabled', true);
                            btn.prop('disabled', true);
                            ngayThu.prop('disabled', true);
                            showBn(_idBenhNhan, false);
                        }
                    },
                    error: function (error) {
                        console.error(error);
                    }
                });
            }
        })
    });
    $(document).on('click', '.btn-print-thungan', function () {
        var id = $(this).val();
        if (id) {

            var s = [];
            $(`input[data-idbn="${id}"]:checked`).each(function () {
                s.push(parseInt($(this).val()));
            });
            var model = {
                IdBn: id,
                ListIn: s
            }
            $.ajax({
                url: '/QuanLy/QL_KhamBenh/printThuNgan',
                method: 'POST',
                data: JSON.stringify(model),
                contentType: "application/json",
                xhrFields: {
                    responseType: 'blob'
                },
                success: function (result) {
                    var a = document.createElement('a');
                    var url = window.URL.createObjectURL(result);
                    a.href = url;
                    a.download = "ThuNgan.pdf";
                    document.body.appendChild(a);
                    a.click();
                    setTimeout(function () {
                        document.body.removeChild(a);
                        window.URL.revokeObjectURL(url);
                    }, 0);
                },
                error: function (error) {
                    console.error(error);
                }
            });
        }
    });
    
    $(document).on('click', '.btn-kiemtra', function () {
        var tr = $(this).closest('tr');
        var soCode = tr.find('input.socode').val();
        var codeVaccine = tr.find('.code-vaccine').val();
        if (soCode == codeVaccine) {
            showToast("Chính xác", 200);
            tr.find('button.btn-save-tiemchung').prop('disabled', false);

        } else {
            showToast("Số code không chính xác", 500);
            tr.find('button.btn-save-tiemchung').prop('disabled', true);
        }
    })
    $(document).on('click', '.btn-save-tiemchung', function () {
        showModalDanger('Bạn có muốn thực hiện thao tác!');
        var id = $(this).val();
        var btn = $(this);
        var tr = $(this).closest('tr');
        $('#btnDanger').on('click', function () {
            var ngayTiem = tr.find('input[name="NgayTiem"]');
            var daTiem = tr.find('input[name="DaTiem"]');
            if (id && ngayTiem.val() && daTiem.prop('checked')) {
                $.ajax({
                    url: '/QuanLy/QL_KhamBenh/tiemChung',
                    method: 'POST',
                    data: {
                        id: id,
                        ngayTiem: ngayTiem.val()
                    },
                    success: function (result) {
                        showToast(result.message, result.statusCode);
                        if (result.statusCode == 200) {
                            /*daTiem.prop('disabled', true);
                            btn.prop('disabled', true);
                            ngayTiem.prop('disabled', true);
                            tr.find('.btn-kiemtra').prop('disabled', true);
                            tr.find('.socode').prop('disabled', true);*/
                            $('#btnReloadDSTiemChung').click();
                        }
                    },
                    error: function (error) {
                        console.error(error);
                    }
                });
            }
        })
    })
})
function showBn(id, show) {
    if (id) {
        $.ajax({
            url: '/QuanLy/QL_KhamBenh/loadTTLichTiem',
            method: 'POST',
            data: "idBn=" + id,
            success: function (result) {
                console.log(result);
                _idBenhNhan = result.idBenhNhan;
                if (show) {
                    var tabLapPhieu = document.getElementById('tabsKhamBenh');
                    var tab = new bootstrap.Tab(tabLapPhieu);
                    tab.show();
                }
                $('#txtTenProfile').val(result.tenProFile);
                $('#txtTenBN').val(result.tenBenhNhan);
                $('#txtTuoi').val(result.tuoi);

                renderTableLichTiem(result.lichTiems, result.soNgayTuoi);
            },
            error: function (error) {
                console.error(error);
            }
        });
    }
}
function renderTableDs(datas) {
    $('#tbody-table').empty();
    datas.forEach(function (d) {
        $('#tbody-table').append(`<tr data-id="${d.id}">
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
                                                    <td class="text-center last-td-column px-2">
                                                        <button class="status status-purple border-0 btn-kham" value="${d.id}">Khám</button>
                                                    </td>
                                                </tr>`)
    })
}
function renderTableLichTiem(datas, soNgayTuoi) {
    $('#tbody-lichTiem').empty();
    datas.forEach(function (d, index) {
        var checkMuiTienQuyet = true;
        
        if (d.muiTienQuyetNavigation) {
            checkMuiTienQuyet = false;
            datas.forEach(function (vc) { 
                if ((vc.idvcNavigation.id == d.muiTienQuyetNavigation.id) && vc.daTiem) {
                    checkMuiTienQuyet = true;
                }
            })
        }
        var checkDuDk = (soNgayTuoi >= (d.tgsomNhat * d.idthoiGianNavigation.soNgay)) && (soNgayTuoi <= (d.tgtreNhat * d.idthoiGianNavigation.soNgay)) && checkMuiTienQuyet && !d.daThu;

        $('#tbody-lichTiem').append(`<tr data-id="${d.id}" class="${checkDuDk && !d.daTiem ? "text-azure" : ""}">
                                                    <td class="p-0 text-center">
                                                        ${index + 1}
                                                    </td>
                                                    <td>
                                                        <input readonly class="form-table form-control" type="text" value="${toEmpty(d.idvcNavigation.tenVaccine)}" style="width: 210px"/>
                                                        <input type="hidden" name="Id" value="${toEmpty(d.id)}"/>
                                                        <input type="hidden" name="Idbn" value="${toEmpty(_idBenhNhan)}"/>
                                                        <input type="hidden" name="Idvc" value="${toEmpty(d.idvcNavigation.id)}"/>
                                                    </td>
                                                    <td class="text-end">
                                                        <input name="SoLanTiem" readonly class="form-table form-control" type="text" value="${toEmpty(d.soLanTiem)}" style="width: 110px"/>
                                                    </td>
                                                    <td class="text-end">
                                                        <input name="TgsomNhat" readonly class="form-table form-control" type="text" value="${toEmpty(d.tgsomNhat)}" style="width: 110px"/>
                                                    </td>
                                                    <td class="text-end">
                                                        <input name="TgtreNhat" readonly class="form-table form-control" type="text" value="${toEmpty(d.tgtreNhat)}" style="width: 110px"/>
                                                    </td>
                                                    <td class="">
                                                        <input readonly class="form-table form-control" type="text" value="${toEmpty(d.idthoiGianNavigation.tenTg)}" style="width: 110px"/>
                                                        <input type="hidden" name="IdthoiGian" value="${toEmpty(d.idthoiGianNavigation.id)}"/>
                                                    </td>
                                                    <td class="">
                                                        <input readonly class="form-table form-control" type="text" value="${toEmpty(d.muiTienQuyetNavigation == null ? "" : d.muiTienQuyetNavigation.tenVaccine)}" style="width: 210px"/>
                                                        <input type="hidden" name="MuiTienQuyet" value="${toEmpty(d.muiTienQuyetNavigation == null ? "" : d.muiTienQuyetNavigation.id)}"/>
                                                    </td>
                                                    <td class="text-center"><input ${d.daTiem ? "checked disabled" : ""} data-checkDuDk="${checkDuDk}" type="checkbox" class="form-check-input checkDaTiem" name="DaTiem" style="transform: scale(1.2);"/></td>
                                                    <td class="text-start"><input ${d.daTiem ? "disabled" : ""} value="${formatDay(toEmpty(d.ngayTiem))}" class="form-control form-table input-date-long-mask w-100" name="NgayTiem"/></td>
                                                    <td class="text-center"><input ${checkDuDk ? `` : `disabled`} ${d.daTiem ? "disabled" : ""} ${d.deNghiTiem ? "checked" : ""} type="checkbox" class="form-check-input" name="DeNghiTiem" style="transform: scale(1.2);"/></td>
                                                    <td class="text-start"><input ${checkDuDk ? `` : `disabled`} ${d.daTiem ? "disabled" : ""} value="${formatDay(toEmpty(d.ngayDeNghiTiem))}" class="form-control form-table input-date-long-mask w-100" name="NgayDeNghiTiem"/></td>
                                                    <td class="text-start px-0"><input ${checkDuDk ? `` : `disabled`} ${d.daTiem ? "disabled" : ""} value="${formatDay(toEmpty(d.ngayHen))}" class="form-control form-table input-date-long-mask w-100" name="NgayHen"/></td>
                                                </tr>`);
        var tr = $('#tbody-lichTiem tr:last');
        configDateLongMaskWithElement(tr.find('.input-date-long-mask'));
    })
}
function renderTableDsThuNgan(gr) {
    $('#tbody-tableThuNhan').empty();
    gr.forEach(function (item) {
        $('#tbody-tableThuNhan').append(`<tr class="bg-azure-lt">
                                                    <td class="px-2 py-1 text-center">
                                                        <button class="btn btn-icon bg-green-lt btn-print-thungan" value="${item.idbnNavigation.id}" data-bs-toggle="tooltip" data-bs-placement="left" title="In">
                                                            <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-printer" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none" /><path d="M17 17h2a2 2 0 0 0 2 -2v-4a2 2 0 0 0 -2 -2h-14a2 2 0 0 0 -2 2v4a2 2 0 0 0 2 2h2" /><path d="M17 9v-4a2 2 0 0 0 -2 -2h-6a2 2 0 0 0 -2 2v4" /><path d="M7 13m0 2a2 2 0 0 1 2 -2h6a2 2 0 0 1 2 2v4a2 2 0 0 1 -2 2h-6a2 2 0 0 1 -2 -2z" /></svg>
                                                        </button>
                                                    </td>
                                                    <td colspan="7" class="px-2 py-1">
                                                        ${item.idbnNavigation.maBn + " - " + item.idbnNavigation.tenBn}
                                                    </td>
                                                </tr>`);
        item.datas.forEach(function (lt, i) {
            i++;
            $('#tbody-tableThuNhan').append(`<tr>
                                                        <td class="text-center px-2">
                                                            <input data-idbn="${item.idbnNavigation.id}" class="form-check-input border-dark single-checkbox checkIn" type="checkbox" value="${lt.id}" style="transform: scale(1.2);" />
                                                        </td>
                                                        <td class="text-center STT px-2">
                                                            ${i}
                                                            <input type="hidden" value="${lt.id}" name="Id"/>
                                                        </td>
                                                        <td class="text-start TenVaccine px-2">${lt.idvcNavigation.tenVaccine}</td>
                                                        <td class="text-end SoLuong px-2">1</td>
                                                        <td class="text-end DonGia px-2">${formatOddNumber(toEmpty(lt.idvcNavigation.giaBan))}</td>
                                                        <td class="text-center">
                                                            <input name="DaThu" class="form-check-input border-dark single-checkbox" ${lt.daThu == true ? "checked disabled" : ""} type="checkbox" style="transform: scale(1.2);">
                                                        </td>
                                                        <td class="text-center NgayThu px-0">
                                                            <input name="NgayThu" style="width: 140px" class="form-control form-table input-date-long-mask" ${lt.daThu == true ? "disabled" : ""} value="${formatDay(toEmpty(lt.ngayThu))}" />
                                                        </td>
                                                        <td class="text-center last-td-column px-1">
                                                            <button class="btn btn-icon bg-azure-lt btn-save-thungan" ${lt.daThu == true ? "disabled" : ""} value="${lt.id}" data-bs-toggle="tooltip" data-bs-placement="left" title="Lưu">
                                                                <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-device-floppy" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M6 4h10l4 4v10a2 2 0 0 1 -2 2h-12a2 2 0 0 1 -2 -2v-12a2 2 0 0 1 2 -2" /><path d="M12 14m-2 0a2 2 0 1 0 4 0a2 2 0 1 0 -4 0" /><path d="M14 4l0 4l-6 0l0 -4" /></svg>
                                                            </button>
                                                        </td>
                                                    </tr>`);
            var tr = $('#tbody-tableThuNhan tr:last');
            configDateLongMaskWithElement(tr.find('.input-date-long-mask'));
        })
    })
}
function renderTableTiemChung(datas) {
    $('#tbody-tiemchung').empty();
    datas.forEach(function (lt, index) {
        var checkDaTiem = lt.daTiem == true;
        $('#tbody-tiemchung').append(`<tr>
                                                    <td class="text-center STT px-2">${index + 1}
                                                        <input class="code-vaccine" value="${lt.idvcNavigation.soCode}" type="hidden"/>
                                                    </td>
                                                    <td class="text-start TenBn px-2">${lt.idbnNavigation.tenBn}</td>
                                                    <td class="text-start TenVaccine px-2">${lt.idvcNavigation.tenVaccine}</td>
                                                    <td class="text-end SoLuong px-2">${lt.soLuong}</td>
                                                    <td class="text-center SoCode">
                                                        <input style="width: 140px" ${checkDaTiem ? "disabled" : ""} class="form-control form-table socode" value="" />
                                                    </td>
                                                    <td class="text-center last-td-column px-2">
                                                        <button ${checkDaTiem ? "disabled" : ""} class="status status-purple border-0 btn-kiemtra">Kiểm tra</button>
                                                    </td>
                                                    <td class="text-center">
                                                        <input ${checkDaTiem ? "disabled checked" : ""} class="form-check-input border-dark single-checkbox daTiem" name="DaTiem" type="checkbox" style="transform: scale(1.2);">
                                                    </td>
                                                    <td class="text-center">
                                                        <input ${checkDaTiem ? "disabled" : ""} name="NgayTiem" style="width: 140px" class="form-control form-table input-date-long-mask" value="${formatDay(lt.ngayTiem)}" />
                                                    </td>
                                                    <td class="text-center last-td-column px-2">
                                                        <button value="${lt.id}" disabled class="btn btn-icon bg-azure-lt btn-save-tiemchung" data-bs-toggle="tooltip" data-bs-placement="left" title="Lưu">
                                                            <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-device-floppy" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round"><path stroke="none" d="M0 0h24v24H0z" fill="none" /><path d="M6 4h10l4 4v10a2 2 0 0 1 -2 2h-12a2 2 0 0 1 -2 -2v-12a2 2 0 0 1 2 -2" /><path d="M12 14m-2 0a2 2 0 1 0 4 0a2 2 0 1 0 -4 0" /><path d="M14 4l0 4l-6 0l0 -4" /></svg>
                                                        </button>
                                                    </td>
                                                </tr>`);
        var tr = $('#tbody-tiemchung tr:last');
        configDateLongMaskWithElement(tr.find('.input-date-long-mask'));
    })
}