angular.module('xwa').controller('ErrorModalController', [
  '$scope',
  '$uibModal',
  '$uibModalInstance',
  'requestNotification',
  function (
    $scope,
    $uibModal,
    $uibModalInstance,
    requestNotification) {

    $scope.init = function () {
      // Do nothing (at this time).
    };

    $scope.load = function () {
      // Do nothing (at this time).
    };

    $scope.close = function () {
      requestNotification.isErrorModalOpen = false;
      $uibModalInstance.close();
    };
  }]);
