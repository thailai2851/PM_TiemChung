var _idBenhNhan = null;
$(document).ready(function () {
    configDateDefault();

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

    $(document).on('click', '.btn-kham', function () {
        var id = $(this).val();
        if (id) {
            $.ajax({
                url: '/QuanLy/QL_KhamBenh/loadTTLichTiem',
                method: 'POST',
                data: "idBn=" + id,
                success: function (result) {
                    console.log(result);
                    _idBenhNhan = result.idBenhNhan;
                    var tabLapPhieu = document.getElementById('tabsKhamBenh');
                    var tab = new bootstrap.Tab(tabLapPhieu);
                    tab.show();
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
    });

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
})
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
    datas.forEach(function (d) {
        var checkDuDk = (soNgayTuoi >= (d.tgsomNhat * d.idthoiGianNavigation.soNgay)) && (soNgayTuoi <= (d.tgtreNhat * d.idthoiGianNavigation.soNgay));

        $('#tbody-lichTiem').append(`<tr data-id="${d.id}">
                                                    <td class="p-0">
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
                                                    <td class="text-center"><input ${d.ngayTiem ? "checked" : ""} type="checkbox" class="form-check-input" name="DaTiem"/></td>
                                                    <td class="text-start"><input ${checkDuDk ? `` : `disabled`} value="${formatDay(toEmpty(d.ngayTiem))}" class="form-control form-table input-date-long-mask w-100" name="NgayTiem"/></td>
                                                    <td class="text-center"><input ${checkDuDk ? `` : `disabled`} ${d.deNghiTiem ? "checked" : ""} type="checkbox" class="form-check-input" name="DeNghiTiem"/></td>
                                                    <td class="text-start"><input ${checkDuDk ? `` : `disabled`} value="${formatDay(toEmpty(d.ngayDeNghi))}" class="form-control form-table input-date-long-mask w-100" name="NgayDeNghiTiem"/></td>
                                                    <td class="text-start px-0"><input ${checkDuDk ? `` : `disabled`} value="${formatDay(toEmpty(d.ngayHen))}" class="form-control form-table input-date-long-mask w-100" name="NgayHen"/></td>
                                                </tr>`);
        var tr = $('#tbody-lichTiem tr:last');
        configDateLongMaskWithElement(tr.find('.input-date-long-mask'));
    })
}