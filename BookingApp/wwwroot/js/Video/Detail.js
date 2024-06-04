function Detail() { };
Detail.prototype = {
    DonViId: -1,
    DonViTongHopId: -1,
    DonViCapHuyenId: -1,
    isCopy: true,
    idCopy: 0,
    role: '',
    flag: '',
    CreatedBy: '',
    VideoId:0,
    init: function () {
        var me = this;
        var id = $('#video-id').val();
        this.VideoId = id;
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
    },
    list_DanhSach: function () {
        var me = this;

        var pageIndex = coreRoot.systemroot.pageIndex_default;
        var pageSize = coreRoot.systemroot.pageSize_default;
        var keyword = $('#keyword').val();
        var videoid = $('#video-id').val();
        coreRoot.systemroot.beginLoading();
        $.ajax({
            type: 'GET',
            crossDomain: true,
            url: '/api/Upload/getListDataVideo',
            //headers: { 'token1': 'b1e813ee2638dcb3b1abd21b4085b0f4d76438ae37bc036225cf26340e945044' },
            data: {
                'KeyWord': keyword,
                'id': videoid,
                'pageIndex': pageIndex,
                'pageSize': pageSize
            }, 
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                coreRoot.systemroot.endLoading();
                if (data.success) {
                    var mystring = JSON.stringify(data.data);
                    var json = $.parseJSON(mystring);
                    coreRoot.systemroot.pagButtonRender("main_doc.Detail.list_DanhSach()", "tbldata", data.pager);
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
                            "bAutoWidth": true,
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
                                {
                                    className: "dt-body-left", "targets": [4]
                                }
                            ],
                            "order": [[1, "desc"]],
                            "aoColumns": [{
                                "mDataProp": "id",
                                "bVisible": false
                            }
                                ,
                            {
                                "mDataProp": "time",
                                "mRender": function (data) {
                                    return moment(data+'').format("DD/MM/YYYY h:mm:ss"); 
                                }
                            }
                                ,
                            {
                                "mDataProp": "userName"
                            }
                                ,
                            {
                                "mDataProp": "windows"
                            }
                                , {

                                    "mDataProp": "keys",
                                    "mRender": function (data) {
                                        return data.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
                                    }
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