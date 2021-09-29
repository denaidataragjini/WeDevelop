$(document).ready(function () {
    $("#buton").click(function() {
        var emriSherbimit = $("#emri").val();
        if (emriSherbimit == null || emriSherbimit == "")
        { alert("Ju nuk keni vendosur asnje te dhene");}
        else {
            $.ajax({
                method: "POST",
                url: "/Home/Search?search=" + emriSherbimit,
                dataType: "json",
                success: function (data) {
                    console.log(data);
                    if (data.length == 0) {
                        
                        alert("Sherbimi qe ju kerkoni nuk u gjend");
                    }
                    else {
                        $("#row2").empty();
                        for (var i = 0; i < data.length; i++) {
                            $("#row2").append("<div class='col-md-3'> <div class='card' style= 'width: 27rem;'><div class='inner'>"
                                + "<a href='@Url.Action('DetajeSherbimesh', 'Home', new { id =" + data[i].Id + "})'>"
                                + "<img src='../Content/Sherbimet/" + data[i].Imazh + "' width='270' height='200' class='card - img - top'></a >"
                                + "</div> <div class='card - body'>"
                                + "<p class='card-text' style = 'color:black;font-size:21px;'> $" + data[i].Cmimi + "All &nbsp; &nbsp; •" + data[i].Ditet + " dite </p >"
                                + "<h5 class='card-title' style='color:black;font-size:21px;'>" + data[i].Emri + "</h5>"
                                + "</div></div></div>");       

                        }
                    }
                }
            });
        }
    });
});