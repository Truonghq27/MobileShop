$(function () {
    $(".featureImage-new").click(function (e) {
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
    //more Images
    $(".Images-new").click(function (e) {
        e.preventDefault();
        var filder = new CKFinder();
        filder.selectActionFunction = function (url, file, files) {
            var urls = "";
            var imgs = "";
            $.each(files, function () {
                urls += $(this)[0].url + ";";
                imgs += "<img src='" + $(this)[0].url + " ' style='max-with:200px;max-height:150px;padding:5px' />";
            })
            //cắt dấu ; cuối cùng

            urls = urls.slice(0, length - 3);
            $("#Images").val(urls);
            $(".icon-images-url-input").html('<i class="glyphicon glyphicon-file"></i>');
            console.log(urls.length);
            if (urls.length >= 100) {
                var sliceUrls = urls.slice(0, 100);
                $(".fileinput-images-filename").text(sliceUrls + "...");
            } else if (urls.length <= 100) {
                $(".fileinput-images-filename").text(urls);
            }
            $(".fileinput-images-new").html("Thay đổi");
            $(".remove-url-images").css({ "display": "" });
            $(".show-ckfilder-Images").html(imgs);
        }
        filder.popup();
    });
    $(".remove-url-images").on("click", function (e) {
        e.preventDefault();
        $(".icon-images-url-input").html('');
        $("#Images").val("");
        $(".fileinput-images-filename").text("");
        $(".fileinput-images-new").html("Chọn ảnh");
        $(".remove-url-images").css({ "display": "none" });
        $(".show-ckfilder-Images").html("");
    });

    //ProductAttr
    $(".Product_Attr").click(function () {
        var index = 0;
        $(".Product_Attr").each(function () {
            if ($(this).is(":checked")) {
                $(this).attr("name", "ProductAttrs[" + index + "].AttrId");
                index++;
            } else {
                $(this).attr("name", "");
            }
        })
    })
});

jQuery(document).ready(function () {
    // Switchery
    var vspinTrue = $(".vertical-spin").TouchSpin({
        verticalbuttons: true
    });
    if (vspinTrue) {
        $('.vertical-spin').prev('.bootstrap-touchspin-prefix').remove();
    }
    $("input[name='Discount']").TouchSpin({
        min: 0,
        max: 100,
        step: 1,
        decimals: 0,
        boostat: 5,
        maxboostedstep: 10,
        postfix: '%'
    });
    $("input[name='PriceIn']").TouchSpin({
        min: 0,
        max: 1000000000000000,
        stepinterval: 50,
        maxboostedstep: 10000000,
        postfix: '₫'
    });
    $("input[name='PriceOut']").TouchSpin({
        min: 0,
        max: 1000000000000000,
        stepinterval: 50,
        maxboostedstep: 10000000,
        postfix: '₫'
    });

});