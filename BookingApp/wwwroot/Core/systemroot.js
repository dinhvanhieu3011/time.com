/*----------------------------------------------
--Author: nxduong
--Phone: 0983029603
--Description:
--Date of created: 13/09/2016
--Input:
--Output:
--Note: File hệ thống, lập trình viên font-end không chỉnh sửa
--Updated by:
--Date of updated: 
----------------------------------------------*/
function systemroot() { };
systemroot.prototype = {
    userId: null,
    userName: null,
    fullName: null,
    langId: null,
    rootPath: '',
    storageFolder: '',
    strApp_Id: '',
    isDebug: false,
    apiUrlCommom: null,
    apiUrl: null,
    apiUrlTemp: null,
    urlService: '',
    funcSearKey: '',
    contentPlacehoder: '',
    pageIndex_default: 1,
    pageSize_default: 20,
    ThangBatDauTinhKhauHao: 1,
    NhomTaiSan_Extend: 0,
    roleId: 0,
    unitId: -1,
    unitCode: '',
    unitName: '',
    unitParentId: -1,
    unitParentCode: '',
    unitParentName: '',
    startApp: function () {
        var me = this;
        var arr_thamsohethong = Init_Prammater();
        me.rootPath = arr_thamsohethong[0];
        me.apiUrl = arr_thamsohethong[1];
        me.apiUrlCommom = arr_thamsohethong[2];
        me.storageFolder = arr_thamsohethong[3];
        me.userId = arr_thamsohethong[4];
        me.userName = arr_thamsohethong[5];
        me.fullName = arr_thamsohethong[6];
        me.apiUrlTemp = arr_thamsohethong[1];
        me.strApp_Id = arr_thamsohethong[7];
        me.langId = arr_thamsohethong[8];
        me.roleId = arr_thamsohethong[9];
        me.unitId = arr_thamsohethong[10];
        me.unitCode = arr_thamsohethong[11];
        me.unitName = arr_thamsohethong[12];
        me.unitParentId = arr_thamsohethong[13];
        me.unitParentCode = arr_thamsohethong[14];
        me.unitParentName = arr_thamsohethong[15];

        me.funcSearKey = $(constant.ModuleSetting.initsystem.func_sear_key).val();
        me.contentPlacehoder = constant.ModuleSetting.initsystem.content_placehoder;
        me.pageIndex = constant.ModuleSetting.initsystem.page_index;
        me.pageSize = constant.ModuleSetting.initsystem.page_size;
        me.NhomTaiSan_Extend = constant.ModuleSetting.initsystem.nhomtaisan_k01;
        me.common_setup_page();
        me.pagInfoRender();
        $("#btnChageMyPass").click(function (e) {
            $("#notif-pass").hide();
            $("#txt_oldpass").val('');
            $("#txt_newpass1").val('');
            $("#txt_newpass2").val('');
            $('#myModal_change_pass').modal('show');
            document.getElementById('txt_oldpass').focus();
        });
        $("#btnProfile").click(function (e) {
            $("#notif-profile").hide();
            $('#myModal_change_profile').modal('show');
        });
        $("#btnSave_Change_Pass").click(function (e) {
            coreRoot.systemextend.changePassword();
        });
        window.addEventListener('offline', function (e) {
            coreRoot.utility.alert('Thông báo', "Không có tín hiệu kết nối, vui lòng kiểm tra kết nối mạng!");
            $('.modal-backdrop').remove();
        });
        window.addEventListener('online', function (e) {
            coreRoot.utility.alert('Thông báo', "Tín hiệu đường truyền tốt!");
            $('.modal-backdrop').remove();
        });
        $("#btnBack,#btnBack_T").click(function (e) {
            coreRoot.utility.goBack();
        });
        $("#btnQuickstart").click(function (e) {
            $('#myModal_Quickstart').modal('show');
        });
    },
    makeRequest: function (params, inc_user, inc_code, inc_lang, container, urlService) {
        var op = params;
        var me = this;
        var onSuccess = function () { };

        if (op.hasOwnProperty('success')) {
            if (typeof (op.success) == 'function') {
                onSuccess = op.success;
            }
        }

        var onError = function () { };
        if (op.hasOwnProperty('error')) {
            if (typeof (op.error) == 'function') {
                onError = op.error;
            }
        }

        var is_inc_code = false;
        var is_inc_lang = false;
        var is_inc_user = false;

        if (inc_code != null) is_inc_code = inc_code;
        if (is_inc_lang != null) is_inc_lang = inc_lang;
        if (is_inc_user != null) is_inc_user = inc_user;
        if (container != null) {
            $('[id$=' + container + ']').html('<div class="IMSLoadingWrapper"><img src="' + me.rootPath + constant.ModuleSetting.imgLoading + '" alt="loading..." /></div>');
        }
        if (urlService != null) {
            me.apiUrl = urlService;
        }
        else {
            me.apiUrl = me.apiUrlTemp;
        }
        if (op.versionAPI != undefined && op.versionAPI != "" && op.versionAPI != null) {
            me.apiUrl = me.apiUrl + op.versionAPI + "/" + op.action;
        }
        else {
            me.apiUrl = me.apiUrl + op.action;
        }
        var dataPost = op.data;
        if (me.isDebug) {
            console.log(dataPost);
        }
        if (me.isDebug) {
            if (typeof (op.fakedb) != 'undefined')
                onSuccess(op.fakedb);
        } else {
            if (op.contentType == false || op.contentType == 'false') {
                var request = $.ajax({
                    type: op.type,
                    crossDomain: true,
                    url: me.apiUrl,
                    data: dataPost,
                    cache: false,
                    dataType: constant.ModuleSetting.method.DATA_TYPE,
                    success: function (d, s, x) {
                        var result = d;
                        if (result != null) {
                            onSuccess(result);
                        }
                    },
                    error: function (x, t, m) {
                        onError(x);
                    },
                    async: op.async,
                    timeout: op.timeout != undefined ? op.timeout : 3000000
                });
            }
            else {
                var request = $.ajax({
                    type: op.type,
                    crossDomain: true,
                    url: me.apiUrl,
                    headers: { 'token1': 'b1e813ee2638dcb3b1abd21b4085b0f4d76438ae37bc036225cf26340e945044' },
                    data: dataPost,
                    cache: false,
                    contentType: constant.ModuleSetting.method.CONTENT_TYPE,
                    dataType: constant.ModuleSetting.method.DATA_TYPE,
                    success: function (d, s, x) {
                        var result = d;
                        if (result != null) {
                            onSuccess(result);
                        }
                    },
                    error: function (x, t, m) {
                        onError(x);
                    },
                    async: op.async,
                    timeout: op.timeout != undefined ? op.timeout : 3000000
                });
            }
        }
    },
    /*
    -- author:
    -- discription:  Hiển thị loadding khi click button
    -- date: 
    */
    buttonLoading: function () {
        var me = this;
        $('.btnwaitsucces').on('click', function () {
            var x = $(this);
            if (me.button_adress) {
                me.button_adress.button('reset');
            }
            me.button_adress = x;
            document.getElementById(this.id).className += "btn-lg";
            x.attr("data-loading-text", "<i class='fa fa-spinner fa-spin '></i> Đang gửi");
            x.button('loading');
            setTimeout(function () { me.button_adress.button('reset'); }, 30000);
        });
    },
    buttonEndLoading: function () {
        if (this.button_adress) this.button_adress.button('reset');
    },
    /*
    xử lý load các module
    1. Hàm kiểm tra quyền truy cập chức năng
    2. Load chức năng theo thông tin đường dẫn file
    Cách sử dụng: Gọi hàm ở sự kiện onclick
    */
    initMain: function (strDisplayedPath, strRootPath) {
        var me = this;
        var m = "";
        if (strDisplayedPath == undefined || strDisplayedPath == null) {
            var hash = window.location.hash, hashArr = hash.split('/'), params = [];

            if (hashArr.length > 1) {
                for (var i = 0; i < hashArr.length; i++) {
                    params.push(hashArr[i]);
                }
            }
            if (params != undefined && params != "") {
                m = params[0];
            }
            else {
                m = "#" + window.location.hash.substr(1);
            }
        }
        else {
            m = strDisplayedPath;
        }
        var path = m;
        if (typeof (Storage) !== "undefined") {
            if (strRootPath != undefined || strRootPath != null) {
                localStorage.setItem("strRootPath", strRootPath);
            }
        }
        if (strRootPath == undefined) {
            strRootPath = localStorage.strRootPath;
        }
        me.loadFunctionPath(strRootPath);
    },
    checkPermissionByUser: function (userName, strDisplayedPath, strRootPath) {
        var me = this;
        coreRoot.systemroot.makeRequest({
            success: function (data) {
                if (data.Success) {
                    me.init(FunctionUrl);
                }
                else {
                    $(me.contentPlacehoder).html("");
                    $(me.contentPlacehoder).html("<div class='IMSNotesWrapper'>Bạn không có quyền truy cập chức năng này!</div>");
                }
            },
            error: function (er) {
                $(me.contentPlacehoder).html("");
                $(me.contentPlacehoder).html("<div class='IMSNotesWrapper'>Bạn không có quyền truy cập chức năng này!</div>");
            },
            action: 'NguoiDung/checkPermissionByUser',
            data: {
                'userName': userName,
                'functionPath': strDisplayedPath
            },
            fakedb: [
            ]
        }, false, false, false, coreRoot.systemroot.apiUrlCommom);
    },
    loadFunctionPath: function (strRootPath) {
        var me = this;
        var urlPage = strRootPath;
        var main_place = me.contentPlacehoder;
        if (urlPage != null && urlPage != "") {
            $(main_place).html("");
            $(main_place).load(strRootPath);
        }
        else {
            $(main_place).html("");
            $(main_place).html("<div class='IMSNotesWrapper'>Chức năng chưa được kích hoạt!</div>");
        }
    },
    /*
    .xử lý load các module
    */
    common_setup_page: function () {
        $(".select-opt").select2();
        /*Start Cấu hình curency*/
        $('[data-ax5formatter]').ax5formatter();
        /*End Cấu hình curency*/
        /* Xử lý enter di chuyển các input */
        $("input").not($(":button")).keypress(function (evt) {
            if (evt.keyCode == 13) {
                iname = $(this).val();
                if (iname !== 'Submit') {
                    var fields = $(this).parents('form:eq(0),body').find('button, input, textarea, select');
                    var index = fields.index(this);
                    if (index > -1 && (index + 1) < fields.length) {
                        fields.eq(index + 1).focus();
                    }
                    return false;
                }
            }
        });
    },
    /*
    xử lý phân trang
    1. Hàm render html số trang tùy chọn và thông tin dữ liệu hiển thị
    2. Hàm setup thông tin phân trang và xử lý sự kiện next page (strFuntionName: Tên khai báo hàm trong file js,
    strTable_Id: Id bảng dữ liệu, iDataRow: Tổng số dữ liệu)
    Cách gọi như sau:
    1. Tại hàm init gọi hàm pagInfoRender để render html
    2. Tại hàm load dữ liệu sau khi trả về kết quả json thì gọi hàm pagButtonRender
    */
    pagInfoRender: function (strClassName) {
        var me = this;
        if (strClassName == undefined || strClassName == null || strClassName == "")
            strClassName = "tbl-pagination";

        var x = document.getElementsByClassName(strClassName);
        for (var i = 0; i < x.length; i++) {
            var strTable_Id = x[i].id;
            var strPageSize_Id = "Change" + strTable_Id;
            var class_pull_left = '';
            class_pull_left += '<div style="margin-top:10px" class="pull-left" id="' + strPageSize_Id + '" style="padding-left:0px"></div>';
            $("#" + strTable_Id).parent().prev().append(class_pull_left);

            var row_change = "";
            row_change += '<div style="padding-left:0 !important; margin-top:6px; float:left">';
            row_change += '<label>Hiển thị</label>';
            row_change += '</div>';
            row_change += '<div style="width: 70px; padding-left:3px !important; float:left">';
            row_change += '<select id="DropPageSize' + strPageSize_Id + '" class="select-opt">';
            row_change += '<option value="10"> 10 </option>';
            row_change += '<option value="20"> 20 </option>';
            row_change += '<option value="25"> 25 </option>';
            row_change += '<option value="50"> 50 </option>';
            row_change += '<option value="-1"> Tất cả </option>';
            row_change += '</select>';
            row_change += '</div>';
            row_change += '<div style="padding-left:3px !important; margin-top:6px; float:left">';
            row_change += '<label>dữ liệu</label>';
            row_change += '</div>';
            $("#" + strPageSize_Id).html(row_change);
            $(".select-opt").select2();
            $("#DropPageSize" + strPageSize_Id).val("20").trigger("change");
            var row_info = '';
            row_info += '<div style="width:100%; float:right">';
            row_info += '<div id="tbldata_info' + strTable_Id + '" style="width: 40%; float:left"></div>';
            row_info += '<input type="hidden" id="hr_total_rows' + strTable_Id + '" value="0" />';
            row_info += '<div id="light-pagination' + strTable_Id + '" style="float:left; width: 60%"></div>';
            row_info += '</div>';
            $("#" + strTable_Id).after(row_info);

            row_inputhiden = '';
            row_inputhiden += '<input type="hidden" value="10" id="' + strTable_Id + '_DataRow" />';
            $("#" + strTable_Id).after(row_inputhiden);
        }
    },
    pagInfoRender_basic: function (strClassName) {
        var me = this;
        if (strClassName == undefined || strClassName == null || strClassName == "")
            strClassName = "tbl-paginationbasic";

        var x = document.getElementsByClassName(strClassName);
        for (var i = 0; i < x.length; i++) {
            var strTable_Id = x[i].id;

            var strPageSize_Id = "Change" + strTable_Id;
            var class_pull_left = '';
            class_pull_left += '<div class="pull-left col-sm-6" id="' + strPageSize_Id + '" style="padding-left:0px"></div>';
            $("#" + strTable_Id).before(class_pull_left);

            var strFilter_Id = strTable_Id + '_filter';
            var class_pull_right = '';
            class_pull_right += '<div id="' + strFilter_Id + '" class="col-sm-6 dataTables_filter"></div>';
            $("#" + strTable_Id).before(class_pull_right);

            var row_change = "";
            row_change += '<div style="padding-left:0 !important; margin-top:6px; float:left">';
            row_change += '<label>Hiển thị</label>';
            row_change += '</div>';
            row_change += '<div style="width: 70px; padding-left:3px !important; float:left">';
            row_change += '<select id="DropPageSize' + strPageSize_Id + '" class="select-opt">';
            row_change += '<option value="10"> 10 </option>';
            row_change += '<option value="15"> 15 </option>';
            row_change += '<option value="25"> 25 </option>';
            row_change += '<option value="50"> 50 </option>';
            row_change += '<option value="-1"> Tất cả </option>';
            row_change += '</select>';
            row_change += '</div>';
            row_change += '<div style="padding-left:3px !important; margin-top:6px; float:left">';
            row_change += '<label>dữ liệu</label>';
            row_change += '</div>';
            $("#" + strPageSize_Id).html(row_change);
            $(".select-opt").select2();

            var row_filter = '';
            row_filter += '<div class="pull-right">';
            row_filter += '<label>';
            row_filter += 'Tìm kiếm theo từ khóa:';
            row_filter += '<input id="' + strTable_Id + '_input" class="form-control ui-touched" style="width: 250px">';
            row_filter += '</label>';
            row_filter += '</div>';
            $("#" + strFilter_Id).html(row_filter);

            var row_info = '';
            row_info += '<div style="width:100%; float:right">';
            row_info += '<div id="tbldata_info' + strTable_Id + '" style="width: 40%; float:left"></div>';
            row_info += '<input type="hidden" id="hr_total_rows' + strTable_Id + '" value="0" />';
            row_info += '<div id="light-pagination' + strTable_Id + '" style="float:left; width: 60%"></div>';
            row_info += '</div>';
            $("#" + strTable_Id).after(row_info);

            row_inputhiden = '';
            row_inputhiden += '<input type="hidden" value="10" id="' + strTable_Id + '_DataRow" />';
            $("#" + strTable_Id).after(row_inputhiden);
        }
    },
    pagButtonRender: function (strFuntionName, strTable_Id, iDataRow) {
        var me = this;
        var pageIndex = me.pageIndex_default;
        var pageSize = me.pageSize_default;
        $("#hr_total_rows" + strTable_Id).val(iDataRow);
        var eDataRow = $("#" + strTable_Id + "_DataRow").val();
        eDataRow = parseInt(eDataRow);
        if (eDataRow != parseInt(iDataRow) || pageIndex == 1) {
            pageIndex = 1;
            me.pagInit(strFuntionName, strTable_Id, pageSize);
        }
        $("#" + strTable_Id + "_DataRow").val(iDataRow);
        if (parseInt(iDataRow) > 0) {
            var first_item = 1;
            if (pageIndex != 1) {
                first_item = (pageSize * pageIndex) - pageSize + 1;
            }
            if (pageSize == 1000000) {
                first_item = 1;
            }
            var items_in = "";
            if (parseInt(iDataRow) < parseInt(pageSize)) {
                items_in = iDataRow.toString();
            }
            else {
                items_in = (pageSize * pageIndex).toString();
            }
            if (parseInt(iDataRow) < parseInt(items_in)) {
                items_in = iDataRow.toString();
            }
            $("#tbldata_info" + strTable_Id).html("<span>Hiển thị " + first_item + " đến " + items_in + " trong " + iDataRow + " dữ liệu<span>");
        }
        else {
            me.pagInit(strFuntionName, strTable_Id, pageSize);
            $("#tbldata_info" + strTable_Id).html("<span>Hiển thị 0 đến 0 trong 0 dữ liệu</span>");
        }
    },
    pagInit: function (strFuntionName, strTable_Id, pageSize_default) {
        var me = this;
        var totalRows = $('[id$="hr_total_rows' + strTable_Id + '"]').attr('value');
        $('#light-pagination' + strTable_Id).pagination({
            items: totalRows,
            itemsOnPage: pageSize_default,
            cssStyle: 'compact-theme',
            onPageClick: function (pageNumber, event) {
                me.pageIndex_default = pageNumber;
                eval(strFuntionName);
            }
        });

        $('#DropPageSizeChange' + strTable_Id).on('select2:select', function (e) {
            e.stopImmediatePropagation();
            me.pageIndex_default = 1;
            me.pageSize_default = $("#DropPageSizeChange" + strTable_Id).val();
            if (me.pageSize_default == "-1" || me.pageSize_default == -1) {
                me.pageSize_default = 1000000;
            }
            eval(strFuntionName);
            return false;
        });
        if ($("#" + strTable_Id).hasClass("tbl-paginationbasic")) {
            $("#" + strTable_Id + "_input").keypress(function (e) {
                e.stopImmediatePropagation();
                if (e.which == 13) {
                    eval(strFuntionName);
                }
            });
        }
    },
    /*
    Plugin soạn thảo văn bản
    */
    LoadEditor: function (editorName) {
        var me = this;
        if (typeof (CKEDITOR) != "undefined") {
            var configCK = {
                customConfig: '',
                height: '320px',
                width: '100%',
                language: 'vi',
                entities: false,
                fullPage: false,
                toolbarCanCollapse: false,
                resize_enabled: false,
                startupOutlineBlocks: true,
                colorButton_enableMore: false,
                toolbar:
                    [
                        { name: 'document', items: ['Source'] },
                        { name: 'tools', items: ['Maximize'] },
                        { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'TextColor', 'BGColor'] },
                        { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight'] },
                        { name: 'insert', items: ['HorizontalRule', 'SpecialChar'] },
                        { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
                    ],
                filebrowserImageUploadUrl: me.rootPath + '/App_Themes/Plugins/Upload.ashx',
                filebrowserBrowseUrl: me.rootPath + '/App_Themes/Plugins/ckfinder/ckfinder.html'
            };
            var editor = CKEDITOR.replace(editorName, configCK);
            CKEDITOR.on('instanceReady', function (e) {
                CKFinder.setupCKEditor(editor, me.rootPath + '/App_Themes/Plugins/ckfinder/');
            });
        }
    },
    common_setup_datepicker: function (strClassName) {
        if (strClassName == undefined || strClassName == null || strClassName == "")
            strClassName = "input-datepicker";
        var x = document.getElementsByClassName(strClassName);
        for (var i = 0; i < x.length; i++) {
            var strInput_Id = x[i].id;
            var cleave = new Cleave('#' + strInput_Id, {
                date: true,
                datePattern: ['d', 'm', 'Y']
            });
            $('#' + strInput_Id).datepicker({
                autoclose: true,
                dateFormat: "dd/mm/yy",
                changeMonth: true,
                changeYear: true
            });
        }
    },
    beginLoading: function () {
        document.getElementById('overlay').style.display = "";
    },
    endLoading: function () {
        document.getElementById('overlay').style.display = "none";
    },
    resetchkSelect: function () {
        $('[id^=chkSelect]').attr('checked', false);
    },
    /*
    Cây thư mục trái
    */
    config_left_content_tree: function () {
        var flag_sh = true;
        var content_height = main_content_height();
        $("#close_tree").on("click", function (e) {/*closed-open button treejs*/
            if (flag_sh == true) {
                $("#left_tree").animate(moveRight());
                flag_sh = false;
            }
            else {
                $("#left_tree").animate(moveLeft());
                flag_sh = true;
            }
        });
        function moveRight() {
            $("#left_tree").removeClass("col-sm-4");
            $("#right_tree").removeClass("col-sm-8");
            $("#close_tree").html('');
            $("#left_tree").addClass("col-sm-5");
            $("#right_tree").addClass("col-sm-7");
            $("#close_tree").html('<i class="fa fa-caret-left"></i>');
        };
        function moveLeft() {
            $("#left_tree").removeClass("col-sm-5");
            $("#right_tree").removeClass("col-sm-7");
            $("#close_tree").html('');
            $("#left_tree").addClass("col-sm-4");
            $("#right_tree").addClass("col-sm-8");
            $("#close_tree").html('<i class="fa fa-caret-right"></i>');
        };
        function main_content_height() {
            /*processing height*/
            var main_content_wrapper = $("#main-content-wrapper").height();
            var content_header = $(".content-header").height();
            var area_step = $(".area-step").height();
            var main_footer = $(".main-footer").height();
            var content = main_content_wrapper - content_header - area_step - main_footer - 15;
            return content;
        };
    }
};