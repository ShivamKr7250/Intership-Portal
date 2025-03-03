var dataTable;

$(document).ready(function () {
    loadDataTable();

    // Filter button event listener
    $('#applyFilters').on('click', function () {
        loadDataTable();
    });
});

function loadDataTable() {
    // Get filter values
    var year = $('#filterYear').val();
    var placed = $('#filterPlaced').val();
    var section = $('#filterSection').val();
    var batch = $('#filterBatch').val();
    var skills = $('#filterSkills').val();

    // Destroy existing table if already initialized
    if ($.fn.DataTable.isDataTable("#tblBookings")) {
        $('#tblBookings').DataTable().destroy();
    }

    // Initialize DataTable
    dataTable = $('#tblBookings').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: `/studentData/getall?year=${year}&placed=${placed}&section=${section}&batch=${batch}&skills=${skills}`,
            error: function (xhr, error, thrown) {
                console.error('Error loading data: ', error);
                toastr.error('Failed to load data.');
            }
        },
        "columns": [
            { data: 'name', "width": "10%" },
            { data: 'email', "width": "10%" },
            { data: 'rollNumber', "width": "15%" },
            { data: 'year', "width": "5%" },
            { data: 'section', "width": "5%" },
            { data: 'batch', "width": "10%" },
            { data: 'skills', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/student/registrationUpdate?registrationId=${data}" class="btn btn-outline-warning mx-2">
                            <i class="bi bi-pencil-square"></i> Details
                        </a>
                        <a onClick=Delete('/student/delete/${data}') class="btn btn-outline-danger mx-2">
                            <i class="bi bi-trash-fill"></i> Delete
                        </a>
                    </div>`;
                },
                "width": "5%"
            }
        ],
        "language": {
            "emptyTable": "No Registrations available"
        }
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                },
                error: function (xhr, error, thrown) {
                    console.error('Error deleting record: ', error);
                    toastr.error('Failed to delete record.');
                }
            });
        }
    });
}
