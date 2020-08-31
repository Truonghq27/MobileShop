//for edit and create
$(function () {
    $(".featureImage-news").click(function (e) {
        e.preventDefault();
        var filder = new CKFinder();
        filder.selectActionFunction = function (url, file, files) {
            $(".icon-feature-url-input").html('<i class="glyphicon glyphicon-file"></i>');
            $("#FeatureImage").val(url);
            var reUrl = url.slice(0, 100) + "...";
            if (url.length >= 100) {
                $(".fileinput-freature-filename").text(reUrl);
            } else {
                $(".fileinput-freature-filename").text(url);
            } 
            $(".fileinput-feature-new").html("Thay đổi");
            $(".remove-url-feature").css({ "display": "" });
            $(".show-ckfilder-FeatureImage").html("<img src='" + url + " ' style='max-with:200px;max-height:150px' />")
        }
        filder.popup();
    });
    $(".remove-url-feature").on("click", function (e) {
        e.preventDefault();
        $(".icon-feature-url-input").html('');
        $("#FeatureImage").val("");
        $(".fileinput-freature-filename").text("");
        $(".fileinput-feature-new").html("Chọn ảnh");
        $(".remove-url-feature").css({ "display": "none" });
        $(".show-ckfilder-FeatureImage").html("");
    });
});