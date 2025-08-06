angular.module('xwa.parkVisit', [
  'ui.router',
  'ui.bootstrap',
  'ngSanitize',
  'xwa.parkVisitService',
  'xwa.parkVisitPersistenceFactory',
  'xwa.sessionFactory',
  'xwa.common',
  'xwa.d3'
]).controller('ParkVisitController',
  function (
    $scope,
    $rootScope,
    $sanitize,
    ParkVisitService,
    ParkVisitPersistenceFactory,
    SessionFactory,
    Common) {

    $scope.sessionFactory = SessionFactory;
    $scope.common = Common;

    $scope.init = function () {
      // These are the collections for the UI controls.
      $scope.parks = [];
      $scope.regions = [];
      // This loads the filter.selected<T> variables.
      $scope.filter = ParkVisitPersistenceFactory.filter;
      var doApplyFilter = false;
      if (
        $scope.common.isEmptyArray(ParkVisitPersistenceFactory.data.parks) ||
        $scope.common.isEmptyArray(ParkVisitPersistenceFactory.data.regions)) {
        ParkVisitService.getParkVisits()
          .then(function (response) {
            // Preload response data.
            $scope.parks = response.data.parks;
            $scope.parks.forEach(function (park) {
              park['filtered'] = true;
              park['selected'] = false;
            });
            $scope.regions = response.data.regions;
          })
          .then(function () {
            // Shape the UI control data.

            // Regions drop-down list...
            $scope.regions.sort(function (a, b) {
              return a.name.localeCompare(b.name);
            });
            $scope.regions.splice(0, 0, {
              id: 'ALL',
              name: 'All'});
            $scope.filter.selectedRegion = $scope.regions[0];

            // Parks checkbox list...
          })
          .then(function () {
            // Persist the shaped data.
            ParkVisitPersistenceFactory.data.parks = $scope.parks;
            ParkVisitPersistenceFactory.data.regions = $scope.regions;
          })
          .then(function () {
            // Reload the selected filter critera.
            ParkVisitPersistenceFactory.filter = $scope.filter;
          });
      } else {
        // Reload the persisted data.
        $scope.parks = ParkVisitPersistenceFactory.data.parks;
        $scope.regions = ParkVisitPersistenceFactory.data.regions;
        doApplyFilter = true;
      }

      // These are the selected values for the controls.
      if ($scope.common.isEmptyArray($scope.parks)) {
        $scope.parks = ParkVisitPersistenceFactory.data.parks;
      }
      if ($scope.common.isEmpty($scope.filter.selectedRegion)) {
        $scope.filter.selectedRegion = ParkVisitPersistenceFactory.filter.selectedRegion;
      }
      if (doApplyFilter) {
        $scope.applyFilter();
      }
    };

    $scope.load = function () {
      // Do nothing (at this time).
    };

    $scope.applyFilter = function () {
      var filterRegion = false;
      if (!$scope.common.isEmpty($scope.filter.selectedRegion.id)) {
        filterRegion = 'ALL' !== $scope.filter.selectedRegion.id.toUpperCase();
      }
      ParkVisitPersistenceFactory.filter.selectedRegion = $scope.filter.selectedRegion;
      $scope.parks = ParkVisitPersistenceFactory.data.parks;
      if (filterRegion) {
        $scope.parks.forEach(function (park) {
          park['filtered'] = ($scope.filter.selectedRegion.id === park['region']);
        });
      } else {
        $scope.parks.forEach(function (park) {
          park['filtered'] = true;
        });
      }
    };

    $scope.filterById = function (type, id) {
      var doFilter = true;
      switch (type.toLowerCase()) {
        case ('regions'):
          $scope.filter.selectedRegion = $scope.getItemById($scope.regions, id);
          break;
        default:
          doFilter = false;
          break;
      }
      if (doFilter) {
        $scope.filterBySelected(type);
      }
    };

    $scope.filterBySelected = function (type) {
      // Add additional per-drop-down functionality here.
      switch (type.toLowerCase()) {
        default:
          // Do nothing.
          break;
      }
      $scope.applyFilter();
      ParkVisitPersistenceFactory.filter = $scope.filter;
    };

    $scope.saveState = function () {
      $scope.state = $scope.gridApi.saveState.save();
    };

    $scope.restoreState = function () {
      $scope.gridApi.saveState.restore($scope, $scope.state);
    };

    $scope.getItemById = function (arr, id) {
      return arr[$scope.getIndexById(arr, id)];
    };

    $scope.getIndexById = function (arr, id) {
      var idx = -1;
      for (var i = 0, l = arr.length; i < l; i++) {
        if (id === arr[i].id) {
          idx = i;
          break;
        }
      }
      return idx;
    };

  });
