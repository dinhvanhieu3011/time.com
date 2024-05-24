
/*Written by:Zest Nguyen*/
//Date Created:19/1/2010

function UploadImage(root, w, h, sPath, divcontrol, divresult, divimageid, inputUrl, isFile, isExcel) {
    var button = $(document.getElementById(divcontrol)), interval;
    new AjaxUpload(button, {
        action: root + "/Handler/UploadFile.ashx?W=" + w + "&H=" + h + "&P=" + sPath + "&isScale=" + isFile + "&isExcel=" + isExcel,
        name: 'myfile',
        
        onSubmit: function (file, ext) {
            if (isFile == "true")
            {
                if (!(ext && /^(doc|docx|xls|xlsx|pdf|rar)$/i.test(ext))) {
                    // extension is not allowed
                    eduroot.util.alert("Thông báo", 'File đính kèm không đúng định dạng!');
                    // cancel upload
                    return false;
                }
                else {
                    // change button text, when user selects file			
                    button.text('Đang tải lên hệ thống...');

                    // If you want to allow uploading only 1 file at time,
                    // you can disable upload button
                    this.disable();

                    // Uploading -> Uploading. -> Uploading...
                    interval = window.setInterval(function () {
                        var text = button.text();
                        if (text.length < 13) {
                            button.text(text + '.');
                        } else {
                            button.text('Đang tải lên hệ thống...');
                        }
                    }, 200);
                }
            }
            else
            {
                if (!(ext && /^(jpg|png|jpeg|gif)$/i.test(ext))) {
                    // extension is not allowed
                    eduroot.util.alert("Thông báo", 'File hình không đúng định dạng!');
                    // cancel upload
                    return false;
                }
                else
                {
                    // change button text, when user selects file			
                    button.text('Đang tải lên hệ thống...');

                    // If you want to allow uploading only 1 file at time,
                    // you can disable upload button
                    this.disable();

                    // Uploading -> Uploading. -> Uploading...
                    interval = window.setInterval(function () {
                        var text = button.text();
                        if (text.length < 13) {
                            button.text(text + '.');
                        } else {
                            button.text('Đang tải lên hệ thống...');
                        }
                    }, 200);
                }
            }
        },
        onComplete: function (file, response) {
            $(document.getElementById(divcontrol)).html("<input type='image' src='" + root + "/App_Themes/CMS/images/file_edit.png' value='Chage' />");

            $(document.getElementById(divcontrol)).css({ "margin-top": "0px" });
            $(document.getElementById(divcontrol)).attr('class', 'tblUpload');
            window.clearInterval(interval);

            // enable upload button
            this.enable();

            /*fix thick box*/

            $(document.getElementById(divimageid)).html(response);
            var result = $(document.getElementById(divimageid)).find('pre').html();
            if (result.length > 0) {
                // add file to the list
                var urlimage = result;
               
                $(document.getElementById(inputUrl)).attr('value', result);

                if (isFile == "true")
                {
                    var filename = "";
                    filename = eduroot.util.getFileNameUpload(result, "/");
                    $(document.getElementById(divresult)).html('<table><tr><td><a href="' + 'javascript:void(0)' + '" class="thickbox"  >' + filename + '</a></td><td style="padding-left:10px; font-weight:bold"><a href="javascript:void(0)" onclick="return deleupload(\'' + result + '\',\'' + inputUrl + '\',\'' + divresult + '\',\'' + divcontrol + '\')">X</a></td></tr></table>');
                }
                else
                {
                    $(document.getElementById(divresult)).html('<table><tr><td><a href="' + 'javascript:void(0)' + '" class="thickbox"  ><img width=" ' + 84 + 'px" ' + 'src="' + urlimage + '"/></a></td><td style="padding-left:10px; font-weight:bold"><a href="javascript:void(0)" onclick="return deleupload(\'' + result + '\',\'' + inputUrl + '\',\'' + divresult + '\',\'' + divcontrol + '\')">X</a></td></tr></table>');
                }
                
            }
        }
    });

};
function deleupload(param, param2, param3, divcontrol) {
    var del = eduroot.util.confirm("Thông báo", 'Bạn có chắc chắn muốn xóa file này không?');
    $("#btnYes").click(function (e) {
        $.ajax({
            url: eduroot.util.root + '/Handler/DelFile.ashx?p=' + param,
            success: function (data) {
                $(document.getElementById(param2)).attr('value', " ");
                $(document.getElementById(param3)).html(" ");
                $(document.getElementById(divcontrol)).css({ "margin-top": "0px" });
                $(document.getElementById(divcontrol)).html("<input type='image' src='" + eduroot.systemroot.rootPath + "/App_Themes/CMS/images/file_add.png' value='Upload' />");
                $(document.getElementById(divcontrol)).attr('class', 'tblUploadDel');
            }
        });
        $('#myModalAlert').modal('hide');
    });
}