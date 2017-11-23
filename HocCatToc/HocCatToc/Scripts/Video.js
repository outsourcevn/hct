function openVideo(id, name, link, img,date,des) {
    $("#cp_ID").val(id);
    $("#cp_name").val(name);
    $("#cp_link").val(link);
    $("#cp_img").val(img);
    $("#cp_date").val(date);
    $("#cp_des").val(des);
    $("#VideoDialog").show();
}

function saveVideo() {

    if ($("#cp_name").val() == "") {
        alert("Nhập tên!");
        return;
    }
    if ($("#cp_link").val() == "") {
        alert("Nhập link!");
        return;
    }
    if ($("#cp_img").val() == "") {
        alert("Nhập ảnh!");
        return;
    }
    if ($("#cp_date").val() == "") {
        alert("Nhập ngày tháng!");
        return;
    }
    $.ajax({
        url: url_addUpdateVideo, type: 'post',
        contentType: 'application/json',
        data: JSON.stringify({
            ID: $("#cp_ID").val(), name: $("#cp_name").val(), link: $("#cp_link").val(), img: $("#cp_img").val(), date: $("#cp_date").val(), des: $("#cp_des").val()
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

    window.location.href = "/Admin/Video?k=" + $("#keyword").val();

}