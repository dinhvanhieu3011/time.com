7/*----------------------------------------------
--Author: hieudv
--Description:
--Date of created: 30/09/2021
--Input:
--Output:
--Note: 
--Updated by:
--Date of updated: 
----------------------------------------------*/
function hubATP() { };
hubATP.prototype = {
    urlHub: "http://localhost:18868/signalr",
    urlFile: "",
    result: false,
    SignalrConnection: $.connection.signHub,
    CheckPlugin: function (str) {
        var SignalrConnection;
        $.connection.hub.url = this.urlHub;
        SignalrConnection = $.connection.signHub;
        if (SignalrConnection == null) {
            coreRoot.utility.alert("Thông báo", "Chưa bật plugin ký điện tử. Vui lòng kiểm tra lại (hoặc liên hệ Quản trị viên để được hỗ trợ). Tải lại trang web để thực hiện chức năng!");
            return false;
        }
    },
    SignPDF: function (base64PDF, fileName, logo, reason, fieldName, rectangle, page, IdBaocao, capKySo) {
        coreRoot.systemroot.beginLoading();
        var SignalrConnection;
        $.connection.hub.url = this.urlHub;
        SignalrConnection = $.connection.signHub;
        if (SignalrConnection == null) {
            coreRoot.utility.alert("Thông báo", "Chưa bật plugin ký điện tử. Vui lòng kiểm tra lại (hoặc liên hệ Quản trị viên để được hỗ trợ). Tải lại trang web để thực hiện chức năng!");
            coreRoot.systemroot.endLoading();
            return false;
        }
        $.connection.hub.start().done(function () {
            coreRoot.utility.alert("Thông báo", "Đang ký hoá đơn...!");
            var progressbarLabel = $('#progressbar-label');
            var progressbarDiv = $('#progress-bar');
            console.log(base64PDF);

            var interVal = setTimeout(function () {
                SignalrConnection.server.SignHashPDF(base64PDF, fileName, "", "", fieldName, "", -1).done(function (result) {
                    console.log(result);
                    if (result.Success == "true") {
                        var jsonData =
                        {
                            'Id': IdBaocao,
                            'C1': capKySo,
                            'C2': result.base64Data,
                            'C3': fileName
                        };
                        console.log(result.base64Data);
                        coreRoot.systemroot.makeRequest({
                            success: function (data) {
                                $(".modal-backdrop").css("display", "none");
                                document.getElementById("pdfFile").contentDocument.location.reload(true);
                                coreRoot.utility.alert("Thông báo", "Ký số thành công!");
                                coreRoot.systemroot.endLoading();
                            },
                            error: function (er) { },
                            type: 'POST',
                            action: 'THDTQT_KT_BaoCao/KySo',
                            data: JSON.stringify(jsonData),
                            fakedb: [
                            ]
                        }, false, false, false, null);
                    }
                    else {
                        $(".modal-backdrop").css("display", "none");
                        coreRoot.utility.alert("Thông báo", "Ký số thất bại. Vui lòng kiểm tra lại!");
                        coreRoot.systemroot.endLoading();
                    }
                });
            }, 3000);
        }).fail(function () {
            $(".modal-backdrop").css("display", "none");
            coreRoot.utility.alert("Thông báo", "Kết nối Plugin ký thất bại. Vui lòng kiểm tra lại!");
            coreRoot.systemroot.endLoading();
        });
    },
};