$(document).ready(function () {

    hideDivs();

    refreshLabels();
    $("#NavigationType").val(0);

    $("#NavigationType").on('change', function () {
        if (this.value == 1) {
            $("#address").show();
            $("#latitude").hide();
            $("#longitude").hide();
            $("#find-destination").show();
            $("#destination").hide();
            $("#DestinationDropdown").hide();
        }
        else if (this.value == 2) {
            $("#address").hide();
            $("#latitude").show();
            $("#longitude").show();
            $("#find-destination").show();
            $("#destination").hide();
            $("#DestinationDropdown").hide();
        }
    });

    function hideDivs() {
        $("#find-destination").hide();
        $("#address").hide();
        $("#latitude").hide();
        $("#longitude").hide();
        $("#destination").hide();
        $("#DestinationDropdown").hide();
    }

    function refreshLabels() {
        if ($("#NavigationType").value == 1) {
            $("#address").show();
            $("#latitude").hide();
            $("#longitude").hide();
            $("#find-destination").show();

        }
        else if ($("#NavigationType").value == 2) {
            $("#address").hide();
            $("#latitude").show();
            $("#longitude").show();
            $("#find-destination").show();
        }
    }



    $("#find-destination").click(function (event) {
        event.preventDefault();
        if ($("#NavigationType").val() == 1) {

            if ($.trim($("#Address").val()).length == 0) {
                alert("Destination could not be obtained!");
            }
            else {
                var input = $("#Address").val();
                $.ajax({
                    url: ("Travel/GetDestinationsByAddress"),
                    type: ("GET"),
                    data: { address: input },
                    success: function (data) {
                        refreshDestinationFromAddress(data);
                    }
                });
            }
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
                        refreshDestinationFromGPSCoordinates(data);
                    }
                });
            }
        }
    });

    function refreshDestinationFromGPSCoordinates(data) {
        var address = data[0].Address;
        $("#destination input").val(address);
        $("#destination").show();
    }

    function refreshDestinationFromAddress(data) {
        $("#DestinationDropdown").find("option").remove();
        var optionsAsString = "";
        for (var i = 0; i < data.length; i++) {
            optionsAsString += "<option value='" + data[i].Value + "'>" + data[i].Address + "</option>";
        }

        $('select[name="DestinationDropdown"]').append(optionsAsString);
        $("#DestinationDropdown").show();
    }
});