var question = {
    init: function () {
        question.registerEvents();
    },
    registerEvents: function () {
        $('#btnAddNew').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            
            var name = document.getElementById('m_name');
            var content = document.getElementById('m_content');
            var answer = document.getElementById('m_answer');
            var productid = document.getElementById('ddlProduct');
            

            if (name.value == "" || content.value == "") {
                bootbox.alert("Chưa nhập thông tin cần thiết")
                return;
            }
            $.ajax({
                url: "/Question/AddQuestionAjax",
                data:
                    {
                        name: name.value,
                        content: content.value,
                        answer: answer.value,
                        productid: productid.value
                    },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        bootbox.alert({
                            message: "Thêm câu hỏi thành công!",
                            size: 'medium',
                            closeButton: false
                        });
                        name.value = "";
                        content.value = "";
                        answer.value = "";

                        window.location.href = "/Admin/Question";
                    }
                    else {
                        bootbox.alert(
                            {
                                message: "Thêm lỗi",
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
            var content = btn.data('content');
            var answer = btn.data('answer');
            var productid = btn.data('productid');

            var valuename = document.getElementById(name);
            var valuecontent = document.getElementById(content);
            var valueanswer = document.getElementById(answer);
            var valueproductid = document.getElementById(productid);


            $.ajax({
                url: "/Question/UpdateQuestionAjax",
                data:
                    {
                        id: listid,
                        name: valuename.value,
                        content: valuecontent.value,
                        answer: valueanswer.value,
                        productid: valueproductid.value
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

                        window.location.href = "/Admin/Question";
                    }
                    else {
                        bootbox.alert(
                            {
                                message: "Cập nhật lỗi",
                                closeButton: false
                            }
                           );
                    }
                }
            });
        });
         
     }
}
question.init();