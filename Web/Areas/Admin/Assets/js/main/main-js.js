var leftime = 30;
var xhr;
function ajax() {
    leftime--;
    if (leftime == 1) {
        xhr = $.ajax({
            type: "POST",
            url: "/Admin/Home/TestAutoSend",
            success: function (res) {
                if (res.success) {
                    console.log(res);
                    //Warning Message
                    swal({
                        title: "Thông báo",
                        text: "Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại",
                        type: "warning",
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Đăng nhập lại",
                    }, function () {
                        window.location.replace("/admin/dang-nhap");
                    });
                } else {
                    console.log(res);
                }
            }
        })
    } else {

    }
}
if (leftime >= 1) {
    setInterval(ajax, 1000 * 60);
}