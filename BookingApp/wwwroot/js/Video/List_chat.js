function List_chat() { };
List_chat.prototype = {
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
            url: '/api/Chat/getAll',
            data: {
                'Keyword': strNgayChungTu,
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
                    coreRoot.systemroot.pagButtonRender("main_doc.List_chat.list_DanhSach()", "tbldata", data.pager);
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
                                "mDataProp": "fromPhoneNumber",
                                "mRender": function (data) {
                                    return '<a  href="/Whatsapp/index/' + data +'">' + data + '</a>';
                                }
                            },
                                       {
                                           "mData": "toPhoneNumber",
                                    "mRender": function (data) {
                                        return '<a  href="/Whatsapp/index/' + data +'">' + data +'</a>';
                                    }
                                },
                                {
                                    "mDataProp": "time"
                                    ,
                                "mRender": function (data) {
                                    return moment(data + '').format("DD/MM/YYYY hh:mm:ss");
                                }
                                }
                                    ,
                            {
                                    "mDataProp": "message"
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