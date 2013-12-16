$(document).ready(function () {
    if ($.trim($("#successMessage").text()).length == 0) {
        $("#successMessage").hide();
    }
    else {
        $("#successMessage").show();
    }

    $("#settings-dropdown").click(function (event) {
        var className = $("#settings-dropdown").attr('class');

        if(className == "dropdown"){
            $("#settings-dropdown").attr('class', 'dropdown open');
        }
        else {
            $("#settings-dropdown").attr('class', 'dropdown');
        }

        var value = $("#settings-dropdown")
    });
});