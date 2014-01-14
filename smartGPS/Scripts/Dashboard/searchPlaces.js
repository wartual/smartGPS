$(document).ready(function () {

    $("#SearchType").val(0);
    hideDivs();
    handleSearchType();

    function handleSearchType() {
        $("#SearchType").on('change', function () {
            if (this.value == 0) {
                hideDivs();
            }
            if (this.value == 1) {
                $("#place").show();
                $("#latitude").hide();
                $("#longitude").hide();
                $("#find").show();
            }
            else if (this.value == 2) {
                $("#place").hide();
                $("#latitude").show();
                $("#longitude").show();
                $("#find").show();
            }
        });
    }

    function hideDivs() {
        $("#place").hide();
        $("#latitude").hide();
        $("#longitude").hide();
        $("#find").hide();
    }

});