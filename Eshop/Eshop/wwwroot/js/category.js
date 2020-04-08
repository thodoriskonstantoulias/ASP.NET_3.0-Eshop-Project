var dataTable;

$(document).ready(function () {
	loadDataTable();
});

function loadDataTable() {
	dataTable = $('#tblData').DataTable({
		"ajax": {
			"url": "/admin/category/GetAllCategories",
			"type": "GET",
			"datatype": "json"
		},
		"columns": [
			{ "data": "name", "width": "50%" }, 
			{ "data": "displayOrder", "width": "20%" },
			{
				"data": "id",
				"render": function (data) {
					return `
                <div class="text-center">
                    <a href="/admin/category/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                        <i class="fas fa-edit"></i> Edit
                    </a>
                    &nbsp;
                    <a onclick= Delete("/admin/category/DeleteCategory/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                        <i class="fas fa-trash-alt"></i> Delete
                    </a>
                </div>
                `;
				}, "width": "30%"
			}
		],
		"language": {
			"emptyTable": "No records found."
		},
		"width": "100%"
	})
}
