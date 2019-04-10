routerApp.controller('MapController', function (NgMap, $scope) {

    $scope.googleMapsUrl = "https://maps.googleapis.com/maps/api/js?key=AIzaSyAoVvlpqMWmWR4LjFmSLCruQIaBXtkyiOw";

    NgMap.getMap().then(function (map) {
        console.log(map.getCenter());
        console.log('markers', map.markers);
        console.log('shapes', map.shapes);

    });
});