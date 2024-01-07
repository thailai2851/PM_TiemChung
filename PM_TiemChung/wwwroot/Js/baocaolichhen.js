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
             //   console.log(response);
                $('#tBody-BaoCaoLoiNhuan').empty();
                response.forEach(function (data, index) {
                    $('#tBody-BaoCaoLoiNhuan').append(`<tr>
                                                <td class="text-center ">${index + 1}</td>
                                                <td class="text-center ">${formatDay(data.ngayHen)}</td>
                                                <td class="text-start">${data.idbnNavigation.tenBn}</td>
                                                <td class="text-center">${data.idbnNavigation.namSinh}</td>
                                                <td class="text-center">${data.idbnNavigation.idgtNavigation.tenGioiTinh}</td>
                                                <td class="text-start">${data.idbnNavigation.diaChi}</td>
                                                <td class="text-end">${data.idbnNavigation.dienThoai}</td>
                                                <td class="text-start">${data.idbnNavigation.email == null ? "" : data.idbnNavigation.email}</td>
                                                <td class="text-start">${data.idbnNavigation?.idnnNavigation == null ? "" : data.idbnNavigation?.idnnNavigation.tenNgheNghiep}</td>
                                                <td class="text-start">${data.idvcNavigation.tenVaccine}</td>
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