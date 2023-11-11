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
})
function renderTableDs(datas, table) {
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