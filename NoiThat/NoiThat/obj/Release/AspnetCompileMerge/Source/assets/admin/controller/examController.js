var exam = {
    init: function () {
        exam.registerEvents();
    },
    registerEvents: function () {
        $('#btnAddNew').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var name = document.getElementById('m_name');
            var metatitle = document.getElementById('m_metatitle');
            var code = document.getElementById('m_code');
            var questionlist = document.getElementById('m_questionlist');
            var answerlist = document.getElementById('m_answerlist');
            var productid = document.getElementById('ddlProduct');
            var startdate = document.getElementById('m_startdate');
            var enddate = document.getElementById('m_enddate');
            var totalscore = document.getElementById('m_totalscore');
            var time = document.getElementById('m_time');

            var totalquestion = document.getElementById('m_totalquestion');
            var questionessay = document.getElementById('m_questionessay');
            var userlist = document.getElementById('m_userlist');
            var scorelist = document.getElementById('m_scorelist');

            if (name.value == "" || questionlist.value == "" || time.value == "") {
                bootbox.alert("Chưa nhập thông tin cần thiết")
                return;
            }
            $.ajax({
                url: "/Exam/AddExamAjax",
                data:
                    {
                        name: name.value,
                        metatitle: metatitle.value,
                        code: code.value,
                        questionlist: questionlist.value,
                        answerlist: answerlist.value,
                        productid: productid.value,
                        startdate: startdate.value,
                        enddate: enddate.value,
                        totalscore: totalscore.value,
                        time: time.value,
                        totalquestion: totalquestion.value,
                        questionessay: questionessay.value,
                        userlist: userlist.value,
                        scorelist: scorelist.value                        
                    },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        bootbox.alert({
                            message: "Thêm bài kiểm tra thành công!",
                            size: 'medium',
                            closeButton: false
                        });
                        name.value = "";
                        metatitle.value = "";
                        code.value = "";
                        questionlist.value = "";
                        answerlist.value = "";
                        productid.value = "";
                        totalscore.value = "";
                        time.value = "";
                        totalquestion.value = "";
                        questionessay.value = "";
                        userlist.value = "";
                        scorelist.value = "";
                        window.location.href = "/Admin/Exam";
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
            var code = btn.data('code');
            var metatitle = btn.data('metatitle');
            var questionlist = btn.data('questionlist');
            var answerlist = btn.data('answerlist');
            var time = btn.data('time');
            var productid = btn.data('productid');
            var totalquestion = btn.data('totalquestion');
            var totalscore = btn.data('totalscore');
            var questionessay = btn.data('questionessay');
            var scorelist = btn.data('scorelist');
            var userlist = btn.data('userlist');
            var startdate = btn.data('startdate');
            var enddate = btn.data('enddate');

            var valuename = document.getElementById(name);
            var valuecode = document.getElementById(code);
            var valuemetatitle = document.getElementById(metatitle);
            var valuequestionlist = document.getElementById(questionlist);
            var valueanswerlist = document.getElementById(answerlist);
            var valuetime = document.getElementById(time);
            var valueproductid = document.getElementById(productid);
            var valuetotalquestion = document.getElementById(totalquestion);
            var valuetotalscore = document.getElementById(totalscore);
            var valuequestionessay = document.getElementById(questionessay);
            var valuescorelist = document.getElementById(scorelist);
            var valueuserlist = document.getElementById(userlist);
            var valuestartdate = document.getElementById(startdate);
            var valueenddate = document.getElementById(enddate);

            $.ajax({
                url: "/Exam/UpdateExamAjax",
                data:
                    {
                        id: listid,
                        name: valuename.value,
                        code: valuecode.value,
                        metatitle: valuemetatitle.value,
                        questionlist: valuequestionlist.value,
                        answerlist: valueanswerlist.value,
                        time: valuetime.value,
                        productid: valueproductid.value,
                        totalquestion: valuetotalquestion.value,
                        totalscore: valuetotalscore.value,
                        questionessay: valuequestionessay.value,
                        scorelist: valuescorelist.value,
                        userlist: valueuserlist.value,
                        startdate: valuestartdate.value,
                        enddate: valueenddate.value

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

                        window.location.href = "/Admin/Exam";
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
exam.init();