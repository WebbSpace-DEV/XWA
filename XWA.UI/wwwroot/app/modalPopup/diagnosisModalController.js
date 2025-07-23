angular.module('xwa.analysis').controller('DiagnosisModalController', [
  '$scope',
  '$sanitize',
  '$uibModal',
  '$uibModalInstance',
  'AnalysisService',
  'Common',
  'data',
  function (
    $scope,
    $sanitize,
    $uibModal,
    $uibModalInstance,
    AnalysisService,
    Common,
    data) {

    $scope.init = function () {
      $scope.getColorByLevel = Common.getColorByLevel;
      $scope.getColorByScore = Common.getColorByScore;
      $scope.data = data;
      $scope.provisions = [
        {provision: 'shield', score: data.selection.shield},
        {provision: 'power', score: data.selection.power},
        {provision: 'droid', score: data.selection.droid},
        {provision: 'servo', score: data.selection.servo},
        {provision: 'sensor', score: data.selection.sensor}
      ];
    };

    $scope.load = function () {
      // Do nothing (at this time).
    };

    $scope.close = function () {
      $uibModalInstance.dismiss();
    };
  }]);
