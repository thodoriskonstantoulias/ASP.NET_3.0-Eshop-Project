﻿var dataTable;

$(document).ready(function () {
	var url = window.location.search;
	if (url.includes("approved")) {
		loadDataTable("GetAllApprovedOrders");
	}
	else if (url.includes("all")) {
		loadDataTable("GetAllOrders");
	}
	else {
		loadDataTable("GetAllPendingOrders");
	}
	
});

function loadDataTable(url) {
	dataTable = $('#tblData').DataTable({
		"ajax": {
			"url": "/admin/order/" + url,
			"type": "GET",
			"datatype": "json"
		},
		"columns": [
			{ "data": "name", "width": "20%" },
			{ "data": "phone", "width": "20%" },
			{ "data": "email", "width": "15%" },
			{ "data": "serviceCount", "width": "15%" },
			{ "data": "status", "width": "15%" },
			{
				"data": "id",
				"render": function (data) {
					return `
                <div class="text-center">
                    <a href="/admin/order/Details/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                        <i class="fas fa-edit"></i> Details
                    </a>
                </div>
                `;
				}, "width": "15%"
			}
		],
		"language": {
			"emptyTable": "No records found."
		},
		"width": "100%"
	})
}


