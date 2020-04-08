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

function Delete(url) {
	swal({
		title: "Are you sure you want to delete",
		text: "You will permanently delete this",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "Yes delete it",
		closeOnConfirm: true
	}, function () {
			$.ajax({
				url: url,
				type: "DELETE",
				success: function (data) {
					if (data.success) {
						toastr.success(data.message);
						dataTable.ajax.reload();
					}
					else {
						toastr.error(data.message);
					}
				}
			});
	});
}


