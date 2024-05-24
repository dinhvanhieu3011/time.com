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
function utility() { };
utility.prototype = {
    /*#############################################
     Common functions
    #############################################*/
    SubString: function (str, n) {
        if (n <= 0)
            return "";
        else if (n > String(str).length)
            return str;
        else {
            return String(str).substring(0, n) + "...";
        }
    },
    GoTo: function (i) {
        return goTo(i);
    },
    goTo: function (i) {
        location.href = i;
        return false;
    },
    ToggleCheckAll: function (o, Name) {
        $("input[name='" + Name + "']").each(function () {
            this.checked = o.checked;
        });
        var thisName = $(o).attr("name");
        $("input[name='" + thisName + "']").each(function () {
            this.checked = o.checked;
        });
    },
    CheckMe: function (o, Name, chkCheckAll) {
        if (o.checked) {
            $("input[name='" + Name + "']").each(function () {
                if (!this.checked) {
                    $(chkCheckAll).checked;
                }
            });
        } else {
            $(chkCheckAll).checked;
        }
    },
    IsNullOrEmpty: function (str) {
        if (typeof str == undefined) return true;
        else if (str == null) return true;
        else if (str.trim() == "") return true;
        else return false;
    },
    FormatDateTime: function (strDate) {
        // Vài giây trước
        // 1-59 phút trước
        // 1-23 tiếng trước
        // 9:58 15/08/2010

        // Input DateTime
        strDate = strDate.replace("/Date(", "").replace(")/", "");
        var dt = new Date(parseInt(strDate));
        var yy = dt.getFullYear();
        var mm = (dt.getMonth() + 1);
        var dd = dt.getDate();
        var hr = dt.getHours();
        var mi = dt.getMinutes();
        var ss = dt.getSeconds();
        // Curent DateTime
        var curentDate = new Date();
        var cYY = curentDate.getFullYear();
        var cMM = (curentDate.getMonth() + 1);
        var cDD = curentDate.getDate();
        var cHR = curentDate.getHours();
        var cMI = curentDate.getMinutes();
        var cSS = curentDate.getSeconds();

        var strTime = "";
        // If InputDate Is Today
        var dif = curentDate.getTime() - dt.getTime();
        var numberSecond = Math.abs(dif / 1000);
        var numberMinute = Math.floor(numberSecond / 60);
        var numberHour = Math.floor(numberSecond / 3600);
        if (numberSecond < 60)
            strTime = "Vài giây trước";
        else if (numberSecond < 3600)
            strTime = numberMinute.toString() + " phút trước";
        else if (numberSecond < 3600 * 24)
            strTime = numberHour.toString() + " giờ trước";

        else {
            strTime = "";
            strTime += parseInt(dd) < 10 ? ('0' + dd) : dd;
            strTime += "/" + (parseInt(mm) < 10 ? ('0' + mm) : mm);
            strTime += "/" + (parseInt(yy) < 10 ? ('0' + yy) : yy);
            strTime += " " + (parseInt(hr) < 10 ? ('0' + hr) : hr);
            strTime += ":" + (parseInt(mi) < 10 ? ('0' + mi) : mi);
        }
        return strTime;
    },
    GoTopPage: function (top) {
        if (typeof (top) == "undefined") top = 0;
        $('html, body').animate({ scrollTop: top }, 500);
    },
    GetExtension: function (fileName) {
        var re = /(?:\.([^.]+))?$/;
        var result = re.exec(fileName)[1];
        return result != undefined ? result : "";
    },
    IsUnicode: function (str) {
        var pattern = /[^\u0000-\u0080]+/;
        return pattern.test(str);
    },
    randomString: function (len, charSet) {
        charSet = charSet || 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        var randomString = '';
        for (var i = 0; i < len; i++) {
            var randomPoz = Math.floor(Math.random() * charSet.length);
            randomString += charSet.substring(randomPoz, randomPoz + 1);
        }
        return randomString;
    },
    getParameterByName: function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    },
    alert: function (title, content) {
        if (title == "undefined") {
            title = "<i class='fa fa-bullhorn' style='color:Gray'></i> Thông báo";
            content = "Bạn chưa chọn dữ liệu nào!";
        }
        $("#alert").html('');
        var render = "";
        render += '<div id="myModalAlert" class="modal fade" role="dialog" style="display: block;margin-top: 6%; height:auto; width:auto !important; background:none !important"><div class="modal-dialog">';
        render += '<div class="modal-content "><div class="modal-header alert-danger">';
        render += '<button type="button" class="close" data-dismiss="modal">&times;</button>';
        render += '<h4 class="modal-title">' + title + '</h4>';
        render += ' </div>';
        render += '<div class="modal-body">';
        render += ' <p>' + content + '</p>';
        render += '</div>';
        render += '<div class="modal-footer">';
        render += '<button type="button" class="btn btn-default" data-dismiss="modal"><i class="fa fa-window-close-o"></i> Đóng</button>';
        render += '</div>';
        render += '</div>';
        $("#alert").html(render);
        $('#myModalAlert').modal('show');
    },
    alert: function (title, content,state) {
        if (title == "undefined") {
            title = "<i class='fa fa-bullhorn' style='color:Gray'></i> Thông báo";
            content = "Bạn chưa chọn dữ liệu nào!";
        }
        $("#alert").html('');
        var render = "";
        render += '<div id="myModalAlert" class="modal fade" role="dialog" style="display: block;margin-top: 6%; height:auto; width:auto !important; background:none !important"><div class="modal-dialog">';
        render += '<div class="modal-content "><div class="modal-header '+state+'">';
        render += '<button type="button" class="close" data-dismiss="modal">&times;</button>';
        render += '<h4 class="modal-title">' + title + '</h4>';
        render += ' </div>';
        render += '<div class="modal-body">';
        render += ' <p>' + content + '</p>';
        render += '</div>';
        render += '<div class="modal-footer">';
        render += '<button type="button" class="btn btn-default" data-dismiss="modal"><i class="fa fa-window-close-o"></i> Đóng</button>';
        render += '</div>';
        render += '</div>';
        $("#alert").html(render);
        $('#myModalAlert').modal('show');
    },
    confirm: function (title, content) {
        if (title == "delete") {
            title = "<i class='fa fa-question-circle' style='color:Gray'></i> Thông báo";
            content = "Bạn có chắc chắn muốn xóa dữ liệu hệ thống?";
        }
        else if (title == "success_del") {

        }
        $("#alert").html('');
        var render = "";
        render += '<div id="myModalAlert" class="modal fade" role="dialog" style="display: block; margin-top: 6%; height:290px; width:auto !important; background:none !important"><div class="modal-dialog">';
        render += '<div class="modal-content"><div class="modal-header">';
        render += '<button type="button" class="close" data-dismiss="modal">&times;</button>';
        render += '<h4 class="modal-title">' + title + '</h4>';
        render += ' </div>';
        render += '<div class="modal-body">';
        render += '<p id=tbldataKemTheo>' + content + '</p>';
        render += '</div>';
        render += '<div class="modal-footer">';
        render += '<button type="button" class="btn btn-default" data-dismiss="modal"><i class="fa fa-window-close-o"></i> Đóng</button>';
        render += '<button type="button" class="btn btn-primary" id="btnYes">Đồng ý</button>';
        render += '</div>';
        render += '</div>';
        $("#alert").html(render);
        $('#myModalAlert').modal('show');
    },
    confirm: function (title, content,state) {
        if (title == "delete") {
            title = "<i class='fa fa-question-circle' style='color:Gray'></i> Thông báo";
            content = "Bạn có chắc chắn muốn xóa dữ liệu hệ thống?";
        }
        else if (title == "success_del") {

        }
        $("#alert").html('');
        var render = "";
        render += '<div id="myModalAlert" class="modal fade" role="dialog" style="display: block; margin-top: 6%; height:290px; width:auto !important; background:none !important"><div class="modal-dialog">';
        render += '<div class="modal-content"><div class="modal-header ' + state+'">';
        render += '<button type="button" class="close" data-dismiss="modal">&times;</button>';
        render += '<h4 class="modal-title">' + title + '</h4>';
        render += ' </div>';
        render += '<div class="modal-body">';
        render += '<p id=tbldataKemTheo>' + content + '</p>';
        render += '</div>';
        render += '<div class="modal-footer">';
        render += '<button type="button" class="btn btn-default" data-dismiss="modal"><i class="fa fa-window-close-o"></i> Đóng</button>';
        render += '<button type="button" class="btn btn-primary" id="btnYes">Đồng ý</button>';
        render += '</div>';
        render += '</div>';
        $("#alert").html(render);
        $('#myModalAlert').modal('show');
    },
    getFileNameUpload: function (strGiaTri, strKyTu) {
        console.log(strKyTu);

        var ketqua = "";
        if (strKyTu == ".") {
            ketqua = strGiaTri.substr(strGiaTri.lastIndexOf(strKyTu) - 36, strGiaTri.length - 1);
        }
        else {
            ketqua = strGiaTri.substr(strGiaTri.lastIndexOf(strKyTu) + 1, strGiaTri.length - 1);
        }

        return ketqua;
    },
    getImageUrl: function (ImageName, imageType) {
        var me = this;
        if (ImageName == "" || ImageName == null || ImageName == undefined) {
            return me.root + "/Upload/Avatar/no-avatar.png";
        }
        else {
            switch (imageType) {
                case me.EnumImageType.ACCOUNT:
                    return me.root + "/Upload/Avatar/" + ImageName;
                    break;
                case me.EnumImageType.DOCUMENT:
                    return me.root + "/Upload/Files/" + ImageName;
                    break;
                default:
                    return me.root + "/Upload/Avatar/no-avatar.png";
            }
        }
    },
    LoiHinhDaiDien: function (id) {
        var me = this;
        id = "#" + id;
        var url = me.getImageUrl("", me.EnumImageType.ACCOUNT);
        $(id).attr("src", url);
    },
    goBack: function () {
        window.history.back();
    },
    htmlEscape: function (strGiaTri) {
        return strGiaTri.replace(/[\-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "").replace(/</g, "").replace(/>/g, "").replace(/&/g, "");
    },
    fillDataToTable: function (config, table_Id, dataDanhMuc, dataTable) {
        var config_ob = config;
        /*A. temp local variables*/
        var table_id = "#" + table_Id;
        var table_id_head = table_id + " thead";
        var table_id_tbody = table_id + " tbody";
        var create_thead_Row = "";
        var create_tbody_Row = "";
        var table_head_name = "";
        $(table_id_head).html("");
        $(table_id_tbody).html("");
        $(".countItems").html("");
        var madanhmuc = "";
        var cell_values = "";
        var countItems = "<p class='countItems' style='float:left; color:#808080;'>Số lượng: " + dataTable.length + " dữ liệu</p>";
        /*B. Check validate*/
        for (var valLeft = 0; valLeft < config_ob.toLeft.length; valLeft++) {
            if (config_ob.toLeft[valLeft] > dataDanhMuc.length) {
                alert("Không tồn tại chỉ số trường dữ liệu: toLeft[?]");
                return false;
            }
        }
        for (var valRight = 0; valRight < config_ob.toRight.length; valRight++) {
            if (config_ob.toRight[valRight] > dataDanhMuc.length) {
                alert("Không tồn tại chỉ số trường dữ liệu: toRight[?]");
                return false;
            }
        }
        if (config_ob.width.length > dataDanhMuc.length) {
            alert("Không tồn tại chỉ số trường dữ liệu: width[?]");
            return false;
        }
        if (config_ob.cellColor.length > dataDanhMuc.length) {
            alert("Không tồn tại chỉ số trường dữ liệu: cellColor[?]");
            return false;
        }
        /*C. processing fill data to Table*/
        if (dataTable == "" || dataTable == null || dataTable == undefined) {
            $(table_id_tbody).append("<tr><td colspan = '" + dataDanhMuc.length + "'>Không có dữ liệu tìm kiếm</td></tr>");
        }
        else {
            /*1. title of table-heade*/
            var flag = false;
            create_thead_Row += "<tr style='background-color:#3c8dbc !important; color:white;'>";
            create_thead_Row += "<th style='width:40px'>#</td>";
            for (var i = 0; i < dataDanhMuc.length; i++) {
                table_head_name = dataDanhMuc[i].TEN;
                if (config_ob.upperCase == true) {
                    /*float text to LEFT*/
                    for (var l = 0; l < config_ob.toLeft.length; l++) {
                        if (config_ob.toLeft[l] == i) {
                            create_thead_Row += "<th style='text-align:left; width:" + config_ob.width[i] + "px'>" + table_head_name.toString().toUpperCase() + "</th>";
                            flag = true;
                        }
                    }
                    /*float text to RIGHT*/
                    for (var r = 0; r < config_ob.toRight.length; r++) {
                        if (config_ob.toRight[r] == i) {
                            create_thead_Row += "<th style='text-align:right; width:" + config_ob.width[i] + "px'>" + table_head_name.toString().toUpperCase() + "</th>";
                            flag = true;
                        }
                    }
                    /*float text to MIDDEL*/
                    if (flag == false) {
                        create_thead_Row += "<th style='width:" + config_ob.width[i] + "px'>" + table_head_name.toString().toUpperCase() + "</th>";
                    }
                }
                else {
                    /*float text to LEFT*/
                    for (var l = 0; l < config_ob.toLeft.length; l++) {
                        if (config_ob.toLeft[l] == i) {
                            create_thead_Row += "<th style='text-align:left; width:" + config_ob.width[i] + "px'>" + table_head_name + "</th>";
                            flag = true;
                        }
                    }
                    /*float text to RIGHT*/
                    for (var r = 0; r < config_ob.toRight.length; r++) {
                        if (config_ob.toRight[r] == i) {
                            create_thead_Row += "<th style='text-align:right; width:" + config_ob.width[i] + "px'>" + table_head_name + "</th>";
                            flag = true;
                        }
                    }
                    /*float text to MIDDEL*/
                    if (flag == false) {
                        create_thead_Row += "<th style='width:" + config_ob.width[i] + "px'>" + table_head_name + "</th>";
                    }
                }
            }
            if (config_ob.actionType == 0) {
                create_thead_Row += "<th style='width:40px'>Sửa</th>";
                create_thead_Row += "<th style='width:40px'><input type='checkbox' name='chkSelectAll_" + table_Id + "' id='chkSelectAll_" + table_Id + "' /><label for='chkSelectAll_" + table_Id + "' class='table-checkbox-title-name'></label></th>";
            }
            else if (config_ob.actionType == 1) {//textBox
                create_thead_Row += "<th style='width:40px'>#</th>";
            }
            else if (config_ob.actionType == 2) {//Combobox
                create_thead_Row += "<th style='width:40px'>#</th>";
            }
            create_thead_Row += "</tr>";
            $(table_id_head).append(create_thead_Row);
            create_thead_Row = "";
            /*2. content of each cell-tbody */
            for (var h = 0; h < dataTable.length; h++) {
                create_tbody_Row += "<tr class='" + table_Id + h + "'>";
                create_tbody_Row += "<td>" + (h + 1) + "</td>";
                for (var j = 0; j < dataDanhMuc.length; j++) {
                    madanhmuc = dataDanhMuc[j].MA;
                    cell_values = dataTable[h][madanhmuc];
                    if (cell_values == "" || cell_values == null || cell_values == undefined) {
                        cell_values = "";
                    }
                    /*float text to LEFT*/
                    var flagTbody = false;
                    for (var l = 0; l < config_ob.toLeft.length; l++) {
                        if (config_ob.toLeft[l] == j) {
                            create_tbody_Row += "<td style='text-align:left; color:" + config_ob.cellColor[j] + "'>" + cell_values + "</td>";
                            flagTbody = true;
                        }
                    }
                    /*float text to RIGHT*/
                    for (var r = 0; r < config_ob.toRight.length; r++) {
                        if (config_ob.toRight[r] == j) {
                            create_tbody_Row += "<td style='text-align:right; color:" + config_ob.cellColor[j] + "'>" + cell_values + "</td>";
                            flagTbody = true;
                        }
                    }
                    /*float text to MIDDEL*/
                    if (flagTbody == false) {
                        create_tbody_Row += "<td style='; color:" + config_ob.cellColor[j] + "'>" + cell_values + "</td>";
                    }

                }
                if (config_ob.actionType == 0) {
                    create_tbody_Row += "<td><a class='btnEditRole_" + table_Id + "' id='" + dataTable[h].ID + "' href='#'><i class='fa fa-edit fa-customer'></i></a></td>";
                    create_tbody_Row += "<td><input type='checkbox' id='chkSelect" + dataTable[h].ID + "' name='chkSelect" + dataTable[h].ID + "'><label for='chkSelect" + dataTable[h].ID + "' class='modal-checkbox-title-name'></td>";
                }
                else if (config_ob.actionType == 1) {
                    create_tbody_Row += "<td><input id='txt_" + dataTable[h].ID + "' /></td>";
                }
                else if (config_ob.actionType == 2) {
                    create_tbody_Row += "<td>" +
                        "<select id='Drop_" + table_Id + h + "'>" +
                            "<option value =''>-- Chọn dữ liệu --</option>" +
                        "</select>";
                }
                create_tbody_Row += "</tr>";
                $(table_id_tbody).append(create_tbody_Row);
                create_tbody_Row = "";
            }
            /*3. Notify amount of items*/
            $("#sl_banghi").prepend(countItems);
            /*4. freeze header for table*/
            $(table_id).tableHeadFixer();
        }
    },
    actionOnRowWhenHover: function (id) {
        var strId = "#" + id;
        var Id = id.substr(0, id.lastIndexOf("#"));
        console.log("Id: " + Id);

        var listAction = "<ul>" +
                            "<li><a href='#'>Sửa</a></li>" +
                            "<li><a href='#'>Xóa</a></li>" +
                        "</ul>";

        $(document).delegate(strId, 'mouseover mouseleave', function (e) {
            var id = this.id;
            var row_Id = "#" + id;
            if (e.type === 'mouseover') {//Nếu đưa chuột vào thì background
                $(row_Id).addClass("rowActive");
            }
            else if (e.type === 'mouseleave') {//Nếu đưa chuột ra thì xóa background
                $(row_Id).removeClass("rowActive");
            }
            if (id != "") {
                $(strId).css("cursor", "pointer");
                $(strId).popover({
                    title: "<p style=''>Title</p>",
                    content: listAction,
                    html: true,
                    trigger: "hover",
                    placement: "right"
                });
            }
        });
    },
    alertOnModal: function (title, content, colorText, prePos) {
        $("#btnNotifyModal").remove();
        var htm = "<div id='btnNotifyModal' class='alert " + colorText + " alert-dismissible' style='text-align:left;'>" +
                    "<a href='#' class='close' data-dismiss='alert' aria-label='close'>&times;</a>" +
                    "<strong><span style='font-size:15px;'>" + title + ": " + content + "</span></strong>" +
                "</div>";
        $(prePos).append(htm);
        $("#notify").slideDown("slow");
        /*colorCode: - 'alert-danger' - 'alert-dismissible' - 'alert-info' - 'alert-link' - 'alert-success'- 'alert-warning' */
    },
    //Ký tự hoa đầu tiên
    capitalize: function (string) {
        return string.charAt(0).toUpperCase() + string.slice(1).toLowerCase();
    },
    getYear: function (year, DropControlName, lblName) {//year: Năm bắt đầu cần hiển thị.vd 1930, 1990... đến năm hiện tại
        var me = this;
        var today = new Date(me.getServerTime);
        var nowYear = today.getFullYear();
        var dropControl = "#" + DropControlName;
        if (lblName == "" || lblName == null || lblName == undefined) {
            lblName = "-- Chọn dữ liệu --";
        }
        else {
            lblName = "-- " + lblName + " --";
        }
        $(dropControl).append($('<option value=""></option>').html(lblName));
        for (i = new Date().getFullYear() ; i > year; i--) {
            $(dropControl).append($('<option />').val(i).html(i));
        }
        $(".chosen-select").select();
        $(dropControl).trigger("change");
    },
    /*#############################################
     input functions
    #############################################*/
    checkEmpty: function (value) {
        if (value == "" || value == null || value == undefined)
            return ""
        else
            return value
    },
    convertStrToNum: function (value, sign) {
        var result = "";
        if (sign == ",") {
            result = value.replace(/,/g, "");
        }
        else if (sign == ".") {
            result = value.replace(/./g, "");
        }
        else {
            result = value;
        }
        return result
    },
    convertCommaToDot: function (value) {
        var result = "";
        if (value == "" || value == null || value == undefined) {
            return 0
        }
        else {
            result = value.replace(',', ".");
            return result;
        }

    },
    formatCurrency: function (value) {
        if (value == "" || value == null || value == 0 || value == undefined) {
            return "";
        }
        return value.toString().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1.");
    },
    convertStringToNumber: function (strGiaTri, strDau) {
        var ketqua = "";
        if (strGiaTri == "" || strGiaTri == null || strGiaTri == undefined) {
            ketqua = "";
        }
        else {
            if (strDau == "," || strDau == null || strDau == undefined) {
                ketqua = strGiaTri.replace(/,/g, "");
            }
            else {
                ketqua = strGiaTri.replace(/./g, "");
            }
        }
        
        return ketqua;
    },
    /*#############################################
    date functions
    #############################################*/
    getServerTime: function ()//get datetime on server
    {
        try {
            //FF, Opera, Safari, Chrome
            xmlHttp = new XMLHttpRequest();
        }
        catch (err1) {
            //IE
            try {
                xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
            }
            catch (err2) {
                try {
                    xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                }
                catch (eerr3) {
                    //AJAX not supported, use CPU time.
                    alert("AJAX not supported");
                }
            }
        }
        xmlHttp.open('HEAD', window.location.href.toString(), false);
        xmlHttp.setRequestHeader("Content-Type", "text/html");
        xmlHttp.send('');
        return xmlHttp.getResponseHeader("Date");
    },
    dateGetInFuture: function (date_dmy, numberofdays) {
        /*------------------------------
        --Input: format date: dd/mm/yyyy && a number of days that you want in the future to create a distance time
        --Output: mm/dd/yyyy
        -------------------------------*/
        var me = this;
        if (me.dateValid(date_dmy) && me.intValid(numberofdays)) {
            var newdate = "";
            var dd = "";
            var mm = "";
            var yyyy = "";
            var str_dd_mm_yyyy = "";
            var str_mm_dd_yyyy = "";

            str_mm_dd_yyyy = me.dateConvertDMYtoMDY(date_dmy);
            newdate = new Date(str_mm_dd_yyyy);//initialize format date: mm/dd/yyyy
            newdate.setDate(newdate.getDate() + numberofdays);
            dd = newdate.getDate();
            mm = newdate.getMonth() + 1;
            yyyy = newdate.getFullYear();
            str_dd_mm_yyyy = dd + "/" + mm + "/" + yyyy;
            return str_dd_mm_yyyy;
        }
        else {
            return "";
        }
    },
    dateConvertDMYtoMDY: function (date_dmy) {
        /*------------------------------
        --Input: format date: dd/mm/yyyy
        --Output: mm/dd/yyyy
        -------------------------------*/
        var str_mm_dd_yyyy = "";
        arr_date = date_dmy.split("/");
        str_mm_dd_yyyy = arr_date[1] + "/" + arr_date[0] + "/" + arr_date[2];
        return str_mm_dd_yyyy;

    },
    dateToday: function () {
        var dd = "";
        var mm = "";
        var yyyy = "";
        var str_dd_mm_yyyy = "";

        var date = new Date();
        dd = date.getDate();
        mm = date.getMonth() + 1;
        yyyy = date.getFullYear();
        str_dd_mm_yyyy = dd + "/" + mm + "/" + yyyy;
        return str_dd_mm_yyyy;
    },
    dateInRange: function (date_dmy, date_dmy_start, date_dmy_end) {
        var me = this;
        if (me.dateValid(date_dmy_start) && me.dateValid(date_dmy_end) && me.dateValid(date_dmy)) {
            var arrDate = date_dmy.split("/");
            var arrDate_start = date_dmy_start.split("/");
            var arrDate_end = date_dmy_end.split("/");

            var valDate = new Date(arrDate[2], arrDate[1] - 1, arrDate[0]);
            var valDate_start = new Date(arrDate_start[2], arrDate_start[1] - 1, arrDate_start[0]);
            var valDate_end = new Date(arrDate_end[2], arrDate_end[1] - 1, arrDate_end[0]);

            if (valDate >= valDate_start && valDate <= valDate_end) {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    },
    dateCompare: function (date_dmy_first, date_dmy_second) {
        var me = this;
        if (me.dateValid(date_dmy_first) && me.dateValid(date_dmy_second)) {
            var arrDate_first = date_dmy_first.split("/");
            var arrDate_second = date_dmy_second.split("/");
            var valDate_first = new Date(arrDate_first[2], arrDate_first[1] - 1, arrDate_first[0]);
            var valDate_second = new Date(arrDate_second[2], arrDate_second[1] - 1, arrDate_second[0]);
            if (valDate_first.getTime() == valDate_second.getTime()) {
                return 0;
            }
            else if (valDate_first.getTime() > valDate_second.getTime()) {
                return 1
            }
            else if (valDate_first.getTime() < valDate_second.getTime()) {
                return -1
            }
        }
        else {
            return false;
        }
    },
    parseDate: function (input) {
        var str = input.split('/');
        return {
            year: str[2],
            month: str[1],
            day: str[0]
        }
    },
    yearCompare: function (date_dmy_first, year_now) {
        var me = this;
        if (me.dateValid(date_dmy_first)) {
            var strfirst = date_dmy_first.split('/');
            return parseInt(year_now) - parseInt(strfirst[2]);
        }
        else {
            return -999;
        }
    },
    datepickerInit: function (strClassName) {
        var arrClass = document.getElementsByClassName(strClassName);//get all the same class and push into arrClass [HTMLCollection]
        var strId = "";
        var cleave = "";
        for (var i = 0; i < arrClass.length; i++) {
            strId = arrClass[i].id;
            if (strId == "" || strId == undefined || strId == null) {
                console.log("Thông báo id ở vị trí '" + i + "' không tồn tại");
            }
            else {
                cleave = new Cleave('#' + strId, {
                    date: true,
                    datePattern: ['d', 'm', 'Y']
                });
                $('#' + strId).datepicker({
                    autoclose: true,
                    dateFormat: "dd/mm/yy",
                    changeMonth: true,
                    changeYear: true
                });
            }
        }
    },
    /*#############################################
     validate functions
    #############################################*/
    /*start - valid*/
    dateValid: function (date_dmy) {
        if (date_dmy == "" || date_dmy == null) {
            return true;
        }
        /*------------------------------
        --Input: format date: dd/mm/yyyy 
        --Output: true/false
        -------------------------------*/
        var me = this;
        var pattern = /(\d+)(-|\/)(\d+)(?:-|\/)(?:(\d+)\s+(\d+):(\d+)(?::(\d+))?(?:\.(\d+))?)?/;//[dd/mm/yyyy || d/m/yyyy|| dd/m/yyyy||d/mm/yyyy] is ok
        if (pattern.test(date_dmy)) {//check format date
            var date = "";
            var str_mm_dd_yyyy = "";
            var arrdate = [];
            arrdate = date_dmy.split("/");
            str_mm_dd_yyyy = me.dateConvertDMYtoMDY(date_dmy);
            date = new Date(str_mm_dd_yyyy);
            if (date.getFullYear() == arrdate[2] && (date.getMonth() + 1) == arrdate[1] && date.getDate() == Number(arrdate[0])) {//check valid date
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    },
    intValid: function (value) {
        if (value == "" || value == null) {
            return true;
        }
        var pattern = /^-?[0-9]+$/;
        if (pattern.test(value)) {
            return true;
        }
        else {
            return false;
        }
    },
    floatValid: function (value) {
        if (value == "" || value == null) {
            return true;
        }
        var pattern = /^[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?$/;
        if (pattern.test(value)) {
            return true;
        }
        else {
            return false;
        }
    },
    strValid: function (name_id) {
        var value = $(name_id).val();
        if (value == "" || value == null || value == undefined) {
            return false;
        }
        else {
            value = value.trim();
            if (value) return true;
            else return false;
        }
    },
    //
    intValidForm_onblur: function (name_id) {
        var me = this;
        if (name_id != "" || name_id != null || name_id != undefined) {
            var id_check;
            var val_check;
            var flag = false;
            id_check = "#" + name_id;
            val_check = $(id_check).val();
            flag = me.intValid(val_check);
            if (!flag) {
                $(id_check).val("Orror!");
                return false;
            }
            else {
                return true;
            }
        }
        else {
            console.log("Please, input your Name_Id befor checking empty!");
        }
    },
    floatValidForm_onblur: function (name_id) {
        var me = this;
        if (name_id != "" || name_id != null || name_id != undefined) {
            var id_check;
            var val_check;
            var flag = false;
            id_check = "#" + name_id;
            val_check = $(id_check).val();
            flag = me.floatValid(val_check);
            if (!flag) {
                $(id_check).val("Orror!");
                return false;
            }
            else {
                return true;
            }
        }
        else {
            console.log("Please, input your Name_Id befor checking empty!");
        }
    },
    dateValidForm_onblur: function (name_id) {
        /*------------------------------
        --Input: format date: dd/mm/yyyy 
        --Output: true/false
        -------------------------------*/
        var me = this;
        var strId = "#" + name_id;
        var date_dmy = $(strId).val();
        var pattern = /(\d+)(-|\/)(\d+)(?:-|\/)(?:(\d+)\s+(\d+):(\d+)(?::(\d+))?(?:\.(\d+))?)?/;//[dd/mm/yyyy || d/m/yyyy|| dd/m/yyyy||d/mm/yyyy] is ok
        if (pattern.test(date_dmy)) {//check format date
            var date = "";
            var str_mm_dd_yyyy = "";
            var arrdate = [];
            arrdate = date_dmy.split("/");
            str_mm_dd_yyyy = me.dateConvertDMYtoMDY(date_dmy);
            date = new Date(str_mm_dd_yyyy);
            if (date.getFullYear() == arrdate[2] && (date.getMonth() + 1) == arrdate[1] && date.getDate() == Number(arrdate[0])) {//check valid date
                return true;
            }
            else {
                console.log("Sorry !! - wrong date");
                $(strId).val("Orror!");
            }
        }
        else {
            $(strId).val("Orror!");
            return false;
        }
    },
    //
    interValidForm: function (arr) {
        var me = this;
        if (arr.length > 0) {
            var valid_arr = [];
            var id_check;
            var val_check;
            var flag = false;
            for (var i = 0; i < arr.length; i++) {
                id_check = "#" + arr[i];
                val_check = $(id_check).val();
                flag = me.intValid(val_check);
                if (!flag) {
                    valid_arr.push(id_check);
                    $(id_check).val("Error!");
                }
                else {
                    $(id_check).removeClass("input-bg-change");
                }
            }
            if (valid_arr.length > 0) {
                for (var j = 0; j < valid_arr.length; j++) {
                    $(valid_arr[j]).val("Error!");
                    $(valid_arr[j]).addClass("input-bg-change");
                }
                return false;
            }
            else {
                return true;
            }
        }
        else {
            console.log("Please, input your array befor checking empty!");
        }
    },
    floatValidForm: function (arr) {
        var me = this;
        if (arr.length > 0) {
            var valid_arr = [];
            var id_check;
            var val_check;
            var flag = false;
            for (var i = 0; i < arr.length; i++) {
                id_check = "#" + arr[i];
                val_check = $(id_check).val();
                flag = me.floatValid(val_check);
                if (!flag) {
                    valid_arr.push(id_check);
                    $(id_check).val("Error!");
                }
                else {
                    $(id_check).removeClass("input-bg-change");
                }
            }
            if (valid_arr.length > 0) {
                for (var j = 0; j < valid_arr.length; j++) {
                    $(valid_arr[j]).val("Error!");
                    $(valid_arr[j]).addClass("input-bg-change");
                }
                return false;
            }
            else {
                return true;
            }
        }
        else {
            console.log("Please, input your array befor checking empty!");
        }
    },
    emptyValidForm: function (arr) {
        var me = this;
        if (arr.length > 0) {
            var arr_valid = [];
            var id_check;
            var value;
            for (var i = 0; i < arr.length; i++) {
                id_check = "#" + arr[i];
                value = me.strValid(id_check);
                if (!value) {
                    arr_valid.push(arr[i]);
                }
                else {
                    //for normal input
                    $(id_check).removeClass("input-bg-change");
                    //for select2
                    id_check = "select2-" + arr[i] + "-container";
                    $("span[aria-labelledby='" + id_check + "']").removeClass("input-bg-change");
                }
            }
            if (arr_valid.length > 0) {
                for (var j = 0; j < arr_valid.length; j++) {
                    //bg for input
                    id_check = "#" + arr_valid[j];
                    $(id_check).addClass("input-bg-change");
                    //bg for select2
                    id_check = "select2-" + arr_valid[j] + "-container";
                    $("span[aria-labelledby='" + id_check + "']").addClass("input-bg-change");
                }
                return false;
            }
            else {
                return true;
            }
        }
        else {
            console.log("Please, input your array befor checking empty!");
        }
    },
    dateValidForm: function (arr) {
        var me = this;
        if (arr.length > 0) {
            var valid_arr = [];
            var id_check;
            var val_check;
            var flag = false;
            for (var i = 0; i < arr.length; i++) {
                id_check = "#" + arr[i];
                val_check = $(id_check).val();
                flag = me.dateValid(val_check);
                if (!flag) {
                    valid_arr.push(id_check);
                    $(id_check).val("Error!");
                }
                else {
                    $(id_check).removeClass("input-bg-change");
                }
            }
            if (valid_arr.length > 0) {
                for (var j = 0; j < valid_arr.length; j++) {
                    $(valid_arr[j]).val("Error!");
                    $(valid_arr[j]).addClass("input-bg-change");
                }
                return false;
            }
            else {
                return true;
            }
        }
        else {
            console.log("Please, input your array befor checking empty!");
        }
    },
    //Valid throught DanhMuc data go into database and direct(jsonDataDir)
    getData_FromCacheTo_Valid: function (strMaBangDanhMuc) {
        var me = this;
        var valid = false;
        //xu ly truong hop mot page co nhieu form nhap va moi form nhap lai valid truong thong tin khac nhau
        if (me.strMaBangDanhMuc_Change != strMaBangDanhMuc) {//Thay doi MaDanhMuc
            coreRoot.systemextend.getDataTo_Valid(strMaBangDanhMuc);
            valid = me.checkInputForm_Valid("");
        }
        else {
            //kiem tra trinh duyet ho tro HTML5?
            if (typeof (Storage) !== "undefined") {
                if (localStorage.jsonValidClient.length < 0 || localStorage.jsonValidClient == "" || localStorage.jsonValidClient == undefined) {
                    coreRoot.systemextend.getDataTo_Valid(strMaBangDanhMuc);
                }
                valid = me.checkInputForm_Valid("");
            }
            else {
                if (me.jsonValid.length < 0 || me.jsonValid.length == "") {
                    coreRoot.systemextend.getDataTo_Valid(strMaBangDanhMuc);
                }
                valid = me.checkInputForm_Valid("");
            }
        }
        //set strMaDanhMuc_Change
        me.strMaBangDanhMuc_Change = strMaBangDanhMuc;
        //Tra ve ket qua
        if (valid == true) {
            return true;
        }
        else {
            return false;
        }
    },
    checkInputForm_Valid: function (jsonDataDir) {
        var me = this;
        var jsonData = {};
        if (jsonDataDir == "" || jsonDataDir == null || jsonDataDir == undefined) {
            if (localStorage.jsonValidClient.length > 0) {
                jsonData = JSON.parse(localStorage.jsonValidClient);
            }
            else {
                jsonData = me.jsonValid;
            }
        }
        else {
            jsonData = jsonDataDir;
        }
        if (jsonData.length > 0) {
            var strValid_Id;
            var strValid_Cat;
            var arr_Cater = [];
            var arr_Empty = [];
            var arr_Float = [];
            var arr_Inter = [];
            var arr_Date = [];
            for (var i = 0; i < jsonData.length; i++) {
                strValid_Id = jsonData[i].MA;
                strValid_Cat = jsonData[i].THONGTIN1;
                arr_Cater = strValid_Cat.split("#");
                for (var j = 0; j < arr_Cater.length; j++) {
                    if (arr_Cater[j] == "1") {
                        arr_Empty.push(strValid_Id);
                    }
                    else if (arr_Cater[j] == "2") {
                        arr_Float.push(strValid_Id);
                    }
                    else if (arr_Cater[j] == "3") {
                        arr_Inter.push(strValid_Id);
                    }
                    else if (arr_Cater[j] == "4") {
                        arr_Date.push(strValid_Id);
                    }
                }
            }
            var flag_Empty = me.emptyValidForm(arr_Empty);
            var flag_Float = me.floatValidForm(arr_Float);
            var flag_Inter = me.interValidForm(arr_Inter);
            var flag_Date = me.dateValidForm(arr_Date);
            if (flag_Empty == undefined) { flag_Empty = true; }
            if (flag_Float == undefined) { flag_Float = true; }
            if (flag_Inter == undefined) { flag_Inter = true; }
            if (flag_Date == undefined) { flag_Date = true; }

            if (flag_Empty == true && flag_Float == true && flag_Inter == true && flag_Date == true) {
                return true;
            }
            else {
                return false;
            }
        }
    },
    /*end - valid*/
    roundTo: function (n, digits) {
    var negative = false;
    if (digits === undefined) {
        digits = 0;
    }
    if (n < 0) {
        negative = true;
        n = n * -1;
    }
    var multiplicator = Math.pow(10, digits);
    n = parseFloat((n * multiplicator).toFixed(11));
    n = (Math.round(n) / multiplicator).toFixed(2);
    if (negative) {
        n = (n * -1).toFixed(2);
    }
    return n;
    },
    getNumber: function (val) {
        if (val == "" || val == null || val == undefined)
            return '0';
        else
            return val;
    },
    removeVietnameseTones: function(str) {
        str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
        str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
        str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
        str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
        str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
        str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
        str = str.replace(/đ/g, "d");
        str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
        str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
        str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
        str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
        str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
        str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
        str = str.replace(/Đ/g, "D");
        // Some system encode vietnamese combining accent as individual utf-8 characters
        // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
        str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
        str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
        // Remove extra spaces
        // Bỏ các khoảng trắng liền nhau
        str = str.replace(/ + /g, " ");
        str = str.trim();
        // Remove punctuations
        // Bỏ dấu câu, kí tự đặc biệt
        str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g, " ");
        str = str.toLowerCase();
        str = str.replace("   ", "");
        str = str.replace("  ", "");
        str = str.replace(" ", "");
        str = str.replace(" ", "");
        str = str.replace(" ", "");
        str = str.replace(" ", "");
        str = str.replace(" ", "");
        return str;
    },
    isEmptyOrSpaces: function(str){
        return str === null || str.match(/^ *$/) !== null;
    }
};