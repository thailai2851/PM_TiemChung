$(document).ready(function () {
    configDateDefault();
    $('#formBaoCaoLichHen').on('submit', function (event) {
        event.preventDefault();
        event.stopPropagation();
        var formData = $(this).serialize();

        $.ajax({
            url: '/QuanLy/QL_BaoCaoLichHen/searchBaoCaoLichHen',
            method: 'POST',
            data: formData,
            success: function (response) {
                console.log(response);
                $('#tBody-BaoCaoLoiNhuan').empty();
                response.forEach(function (data, index) {
                    $('#tBody-BaoCaoLoiNhuan').append(`<tr>
                                                <td class="text-center p-1">${index + 1}</td>
                                                <td class="text-center p-1">${formatDay(data.ngayHen)}</td>
                                                <td class="p-1">${data.idbnNavigation.tenBn}</td>
                                                <td class="text-center p-1">${data.idbnNavigation.namSinh}</td>
                                                <td class="p-1">${data.idbnNavigation.idgtNavigation.tenGioiTinh}</td>
                                                <td class="p-1">${data.idbnNavigation.diaChi}</td>
                                                <td class="p-1">${data.idbnNavigation.dienThoai}</td>
                                                <td class="p-1">${data.idbnNavigation.email}</td>
                                                <td class="p-1">${data.idbnNavigation.idnnNavigation.tenNgheNghiep}</td>
                                                <td class="p-1">${data.idvcNavigation.tenVaccine}</td>
                                            </tr>`);
                })
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    })
    $('#btnInPDFBaoCao').on('click', function () {
        var formData = $('#formBaoCaoLichHen').serialize();

        $.ajax({
            url: '/QuanLy/QL_BaoCaoLichHen/inBaoCaoLichHen',
            method: 'POST',
            data: formData,
            xhrFields: {
                responseType: 'blob'
            },
            success: function (result) {
                var a = document.createElement('a');
                var url = window.URL.createObjectURL(result);
                a.href = url;
                a.download = "file.pdf";
                document.body.appendChild(a);
                a.click();
                setTimeout(function () {
                    document.body.removeChild(a);
                    window.URL.revokeObjectURL(url);
                }, 0);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    })
})