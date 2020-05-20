

$(document).ready(function () {

    $('#search-bar').keyup(function () {
        onInputKeySearchBar();
    });

    merrListMeKerkesa();

});


function Shkarko() {


    var id = this.getAttribute("id-value");

    window.location.pathname = "api/kerkesat/"+id+"/dokument";
}

function Modifiko() {

    var id = this.getAttribute("id-value");
    window.location.pathname = "/MenaxhimiKerkesave/modifiko/" + id;


}


function Fshij() {
    var id = this.getAttribute("id-value");

    var row = this.parentElement.parentElement;

    $.ajax({
        dataType: "json",
        url: window.location.origin +"/api/kerkesat/" + id,
        type: "DELETE",
        success: function (dt) {
            console.log(dt);
            if (dt["MeSukses"]) {

                row.parentNode.removeChild(row);
            }
        }
    });


}





function merrListMeKerkesa() {

    let numriFaqes = $("#show_more").attr("numriFaqes-value");
    if (numriFaqes == null)
        numriFaqes = 1;

    console.log("merr list ....... kerkesa ");

    let url = window.location.origin + "/api/kerkesat/faqa/" + numriFaqes;

    $.ajax({
        dataType: "json",
        type: "GET",
        url: url,
        success: function (kerkesat) {
            console.log(kerkesat);

   

            if (kerkesat.length < 1) {
                $("#show_more").prop("disabled", true);
                return;
            } else {
                $("#show_more").prop("disabled", false);
            }



            for (var i = 0; i < kerkesat.length;i++) {
                insertRow(kerkesat[i]);
            }


            numriFaqes++;
            $("#show_more").attr("numriFaqes-value", numriFaqes);   
            $("#show_more").attr('onClick', "merrListMeKerkesa()");
           
        }
    });

}




function merrKerkesatNgaSearch() {




    var filter = $('#search-bar').val();
    if (!filter) {
        $("#show_more").attr("numriFaqes-value", 1);
        merrListMeKerkesa();
        return;
    }

    let numriFaqes = $("#show_more").attr("numriFaqes-value");
    if (numriFaqes == null)
        numriFaqes = 1;


    let url = window.location.origin + "/api/kerkesat/search/" + filter + "/" + numriFaqes;


    $.ajax({
        dataType: "json",
        type: "GET",
        url: url,
        success: function (kerkesat) {


            if (kerkesat.length < 1) {
                $("#show_more").prop("disabled", true);
                return;
            } else {
                $("#show_more").prop("disabled", false);
            }

            

            for (var i = 0; i < kerkesat.length; i++) {
                insertRow(kerkesat[i]);
            }


            numriFaqes++;
            $("#show_more").attr("numriFaqes-value", numriFaqes);
            $("#show_more").attr('onClick', "merrKerkesatNgaSearch()");



        }
    });


}






function onInputKeySearchBar() {


    var filter = $('#search-bar').val();
    if (!filter) {
        deleteRows();
        $("#show_more").attr("numriFaqes-value", 1);
        merrListMeKerkesa();
        return;
    }

    let url = window.location.origin + "/api/kerkesat/search/" + filter + "/" + 1;


    $.ajax({
        dataType: "json",
        type: "GET",
        url: url,
        success: function (kerkesat) {

            deleteRows();


            if (kerkesat.length < 1) {
                $("#show_more").prop("disabled", true);
                return;
            } else {
                $("#show_more").prop("disabled", false);
            }


            for (var i = 0; i < kerkesat.length; i++) {
                insertRow(kerkesat[i]);
            }



            $("#show_more").attr("numriFaqes-value", 2);
            $("#show_more").attr('onClick', "merrKerkesatNgaSearch()");



        }
    });

}


function deleteRows() {

    var rows = document.getElementsByClassName("kerkesRow");


    var length = rows.length;

    for (var i = length - 1; i >= 0; i--) {
        rows[i].parentNode.removeChild(rows[i]);
    }


}


function insertRow(row) {

    var rowId;

    var rows = document.getElementsByClassName("row_id"); 

    if (rows.length > 0) {
        rowId = rows[rows.length - 1].innerHTML;
        rowId++;
    } else {
        rowId = 1;
    }


    var html =
        '<tr class="d-lg - table-row  kerkesRow" >' +
        '<td class="text-center row_id font-weight-bold">' + rowId + '</td>' +
        '<td class="text-capitalize" >' + row["Titulli"] + '</td>' +
        '<td>' + formtDataKerkeses(row["DataKerkeses"]) + '</td>' +
        '<td>' + formateDataRegjistrimit(row["DataRegjistrimit"]) + '</td>' +
        '<td>' + row["Status"]["Emri"] + '</td>' +
        '<td>' +
        '<button type="button" class="btn btn-outline-success shkarkoBtn" id-value="' + row["Id"] + '"   data-toggle="tooltip" data-placement="right" title="' + row["EmriDokumentit"] + '">' +
        '<i class="fa fa-download"></i>' +
        '</button>' +
        '</td>' +
        '<td>' +
        '<button type="button" class="btn btn-outline-warning modifikoBtn" id-value="' + row["Id"] + '" data-toggle="tooltip" data-placement="right" title="Modifiko kerkesen">' +
        '<i class="fa fa-edit"></i>' +
        '</button>' +
        '</td>' +
        '<td>' +
        '<button type="button" class="btn btn-outline-danger fshijBtn" id-value="' + row["Id"] + '" data-toggle="tooltip" data-placement="right" title="Fshij kerkesen">' +
        '<i class="fa fa-remove"></i>' +
        '</button>' +
        '</td>' +
        '</tr >';

    var tr = document.createElement('TR');
    tr.innerHTML = html;
    $(tr).addClass("d-lg - table-row  kerkesRow");
    document.getElementById("listaKerkesave").getElementsByTagName('tbody')[0].append(tr);

    btnState();

}








function formtDataKerkeses(dateTime) {
    return dateTime.split('T')[0];
}

function formateDataRegjistrimit(data) {

    let current_datetime = new Date(data)
    let formatted_date = current_datetime.getFullYear() + "-" + (current_datetime.getMonth() + 1) + "-" + current_datetime.getDate() + " " + current_datetime.getHours() + ":" + current_datetime.getMinutes() + ":" + current_datetime.getSeconds()
    return formatted_date;

}







function btnState() {

    var allObj = document.getElementsByClassName("shkarkoBtn");


    if (allObj.length>0) {

        let kerkesa = allObj[allObj.length-1];
        let path = kerkesa.getAttribute("title");


        if (path != null && path != "null") {
            kerkesa.addEventListener("click", Shkarko);
        } else {
            $(kerkesa).prop('disabled', true);
            kerkesa.setAttribute("title", "Nuk keni ngarku dokument!");

        }


    }




    allObj = document.getElementsByClassName("modifikoBtn");


    if (allObj.length>0) {

        let kerkesa = allObj[allObj.length-1];
        kerkesa.addEventListener("click", Modifiko);

    }



    allObj = document.getElementsByClassName("fshijBtn");


    if (allObj.length>0) {

        let kerkesa = allObj[allObj.length-1];
        kerkesa.addEventListener("click", Fshij);

    }

}