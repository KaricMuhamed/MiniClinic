$(document).ready(function () {
    var apiToken = window.appConfig.apiToken;
    var userGender = window.appConfig.gender;
    var userYearOfBirth = window.appConfig.yearOfBirth;
    var apiBaseUrl = 'https://sandbox-healthservice.priaid.ch/';

    $.ajax({
        url: apiBaseUrl + 'body/locations',
        type: 'GET',
        data: {
            token: apiToken,
            format: 'json',
            language: 'en-gb'
        },
        success: function (data) {
            var bodyLocationSelect = $('#bodyLocation');
            for (var i = 0; i < data.length; i++) {
                bodyLocationSelect.append($('<option>', {
                    value: data[i].ID,
                    text: data[i].Name
                }));
            }
        },
        error: function (err) {
            console.log("Error fetching body locations", err);
        }
    });

    $('#bodyLocation').change(function () {
        var bodyLocationId = $(this).val();
        var subLocationSelect = $('#subLocation');
        subLocationSelect.empty().append('<option value="">-- Select Sub-Location --</option>');
        $('#symptomsContainer').empty();

        if (bodyLocationId) {
            $.ajax({
                url: apiBaseUrl + 'body/locations/' + bodyLocationId,
                type: 'GET',
                data: {
                    token: apiToken,
                    format: 'json',
                    language: 'en-gb'
                },
                success: function (data) {
                    for (var i = 0; i < data.length; i++) {
                        subLocationSelect.append($('<option>', {
                            value: data[i].ID,
                            text: data[i].Name
                        }));
                    }
                },
                error: function (err) {
                    console.log("Error fetching sub-locations", err);
                }
            });
        }
    });

    $('#subLocation').change(function () {
        var subLocationId = $(this).val();
        var symptomsContainer = $('#symptomsContainer');
        symptomsContainer.empty();

        if (subLocationId) {
            $.ajax({
                url: apiBaseUrl + 'symptoms/' + subLocationId + '/0',
                type: 'GET',
                data: {
                    token: apiToken,
                    format: 'json',
                    language: 'en-gb'
                },
                success: function (data) {
                    symptomsContainer.empty();

                    if (!data || data.length === 0) {
                        symptomsContainer.append('<p style="color: red">No data retrieved!</p>');
                        return;
                    }

                    for (var i = 0; i < data.length; i++) {
                        var checkbox = $('<input>', {
                            type: 'checkbox',
                            value: data[i].ID,
                            id: 'symptom_' + data[i].ID
                        });
                        var div = $('<div>').append(checkbox).append(' ' + data[i].Name);
                        symptomsContainer.append(div);
                    }
                },
                error: function (err) {
                    console.log("Error fetching symptoms", err);
                }
            });
        }
    });

        $('#getDiagnosis').click(function () {
        var selectedSymptoms = $('#symptomsContainer input[type=checkbox]:checked')
            .map(function () {
                return parseInt(this.value, 10);
            })
            .get();

        if (selectedSymptoms.length === 0) {
            alert('Please select at least one symptom.');
            return;
        }

        $.ajax({
            url: apiBaseUrl + 'diagnosis',
            type: 'GET',
            data: {
                token: apiToken,
                format: 'json',
                language: 'en-gb',
                symptoms: JSON.stringify(selectedSymptoms),
                gender: userGender,
                year_of_birth: userYearOfBirth
            },
            success: function (data) {
                var diagnosisTableBody = $('#diagnosisTableBody');
                diagnosisTableBody.empty();
                for (var i = 0; i < data.length; i++) {
                    var row = $('<tr>');
                    var radio = $('<input>', {
                        type: 'radio',
                        name: 'selectedDiagnosis',
                        value: data[i].Issue.ID
                    });
                    row.append($('<td>').append(radio));
                    row.append($('<td>').text(data[i].Issue.Name));
                    row.append($('<td>').text(data[i].Issue.ProfName));
                    row.append($('<td>').text(data[i].Issue.IcdName));
                    row.append($('<td>').text(data[i].Issue.Icd));
                    row.append($('<td>').text(data[i].Issue.Accuracy)); 
                    diagnosisTableBody.append(row);
                }
                $('#diagnosisResults').show();
            },
            error: function (err) {
                console.log("Error fetching diagnosis", err);
            }
        });
    });

    $(document).on('change', 'input[name="selectedDiagnosis"]', function () {
        var selectedRow = $(this).closest("tr");

        $('#MedicalRecordName').val(selectedRow.find("td:eq(1)").text());
        $('#MedicalRecordDescription').val(selectedRow.find("td:eq(2)").text());
        $('#MedicalRecordIdcDescription').val(selectedRow.find("td:eq(3)").text());
        $('#MedicalRecordIdcCode').val(selectedRow.find("td:eq(4)").text());
    });


});
