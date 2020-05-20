





function inicializoKerkesen() {

    var id = window.location.pathname.split('/')[3];


    $.ajax({
        dataType: "json",
        url: window.location.pathname.split('/')[0] +"/api/kerkesat/" + id,
        type: "GET",
        data: {},
        success: function (kerkes) {
            console.log(kerkes);
            perfundoInicializimin(kerkes);
        }
    });



    function perfundoInicializimin(kerkes) {

        var statusId = kerkes["StatusId"];


        console.log(statusId);
        $("#Titulli").val(kerkes["Titulli"]);
        $("#Pershkrimi").val(kerkes["Pershkrimi"]);
        $("#StatusId").val(statusId);
        $("#DataKerkeses").val(kerkes["DataKerkeses"].split('T')[0]);
        if (kerkes["EmriDokumentit"])
        $("#labelDokument").text(kerkes["EmriDokumentit"]);

        if (statusId == 3) {
            $("#DataPerfundimit").val(kerkes["DataPerfundimit"].split('T')[0]);
            $("#DataPerfundimit").prop("disabled", false);
        }


    }


}




function modifikoKerkesen(id) {

    var id = window.location.pathname.split('/')[3];
    console.log("id: "+id);
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

    formData.append("Id", id);
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
        type: 'PUT',
        success: function (dt) {

            // pergjigja e serverit
            console.log(dt);
            kerkesResponse(dt);

        }


    });// mbaron ajax

}






function kerkesResponse(dt) {

    var input = document.getElementsByClassName("input");

    for (var i = 0; i < input.length; i++) {
        $(input[i]).removeClass("is-invalid");
    }


    if (dt["MeSukses"]) {


        $("#success").text("Kerkesa u ndryshua me sukses!");
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