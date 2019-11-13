function openGroup(id, name, email, phone,address,price) {

    $("#cp_ID").val(id);
    $("#cp_group_name").val(name);
    $("#cp_email").val(email);
    $("#cp_phone").val(phone);
    $("#cp_address").val(address);
    $("#cp_price").val(price);
    $("#GroupDialog").show();
}

function saveGroup() {
    
    if ($("#cp_group_name").val() == "") {
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
    if ($("#cp_address").val() == "") {
        alert("Nhập địa chỉ!");
        return;
    }
    
    $.ajax({
        url: url_addUpdateGroup, type: 'post',
        contentType: 'application/json',
        data: JSON.stringify({
            ID: $("#cp_ID").val(), group_name: $("#cp_group_name").val(), email: $("#cp_email").val(), phone: $("#cp_phone").val(), address: $("#cp_address").val()
            , price: $("#cp_price").val()
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

function confirmDelGroup(cpId, group_name) {
    $("#cp_ID").val(cpId);
    openNotification("Bạn có chắc chắn xóa " + group_name + " ?", "deleteGroup");
}

function deleteGroup() {
    $.ajax({
        url: url_deleteGroup, type: 'post',
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

function searchGroup() {
    
        window.location.href = "/Admin/Group?k=" + $("#keyword").val();
   
}
