var _id = 0;
$(document).ready(function () {
    $(document).on('click', '#tbody-Form tr', function () {
        var id = $(this).data('id');

        if (id) {
            $.ajax({
                type: "post",
                url: "/DanhMuc/DM_Profile/showProfile",
                data: "id=" + id,
                success: function (response) {
                    _id = id;
                    $('#txtTenProfile').val(response.tenProfile);
                    $('#cbGioiTinh').val(response.idgt);
                    updateTable(response.dmProfileCts);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    });

    $('#btnAddRowCt').on('click', function () {
        $.ajax({
            type: "post",
            url: "/DanhMuc/DM_ThoiGian/getListThoiGian",
            success: function (r1) {
                $.ajax({
                    type: "post",
                    url: "/DanhMuc/DM_Vaccine/getListVaccine",
                    success: function (r2) {
                        var options = `<option value="">-- Thời gian --</option>`;
                        r1.forEach(function (item) {
                            options += `<option value="${item.id}">${item.tenTg}</option>`
                        });
                        var stt = $('#tbody-tableCt tr').length;
                        $('#tbody-tableCt').append(`<tr>
                            <td class="text-center p-0 stt">${stt + 1}
                                <input type="hidden" value="0" name="Id"/>
                            </td>
                            <td>
                                <select style="width:260px;" class="form-select form-table" name="Idvaccine">
                                    
                                </select>
                            </td>
                            <td><input value="" class="form-control form-table input-number" style="width:100px;" name="SoLanTiem"/></td>
                            <td><input value="" class="form-control form-table input-number" style="width:120px;" name="TgsomNhat"/></td>
                            <td><input value="" class="form-control form-table input-number" style="width:100px;" name="TgtreNhat"/></td>
                            <td>
                                <select style="width:100px;" class="form-select form-table" name="IdthoiGian">
                                    ${options}
                                </select>
                            </td>
                            <td>
                                <select style="width:260px;" class="form-select form-table" name="MuiTienQuyet">
                                    
                                </select>
                            </td>
                            <td class="text-center last-td-column p-1">
                              <button class="btn btn-icon btn-sm text-red btn-remove-ct" value="0" data-bs-toggle="tooltip" data-bs-placement="right" title="Xoá">
                                    <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-trash" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">< path stroke = "none" d = "M0 0h24v24H0z" fill = "none" ></path ><path d="M4 7l16 0"></path><path d="M10 11l0 6"></path><path d="M14 11l0 6"></path><path d="M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12"></path><path d="M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3"></path></svg >
                                </button>
                            </td>
                        </tr>`);
                        configRow(r2);
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            },
            error: function (error) {
                console.log(error);
            }
        });
    });

    $('#btnLuuForm').on('click', function (event) {
        if (_id) {
            var tenProfile = $('#txtTenProfile').val();
            if (tenProfile) {
                var btn = $(this);
                spinnerBtn(btn);
                var DmProfileCts = [];
                $('#tbody-tableCt tr').each(function () {
                    DmProfileCts.push(getDataFromTr($(this)));
                });

                var profile = {
                    Id: _id,
                    Idgt: parseInt($('#cbGioiTinh').val()),
                    TenProfile: tenProfile,
                    DmProfileCts: DmProfileCts,
                }

                $.ajax({
                    url: '/DanhMuc/DM_Profile/modifyProfile', // Đường dẫn đến action xử lý form
                    method: 'POST',
                    data: JSON.stringify(profile),
                    contentType: "application/json",
                    success: function (response) {
                        showBtn(btn, 'Lưu');
                        showToast(response.message, response.statusCode);
                        if (response.statusCode == 200) {
                            $(`#tbody-Form tr[data-id="${_id}"]`).find(".ten").text(response.data.tenProfile);
                        }
                    },
                    error: function (xhr, status, error) {
                        // Xử lý lỗi (nếu có) khi gửi form
                        console.error(error);
                    }
                });
            } else {
                showToast('Chưa nhập tên profile!', 400);
            }
        } else {
            showToast('Chưa chọn profile!', 400);
        }
    });
    $(document).on('click', '.btn-remove-ct', function () {
        var tr = $(this).closest('tr');
        var id = $(this).val();
        if (id == 0) {
            tr.find("select").each(function () {
                if ($(this)[0].tomselect) {
                    $(this)[0].tomselect.destroy();
                }
            })
            tr.remove();
            $('#tbody-tableCt tr').each(function (index) {
                $(this).find('.stt').text(index + 1);
            });
            return;
        }
        showModalDanger('Bạn có muốn thực hiện thao tác!');
        $('#btnDanger').on('click', function () {
            $.ajax({
                url: '/DanhMuc/DM_Profile/removeProfileCt', // Đường dẫn đến action xử lý form
                method: 'POST',
                data: "id=" + id,
                success: function (response) {
                    showToast(response.message, response.statusCode);
                    if (response.statusCode == 200) {
                        tr.find("select").each(function () {
                            if ($(this)[0].tomselect) {
                                $(this)[0].tomselect.destroy();
                            }
                        })
                        tr.remove();
                        $('#tbody-tableCt tr').each(function (index) {
                            $(this).find('.stt').text(index + 1);
                        });
                    }
                },
                error: function (xhr, status, error) {
                    console.error(error);
                }
            });
        })
    });
})
function updateTable(datas) {
    emptyTableCt();
    $.ajax({
        type: "post",
        url: "/DanhMuc/DM_ThoiGian/getListThoiGian",
        success: function (r1) {
            $.ajax({
                type: "post",
                url: "/DanhMuc/DM_Vaccine/getListVaccine",
                success: function (r2) {
                    datas.forEach(function (data, index) {
                        addRowTableCt(data, r2, r1, index + 1);
                    })
                },
                error: function (error) {
                    console.log(error);
                }
            });
        },
        error: function (error) {
            console.log(error);
        }
    });
}
function addRowTableCt(data, optionVcs, optionsTg, stt) {
    var options = `<option value="">-- Thời gian --</option>`;
    optionsTg.forEach(function (item) {
        
        options += `<option value="${item.id}" ${item.id == data.idthoiGian ? `selected` : ``}>${item.tenTg}</option>`
    });
    $('#tbody-tableCt').append(`<tr>
            <td class="text-center p-0 stt">${stt}
                <input type="hidden" value="${data.id}" name="Id"/>
            </td>
            <td>
                <select style="width:260px;" class="form-select form-table" name="Idvaccine">
                    <option value="${data.idvaccine}"></option>
                </select>
            </td>
            <td><input value="${data.soLanTiem}" class="form-control form-table input-number" style="width:100px;" name="SoLanTiem"/></td>
            <td><input value="${data.tgsomNhat}" class="form-control form-table input-number" style="width:120px;" name="TgsomNhat"/></td>
            <td><input value="${data.tgtreNhat}" class="form-control form-table input-number" style="width:100px;" name="TgtreNhat"/></td>
            <td>
                <select style="width:100px;" class="form-select form-table" name="IdthoiGian">
                    ${options}
                </select>
            </td>
            <td>
                <select style="width:260px;" class="form-select form-table" name="MuiTienQuyet">
                    <option value="${data.muiTienQuyet}"></option>
                </select>
            </td>
            <td class="text-center last-td-column p-1">
              <button class="btn btn-icon btn-sm text-red btn-remove-ct" value="${data.id}" data-bs-toggle="tooltip" data-bs-placement="right" title="Xoá">
                    <svg xmlns="http://www.w3.org/2000/svg" class="icon icon-tabler icon-tabler-trash" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">< path stroke = "none" d = "M0 0h24v24H0z" fill = "none" ></path ><path d="M4 7l16 0"></path><path d="M10 11l0 6"></path><path d="M14 11l0 6"></path><path d="M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12"></path><path d="M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3"></path></svg >
                </button>
            </td>
        </tr>`);
    configRow(optionVcs);
}
function xoaTrangForm() {
    _id = 0;
    $('#txtTenProfile').val('');
    $('#cbGioiTinh').val('');
}
function emptyTableCt() {
    $("#tbody-tableCt tr").each(function () {
        $(this).find("select").each(function () {
            if ($(this)[0].tomselect) {
                $(this)[0].tomselect.destroy();
            }
        })
    });
    $("#tbody-tableCt").empty();
}
function configRow(optionVcs) {
    var tr = $('#tbody-tableCt tr:last');
    formatNumberWithElement(tr.find('.input-number'));

    var cbVaccine = tr.find('select[name="Idvaccine"]');
    var selectVaccine = new TomSelect(cbVaccine, {
        selectOnTab: true,
        loadingClass: "Đang tìm kiếm...",
        maxOptions: 50,
        valueField: 'id',
        labelField: 'ten',
        placeholder: '-- Vaccine --',
        options: optionVcs,
        searchField: ["ten", "ma"],
        // fetch remote data
        dropdownParent: '#dropdow-show',
        onDropdownOpen: function () {
            showDropdownMenu(cbVaccine, selectVaccine);
        },
        render: {
            no_results: function (data, escape) {
                return '<div class="no-results">Không tìm thấy dữ liệu </div>';
            },
            dropdown: function () {
                return '<div style="min-width: 50vw;"></div>';
            }
        },
        loadThrottle: 400,
    });

    var cbMuiTienQuyet = tr.find('select[name="MuiTienQuyet"]');
    var selectMuiTienQuyet = new TomSelect(cbMuiTienQuyet, {
        selectOnTab: true,
        loadingClass: "Đang tìm kiếm...",
        maxOptions: 50,
        valueField: 'id',
        labelField: 'ten',
        placeholder: '-- Vaccine --',
        options: optionVcs,
        searchField: ["ten", "ma"],
        // fetch remote data
        dropdownParent: '#dropdow-show',
        onDropdownOpen: function () {
            showDropdownMenu(cbMuiTienQuyet, selectMuiTienQuyet);
        },
        render: {
            no_results: function (data, escape) {
                return '<div class="no-results">Không tìm thấy dữ liệu </div>';
            },
            dropdown: function () {
                return '<div style="min-width: 50vw;"></div>';
            }
        },
        loadThrottle: 400,
    });
}