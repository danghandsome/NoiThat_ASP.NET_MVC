var product = {
    init: function () {
        product.registerEvents();
    },
    registerEvents: function () {

        $('#categoryDropDownList').on("change", function ()
        {
            var category = $('#categoryDropDownList').val();
            window.location.href = "/Admin/Product?dropdownid=" + category;
        });


        $('.editabcdef').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var detailedit = btn.data('detailedit');
            CKEDITOR.instances['m_detailedit'].setData(detailedit);

        });
        
        $('.badgebtnlink').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var valueimageurl = document.getElementById('e_image');
            valueimageurl.value = "";
            CKEDITOR.instances['m_detailedit'].setData("");
        });

        $('.abclaice').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var listid = btn.data("listid");
            var name = btn.data('name');
            var code = btn.data('code');
            var metatitle = btn.data('metatitle');
            var description = btn.data('description');
            var detail = CKEDITOR.instances['m_detailedit'].getData();
            var image = btn.data('image');
            var listtype = btn.data('listtype');
            var listfile = btn.data('listfile');

            var valuename = document.getElementById(name);
            var valuecode = document.getElementById(code);
            var valuemetatitle = document.getElementById(metatitle);
            var valuedescription = document.getElementById(description);
            var valueimage = document.getElementById(image);
            var valuelisttype = document.getElementById(listtype);
            var valuelistfile = document.getElementById(listfile);
 
            var valueimageurl = document.getElementById('e_image');
            if (valueimageurl.value == "")
            {
                valueimageurl.value = valueimage.value;
            }

            var categoryid = btn.data("categoryid");
            var valuecategoryid = document.getElementById(categoryid);

            var ContributeModel = {
                id:listid,
                name: valuename.value,
                code: valuecode.value,
                metatitle: valuemetatitle.value,
                description: valuedescription.value,
                detail: detail,
                image: valueimageurl.value,
                listtype: valuelisttype.value,
                listfile: valuelistfile.value,
                categoryid: valuecategoryid.value
            }
            if (name.value == "" || metatitle.value == "") {
                bootbox.alert("Chưa nhập thông tin cần thiết")
                return;
            }
            $.ajax({
                url: "Product/UpdateProductAjax",
                data: JSON.stringify(ContributeModel),
                contentType: 'application/json; charset=utf-8',
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        bootbox.alert({
                            message: "Cập nhật khóa học thành công!",
                            size: 'medium',
                            closeButton: false
                        });
                        name.value = "";
                        code.value = "";
                        metatitle.value = "";
                        description.value = "";
                        image.value = "";
                        valueimageurl.value = "";
                        listtype.value = "";
                        listfile.value = "";
                        window.location.href = "/Admin/Product";
                    }
                    else {
                        bootbox.alert(
                            {
                                message: "Cập nhật khóa học lỗi",
                                closeButton: false
                            }
                           );
                    }
                }
            });
        });


        $('#btnAddNew').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var name = document.getElementById('m_name');
            var code = document.getElementById('m_code');
            var metatitle = document.getElementById('m_metatitle');
            var description = document.getElementById('m_description');
            var image = document.getElementById('m_image');
            var categoryid = document.getElementById('ddlCategory');
            var detail = CKEDITOR.instances['m_detail'].getData();
            var listtype = document.getElementById('m_listtype');
            var listfile = document.getElementById('m_listfile');


            var ContributeModel = {
                name: name.value,
                code: code.value,
                metatitle: metatitle.value,
                description: description.value,
                image: image.value,
                categoryid: categoryid.value,
                detail: detail,
                listtype: listtype.value,
                listfile: listfile.value

            }


            if (name.value == "" || metatitle.value == "" || image.value == "") {
                bootbox.alert("Chưa nhập thông tin cần thiết")
                return;
            }
            $.ajax({
                url: "/Product/AddProductAjax",
                data:JSON.stringify(ContributeModel),              
                contentType: 'application/json; charset=utf-8',
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        bootbox.alert({
                            message: "Thêm khóa học thành công!",
                            size: 'medium',
                            closeButton: false
                        });
                        name.value = "";
                        code.value = "";
                        metatitle.value = "";
                        description.value = "";
                        image.value = "";
                        listtype.value = "";
                        listfile.value = "";

                        window.location.href = "/Admin/Product";
                    }
                    else {
                        bootbox.alert(
                            {
                                message: "Thêm khóa học lỗi",
                                closeButton: false
                            }
                           );
                    }
                }
            });
        });
    }
}
product.init();