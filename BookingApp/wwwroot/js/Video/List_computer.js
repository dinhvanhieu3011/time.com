function List_computer() { };
List_computer.prototype = {
    init: function () {
        var me = this;
        me.list_DanhSach();

        setInterval(() => {
            me.list_DanhSach();
        }, 60000);

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
        $(document).delegate('.btnEdit', 'click', function () {
            var id = this.id;
            if (id != "") {
                window.open(window.location.origin + "/Computer/Update/" + id, "_blank");
            }
        });
        $(document).delegate('.btnDelete', 'click', function () {
            var id = this.id;
            if (id != "") {
                window.open(window.location.origin + "/Computer/Delete/" + id, "_blank");
            }
        });
        $(document).delegate('.btnHistory', 'click', function () {
            var id = this.id;
            if (id != "") {
                window.open(window.location.origin + "/Video/List/" + id, "_blank");
            }
        });
        $(document).delegate('.btnLive', 'click', function () {
            var id = this.id;
            if (id != "") {
                window.open(window.location.origin + "/Computer/Live/" + id, "_blank");
            }
        });
        $(document).delegate('.btnLive2', 'click', function () {
            var id = this.id;
            if (id != "") {
                window.open(window.location.origin + "/Computer/Live_v2/" + id, "_blank");
            }
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
        var pageIndex = 1;
        var pageSize = 1000;
        var strNgayChungTu = $('#ngay_nhapxuat').val();
        coreRoot.systemroot.beginLoading();
        $.ajax({
            type: 'GET',
            crossDomain: true,
            url: '/api/Upload/getListComputer',
            //headers: { 'token1': 'b1e813ee2638dcb3b1abd21b4085b0f4d76438ae37bc036225cf26340e945044' },
            data: {
                'Ngay': strNgayChungTu,
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
                    console.log('data', json)
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

                            "order": [[1, "desc"]],
                            "aoColumns": [{
                                "mDataProp": "id",
                                "bVisible": false
                            }
                                ,
                            {
                                "mDataProp": "name"
                            }
                                ,
                            {
                                "mDataProp": "token"
                            }
                                ,
                            {
                                "mDataProp": "employeeName"
                            }
                                ,
                            {
                                "mDataProp": "version"
                            }
                                , {
                                "mDataProp": "status"
                            }
                                , {
                                "mData": "token",
                                "mRender": function (data, type, full) {
                                    return '<img id="' + full.id + '" src="/live/' + full.token + '.png?random=' + new Date().getTime() + '" class="img-responsive btnLive2" style="max-width: 100%;"/>';
                                }
                            }
                                ,

                            {
                                "mData": "id",
                                "mRender": function (data) {
                                    return '<a title="Chỉnh sửa" id="' + data + '" class="btnEdit" href="javascript:void(0);"><i class="fa fa-edit fa-customer"></i></a>';
                                }
                            },

                            {
                                "mData": "id",
                                "mRender": function (data) {
                                    return '<a title="Xoá" id="' + data + '" class="btnDelete" href="javascript:void(0);"><i class="fa fa-trash fa-customer"></i></a>';
                                }
                            },

                            {
                                "mData": "id",
                                "mRender": function (data) {
                                    return '<a title="Lịch sử" id="' + data + '" class="btnHistory" href="javascript:void(0);"><i class="fa fa-history fa-customer"></i></a>';
                                }
                            },

                            {
                                "mData": "id",
                                "mRender": function (data) {
                                    return '<a title="Live" id="' + data + '" class="btnLive" href="javascript:void(0);"><i class="fa fa-eye fa-customer"></i></a>';
                                },
                                "bVisible": false
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
            timeout: 3000000
        });
    },
    common_huytimkiem: function () {
        $("#ngay_nhapxuat").val('');
    },
}