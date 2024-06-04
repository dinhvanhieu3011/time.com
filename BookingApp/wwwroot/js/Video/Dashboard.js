﻿function Dashboard() { };
Dashboard.prototype = {
    DonViId: -1,
    DonViTongHopId: -1,
    DonViCapHuyenId: -1,
    isCopy: true,
    idCopy: 0,
    role: '',
    flag: '',
    CreatedBy: '',
    init: function () {
        var me = this;
        $('#ngay_nhapxuat').datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            autoclose: true
        });


        /*
        Datetimepicker
        */
        me.list_DanhSach();
        $("#btnTimKiem").click(function (e) {
            $('#tbldata').dataTable().fnClearTable();
            me.list_DanhSach();
            e.preventDefault();
        });
        $("#btnHuyTimKiem").click(function (e) {
            me.common_huytimkiem();
            e.preventDefault();
            me.list_DanhSach();
        });

        $("#btnrefresh").click(function (e) {
            $('#tbldata').dataTable().fnClearTable();
            me.list_DanhSach();
            e.preventDefault();
        });
        $(document).on("keypress", "input", function (event) {
            if (event.which === 13) {
                $('#tbldata').dataTable().fnClearTable();
                me.list_DanhSach();
            }
        });
    },
    list_DanhSach: function () {
        var me = this;
        var pageIndex = coreRoot.systemroot.pageIndex_default;
        var pageSize = coreRoot.systemroot.pageSize_default;
        var strNgayChungTu = $('#ngay_nhapxuat').val();
        var user = $('#user').val() ? $('#user').val().toString() : "";
        coreRoot.systemroot.beginLoading();
        $.ajax({
            type: 'GET',
            crossDomain: true,
            url: '/api/Upload/getDashboard',
            //headers: { 'token1': 'b1e813ee2638dcb3b1abd21b4085b0f4d76438ae37bc036225cf26340e945044' },
            data: {
                'Ngay': strNgayChungTu,
                'channelId': user,
                'pageIndex': pageIndex,
                'pageSize': pageSize
            }, 
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                coreRoot.systemroot.endLoading();
                console.log(data.success)
                if (data.success) {
                    var mystring = JSON.stringify(data.data);
                    var json = $.parseJSON(mystring);
                    coreRoot.systemroot.pagButtonRender("main_doc.Dashboard.list_DanhSach()", "tbldata", data.pager);
                    if (json != null) {
                        var mlen = json.length;
                        $("#zone_soluong").html(data.pager);
                        $('#tbldata').dataTable({
                            "aaData": json,
                            "destroy": true,
                            "bPaginate": false,
                            "bLengthChange": false,
                            "bFilter": false,
                            "processing": true,
                            "bSort": true,
                            "bInfo": false,
                            "bAutoWidth": false,
                            "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "Tất cả"]],
                            "language": {
                                "search": "Tìm kiếm theo từ khóa:",
                                "lengthMenu": "Hiển thị _MENU_ dữ liệu",
                                "zeroRecords": "Không có dữ liệu nào được tìm thấy!",
                                "info": "Hiển thị _START_ đến _END_ trong _TOTAL_ dữ liệu",
                                "infoEmpty": "Hiển thị 0 đến 0 của 0 dữ liệu ",
                                "infoFiltered": "(Dữ liệu tìm kiếm trong _MAX_ dữ liệu)",
                                "processing": "Đang tải dữ liệu...",
                                "emptyTable": "Không có dữ liệu nào được tìm thấy!",
                                "paginate": {
                                    "first": "Đầu",
                                    "last": "Cuối",
                                    "next": "Tiếp theo",
                                    "previous": "Quay lại"
                                }
                            },
                            "aoColumnDefs": [
                                { orderable: true, "targets": [0,1,2,3]  },
                            ],
                            "order": [[0, "desc"]],
                            "aoColumns": [
                                {
                                    "mDataProp": "computerName"
                                },
                                {
                                    "mDataProp": "windows"
                                },

                            {
                                "mDataProp": "start",
                                "mRender": function (data) {
                                    return moment(data + '').format("DD/MM/YYYY h:mm:ss");
                                }
                            }
                                ,
                            {
                                "mDataProp": "end",
                                "mRender": function (data) {
                                    return moment(data + '').format("DD/MM/YYYY h:mm:ss");
                                }
                            }
                                ,
                            {
                                "mDataProp": "useTime"
                            }
                        
                            ],
                        });
                    }
                }
                else {
                    coreRoot.utility.alert("Thông báo", data.Message);
                    console.log(data.Message);
                }
            },
            error: function (er) { coreRoot.systemroot.endLoading(); },
            timeout:  3000000
        });
    },
    common_huytimkiem: function () {
        $("#ngay_nhapxuat").val('');
    },
}