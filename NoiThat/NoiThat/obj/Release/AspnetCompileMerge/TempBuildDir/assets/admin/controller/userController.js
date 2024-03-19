var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('#btnAddNew').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var username = document.getElementById('m_username');
            var name = document.getElementById('m_name');
            var password = document.getElementById('m_password');
            var address = document.getElementById('m_address');
            var email = document.getElementById('m_email');
            var phone = document.getElementById('m_phone');

            if (username.value == "" || name.value == "" || password.value == "") {
                bootbox.alert("Chưa nhập thông tin cần thiết")
                return;
            }
            $.ajax({
                url: "/User/AddUserAjax",
                data:
                    {
                        username: username.value,
                        name: name.value,
                        password: password.value,
                        address: address.value,
                        email: email.value,
                        phone: phone.value
                    },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        bootbox.alert({
                            message: "Thêm tài khoản thành công!",
                            size: 'medium',
                            closeButton: false
                        });
                        username.value = "";
                        name.value = "";
                        password = "";

                        window.location.href = "/Admin/User";
                    }
                    else {
                        bootbox.alert(
                            {
                                message: "Thêm tài khoản lỗi",
                                closeButton: false
                            }
                           );
                    }
                }
            });
        });


        $('.abclaice').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var listid = btn.data("listid");
            var name = btn.data('name');
            var address = btn.data('address');
            var email = btn.data('email');
            var phone = btn.data('phone');

            var valuename = document.getElementById(name);
            var valueaddress = document.getElementById(address);
            var valueemail = document.getElementById(email);
            var valuephone = document.getElementById(phone);
            

            $.ajax({
                url: "/User/UpdateUserAjax",
                data:
                    {
                        userid: listid,
                        name: valuename.value,                      
                        address: valueaddress.value,
                        email: valueemail.value,
                        phone : valuephone.value
                    },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        bootbox.alert({
                            message: "Cập nhật thành công!",
                            size: 'medium',
                            closeButton: false
                        });

                        window.location.href = "/Admin/User";
                    }
                    else {
                        bootbox.alert(
                            {
                                message: "Cập nhật tài khoản lỗi",
                                closeButton: false
                            }
                           );
                    }
                }
            });
        });
     }
}
user.init();