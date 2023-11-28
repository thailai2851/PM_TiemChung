$(document).ready(function () {
    configDateDefault();
    datas = [
        {
            className: "select.cbHangHoa",
            placeholder: "-- Vaccine --",
            action: "HH_NhapKho/getListVaccine",
        }
    ];
    configCbDataBase(datas);

    $('#formBaoCaoHSD').on('submit', function (event) {
        event.preventDefault();
        event.stopPropagation();
        var formData = $(this).serialize();

        $.ajax({
            url: '/HangHoa/HH_BaoCaoHSD/searchBaoCaoHSD',
            method: 'POST',
            data: formData,
            success: function (response) {
                console.log(response)
                $('#tBody-BaoCaoHSD').empty();
                response.forEach(function (data, index) {
                    $('#tBody-BaoCaoHSD').append(`<tr>
                                                <td class="text-center ">${index + 1}</td>
                                                <td class="text-center">${formatDay(data.nsx) ?? ''}</td>
                                                <td class="text-center">${formatDay(data.hsd) ?? ''}</td>
                                                <td class="text-center">${data.mavc ?? ''}</td>
                                                <td>${data.tenvc ?? ''}</td>
                                                <td>${data.dvt ?? ''}</td>
                                                <td class="text-end">${formatOddNumber(data.sl)}</td>
                                                <td class="text-end ">${formatOddNumber(data.slx)}</td>
                                                <td class="text-end ">${formatOddNumber(data.sl - data.slx)}</td>
                                                <td>${data.ncc ?? ''}</td>
                                            </tr>`)
                })
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    })
    $('#btnInPDFBaoCao').on('click', function () {
        var formData = $('#formBaoCaoHSD').serialize();

        $.ajax({
            url: '/HangHoa/HH_BaoCaoHSD/inBaoCaoHSD',
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
})