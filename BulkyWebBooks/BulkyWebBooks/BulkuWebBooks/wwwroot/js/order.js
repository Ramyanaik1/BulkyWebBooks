var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("inprocess")) {
        loadOrderTable("inprocess");
    } else if (url.includes("completed")) {
        loadOrderTable("completed");
    } else if (url.includes("pending")) {
        loadOrderTable("pending");
    } else if (url.includes("approved")) {
        loadOrderTable("approved");
    } else {
        loadOrderTable("all");
    }
});

function loadOrderTable(status) {
    dataTable = $('#tblorderData').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + status },
        "columns": [
            { data: 'orderHeaderId', "width": "5%" },   // or 'id' if API returns that
            { data: 'name', "width": "20%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'applicationUser.email', "width": "20%" }, // adjust if flattened
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'orderHeaderId', // or 'id'
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/admin/order/details?orderId=${data}" 
                               class="btn btn-primary mx-2"> 
                               <i class="bi bi-pencil-square"></i>
                            </a>               
                        </div>`;
                },
                "width": "10%"
            }
        ]
    });
}

