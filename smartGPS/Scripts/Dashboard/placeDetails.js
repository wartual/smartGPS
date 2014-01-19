$(document).ready(function () {

    var latitude = $("#latitude").val().replace(",", ".");;
    var longitude = $("#longitude").val().replace(",", ".");;
    var location = new google.maps.LatLng(latitude, longitude);
    var iconUrl = $("#iconUrl").val();

    // global variables
    var mapOptions = {
        zoom: 15,
        center: new google.maps.LatLng(latitude, longitude),
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
    google.maps.event.addDomListener(window, 'load', initialize);
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        if (e.target.hash == "#map") {
            google.maps.event.trigger(map, 'resize');
            initialize();
        }
    })
   
    
    function initialize() {
        var marker = new google.maps.Marker({
            position: location,
            map: map,
            icon: iconUrl,
            title: "Mirko"
        });
    }
});