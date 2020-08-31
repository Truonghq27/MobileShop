$(document).ready(function () {
    var datatable = $("#myTable").DataTable({
        ajax: {
            type: "GET",
            url: "News/GetAllNews",
            dataType: "JSON"
        },
        columns: [
            {
                data: null, render: function (full) {
                    if (full["NewsTitle"].length >= 50) {
                        return "<img src='" + full["FeatureImage"] + "' width='80' />" +
                            full["NewsTitle"].substring(0, 50) + "...";
                    } else {
                        return "<img src='" + full["FeatureImage"] + "' width='80' />" +
                            full["NewsTitle"]
                    }
                }
            },
            { data: "FullName" },
            {
                data: "Created", render: function (data, type, row) {
                    return moment(data).format('DD-MM-YYYY HH:mm:ss');
                }
            },
            { data: "CountView" },
            {
                data: "Status", render: function (data) {
                    if (data == 1) {
                        return "<label class='label label-success'>Hiển thị</label>";
                    } else if (data == 0) {
                        return "<label class='label label-inverse'>Tạm ẩn</label>";
                    }
                }
            },
            {
                data: "NewsId", render: function (data) {
                    return '<a href="News/Edit/' + data + '" class=" btn btn-sm btn-icon btn-pure btn-outline edit-row-btn" ><i data-toggle="tooltip" data-original-title="Chỉnh sửa" style="font-size:15px" class="ti-pencil-alt text-inverse tooltip-inverse" aria-hidden="true"></i></a>' +
                        '<button data-id="' + data + '" class="mr-0 btn btn-sm btn-icon btn-pure btn-outline edit-row-btn btn-delete-news" ><i data-toggle="tooltip" data-original-title="Xoá" style="font-size:15px" class="  ti-trash text-danger font-weight-bold tooltip-danger" aria-hidden="true"></i></button>'
                }
            },
        ],
        "oLanguage": {
            "oPaginate": {
                "sPrevious": "Trang trước",
                "sNext": "Trang sau",
                "sLast": "Trang cuối",
                "sFirst": "Trang đầu",
            },
            //search
            "sSearch": "Tìm kiếm:",
            "sLengthMenu": "Hiện thị _MENU_ số hàng",
            "sInfo": "Trang _START_ tổng _TOTAL_ (_START_ to _END_)",
            "sInfoEmpty": 'Không có gì để hiển thị',
            "sEmptyTable": "Không có dữ liệu, click vào <span style='font-weight:700'>Thêm mới</span> để thêm dữ liệu",
        },
        "order": [[2, "desc"]]
    });
    //allow tooltip active
    datatable.on('draw', function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
    //delete news
    var id = 0;
    $(document).on('click', '.btn-delete-news', function () {
        id = $(this).data("id");
        $("#myModal").modal('show');
    })
    $(document).on('click', '#confirmdelete', function () {
        if (id != null) {
            $.ajax({
                type: "POST",
                url: "News/Delete/" + id,
                dataType: "JSON",
                success: function (res) {
                    if (res.success) {
                        $('.notifyjs-wrapper').remove();
                        $.notify(res.success, {
                            globalPosition: "top center",
                            className: "success",
                        })
                        $("#myModal").modal("hide");
                        datatable.ajax.reload();
                    } else if (res.nulluser) {
                        window.location.reload();
                    } else {
                        $('.notifyjs-wrapper').remove();
                        $.notify(res.error, {
                            globalPosition: "top center",
                            className: "error",
                        })
                        datatable.ajax.reload();
                    }
                }
            })
        }
    })
})
