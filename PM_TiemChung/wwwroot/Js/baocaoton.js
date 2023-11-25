$(document).ready(function () {
    configDateDefault();
    datas = [
        {
            className: "select.cbHangHoa",
            placeholder: "-- Vaccine --",
            action: "HH_NhapKho/getListVaccine",
        },
        {
            className: "select.cbNhaCungCap",
            placeholder: "-- Nơi cung Cấp --",
            action: "HH_NhapKho/getListNhaCungCap",
        }
    ];
    configCbDataBase(datas);
    $('#formBaoCaoTon').on('submit', function (event) {
        event.preventDefault();
        event.stopPropagation();
        var formData = $(this).serialize();

        $.ajax({
            url: '/HangHoa/HH_BaoCaoTon/searchBaoCaoTon', 
            method: 'POST',
            data: formData,
            success: function (response) {
                console.log(response)
                $('#tBody-BaoCaoTon').empty();
                response.forEach(function (data, index) {
                    $('#tBody-BaoCaoTon').append(`<tr>
                                                <td class="text-center p-1">${index + 1}</td>
                                                <td class="text-center p-1">${data.ngayGioNhap ?? ''}</td>
                                                <td class="text-center p-1">${data.soPhieuNhap ?? ''}</td>
                                                <td class="text-center p-1">${data.maNcc ?? ''}</td>
                                                <td class="p-1">${data.tenNcc ?? ''}</td>
                                                <td class="p-1">${data.soHd ?? ''}</td>
                                                <td class="p-1">${formatDay(data.ngayHd) ?? ''}</td>
                                                <td class="text-center p-1">${data.maHh ?? ''}</td>
                                                <td class="p-1">${data.tenHh ?? ''}</td>
                                                <td class="text-end p-1">${formatOddNumber(data.soLuongNhap)}</td>
                                                <td class="p-1">${data.donViTinh ?? ''}</td>
                                                <td class="text-end p-1">${formatOddNumber(data.giaNhap)}</td>
                                                <td class="text-end p-1">${formatOddNumber(data.soLuongNhap * data.giaNhap)}</td>
                                                <td class="text-end p-1">${formatOddNumber(data.soLuongTon)}</td>
                                                <td class="text-end p-1">${formatOddNumber(data.soLuongTon * data.giaNhap)}</td>
                                                <td class="text-center p-1">${formatDay(data.hanDung) ?? ''}</td>
                                            </tr>`)
                })
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    })
    $('#btnInPDFBaoCao').on('click', function () {
        var formData = $('#formBaoCaoTon').serialize();

        $.ajax({
            url: '/HangHoa/HH_BaoCaoTon/inBaoCaoTon',
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

    $('#formBaoCaoLoiNhuan').on('submit', function (event) {
        event.preventDefault();
        event.stopPropagation();
        var formData = $(this).serialize();

        $.ajax({
            url: '/HangHoa/HH_BaoCaoTon/searchBaoCaoLoiNhuan',
            method: 'POST',
            data: formData,
            success: function (response) {
                console.log(response);
                $('#tBody-BaoCaoLoiNhuan').empty();
                response.forEach(function (data, index) {
                    $('#tBody-BaoCaoLoiNhuan').append(`<tr>
                                                <td class="text-center p-1">${index + 1}</td>
                                                <td class="text-center p-1">${formatDay(data.ngayThu) ?? ''}</td>
                                                <td class="text-center p-1">${data.idbnNavigation.maBn ?? ''}</td>
                                                <td class="p-1">${data.idbnNavigation.tenBn ?? ''}</td>
                                                <td class="text-end p-1">${formatOddNumber(data.idpnctNavigation.donGia)}</td>
                                                <td class="text-end p-1">${formatOddNumber(data.donGia)}</td>
                                                <td class="text-end p-1">${formatOddNumber(data.donGia - data.idpnctNavigation.donGia)}</td>
                                            </tr>`);
                })
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    })
    $('#btnInPDFBaoCaoLoiNhuan').on('click', function () {
        var formData = $('#formBaoCaoLoiNhuan').serialize();

        $.ajax({
            url: '/HangHoa/HH_BaoCaoTon/inBaoCaoLoiNhuan',
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