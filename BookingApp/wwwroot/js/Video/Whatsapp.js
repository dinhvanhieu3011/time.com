function Whatsapp() { };
Whatsapp.prototype = {
    id: '',
    init: function () {
        var me = this;
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
            },
            cache: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //coreRoot.systemroot.endLoading();
                $('#curren_number').html(data.phoneNumer) ;
                let html = '';
                let mynum = $('#my_num').val();
                for (let i = 0; i < data.whatsAppChats.length; i++) {
                    if (data.whatsAppChats[i].fromPhoneNumber === mynum) {
                        html = html + "<div class='chat-message-right pb-4'>   "
                            +    "<div>                                                                                  "
                            +  "<div class='text-muted small text-nowrap mt-2'>" + data.whatsAppChats[i].time +"</div>   "
                            +	"				</div>                                                                   "
                            +    "<div class='flex-shrink-1 bg-light rounded py-2 px-3 mr-3'>                              "
                            + "    <div class='font-weight-bold mb-1'>" + data.whatsAppChats[i].fromPhoneNumber +"</div>                       "
                            + data.whatsAppChats[i].message  +                                                                      
                             "</div>"
                            + "			</div> "
                    }
                    else {
                        html = html + "									<div class='chat-message-left pb-4'>   "
                        +"<div>                                                                                  "
                        +"<div class='text-muted small text-nowrap mt-2'>" + data.whatsAppChats[i].time + "</div>                          "
                        +"				</div>                                                                   "
                        +"<div class='flex-shrink-1 bg-light rounded py-2 px-3 mr-3'>                              "
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