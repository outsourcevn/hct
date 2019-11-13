function openVideo(id, name, link, img,group_list_id,is_free) {
    $("#cp_ID").val(id);
    $("#cp_name").val(name);
    $("#cp_link").val(link);
    //$("#cp_des").val(des);    
    $("#cp_group_id").val(group_list_id);
    $("#image").val(img);
    $("#prd_image").attr("src", img);
    $('input:radio[id="is_free"][value="' + is_free + '"]').prop('checked', true);
    if (group_list_id == null || group_list_id == "") group_list_id = ",";
    var values = group_list_id;
    $.each(values.split(","), function (i, e) {
        $("#cp_group_id option[value='" + e + "']").prop("selected", true);
    });
    
    if (id != 0 && id != "" && id != null) {
        $.ajax({
            url: "/home/getdesvideo?id="+id, type: 'get',
            success: function (rs) {                
                $("#cp_des").val(rs);
            }
        });
    }
    $("#VideoDialog").show();
}

function saveVideo() {
    
        //alert($("#cp_group_id").val());
        //return;
    
    
    //alert(radioValue);
    //return;
    if ($("#cp_name").val() == "") {
        alert("Nhập tên!");
        return;
    }
    if ($("#cp_link").val() == "") {
        alert("Nhập link!");
        return;
    }
    if ($("#image").val() == "") {
        alert("Nhập ảnh!");
        return;
    }
    if ($("#cp_group_id").val() == "0") {
        alert("Nhập học viện tóc!");
        return;
    }
    
    var group_id_list = ",";//$("#cp_group_id").val();//document.getElementById("cp_group_id").value;
    $("#cp_group_id option:selected").each(function () {
        var $this = $(this);
        if ($this.length) {
            var selText = $this.val() + ",";
            group_id_list += selText;
        }
    });
    //alert(group_id_list);
    //return;
    var is_free = $("input[id='is_free']:checked").val();
    //alert($("#cp_group_id>option:selected").html());
    //alert($("#cp_group_id").val());
    //alert($("#image").val());
    //return;
    document.getElementById("btdsavevideo").disabled = true;
    $.ajax({
        url: url_addUpdateVideo, type: 'post',
        contentType: 'application/json',
        data: JSON.stringify({
            ID: $("#cp_ID").val(), name: $("#cp_name").val(), link: $("#cp_link").val(), img: $("#image").val(), group_id: document.getElementById("cp_group_id").value, group_id_list: group_id_list, group_name: $("#cp_group_id>option:selected").html(), is_free: is_free, des: $("#cp_des").val()
        }),
        success: function (rs) {
            if (rs != '0') {                
                location.reload();
            } else {
                alert(rs);
                document.getElementById("btdsavevideo").disabled = false;
            }
        }
    })
}

function confirmDelVideo(cpId, Video_email) {
    $("#cp_ID").val(cpId);
    openNotification("Bạn có chắc chắn xóa " + Video_email + " ?", "deleteVideo");
}

function deleteVideo() {
    $.ajax({
        url: url_deleteVideo, type: 'post',
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
function searchVideo() {

    window.location.href = "/Admin/YouTube?k=" + $("#keyword").val();

}