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
                $('#tBody-BaoCaoTon').empty();
                response.forEach(function (data, index) {
                    $('#tBody-BaoCaoTon').append(`<tr>
                                                <td class="text-center ">${index + 1}</td>
                                                <td class="text-center ">${data.ngayGioNhap ?? ''}</td>
                                                <td class="text-center ">${data.soPhieuNhap ?? ''}</td>
                                                <td class="text-center ">${data.maNcc ?? ''}</td>
                                                <td class="">${data.tenNcc ?? ''}</td>
                                                <td class="text-center">${data.soHd ?? ''}</td>
                                                <td class="text-center">${formatDay(data.ngayHd) ?? ''}</td>
                                                <td class="text-center">${data.maHh ?? ''}</td>
                                                <td class="">${data.tenHh ?? ''}</td>
                                                <td class="text-end">${formatOddNumber(data.soLuongNhap)}</td>
                                                <td class="text-center">${data.donViTinh ?? ''}</td>
                                                <td class="text-end ">${formatOddNumber(data.giaNhap)}</td>
                                                <td class="text-end ">${formatOddNumber(data.soLuongNhap * data.giaNhap)}</td>
                                                <td class="text-end ">${formatOddNumber(data.soLuongTon)}</td>
                                                <td class="text-end ">${formatOddNumber(data.soLuongTon * data.giaNhap)}</td>
                                                <td class="text-center ">${formatDay(data.hanDung) ?? ''}</td>
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
                a.download = "BaoCaoTon.pdf";
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
                $('#tBody-BaoCaoLoiNhuan').empty();
                response.forEach(function (data, index) {
                    $('#tBody-BaoCaoLoiNhuan').append(`<tr>
                                                <td class="text-center ">${index + 1}</td>
                                                <td class="text-center ">${formatDay(data.ngayThu) ?? ''}</td>
                                                <td class="text-center ">${data.idbnNavigation.maBn ?? ''}</td>
                                                <td class="">${data.idbnNavigation.tenBn ?? ''}</td>
                                                <td class="">${data.idvcNavigation.tenVaccine ?? ''}</td>
                                                <td class="text-end ">${formatOddNumber(data.idpnctNavigation.donGia)}</td>
                                                <td class="text-end ">${formatOddNumber(data.donGia)}</td>
                                                <td class="text-end ">${formatOddNumber(data.donGia - data.idpnctNavigation.donGia)}</td>
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
                a.download = "BaoCaoLoiNhuan.pdf";
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