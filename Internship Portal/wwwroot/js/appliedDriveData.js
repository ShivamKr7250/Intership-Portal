var dataTable; // Declare globally

$(document).ready(function () {
    dataTable = $('#tblAppliedDrive').DataTable({  // Initialize it inside document ready
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: "/appliedDrive/getall",
            type: "GET",
            data: function (d) {
                d.date = $('#filterDate').val();
                d.company = $('#filterCompany').val();
                d.course = $('#filterCourse').val() || null;
                d.batch = $('#filterBatch').val() ? parseInt($('#filterBatch').val()) : null;
                d.rollNumber = $('#filterStudent').val() || null;
            },
            error: function (xhr, error, thrown) {
                console.error('Error loading data: ', error);
                toastr.error('Failed to load data.');
            }
        },
        "columns": [
            { data: 'blogPost.title', "width": "10%" },
            { data: 'student.name', "width": "10%" },
            { data: 'student.email', "width": "10%" },
            { data: 'student.rollNumber', "width": "15%" },
            { data: 'student.year', "width": "5%" },
            { data: 'student.batch', "width": "10%" },
            { data: 'appliedOn', "width": "15%" },
            {
                data: 'userId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/studentData/studentDataRegistration?userId=${data}" class="btn btn-outline-warning mx-2">
                            <i class="bi bi-pencil-square"></i> Details
                        </a>
                        <a onClick="Delete('/studentData/delete/${data}')" class="btn btn-outline-danger mx-2">
                            <i class="bi bi-trash-fill"></i> Delete
                        </a>
                    </div>`;
                },
                "width": "5%"
            }
        ],
        "language": {
            "emptyTable": "No Data available"
        }
    });

    // ✅ Apply Filters on Button Click
    $('#applyFilters').click(function () {
        console.log("Apply button clicked! Reloading table...");
        dataTable.ajax.reload(); // Refresh DataTable with new filter values
    });
});

// ✅ Delete Function with Global Access to dataTable
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
                    dataTable.ajax.reload(); // ✅ Now accessible globally
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
