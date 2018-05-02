function openCustomer(id, name, email, phone,group_id) {

    $("#cp_ID").val(id);
    $("#cp_name").val(name);
    $("#cp_email").val(email);
    $("#cp_phone").val(phone);
    $("#cp_group_id").val(group_id);
    $("#CustomerDialog").show();
}

function saveCustomer() {
    var group_text = $("#cp_group_id option:selected").text();
    if ($("#cp_name").val() == "") {
        alert("Nhập tên!");
        return;
    }
    if ($("#cp_email").val() == "") {
        alert("Nhập email!");
        return;
    }
    if ($("#cp_phone").val() == "") {
        alert("Nhập phone!");
        return;
    }
    if ($("#cp_group_id").val() == "") {
        alert("Chọn học viện!");
        return;
    }
    if ($("#cp_pass").val() =="") {
        alert("Nhập mật khẩu!");
        return;
    }
    if ($("#cp_pass").val()!="" && $("#cp_pass").val() != $("#cp_pass2").val()) {
        alert("Mật khẩu phải giống nhau!");
        return;
    }
    $.ajax({
        url: url_addUpdateCustomer, type: 'post',
        contentType: 'application/json',
        data: JSON.stringify({
            ID: $("#cp_ID").val(), name: $("#cp_name").val(), email: $("#cp_email").val(), phone: $("#cp_phone").val(), pass: $("#cp_pass").val(),
            group_id:$("#cp_group_id").val(),group_name:group_text
        }),
        success: function (rs) {
            if (rs == '') {
                location.reload();
            } else {
                alert(rs);
            }
        }
    })
}

function confirmDelCustomer(cpId, customer_email) {
    $("#cp_ID").val(cpId);
    openNotification("Bạn có chắc chắn xóa " + customer_email + " ?", "deleteCustomer");
}

function deleteCustomer() {
    $.ajax({
        url: url_deleteCustomer, type: 'post',
        data: { cpId: $("#cp_ID").val() },
        success: function (rs) {
            if (rs == '') {
                location.reload();
            } else {
                alert(rs);
            }
        }
    });
}
function generate(user_id) {
    //alert(user_id);
    $.ajax({
        url: "/Admin/generateCode", type: 'post',
        data: { user_id: user_id },
        success: function (rs) {
            if (rs == '1') {
                alert("Đã sinh mã học các tập video cho học viên. Mời học viên kiểm tra trên app di động");
            } else {
                alert(rs);
            }
        }
    });
}
function confirmAdmin(id) {
    $.ajax({
        url: "/Admin/confirmAdmin", type: 'post',
        data: { id: id },
        success: function (rs) {
            if (rs == '1') {
                alert("Đã xác nhận quyền admin");
                location.reload();
            } else {
                alert(rs);
            }
        }
    });
}
function searchCustomer() {
    
        window.location.href = "/Admin/Customer?k=" + $("#keyword").val();
   
}
