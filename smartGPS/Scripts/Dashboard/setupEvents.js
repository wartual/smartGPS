$(document).ready(function () {

    // global variables
    var startLatitude = $("#startLatitude").val().replace(",", ".");
    var startLongitude = $("#startLongitude").val().replace(",", ".");

    getDataFromFoursquare();

    function getDataFromFoursquare() {
        $.ajax({
            url: ("GetEvents"),
            type: ("GET"),
            data: { latitude: startLatitude, longitude: startLongitude },
            success: function (data) {
                refreshList(data);
            }
        });
    }

    function refreshList(data) {
        var numberOfPages = Math.ceil(data.Items.length / 10);
        setPageData(data, 1);
        var paginatorOptions = {
            currentPage: 1,
            totalPages: numberOfPages,
            onPageChanged: function (e, oldPage, newPage) {
                setPageData(data, newPage);
            }
        }

        $('#paginatorEvents').bootstrapPaginator(paginatorOptions);
    }

    function setPageData(data, currentPage) {
        var start, end;
        if (currentPage == 1) {
            start = 0;
        }
        else {
            start = 10 * (currentPage - 1);
        }

        end = start + 10;


        var row = "";
        var types;
        var category;

        $("#eventsTable tbody").empty();

        for (var i = start; i < end; i++) {
            if (i > data.Items.length - 1) {
                break;
            }
            else {
                row = row + "<tr><td>" + (i + 1) + "</td><td>" + data.Items[i].Venue.Name + "</td><td>" + getAddress(data.Items[i].Venue.Location) + "</td><td>";
                category = "";
                for (var j = 0; j < data.Items[i].Venue.Categories.length; j++) {
                    category = category + capitaliseFirstLetter(data.Items[i].Venue.Categories[j].Name);

                    if (j != (data.Items[i].Venue.Categories.length - 1)) {
                        category = category + ", ";
                    }
                }

                row = row + category + "</td>";

                row = row + "<td><a href=\"EventDetails?id=" + i + "&lat=" + startLatitude + "&lon=" + startLongitude + "\">Details</a></td></tr>";
            }
        }

        $('#eventsTable > tbody:last').append(row);

    }

    function getAddress(addressList){
        var address = "";

        if (addressList.Address != null)
            address = address + addressList.Address;

        if (addressList.City != null)
            address = address + ", " + addressList.City;

        if (addressList.Country != null)
            address = address + ", " + addressList.Country;

        return address;
    }

    function capitaliseFirstLetter(string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    }
});