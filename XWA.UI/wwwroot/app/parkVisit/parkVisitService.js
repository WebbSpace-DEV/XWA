angular.module('xwa.parkVisitService', [
  'ui.bootstrap',
  "xwa.common"
]).service('ParkVisitService', [
  '$http',
  'Common',
  function (
    $http,
    common) {

    this.getParkVisits = function () {
      var url = common.getServiceUrl('parkVisit/parkVisits', true);
      return $http.get(url)
        .then(function success(response) {
          return response;
        });
    };
  }]);
