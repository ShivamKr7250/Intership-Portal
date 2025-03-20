var dataTable;

$(document).ready(function () {
    dataTable = $('#mentorStudentTable').DataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            url: "/mentorAllocation/getall",
            type: "GET",
            data: function (d) {
                d.year = $('#filterYear').val() ? parseInt($('#filterYear').val()) : null;
                d.batch = $('#filterBatch').val() ? parseInt($('#filterBatch').val()) : null;
            },
            error: function (xhr, error, thrown) {
                console.error('Error loading mentor data: ', error);
                toastr.error('Failed to load mentor allocations.');
            }
        },
        "columns": [
            { data: 'mentorName', "width": "25%" },
            { data: 'mentorEmail', "width": "25%" },
            { data: 'years', "width": "10%" },
            { data: 'batches', "width": "10%" },
            { data: 'sections', "width": "10%"},
            {
                data: 'totalAllocatedStudents',
                "render": function (data) {
                    return `<span class="badge bg-success">${data}</span>`;
                },
                "width": "10%"
            },
            {
                data: 'mentorId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/mentorAllocation/upsert?userId=${data}" class="btn btn-outline-warning mx-2">
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
            "emptyTable": "No mentor allocations available."
        }
    });

    // Apply Filters when button is clicked
    $('#applyFilters').click(function () {
        console.log("Filters applied, reloading mentor table...");
        dataTable.ajax.reload();
    });
});
