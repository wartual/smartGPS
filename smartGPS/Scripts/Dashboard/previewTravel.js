$(document).ready(function () {

    // global variables
    var mapOptions = {
        zoom: 3,
        center: new google.maps.LatLng(0, 0),
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        panControl: true,
        panControlOptions: {
            position: google.maps.ControlPosition.RIGHT_TOP
        },
        zoomControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.LARGE,
            position: google.maps.ControlPosition.RIGHT_TOP
        }
    };

    var map = new google.maps.Map(document.getElementById('map-canvas'),
        mapOptions);

    var endLatitude = $("#endLatitude").val().replace(",", ".");
    var endLongitude = $("#endLongitude").val().replace(",", ".");
    var startLatitude = $("#startLatitude").val().replace(",", ".");
    var startLongitude = $("#startLongitude").val().replace(",", ".");
    var startAddress = $("#StartAddress").val();
    var endAddress = $("#EndAddress").val();
    var neBoundLat = $("#neBoundLat").val().replace(",", ".");
    var neBoundLng = $("#neBoundLng").val().replace(",", ".");
    var swBoundLat = $("#swBoundLat").val().replace(",", ".");
    var swBoundLng = $("#swBoundLng").val().replace(",", ".");
    var swBound = new google.maps.LatLng(swBoundLat, swBoundLng);
    var neBound = new google.maps.LatLng(neBoundLat, neBoundLng);
    var travelPath = null;


    google.maps.event.addDomListener(window, 'load', initialize);
    getFoursquareExploreVenues();

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        if (e.target.hash == "#map") {
            google.maps.event.trigger(map, 'resize');
            initialize();
        }
    })

    // Set up loading spinner
    $("#spinner").bind("ajaxSend", function () {
        $(this).show();
    }).bind("ajaxStop", function () {
        $(this).hide();
    }).bind("ajaxError", function () {
        $(this).hide();
    });

    // get directions
    $("#Mode").on('change', function () {
        if (this.value == 1) {
            getDirections(1);
        }
        else if (this.value == 2) {
            getDirections(2);
        }
    });

    function getFoursquareExploreVenues()
    {
        $.ajax({
            url: ("GetFoursquareExploreVenues"),
            type: ("GET"),
            data: { latitude: startLatitude, longitude: startLongitude},
            success: function (data) {
                
            }
        });
    }

    function getDirections(mode) {
        var modeText;
        if (mode == 1) {
            modeText = "driving";
        }
        else {
            modeText = "walking";
        }

        $.ajax({
            url: ("GetDirections"),
            type: ("GET"),
            data: { latitude: endLatitude, longitude: endLongitude, mode: modeText },
            success: function (data) {
                refreshPreviewData(data);
            }
        });
    }


    function initialize() {
        // draw path
        var polylines = $("#polylines").val();
        var path =  google.maps.geometry.encoding.decodePath(polylines);

        travelPath = new google.maps.Polyline({
            path: path,
            geodesic: false,
            strokeColor: '#FF0000',
            strokeOpacity: 1.0,
            strokeWeight: 2
        });

        travelPath.setMap(map);
        var startLocation = new google.maps.LatLng(startLatitude, startLongitude);
        var endLocation = new google.maps.LatLng(endLatitude, endLongitude);

        var startMarker = new google.maps.Marker({
            position: startLocation,
            map: map,
            title: startAddress
        });

        var endMarker = new google.maps.Marker({
            position: endLocation,
            map: map,
            title: endAddress
        });

      
        var bounds = new google.maps.LatLngBounds();
        bounds.extend(neBound);
        bounds.extend(swBound);

        map.fitBounds(bounds);
    }


    function refreshPreviewData(data) {
        $("#StartAddress").val(data.StartAddress);
        $("#EndAddress").val(data.EndAddress);
        $("#DistanceText").val(data.DistanceText);
        $("#DurationText").val(data.DurationText);

        $("#Mode").val(data.Mode);

        // refresh line
        travelPath.setMap(null);
        google.maps.event.trigger(map, 'resize');
        initialize();
    }
});