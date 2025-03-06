dataTable = $('#tblBookings').DataTable({
    "processing": true,
    "serverSide": true,
    "ajax": {
        url: "/studentData/getall",
        type: "GET",
        data: function (d) {
            d.year = $('#filterYear').val();
            d.placed = $('#filterPlaced').val();
            d.section = $('#filterSection').val();
            d.batch = $('#filterBatch').val();
            d.skills = $('#filterSkills').val();
        },
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
