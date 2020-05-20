




function KrijoKerkesen() {


    console.log(" hyme te krijo krijo");

    var titulli = document.getElementById("Titulli").value;
    var pershkrimi = document.getElementById("Pershkrimi").value;
    var selector = document.getElementById("StatusId");
    var status = selector.options[selector.selectedIndex].value;
    var dataKerkeses = document.getElementById("DataKerkeses").value;
    var dataPerfundimit = document.getElementById("DataPerfundimit").value;

    var dokument = document.getElementById("Dokumenti");



    var formData = new FormData();

    if (dokument.files.length > 0) {
        var myFile = dokument.files[0];
        formData.append("Dokument", myFile);
    }

    formData.append("Id", 0);
    formData.append("StatusId", status);
    formData.append("Titulli", titulli);
    formData.append("Pershkrimi", pershkrimi);
    formData.append("DataKerkeses", dataKerkeses);
    formData.append("DataPerfundimit", dataPerfundimit);



    $.ajax({
        url: "/api/kerkesat",
        data: formData,
        processData: false,
        contentType: false,
        type: 'POST',
        success: function (dt) {

            // pergjigja e serverit
            console.log(dt);
            shtoKerkesResponse(dt);

        }


    });// mbaron ajax



}




function shtoKerkesResponse(dt) {

    var input = document.getElementsByClassName("input");

    for (var i = 0; i < input.length; i++) {
        $(input[i]).removeClass("is-invalid");
    }


    // $("input").removeClass("is-invalid");


    if (dt["MeSukses"]) {


        $("#success").text("Kerkesa u ruajt me sukses!");
        $("#Titulli").val('');
        $("#Pershkrimi").val('');
        $("#StatusId").val(0);
        $("#DataKerkeses").val(null);
        $("#DataPerfundimit").prop('disabled', true);
        $("#DataPerfundimit").val(null);
        $("#labelDokument").text(".pdf or .docx");
        $("#Dokumenti").val(null);

    } else {
        var errors = dt["Errors"];


        for (var key in errors) {
            // check if the property/key is defined in the object itself, not in parent
            if (errors.hasOwnProperty(key)) {
                $("#" + key).addClass("is-invalid");
                $("#" + key + "Msg").html(errors[key]);
            }
            $("#success").text("");
        }

    }


}
