

$(document).ready(function () {
    console.log("ready!");
    if (isModifiko()) {
        inicializoKerkesen();
    }
});





function saveKerkesat(){


    if(isModifiko()) {
        
        modifikoKerkesen();
        console.log("modifiko");
    } else {
        KrijoKerkesen();
        console.log("krijo");
    }


}


$(function () {

    $("#StatusId").change(function () {
        var id = $(this).val();

        if (id == 3) {
            $("#DataPerfundimit").prop('disabled', false);
        } else {
            $("#DataPerfundimit").prop('disabled', true);
            $("#DataPerfundimit").val(null);
        }
    });
});



$(function () {
    $("#Dokumenti").on('change', function () {
        var filePath = $(this).val();
        var pieces = filePath.split('\\');
        var filename = pieces[pieces.length - 1];
        $("#labelDokument").text(filename);
    });
});


function isModifiko() {
    var pathname = window.location.pathname.split('/');

  
    if (pathname[2] == "modifiko" && pathname[3] == parseInt(pathname[3], 10)) {  
        return true;
    } else {
        return false;
    }


}













