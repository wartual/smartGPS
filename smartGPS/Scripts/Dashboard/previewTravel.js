$(document).ready(function () {

    $("#spinner").bind("ajaxSend", function() {
        $(this).show();
    }).bind("ajaxStop", function() {
        $(this).hide();
    }).bind("ajaxError", function() {
        $(this).hide();
    });
 

    $("#Mode").on('change', function () {
        if (this.value == 1) {
            getDirections(1);
        }
        else if (this.value == 2) {
            getDirections(2);
        }
    });

    function getDirections(mode) {
        var modeText;
        if (mode == 1) {
            modeText = "driving";
        }
        else {
            modeText = "walking";
        }

        var endLatitude = $("#endLatitude").val();
        var endLongitude =  $("#endLongitude").val()

        $.ajax({
            url: ("GetDirections"),
            type: ("GET"),
            data: { latitude: endLatitude, longitude:endLongitude , mode: modeText },
            success: function (data) {
                refreshPreviewData(data);
            }
        });
    }

    function refreshPreviewData(data) {
        $("#StartAddress").val(data.StartAddress);
        $("#EndAddress").val(data.EndAddress);
        $("#DistanceText").val(data.DistanceText);
        $("#DurationText").val(data.DurationText);

        $("#Mode").val(data.Mode);
    }
});