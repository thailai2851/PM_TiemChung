var _labelsTD = [];
var _valuesTD = [];
var _myChartTD = null;
$(document).ready(function () {
    configDateDefault();
    baoCaoChiTieu();
});
function baoCaoChiTieu() {
    $.ajax({
        url: '/HangHoa/HH_BaoCaoDoThi/BaoCaoChiTieu',
        method: 'POST',
        data: "fromDay=" + $('#tuNgay').val() + "&toDay=" + $('#denNgay').val(),
        success: function (data) {
            console.log(data);
            _labelsTD = [];
            _valuesTD = [];
            $('#tbodyTD').empty();
            // Chuyển dữ liệu từ JSON sang mảng để cấu hình đồ thị
            data.doThiThucDon.forEach(function (item,i) {
                _labelsTD.push(item.label); // Định dạng ngày tháng
                _valuesTD.push(item.soLuong);
                addRowTableTD(item,i);
            });

            // Xóa biểu đồ cũ trước khi vẽ biểu đồ mới
            if (_myChartTD !== null) {
                _myChartTD.destroy();
            }
            renderDoThiThucDon(_labelsTD, _valuesTD);
        },
        error: function (error) {
            console.log(error);
        }
    });
}
function addRowTableTD(data, i) {
    var stt = i + 1;
    var newRow = $(`<tr>
                <td>${stt}</td>
                <td class="text-start">${data.label}</td>
                <td><input type="text" readonly class=" text-center form-control formatted-number" name="doanhThu" value="${data.soLuong}" /></td>
    </tr>`)
    $('#tbodyTD').append(newRow);
    TinhTongTD();
}
function TinhTongTD() {
    var tongTien = 0;
    $('#tbodyTD tr').each(function () {
        var thanhTien = parseInt($(this).find('input[name="doanhThu"]').val().replace(/,/g, ''));
        if (!isNaN(thanhTien)) {
            tongTien += thanhTien;
        }

    });
    $('#tongTD').val((tongTien));
}
function renderDoThiThucDon(labels, values) {
    var ctx = document.getElementById('doThiThucDon').getContext('2d');

    var doanhThuData = {
        labels: labels,
        datasets: [{
            label: 'Số Lượng',
            data: values,
            backgroundColor: 'rgba(255, 165, 0, 1)',
            borderColor: 'rgba(75, 192, 192, 1)',
            borderWidth: 1
        }]
    };
    console.log(doanhThuData);
    _myChartTD = new Chart(ctx, {
        type: 'bar',
        data: doanhThuData,
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value, index, values) {
                            return value; // Định dạng các giá trị trên trục y thành kiểu tiền tệ
                        }
                    }
                }
            }
        }
    });
}
function formatCurrencyVN(value) {
    console.log(value);
    // Sử dụng toLocaleString để định dạng số thành tiền tệ
    return value.toLocaleString('vi-VN', {
        style: 'currency',
        currency: 'VND'
    });
}