angular.module('xwa.portalIconService', [
  'ui.bootstrap',
  'xwa.common'
]).service('PortalIconService', [
  '$http',
  'Common',
  function (
    $http,
    common) {

    this.getIcons = function () {
      var url = common.getServiceUrl('portalIcon/portalIcons', true);
      return $http.get(url)
        .then(function success(response) {
          return response;
        });
    };
  }]);
