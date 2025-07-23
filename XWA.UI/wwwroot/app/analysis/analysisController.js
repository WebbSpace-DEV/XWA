angular.module('xwa.analysis', [
  'ui.router',
  'ui.bootstrap',
  'ui.grid',
  'ui.grid.resizeColumns',
  'ui.grid.autoResize',
  'ui.grid.moveColumns',
  'ui.grid.saveState',
  'ngSanitize',
  'xwa.analysisService',
  'xwa.analysisPersistenceFactory',
  'xwa.common',
  'xwa.d3',
  'xwa.dataVisDirectives'
]).controller('AnalysisController',
  function (
    $scope,
    $rootScope,
    $sanitize,
    $uibModal,
    AnalysisService,
    AnalysisPersistenceFactory,
    Common) {

    $scope.common = Common;

    $scope.init = function () {
      $scope.getColorByScore = Common.getColorByScore;
      // The variable $scope.analysis is used for rendering airframe data in tabular form.
      $scope.analysis = [];
      // These are the collections for the UI controls.
      $scope.fleet = [];
      $scope.platforms = [];
      $scope.squadrons = [];
      $scope.airframes = [];
      $scope.provisions = [];
      $scope.airfields = [];
      $scope.flights = [];
      $scope.summaries = [];
      // This loads the filter.selected<T> variables.
      $scope.filter = AnalysisPersistenceFactory.filter;
      $scope.dataVis = [];
      $scope.geoPos = [];
      var doApplyFilter = false;
      if (
        $scope.isEmptyArray(AnalysisPersistenceFactory.data.analysis) ||
        $scope.isEmptyArray(AnalysisPersistenceFactory.data.fleet) ||
        $scope.isEmptyArray(AnalysisPersistenceFactory.data.platforms) ||
        $scope.isEmptyArray(AnalysisPersistenceFactory.data.squadrons) ||
        $scope.isEmptyArray(AnalysisPersistenceFactory.data.airframes) ||
        $scope.isEmptyArray(AnalysisPersistenceFactory.data.provisions) ||
        $scope.isEmptyArray(AnalysisPersistenceFactory.data.airfields) ||
        $scope.isEmptyArray(AnalysisPersistenceFactory.data.flights) ||
        $scope.isEmptyArray(AnalysisPersistenceFactory.data.summaries)) {
        AnalysisService.getAnalysis()
          .then(function (response) {
            // Preload response data.
            $scope.fleet = response.data.fleet;
            $scope.platforms = response.data.platforms;
            $scope.squadrons = response.data.squadrons;
            $scope.airframes = response.data.airframes;
            $scope.provisions = response.data.provisions;
            $scope.airfields = response.data.airfields;
            $scope.flights = response.data.flights;
            $scope.summaries.push({ id: 'PLATFORMS', name: 'Platforms' });
            $scope.summaries.push({id: 'SQUADRONS', name: 'Squadrons'});
            $scope.summaries.push({id: 'AIRFRAMES', name: 'Airframes'});
            $scope.summaries.push({id: 'PROVISIONS', name: 'Provisions'});
            $scope.summaries.push({ id: 'AIRFIELDS', name: 'Airfields' });
          })
          .then(function () {
            // Shape the tabular analysis data.
            $scope.airframes.sort((a, b) => (
              a.platform.localeCompare(b.platform) ||
              a.squadron.localeCompare(b.squadron) ||
              a.id.localeCompare(b.id)));
            $scope.airframes.forEach(function (airframe) {
              $scope.analysis.push(airframe);
            });
          })
          .then(function () {
            // Shape the UI control data.

            // Platform drop-down list...
            $scope.platforms.sort(function (a, b) {
              return a.id.localeCompare(b.id);
            });
            $scope.platforms.splice(0, 0, {
              id: 'ALL'
            });
            $scope.filter.selectedPlatform = $scope.platforms[0];

            // Squadron drop-down list...
            $scope.squadrons.sort(function (a, b) {
              return a.id.localeCompare(b.id);
            });
            $scope.squadrons.splice(0, 0, {
              id: 'ALL'
            });
            $scope.filter.selectedSquadron = $scope.squadrons[0];

            // Airframe drop-down list...
            $scope.airframes.sort(function (a, b) {
              return a.id.localeCompare(b.id);
            });
            $scope.airframes.splice(0, 0, {
              id: 'ALL'});
            $scope.filter.selectedAirframe = $scope.airframes[0];

            // Airfield drop-down list...
            $scope.airfields.sort(function (a, b) {
              return a.id.localeCompare(b.id);
            });
            $scope.airfields.splice(0, 0, {
              id: 'ALL'});
            $scope.filter.selectedAirfield = $scope.airfields[0];

            // Summary drop-down list...
            $scope.filter.selectedSummary = $scope.getItemById($scope.summaries, 'AIRFRAMES');
          })
          .then(function () {
            // Persist the shaped data...
            AnalysisPersistenceFactory.data.analysis = $scope.analysis;
            AnalysisPersistenceFactory.data.fleet = $scope.fleet;
            AnalysisPersistenceFactory.data.platforms = $scope.platforms;
            AnalysisPersistenceFactory.data.squadrons = $scope.squadrons;
            AnalysisPersistenceFactory.data.airframes = $scope.airframes;
            AnalysisPersistenceFactory.data.airfields = $scope.airfields;
            AnalysisPersistenceFactory.data.provisions = $scope.provisions;
            AnalysisPersistenceFactory.data.flights = $scope.flights;
            AnalysisPersistenceFactory.data.summaries = $scope.summaries;
          })
          .then(function () {
            // Reload the selected filter critera.
            AnalysisPersistenceFactory.filter = $scope.filter;
          })
          .then(function () {
            // Process the data visualizations.
            $scope.loadDataVis();
            $scope.loadGeoPos();
          });
        $scope.analysisTableOptions.columnDefs = detailColumnDefs;
      } else {
        // Reload the persisted data.
        $scope.analysis = AnalysisPersistenceFactory.data.analysis;
        $scope.fleet = AnalysisPersistenceFactory.data.fleet;
        $scope.platforms = AnalysisPersistenceFactory.data.platforms;
        $scope.squadrons = AnalysisPersistenceFactory.data.squadrons;
        $scope.airframes = AnalysisPersistenceFactory.data.airframes;
        $scope.provisions = AnalysisPersistenceFactory.data.provisions;
        $scope.airfields = AnalysisPersistenceFactory.data.airfields;
        $scope.flights = AnalysisPersistenceFactory.data.flights;
        $scope.summaries = AnalysisPersistenceFactory.data.summaries;
        doApplyFilter = true;
      }

      // These are the selected values for the controls.
      if ($scope.isEmpty($scope.filter.selectedPlatform)) {
        $scope.filter.selectedPlatform = AnalysisPersistenceFactory.filter.selectedPlatform;
      }
      if ($scope.isEmpty($scope.filter.selectedSquadron)) {
        $scope.filter.selectedSquadron = AnalysisPersistenceFactory.filter.selectedSquadron;
      }
      if ($scope.isEmpty($scope.filter.selectedAirframe)) {
        $scope.filter.selectedAirframe = AnalysisPersistenceFactory.filter.selectedAirframe;
      }
      if ($scope.isEmpty($scope.filter.selectedAirfield)) {
        $scope.filter.selectedAirfield = AnalysisPersistenceFactory.filter.selectedAirfield;
      }
      if ($scope.isEmpty($scope.filter.selectedSummary)) {
        $scope.filter.selectedSummary = AnalysisPersistenceFactory.filter.selectedSummary;
      }
      if (doApplyFilter) {
        $scope.applyFilter();
        $scope.loadDataVis();
        $scope.loadGeoPos();
      }
    };

    $scope.load = function () {
      // Nothing to do at this time.
    };

    $scope.applyFilter = function () {
      var filterPlatform = false;
      var filterSquadron = false;
      var filterAirframe = false;
      var filterAirfield = false;
      if (!$scope.isEmpty($scope.filter.selectedPlatform.id)) {
        filterPlatform = 'ALL' !== $scope.filter.selectedPlatform.id.toUpperCase();
      }
      if (!$scope.isEmpty($scope.filter.selectedSquadron.id)) {
        filterSquadron = 'ALL' !== $scope.filter.selectedSquadron.id.toUpperCase();
      }
      if (!$scope.isEmpty($scope.filter.selectedAirframe.id)) {
        filterAirframe = 'ALL' !== $scope.filter.selectedAirframe.id.toUpperCase();
      }
      if (!$scope.isEmpty($scope.filter.selectedAirfield.id)) {
        filterAirfield = 'ALL' !== $scope.filter.selectedAirfield.id.toUpperCase();
      }

      AnalysisPersistenceFactory.filter.selectedPlatform = $scope.filter.selectedPlatform;
      AnalysisPersistenceFactory.filter.selectedSquadron = $scope.filter.selectedSquadron;
      AnalysisPersistenceFactory.filter.selectedAirframe = $scope.filter.selectedAirframe;
      AnalysisPersistenceFactory.filter.selectedAirfield = $scope.filter.selectedAirfield;
      AnalysisPersistenceFactory.filter.selectedSummary = $scope.filter.selectedSummary;
      $scope.analysis = AnalysisPersistenceFactory.data.analysis;
      if (filterPlatform) {
        $scope.analysis = $scope.analysis
          .filter(function (d) {
            return d.platform === $scope.filter.selectedPlatform.id;
          });
      }
      if (filterSquadron) {
        $scope.analysis = $scope.analysis
          .filter(function (d) {
            return d.squadron === $scope.filter.selectedSquadron.id;
          });
      }
      if (filterAirframe) {
        $scope.analysis = $scope.analysis
          .filter(function (d) {
            return d.id === $scope.filter.selectedAirframe.id;
          });
      }
      if (filterAirfield) {
        $scope.analysis = $scope.analysis
          .filter(function (d) {
            return d.airfield === $scope.filter.selectedAirfield.id;
          });
      }
    };

    $scope.loadDataVis = function () {
      var selected = $scope.filter.selectedSummary.id.toLowerCase();
      $scope.dataVis = [];
      var isSorted = true;
      var id;
      var idx;
      switch (selected) {
        case ('airframes'):
          $scope.airframes.forEach(function (airframe) {
            id = airframe['id'];
            if ('ALL' !== id.toUpperCase()) {
              idx = -1;
              for (var j = 0, k = $scope.dataVis.length; j < k; j++) {
                if (id === $scope.dataVis[j]['id']) {
                  idx = j;
                  break;
                }
              }
              if (-1 === idx) {
                $scope.dataVis.push({
                  id: id,
                  airfield: airframe['airfield'],
                  score: airframe['score']
                });
              }
            }
          });
          break;
        case ('platforms'):
        case ('squadrons'):
        case ('provisions'):
        case ('airfields'):
          var manifest = [];
          var labelField = 'id';
          switch (selected) {
            case ('squadrons'):
              manifest = $scope.squadrons.filter(function (d) {
                return 'ALL' !== d.id.toUpperCase();
              });
              break;
            case ('platforms'):
              manifest = $scope.platforms.filter(function (d) {
                return 'ALL' !== d.id.toUpperCase();
              });
              break;
            case ('provisions'):
              manifest = AnalysisPersistenceFactory.data.provisions;
              labelField = 'name';
              isSorted = false;
              break;
            case ('airfields'):
              manifest = $scope.airfields.filter(function (d) {
                return 'ALL' !== d.id.toUpperCase();
              });
              break;
            default:
              break;
          }
          manifest.forEach(function (item) {
            $scope.dataVis.push({
              id: item[labelField], //needs to be 'name' for provisions
              label: item[labelField],
              totalCount:
                item['level1Count'] +
                item['level2Count'] +
                item['level3Count'],
              level1Count: item['level1Count'],
              level2Count: item['level2Count'],
              level3Count: item['level3Count'],
              level1Percent: parseFloat(item['level1Percent']) || 0,
              level2Percent: parseFloat(item['level2Percent']) || 0,
              level3Percent: parseFloat(item['level3Percent']) || 0,
              score: parseInt(item['score'])
            });
          });
          break;
        default:
          break;
      }
      if (isSorted) {
        $scope.dataVis.sort(function (a, b) {
          return (String('000' + a.score).slice(-3) + a.title).localeCompare((String('000' + b.score).slice(-3) + b.title));
        });
      }
      AnalysisPersistenceFactory.filter.selectedSummary = $scope.filter.selectedSummary;
    };

    $scope.doDataVisClick = function (id) {
      $scope.filterById($scope.filter.selectedSummary.id, id);
    };

    $scope.loadGeoPos = function () {
      $scope.geoPos = [];
      if ('ALL' === $scope.filter.selectedAirfield.id.toUpperCase()) {
        $scope.airfields.forEach(function (airfield) {
          if ('ALL' !== airfield['id'].toUpperCase()) {
            $scope.geoPos.push(airfield);
          }
        });
      } else {
        $scope.geoPos.push($scope.filter.selectedAirfield);
      }
    };

    $scope.filterById = function (type, id) {
      var doFilter = true;
      switch (type.toLowerCase()) {
        case ('platforms'):
          $scope.filter.selectedPlatform = $scope.getItemById($scope.platforms, id);
          break;
        case ('squadrons'):
          $scope.filter.selectedSquadron = $scope.getItemById($scope.squadrons, id);
          break;
        case ('airframes'):
          $scope.filter.selectedAirframe = $scope.getItemById($scope.airframes, id);
          break;
        case ('airfields'):
          $scope.filter.selectedAirfield = $scope.getItemById($scope.airfields, id);
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
      $scope.filter.selectedSummary = $scope.getItemById($scope.summaries, type.toUpperCase());
      switch (type.toLowerCase()) {
        case ('airfields'):
          $scope.loadGeoPos();
          break;
        case ('airframes'):
          if ('ALL' !== $scope.filter.selectedAirframe.id.toUpperCase()) {
            $scope.showDiagnosisModal();
          }
          break;
        default:
          // Do nothing.
          break;
      }
      $scope.applyFilter();
      $scope.loadDataVis();
      AnalysisPersistenceFactory.filter = $scope.filter;
    };

    var platformCellTemplate = '<div ng-click="grid.appScope.filterById(\'platforms\', row.entity.platform)" class="ui-grid-cell-contents clickableCell"><span>{{row.entity.platform}}</span></div>';
    var squadronCellTemplate = '<div ng-click="grid.appScope.filterById(\'squadrons\', row.entity.squadron)" class="ui-grid-cell-contents clickableCell"><span>{{row.entity.squadron}}</span></div>';
    var airframeCellTemplate = '<div ng-click="grid.appScope.filterById(\'airframes\', row.entity.id)" class="ui-grid-cell-contents clickableCell"><span>{{row.entity.id ? row.entity.id : "N/A"}}</span></div>';
    var airfieldCellTemplate = '<div ng-click="grid.appScope.filterById(\'airfields\', row.entity.airfield)" class="ui-grid-cell-contents clickableCell"><span>{{row.entity.airfield}}</span></div>';

    var scoreDetailCellTemplate = '<div ng-attr-title="{{grid.appScope.common.getColorByScore(row.entity.score)}}" class="ui-grid-cell-contents circle"><div class="{{grid.appScope.common.getColorByScore(row.entity.score)}} circle"><span>{{grid.getCellValue(row, col)}}</span></div></div>';
    var shieldDetailCellTemplate = '<div ng-attr-title="{{grid.appScope.common.getColorByScore(row.entity.shield)}}" class="ui-grid-cell-contents circle"><div class="{{grid.appScope.common.getColorByScore(row.entity.shield)}} circle"><span>{{grid.getCellValue(row, col)}}</span></div></div>';
    var powerDetailCellTemplate = '<div ng-attr-title="{{grid.appScope.common.getColorByScore(row.entity.power)}}" class="ui-grid-cell-contents circle"><div class="{{grid.appScope.common.getColorByScore(row.entity.power)}} circle"><span>{{grid.getCellValue(row, col)}}</span></div></div>';
    var droidDetailCellTemplate = '<div ng-attr-title="{{grid.appScope.common.getColorByScore(row.entity.droid)}}" class="ui-grid-cell-contents circle"><div class="{{grid.appScope.common.getColorByScore(row.entity.droid)}} circle"><span>{{grid.getCellValue(row, col)}}</span></div></div>';
    var servoDetailCellTemplate = '<div ng-attr-title="{{grid.appScope.common.getColorByScore(row.entity.servo)}}" class="ui-grid-cell-contents circle"><div class="{{grid.appScope.common.getColorByScore(row.entity.servo)}} circle"><span>{{grid.getCellValue(row, col)}}</span></div></div>';
    var sensorDetailCellTemplate = '<div ng-attr-title="{{grid.appScope.common.getColorByScore(row.entity.sensor)}}" class="ui-grid-cell-contents circle"><div class="{{grid.appScope.common.getColorByScore(row.entity.sensor)}} circle"><span>{{grid.getCellValue(row, col)}}</span></div></div>';

    var textWidthNarrow = 106;
    var textWidth = 110;
    var numberWidth = 150;

    var detailColumnDefs = [
      {field: 'platform',
        displayName: 'Platform',
        width: textWidth,
        minWidth: textWidth,
        enableHiding: false,
        visible: true,
        cellTemplate: platformCellTemplate},
      {
        field: 'squadron',
        displayName: 'Squadron',
        width: textWidthNarrow,
        minWidth: textWidthNarrow,
        enableHiding: false,
        visible: true,
        cellTemplate: squadronCellTemplate
      },
      {field: 'id',
        displayName: 'Airframe',
        width: textWidth,
        minWidth: textWidth,
        enableHiding: false,
        visible: true,
        cellTemplate: airframeCellTemplate},
      {field: 'airfield',
        displayName: 'Airfield',
        width: textWidthNarrow,
        minWidth: textWidthNarrow,
        enableHiding: false,
        visible: true,
        cellTemplate: airfieldCellTemplate},
      {field: 'score',
        displayName: 'Overall Score',
        width: '*',
        minWidth: numberWidth,
        enableHiding: false,
        visible: true,
        cellTemplate: scoreDetailCellTemplate,
        sortingAlgorithm: sortScore},
      {field: 'shield',
        displayName: 'Deflector Shield',
        width: '*',
        minWidth: numberWidth,
        enableHiding: false,
        visible: true,
        cellTemplate: shieldDetailCellTemplate,
        sortingAlgorithm: sortScore},
      {field: 'power',
        displayName: 'Power Generator',
        width: '*',
        minWidth: numberWidth,
        enableHiding: false,
        visible: true,
        cellTemplate: powerDetailCellTemplate,
        sortingAlgorithm: sortScore},
      {field: 'droid',
        displayName: 'Astromech Droid',
        width: '*',
        minWidth: numberWidth,
        enableHiding: false,
        visible: true,
        cellTemplate: droidDetailCellTemplate,
        sortingAlgorithm: sortScore},
      {field: 'servo',
        displayName: 'Servo Actuator',
        width: '*',
        minWidth: numberWidth,
        enableHiding: false,
        visible: true,
        cellTemplate: servoDetailCellTemplate,
        sortingAlgorithm: sortScore},
      {field: 'sensor',
        displayName: 'Sensor Window',
        width: '*',
        minWidth: numberWidth,
        enableHiding: false,
        visible: true,
        cellTemplate: sensorDetailCellTemplate,
        sortingAlgorithm: sortScore}
    ];

    function sortScore(a, b) {
      var s;
      var aParse = Number(a);
      var bParse = Number(b);
      switch (true) {
        case (aParse < bParse):
          s = -1;
          break;
        case (aParse > bParse):
          s = 1;
          break;
        default:
          s = 0;
          break;
      }
      return s;
    }

    $scope.analysisTableOptions = {
      data: 'analysis',
      columnDefs: detailColumnDefs,
      rowHeight: 41,
      flatEntityAccess: true,
      showGridFooter: true,
      gridFooterTemplate: '<div class="gridFooter">Number of airframes: {{grid.appScope.analysis.length}}</div>',
      onRegisterApi: function (gridApi) {
        $scope.gridApi = gridApi;
      }
    };

    $scope.saveState = function () {
      $scope.state = $scope.gridApi.saveState.save();
    };

    $scope.restoreState = function () {
      $scope.gridApi.saveState.restore($scope, $scope.state);
    };

    $scope.showDiagnosisModal = function () {
      var modalInstance = $uibModal.open({
        animation: false,
        templateUrl: 'app/modalPopup/diagnosisModalTemplate.html',
        controller: 'DiagnosisModalController',
        size: 'md',
        windowClass: 'diagnosisModal',
        resolve: {
          data: function () {
            return {selection: $scope.filter.selectedAirframe};
          }
        }
      });
      modalInstance.result.then(
        function success(result) {
        },
        function failure() {
        }
      );
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

    $scope.getFriendlyName = function (obj) {
      var s = obj.id;
      if ('all' !== s.toLowerCase()) {
        s += ': ' + obj.name;
      }
      return s;
    };

    $scope.isEmpty = function (obj) {
      return (null === obj || undefined === obj);
    };

    $scope.isEmptyArray = function (arr) {
      return ($scope.isEmpty(arr) || 0 === arr.length);
    };

    $scope.titleCase = function (str) {
      return str.toLowerCase().split(' ').map(function (word) {
        return word.replace(word[0], word[0].toUpperCase());
      }).join(' ');
    };

  });
