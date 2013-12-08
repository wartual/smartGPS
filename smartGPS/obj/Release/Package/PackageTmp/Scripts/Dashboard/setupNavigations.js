$(document).ready(function () {

    var departureSet;
    var destinationSet;

    hideDivs();

    refreshLabels();
    $("#DestinationType").val(0);
    $("#DepartureType").val(0);

    $("#DestinationType").on('change', function () {
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

    $("#DepartureType").on('change', function () {
        if (this.value == 1) {
            $("#startAddress").show();
            $("#startLatitude").hide();
            $("#startLongitude").hide();
            $("#find-departure").show();
            $("#departure").hide();
            $("#DepartureDropdown").hide();
        }
        else if (this.value == 2) {
            $("#startAddress").hide();
            $("#startLatitude").show();
            $("#startLongitude").show();
            $("#find-departure").show();
            $("#departure").hide();
            $("#DepartureDropdown").hide();
        }
    });

    function hideDivs() {
        $("#startAddress").hide();
        $("#startLatitude").hide();
        $("#startLongitude").hide();
        $("#NavigationType").hide;
        $("#find-departure").hide();
        $("#find-destination").hide();
        $("#address").hide();
        $("#latitude").hide();
        $("#longitude").hide();
        $("#destination").hide();
        $("#DestinationDropdown").hide();
        $("#departure").hide();
        $("#DepartureDropdown").hide();

        // replace start latitude&longitude comma with dot
        var stLatitude = $("#StartLatitude").val().replace(",", ".");
        var stLongitude = $("#StartLongitude").val().replace(",", ".");
        $("#StartLatitude").val(stLatitude);
        $("#StartLongitude").val(stLongitude);
    }

    function refreshLabels() {
        if ($("#DestinationType").value == 1) {
            $("#address").show();
            $("#latitude").hide();
            $("#longitude").hide();
            $("#find-destination").show();

        }
        else if ($("#DestinationType").value == 2) {
            $("#address").hide();
            $("#latitude").show();
            $("#longitude").show();
            $("#find-destination").show();
        }
    }

    $("#submitButton").click(function (event) {
        
        if (!departureSet || !destinationSet) {
            event.preventDefault();

            alert("Please select departure and destination!");
        }
    });

    $("#find-destination").click(function (event) {
        event.preventDefault();
        if ($("#DestinationType").val() == 1) {

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

    $("#find-departure").click(function (event) {
        event.preventDefault();
        if ($("#DepartureType").val() == 1) {

            if ($.trim($("#StartAddress").val()).length == 0) {
                alert("Departure could not be obtained!");
            }
            else {
                var input = $("#StartAddress").val();
                $.ajax({
                    url: ("Travel/GetDestinationsByAddress"),
                    type: ("GET"),
                    data: { address: input },
                    success: function (data) {
                        refreshDepartureFromAddress(data);
                    }
                });
            }
        }
        else {
            if ($("#StartLongitude").val() < -90 || $("#StartLongitude").val() > 90 || $("#StartLatitude").val() < -90 || $("#StartLatitude").val() > 90)
                alert("Departue could not be obtained!");
            else {
                var inputLatitude = $("#StartLatitude").val();
                var inputLongitude = $("#StartLongitude").val();
                $.ajax({
                    url: ("Travel/GetAddressByGPSCoordinates"),
                    type: ("GET"),
                    data: { latitude: inputLatitude, longitude: inputLongitude },
                    success: function (data) {
                        refreshDepartureFromGPSCoordinates(data);
                    }
                });
            }
        }
    });

    function refreshDestinationFromGPSCoordinates(data) {
        var address = data[0].Address;
        $("#destination input").val(address);
        $("#destination").show();

        destinationSet = true;
    }

    function refreshDestinationFromAddress(data) {
        $("#DestinationDropdown").find("option").remove();
        var optionsAsString = "";
        for (var i = 0; i < data.length; i++) {
            optionsAsString += "<option value='" + data[i].Value + "'>" + data[i].Address + "</option>";
        }

        $('select[name="DestinationDropdown"]').append(optionsAsString);
        $("#DestinationDropdown").show();

        destinationSet = true;
    }

    function refreshDepartureFromGPSCoordinates(data) {
        var address = data[0].Address;
        $("#departure input").val(address);
        $("#departure").show();
        departureSet = true;
    }

    function refreshDepartureFromAddress(data) {
        $("#DepartureDropdown").find("option").remove();
        var optionsAsString = "";
        for (var i = 0; i < data.length; i++) {
            optionsAsString += "<option value='" + data[i].Value + "'>" + data[i].Address + "</option>";
        }

        $('select[name="DepartureDropdown"]').append(optionsAsString);
        $("#DepartureDropdown").show();
        departureSet = true;
    }
});