$(document).ready(function () {

    // global variables
    var startLatitude = $("#startLatitude").val().replace(",", ".");
    var startLongitude = $("#startLongitude").val().replace(",", ".");

    getDataFromGooglePlaces();
   
    function getDataFromGooglePlaces()
    {
        $.ajax({
            url: ("GetPlaces"),
            type: ("GET"),
            data: { latitude: startLatitude, longitude: startLongitude },
            success: function (data) {
                refreshList(data);
            }
        });
    }

    function refreshList(data) {
        var numberOfPages = Math.ceil(data.Results.length / 10);
        setPageData(data, 1);
        var paginatorOptions = {
            currentPage: 1,
            totalPages: numberOfPages,
            onPageChanged: function (e, oldPage, newPage) {
                setPageData(data, newPage);
            }
        }

        $('#paginator').bootstrapPaginator(paginatorOptions);
    }

    function setPageData(data, currentPage) {
        var start, end;
        if (currentPage == 1) {
            start = 0;
        }
        else {
            start = 10 * (currentPage-1);
        }

        end = start + 10;


        var row = "";
        var types;
        $("#placesTable tbody").empty();

        for (var i = start; i < end; i++) {
            if (i > data.Results.length -1) {
                break;
            }
            else {
                row = row + "<tr><td>" + (i + 1) + "</td><td>" + data.Results[i].Name + "</td><td>";
                if (data.Results[i].Vicinity != null) {
                    row = row + data.Results[i].Vicinity + "</td><td>";
                }
                else {
                    row = row + "</td><td>";
                }


                for (var j = 0; j < data.Results[i].Types.length; j++) {
                    row = row + capitaliseFirstLetter(data.Results[i].Types[j]);
                    if (j != (data.Results[i].Types.length - 1)) {
                        row = row + ", ";
                    }
                }
                row = row + "</td></tr>";
            }
        }

        $('#placesTable > tbody:last').append(row);

    }

    function capitaliseFirstLetter(string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    }
});