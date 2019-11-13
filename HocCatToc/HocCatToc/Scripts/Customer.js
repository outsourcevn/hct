function openCustomer(id, name, email, phone,group_id,group_list_id) {

    $("#cp_ID").val(id);
    $("#cp_name").val(name);
    $("#cp_email").val(email);
    $("#cp_phone").val(phone);
    $("#cp_group_id").val(group_id);
    if (group_list_id == null || group_list_id == "") group_list_id = ",";
    var values = group_list_id;
    $.each(values.split(","), function (i, e) {
        $("#cp_group_id option[value='" + e + "']").prop("selected", true);
    });

    $("#CustomerDialog").show();
}
function generate(user_id, group_list_id) {
    $("#cp_ID2").val(user_id);
    if (group_list_id == null || group_list_id == "") group_list_id = ",";
    var values = group_list_id;
    $.each(values.split(","), function (i, e) {
        $("#cp_group_id2 option[value='" + e + "']").prop("selected", true);
    });
    $("#CustomerDialog2").show();
}
function saveGroup() {
    var user_id = $("#cp_ID2").val();
    var group_id_list2 = ",";
    $("#cp_group_id2 option:selected").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.val() + ",";
            group_id_list2 += selText;
        }
    });
    document.getElementById("btnsaveGroup").disabled = true;
    $.ajax({
        url: "/Admin/generateCode", type: 'post',
        data: { user_id: user_id, group_id_list: group_id_list2 },
        success: function (rs) {
            if (rs == '1') {
                alert("Đã sinh mã học của các gói học này cho học viên!");
            } else {
                alert(rs);
            }
            document.getElementById("btnsaveGroup").disabled = false;
            closeDDialog('#CustomerDialog2');
        }
    });
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
    if ( ($("#cp_ID").val()=="" || $("#cp_ID").val()==0 || $("#cp_ID").val()==null) && $("#cp_pass").val() =="") {
        alert("Nhập mật khẩu!");
        return;
    } else
    if ($("#cp_pass").val()!="" && $("#cp_pass").val() != $("#cp_pass2").val()) {
        alert("Mật khẩu phải giống nhau!");
        return;
    }
    var group_id_list = ",";
    $("#cp_group_id option:selected").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.val() + ",";
            group_id_list += selText;
        }
    });
    document.getElementById("btnsaveCustomer").disabled = true;
    $.ajax({
        url: url_addUpdateCustomer, type: 'post',
        contentType: 'application/json',
        data: JSON.stringify({
            ID: $("#cp_ID").val(), name: $("#cp_name").val(), email: $("#cp_email").val(), phone: $("#cp_phone").val(), pass: $("#cp_pass").val(),
            group_id: document.getElementById("cp_group_id").value, group_name: group_text, group_id_list: group_id_list
        }),
        success: function (rs) {
            if (rs == '') {
                location.reload();
            } else {
                alert(rs);
            }
            document.getElementById("btnsaveCustomer").disabled = false;
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
//function generate(user_id) {
//    //alert(user_id);
//    $.ajax({
//        url: "/Admin/generateCode", type: 'post',
//        data: { user_id: user_id },
//        success: function (rs) {
//            if (rs == '1') {
//                alert("Đã sinh mã học các tập video cho học viên. Mời học viên kiểm tra trên app di động");
//            } else {
//                alert(rs);
//            }
//        }
//    });
//}
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
