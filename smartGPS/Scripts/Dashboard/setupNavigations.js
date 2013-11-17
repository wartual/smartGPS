$(document).ready(function () {

    hideDivs();

    $("#TypesOfNavigation").on('change', function () {
        if (this.value == 1) {
            $("#address").show();
            $("#latitude").hide();
            $("#longitude").hide();
            $("#find-destination").show();
        }
        else if (this.value == 2) {
            $("#address").hide();
            $("#latitude").show();
            $("#longitude").show();
            $("#find-destination").show();
        }
    });

    function hideDivs() {
        $("#find-destination").hide();
        $("#address").hide();
        $("#latitude").hide();
        $("#longitude").hide();
    }


    $("#find-destination").click(function () {
        if ($("#TypesOfNavigation").val() == 1) {
            alert("SSS");
        }
        else {
            if ($("#Longitude").val() < -90 || $("#Longitude").val() > 90 || $("#Latitude").val() < -90 || $("#Latitude").val() > 90)
                alert("Destination could not be obtained!");
            else {
                var inputLatitude = $("#Latitude").val();
                var inputLongitude = $("#Longitude").val();
                $.ajax({
                    url: ("Travel/GetAddressByGPSCoordinates"),
                    type: ("GET"),
                    data: { latitude: inputLatitude, longitude: inputLongitude },
                    success: function (data) {
                        console.log(data);
                    }
                });
            }
        }
    });

});