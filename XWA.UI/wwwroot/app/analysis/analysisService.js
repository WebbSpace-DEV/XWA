angular.module('xwa.analysisService', [
  'ui.bootstrap',
  'xwa.common'
]).service('AnalysisService', [
  '$http',
  'Common',
  function (
    $http,
    common) {

    this.getAnalysis = function () {
      var url = common.getServiceUrl('analysis/analysis', true);
      return $http.get(url)
        .then(function success(response) {
          return response;
        });
    };
  }]);
