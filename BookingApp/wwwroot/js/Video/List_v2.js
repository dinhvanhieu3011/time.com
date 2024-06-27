function List() { };
List.prototype = {
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

        $('#ngay_giaonhan').datepicker({
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
        $(document).delegate('.btnEdit', 'click', function () {
            var id = this.id;
            if (id != "") {
                window.open(window.location.origin +"/Video/Detail/" + id, "_blank");
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
        var pageIndex = coreRoot.systemroot.pageIndex_default;
        var pageSize = coreRoot.systemroot.pageSize_default;
        var strNgayChungTu = $('#ngay_nhapxuat').val();
        var id = $('#channel_id').val();
        coreRoot.systemroot.beginLoading();
        $.ajax({
            type: 'GET',
            crossDomain: true,
            url: '/api/VideoManager/getList',
            //headers: { 'token1': 'b1e813ee2638dcb3b1abd21b4085b0f4d76438ae37bc036225cf26340e945044' },
            data: {
                'Ngay': strNgayChungTu,
                'id' : id
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