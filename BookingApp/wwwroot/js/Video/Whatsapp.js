function Whatsapp() { };
Whatsapp.prototype = {
    id: '',
    myNum : '',
    init: function () {
        var me = this;
        me.myNum = window.location.pathname.split('/')[3];
        $(document).delegate('.card1', 'click', function () {
            me.id = this.id;
            console.log(me.id)
            me.list_DanhSach()
        });
    },
    list_DanhSach: function () {
        var me = this;
        //coreRoot.systemroot.beginLoading();
        $.ajax({
            type: 'GET',
            crossDomain: true,
            url: '/api/Upload/getChat',
            data: {
                'chatId': me.id,
                'phone': me.myNum,
            },
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //coreRoot.systemroot.endLoading();
                $('#curren_number').html(data.phoneNumer) ;
                let html = '';
                for (let i = 0; i < data.whatsAppChats.length; i++) {
                    if (data.whatsAppChats[i].fromPhoneNumber === me.mynum) {
                        html = html + "<div class='chat-message-right pb-4'>   "
                            + "<div>                                                                                  "
                            + "<div class='rounded-circle mr-1 profileImage' alt='" + data.whatsAppChats[i].fromPhoneNumber + "' width='40' height='40'>" + data.whatsAppChats[i].fromPhoneNumber.substring(data.whatsAppChats[i].fromPhoneNumber.Length - 3, 3) + "</div>"
                            + "<div class='text-muted small text-nowrap mt-2'>" + moment(data.whatsAppChats[i].time + '').format("DD/MM/YYYY h:mm:ss") +"</div>   "
                            +	"				</div>                                                                   "
                            +    "<div class='flex-shrink-1 bg-light rounded py-2 px-5 mr-5'>                              "
                            + "    <div class='font-weight-bold mb-1'>" + data.whatsAppChats[i].fromPhoneNumber +"</div>                       "
                            + data.whatsAppChats[i].message  +                                                                      
                             "</div>"
                            + "			</div> "
                    }
                    else {
                        html = html + "									<div class='chat-message-left pb-4'>   "
                            + "<div>                                                                                  "
                            + "<div class='rounded-circle mr-1 profileImage' alt='" + data.whatsAppChats[i].fromPhoneNumber + "' width='40' height='40'>" + data.whatsAppChats[i].fromPhoneNumber.substring(data.whatsAppChats[i].fromPhoneNumber.Length - 3, 3)+"</div>"
                            + "<div class='text-muted small text-nowrap mt-2'>" + moment(data.whatsAppChats[i].time + '').format("DD/MM/YYYY h:mm:ss") + "</div>                          "
                        +"				</div>                                                                   "
                            + "<div class='flex-shrink-1 bg-light rounded py-2 px-5 mr-5'>                              "
                        +""
                            + "    <div class='font-weight-bold mb-1'>" + data.whatsAppChats[i].fromPhoneNumber + "</div>                       "
                            + data.whatsAppChats[i].message +
                                "</div>"
                            + "			</div> "
                    }
                }
                $('#chat').html(html) ;
                console.log(html)
            },
            error: function (er) { coreRoot.systemroot.endLoading(); },
            timeout: 3000000
        });
    },
}