/*----------------------------------------------
--Author: nxduong
--Phone: 0983029603
--Description:
--Date of created: 13/09/2016
--Input:
--Output:
--Note: 
--Updated by:
--Date of updated: 
----------------------------------------------*/
function systemextend() { };
systemextend.prototype = {
    nodeMenuVertical: '',

    init_sys_extend: function () {
        var me = this;
        me.getlist_ChucNang();
    },
    /*
    Xử lý danh mục chức năng hệ thống
    */
    getlist_ChucNang: function () {
        var me = this;
        var strTuKhoa = coreRoot.systemroot.funcSearKey;
        var strUngDung_Id = coreRoot.systemroot.strApp_Id;
        var userId = coreRoot.systemroot.userId;
        coreRoot.systemroot.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var mystring = JSON.stringify(data.Data);
                    var jsonResult = $.parseJSON(mystring);
                    var node = '';
                    for (var j = 0; j < jsonResult.length; j++) {
                        if (jsonResult[j].ParentId == null || jsonResult[j].ParentId == "") {//get parents
                            node += '<li id=' + jsonResult[j].Id + ' class="treeview btnMenuVertical">';
                            node += '<a href="' + jsonResult[j].FunctionUrl + '">';
                            node += '<i class="' + jsonResult[j].Icon + '""></i> <span>' + jsonResult[j].FunctionsName + '</span>';
                            node += '<span class="pull-right-container"><i class="fa fa-angle-left pull-right"></i></span>';
                            node += ' </a>';
                            me.nodeMenuVertical = "";
                            node += me.menuVertical_DeQuy(jsonResult, jsonResult[j].Id, node);
                            node += '</li>';
                        }
                    }

                    $("#menu_vertical").append(node);//Append to left_content_tree
                    node = "";
                }
                else {
                    console.log(data.Message);
                }
            },
            error: function (er) { },
            type: 'GET',
            action: 'Account/LayDanhSachChucNang',
            versionAPI: 'v1.0',
            contentType: false,
            data: {
                'strUngDung_Id': strUngDung_Id,
                'strTuKhoa': strTuKhoa,
                'userId': userId
            },
            fakedb: [
            ]
        }, false, false, false, null, coreRoot.systemroot.apiUrlCommom);
    },
    menuVertical_DeQuy: function (data, parent_id, nodeMenuVertical) {
        var me = this;
        me.nodeMenuVertical += '<ul class="treeview-menu">';
        for (var i = 0; i < data.length; i++) {
            if (data[i].ParentId == parent_id) {
                me.nodeMenuVertical += '<li id=' + data[i].Id + ' class="treeview btnMenuVertical">';
                me.nodeMenuVertical += '<a href="' + data[i].FunctionUrl + '">';
                me.nodeMenuVertical += '<i class="' + data[i].Icon + '""></i> <span>' + data[i].FunctionsName + '</span>';
                me.nodeMenuVertical += '<span class="pull-right-container"><i class="fa fa-angle-left pull-right"></i></span>';
                me.nodeMenuVertical += ' </a>';
                me.menuVertical_DeQuy(data, data[i].Id, nodeMenuVertical);
                me.nodeMenuVertical += '</li>';
            }
        }
        me.nodeMenuVertical += '</ul>';
        return me.nodeMenuVertical;
    },
    /*
    Kết thúc Xử lý danh mục chức năng hệ thống
    */
    changePassword: function () {
        var me = this;
        var OldPass = $('#txt_oldpass').val();
        var NewPass1 = $('#txt_newpass1').val();
        var NewPass2 = $('#txt_newpass2').val();

        if (OldPass.length <= 0) {
            $("#txt_oldpass").attr("data-toggle", "tooltip");
            $("#notif-pass").show();
            $("#notif-pass").html("<div class='alert alert-info' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Bạn phải nhập vào mật khẩu cũ!</p></div>");
            document.getElementById('txt_oldpass').focus();
            return false;
        }
        else {
            $("#txt_oldpass").removeAttr("data-toggle");
            $("#txt_oldpass").removeAttr("data-original-title");
            $("#txt_oldpass").removeAttr("title");
        }

        if (NewPass1.length <= 0) {
            $("#txt_newpass1").attr("data-toggle", "tooltip");
            $("#notif-pass").show();
            $("#notif-pass").html("<div class='alert alert-warning' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Bạn phải nhập lại mật khẩu mới!</p></div>");
            document.getElementById('txt_newpass1').focus();
            return false;
        }
        else {
            $("#txt_newpass1").removeAttr("data-toggle");
            $("#txt_newpass1").removeAttr("data-original-title");
            $("#txt_newpass1").removeAttr("title");
        }

        if (NewPass2.length <= 0) {
            $("#txt_newpass2").attr("data-toggle", "tooltip");
            $("#notif-pass").show();
            $("#notif-pass").html("<div class='alert alert-warning' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Bạn phải nhập lại mật khẩu mới!</p></div>");
            document.getElementById('txt_newpass2').focus();
            return false;
        }
        else {
            $("#txt_newpass2").removeAttr("data-toggle");
            $("#txt_newpass2").removeAttr("data-original-title");
            $("#txt_newpass2").removeAttr("title");
        }
        if (NewPass1 != NewPass2) {
            $("#txt_newpass2").removeAttr("data-original-title");
            $("#txt_newpass2").attr("data-toggle", "tooltip");
            $("#txt_newpass2").attr("data-original-title", "");
            $("#notif-pass").show();
            $("#notif-pass").html("<div class='alert alert-info' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Mật khẩu mới không trùng nhau!</p></div>");
            document.getElementById('txt_newpass2').focus();
            return false;
        }
        else {
            $("#txt_newpass2").removeAttr("data-toggle");
            $("#txt_newpass2").removeAttr("data-original-title");
            $("#txt_newpass2").removeAttr("title");
        }
        coreRoot.systemroot.makeRequest({
            success: function (data) {
                if (data.Success) {
                    $("#notif-pass").show();
                    $("#notif-pass").html("<div class='alert alert-success' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Thay đổi mật khẩu thành công!</p></div>");
                    $("#txt_oldpass").attr("title", data.Message);
                    document.getElementById('txt_oldpass').focus();
                }
                else {
                    $("#notif-pass").show();
                    $("#notif-pass").html("<div class='alert alert-danger' style='height:10px !important;'><p style='margin-top:-10px'><strong>Thông báo: </strong>Mật khẩu cũ không đúng, vui lòng nhập lại!</p></div>");
                    $("#txt_oldpass").attr("title", data.Message);
                    document.getElementById('txt_oldpass').focus();
                }
            },
            error: function (er) { },
            type: 'GET',
            action: 'Account/DoiMatKhau',
            versionAPI: 'v1.0',
            contentType: false,
            data: {
                'old': OldPass,
                'pass1': NewPass1,
                'pass2': NewPass2,
                'userName': coreRoot.systemroot.userName
            },
            fakedb: [
            ]
        }, false, false, false, null, coreRoot.systemroot.apiUrlCommom);

    },
    loadToCombo_DanhMuc: function (strMaBangDanhMuc, zone_id, strTitle, defaultValue, parent) {
        var me = this;
        if (parent == null || parent == undefined) {
            parent = -1;
        }
        if (defaultValue == null || defaultValue == undefined) {
            defaultValue = '-1';
        }
        coreRoot.systemroot.beginLoading();
        coreRoot.systemroot.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var mystring = JSON.stringify(data.Data);
                    var json = $.parseJSON(mystring);
                    var mlen = json.length;
                    var tbCombo = $('[id$=' + zone_id + ']');
                    tbCombo.html('');
                    if (mlen > 0) {
                        var i;
                        var getList = "";
                        if (strTitle == "" || strTitle == null || strTitle == undefined) {
                            strTitle = "-- Chọn dữ liệu --";
                        }
                        else {
                            strTitle = "-- " + strTitle + " --";
                        }
                        getList += "<option value='-1'>" + strTitle + "</option>";
                        for (i = 0; i < mlen; i++) {
                            getList += "<option value='" + json[i].CateId + "'>" + json[i].CateDescription + "</option>";
                        }
                        tbCombo.html(getList);
                    }
                    tbCombo.val(defaultValue).trigger("change");
                }
                else {
                    console.log(data.Message);
                }
                coreRoot.systemroot.endLoading();
            },
            error: function (er) { coreRoot.systemroot.endLoading(); },
            type: 'GET',
            action: 'Account/LayDanhSachDanhMucChung',
            versionAPI: 'v1.0',
            contentType: false,
            data: {
                'strMaBangDanhMuc': strMaBangDanhMuc,
                'parent': parent
            },
            fakedb: [

            ]
        }, false, false, false, null, coreRoot.systemroot.apiUrlCommom);
    },
    functionSearch: function (value) {
        var me = this;
        var strTuKhoa = "";
        strTuKhoa = $("#navbar-search-input").val();
        /*Display a Div search*/
        if (strTuKhoa == "" || strTuKhoa == null || strTuKhoa == undefined) {
            $("#search-result").hide();
            return false;
        }
        else {
            $("#search-result").show();
        }
        /*Display result, response data to Div search from database*/
        coreRoot.systemroot.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var mystring = JSON.stringify(data.Data);
                    var json = $.parseJSON(mystring);
                    $('#tblSearch').dataTable({
                        "aaData": json,
                        "destroy": true,
                        "bPaginate": false,
                        "bLengthChange": false,
                        "bFilter": false,
                        "processing": false,
                        "bSort": false,
                        "bInfo": false,
                        "bAutoWidth": false,
                        "lengthMenu": [[25, 50, 100, -1], [25, 50, 100, "Tất cả"]],
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
                        ],
                        "order": [[1, "asc"]],
                        "aoColumns": [{
                            "mDataProp": "Path",
                            "bVisible": false
                        }
                            , {
                            "mDataProp": "FunctionsName"
                        }
                        ],
                        "fnRowCallback": function (nRow, aData) {
                            var $nRow = $(nRow); // cache the row wrapped up in jQuery
                            if (aData.Icon == "" || aData.Icon == null || aData.Icon == undefined) {
                                $('td:eq(0)', nRow).html('<a href="' + coreRoot.systemroot.rootPath + "/Index.aspx" + '">' + '<i class="fa fa-bullseye"></i> ' + aData.FunctionsName + '</a>');
                            }
                            else {
                                $('td:eq(0)', nRow).html('<a href="' + coreRoot.systemroot.rootPath + "/" + aData.FunctionUrl + '">' + '<i class="' + aData.Icon + '"></i> ' + aData.FunctionsName + '</a>');
                            }
                            return nRow
                        },
                    });
                }
                else {
                    console.log(data.Message);
                }
                coreRoot.systemroot.endLoading();
            },
            error: function (er) { coreRoot.systemroot.endLoading(); },
            type: 'GET',
            action: 'Account/LayDanhSachChucNang',
            versionAPI: 'v1.0',
            contentType: false,
            data: {
                'UngDungId': coreRoot.systemroot.strApp_Id,
                'strTuKhoa': strTuKhoa,
                'userId': coreRoot.systemroot.userId
            },
            fakedb: [

            ]
        }, false, false, false, null, coreRoot.systemroot.apiUrlCommom);
    },
    hideSeachBox: function () {
        var me = this;
        $("#navbar-search-input").val("");
        me.functionSearch("");
    },
    NgonNgu_LayGiaTri: function (strMaDinhDanh) {
        var me = this;
        var strNgonNgu_Id = coreRoot.systemroot.langId;
        var jsonData =
        {
            'strNgonNgu_Id': strNgonNgu_Id,
            'strMaDinhDanh': strMaDinhDanh
        };
        coreRoot.systemroot.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var arr_tieude = data.Message.split(",");
                    var arr_lb_class = strMaDinhDanh.split(",");
                    var lb_class = "";
                    for (var i = 0; i < arr_tieude.length; i++) {
                        lb_class = "." + arr_lb_class[i];
                        $(lb_class).html(arr_tieude[i]);
                    }
                }
                else {
                    console.log(data.Message);
                }
            },
            error: function (er) { },
            type: 'POST',
            async: false,
            action: 'NgonNgu/LayGiaTriNgonNgu',
            data: JSON.stringify(jsonData),
            fakedb: [

            ]
        }, false, false, false, null, coreRoot.systemroot.apiUrlCommom);
    },
    LamTronSo: function (value, id) {
        var me = this;
        if (value == "" || value == null || value == undefined) {
            $(id).val("0");
            return;
        }

        coreRoot.systemroot.makeRequest({
            success: function (data) {
                if (data.Success) {
                    $(id).val(coreRoot.utility.formatCurrency(data.Message));
                }
                else {
                    $(id).val("0");
                }
            },
            error: function (er) { return "0"; },
            type: 'GET',
            action: 'Account/LamTronSo',
            versionAPI: 'v1.0',
            contentType: false,
            data: {
                'value': value
            },
            fakedb: [

            ]
        }, false, false, false, null, coreRoot.systemroot.apiUrlCommom);
    },
    /*
    Khác
    */
    /*----------------------------------------------
    --Discription: Load danh sách báo cáo vào treejs
    --API:  Common/Controller/Core/Reports
    ----------------------------------------------*/
    getList_BaoCao_Tree: function (functions_id) {
        var me = this;
        var strTuKhoa = "";
        var parent = -1;
        var iTrangThai = 1;
        var pageIndex = 1;
        var pageSize = 1000000;

        coreRoot.systemroot.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var myResult = JSON.stringify(data.Data);
                    var dtResult = $.parseJSON(myResult);
                    console.log("dtResult: " + dtResult);
                    $("#left_content_tree").html('');
                    var node = "";
                    node += '<ul>';
                    for (var i = 0; i < dtResult.length; i++) {
                        if (dtResult[i].ParentId == null || dtResult[i].ParentId == "" || dtResult[i].ParentId == "-1") {
                            var id = dtResult[i].ReportUrl;
                            id = id.replace("Modules/BaoCao/Templates/", "");
                            node += '<li class="btnEvent" id="' + id + '">' + dtResult[i].ReportsName;
                            node += '<ul>';
                            me.treenode = "";
                            node += me.BaoCao_dequy_treejs(dtResult, dtResult[i].ReportsId);
                            node += '</ul>';
                            node += '</li>';
                        }
                    }
                    node += '</ul>';
                    $("#left_content_tree").append(node);//Append to left_content_tree
                    $("#left_content_tree").jstree({});// Config jstree
                }
                else {
                    console.log(data.Message);
                }
            },
            error: function (er) { },
            type: 'GET',
            action: 'Account/LayDanhSachBaoCao',
            versionAPI: 'v1.0',
            contentType: false,
            data: {
                'strTuKhoa': strTuKhoa,
                'Functions_Id': functions_id,
                'parent': parent,
                'pageIndex': pageIndex,
                'pageSize': pageSize,
                'iTrangThai': iTrangThai
            },
            fakedb: [

            ]
        }, false, false, false, null, coreRoot.systemroot.apiUrlCommom);
    },
    BaoCao_dequy_treejs: function (data, parent_id) {
        var me = this;
        for (var i = 0; i < data.length; i++) {
            if (data[i].ParentId == parent_id) {
                var id = data[i].ReportUrl;
                id = id.replace("Modules/BaoCao/Templates/", "");
                me.treenode += '<li id=' + id + ' class="btnEvent">' + data[i].ReportsName;
                me.treenode += '<ul>';
                me.BaoCao_dequy_treejs(data, data[i].ReportsId);
                me.treenode += '</ul>';
                me.treenode += '</li>';
            }
        }
        return me.treenode;
    },
    loadToCombo_DonVi: function (zone_id, strTitle, defaultValue, parent, TinhThanh_Id) {
        var me = this;
        if (parent == null || parent == undefined) {
            parent = -1;
        }
        if (defaultValue == null || defaultValue == undefined) {
            defaultValue = '';
        }
        if (TinhThanh_Id == null || TinhThanh_Id == undefined) {
            TinhThanh_Id = '';
        }
        coreRoot.systemroot.beginLoading();
        coreRoot.systemroot.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var mystring = JSON.stringify(data.Data);
                    var json = $.parseJSON(mystring);
                    var mlen = json.length;
                    var tbCombo = $('[id$=' + zone_id + ']');
                    tbCombo.html('');
                    if (mlen > 0) {
                        var i;
                        var getList = "";
                        if (strTitle == "" || strTitle == null || strTitle == undefined) {
                            strTitle = "-- Chọn dữ liệu --";
                        }
                        else {
                            strTitle = "-- " + strTitle + " --";
                        }
                        getList += "<option value=''>" + strTitle + "</option>";
                        for (i = 0; i < mlen; i++) {
                            getList += "<option value='" + json[i].Id + "'>" + json[i].Name + "</option>";
                        }
                        tbCombo.html(getList);
                    }
                    tbCombo.val(defaultValue).trigger("change");
                }
                else {
                    console.log(data.Message);
                }
                coreRoot.systemroot.endLoading();
            },
            error: function (er) { coreRoot.systemroot.endLoading(); },
            type: 'GET',
            action: 'Account/LayDanhSachDonVi',
            versionAPI: 'v1.0',
            contentType: false,
            data: {
                'TinhThanh_Id': TinhThanh_Id,
                'parent': parent
            },
            fakedb: [

            ]
        }, false, false, false, null, coreRoot.systemroot.apiUrlCommom);
    },
    loadToCombo_DanhMucBaoCao: function (zone_id, strTitle, defaultValue, ApplicationId, ThuocThongTuId) {
        var me = this;
        if (ApplicationId == null || ApplicationId == undefined) {
            ApplicationId = -1;
        }
        if (defaultValue == null || defaultValue == undefined) {
            defaultValue = '';
        }
        if (ThuocThongTuId == null || ThuocThongTuId == undefined) {
            ThuocThongTuId = -1;
        }
        coreRoot.systemroot.makeRequest({
            success: function (data) {
                if (data.Success) {
                    var mystring = JSON.stringify(data.Data);
                    var json = $.parseJSON(mystring);
                    var mlen = json.length;
                    var tbCombo = $('[id$=' + zone_id + ']');
                    tbCombo.html('');
                    if (mlen > 0) {
                        var i;
                        var getList = "";
                        if (strTitle == "" || strTitle == null || strTitle == undefined) {
                            strTitle = "-- Chọn dữ liệu --";
                        }
                        else {
                            strTitle = "-- " + strTitle + " --";
                        }
                        getList += "<option value=''>" + strTitle + "</option>";
                        for (i = 0; i < mlen; i++) {
                            getList += "<option value='" + json[i].Id + "'>" + json[i].TenBaocao + "</option>";
                        }
                        tbCombo.html(getList);
                    }
                    tbCombo.val(defaultValue).trigger("change");
                }
                else {
                    console.log(data.Message);
                }
            },
            error: function (er) {
                console.log(er);
                console.log("Err: " + er);
            },
            type: 'GET',
            action: 'Account/LayDanhSachDanhMucBaoCao',
            versionAPI: 'v1.0',
            contentType: false,
            data: {
                'ThuocThongTuId': ThuocThongTuId,
                'ApplicationId': ApplicationId
            },
            fakedb: [

            ]
        }, false, false, false, null, coreRoot.systemroot.apiUrlCommom);
    }
};