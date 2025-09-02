angular.module('xwa.overview', [
  'ui.router',
  'ui.bootstrap',
  'xwa.sessionFactory'
]).controller('OverviewController',
  function (
    $scope,
    $state,
    $window,
    $http,
    SessionFactory) {

    $scope.sessionFactory = SessionFactory;

    $scope.init = function () {
      $scope.$state = $state;
    };

    $scope.load = function () {
      // Do nothing (at this time).
    };

    /*
    /* Since it reads information from the appsettings configuration file,
     * the session factory REST service takes long enough to return that it
     * occasionally occurs after the call to services that require the user
     * ID.
     *
     * This results in incorrect data, so we need to watch the session
     * factory data and rerun the inital load once it is populated to get
     * everything displaying properly.
     */
    $scope.$watch('sessionFactory.user.id', function () {
      $scope.load();
    }, true);

    $scope.tabs = [
      {
        title: 'Dynamic Tab #1',
        content: 'Dynamic Content #1',
        disabled: false
      },
      {
        title: 'Dynamic Tab #2',
        content: 'Dynamic Content #2',
        disabled: true
      },
      {
        title: 'Dynamic Tab #3',
        content: 'Dynamic Content #3',
        disabled: false
      }
    ];

    $scope.getSessionInfo = function () {
      return JSON.stringify($scope.sessionFactory, null, '\t');
    };

    $scope.alertSessionInfo = function () {
      setTimeout(function () {
        $window.alert('The error message that follows is deliberate.');
        throwError();
      });
    };

    throwError = function () {
      // This url is deliberately broken in order to demonstrate the error modal popup.
      return $http.get('badUrl')
        .then(function success(response) {
          return response;
        });
    };

    $scope.getOverviewMessage = function () {
      var message = '';
      message += 'The "X-Wing Advisor" (XWA) web application demonstrates the ';
      message += 'capabilities of data visualizations, the responsiveness of ';
      message += 'AngularJS, and the application of best-practice information ';
      message += 'assurance and security.\n\n';

      message += 'Data is provided via RESTful web services using Minimal API and ';
      message += 'Vertical Slice Architecture in the orchestration layer. The ';
      message += 'application implements JSON Web Token (JWT) to authenticate ';
      message += 'and authorize access to the web API service endpoints.\n\n';

      message += 'In the data structure, the X-Wing fleet is organized by airframes, ';
      message += 'squadrons, and platforms. Airframes identify specific X-Wing ';
      message += 'vehicles, squadrons identify groups of X-Wing airframes, and ';
      message += 'platforms identify variations between X-Wing airframes.\n\n';

      message += 'Each X-Wing airframe has an identical set of five (5) provisioned ';
      message += 'components: "Sensor Window," "Astromech Droid," "Servo Actuator," ';
      message += '"Power Generator," and "Deflector Shield". The provisions are ';
      message += 'linearly biased according to their criticality. In the module that ';
      message += 'generates "shim" data on the backend of the RESTful web service, ';
      message += 'each of the per-airframe provisions is randomly assigned a weighted ';
      message += 'score, and by applying the provision ordinal-bias business logic, an ';
      message += 'overall score is assigned to the airframe, with score averages ';
      message += 'aggregating forward through the fleet hierarchy. This allows the web ';
      message += 'application user to visualize and analyze the disposition of provisions, ';
      message += 'airframes, squadrons, platforms, airfields, and ultimately the ';
      message += 'readiness of fleet itself.\n\n';

      message += 'Level 3 ("red") scores range from 0 to 34, Level 2 ("amber") scores ';
      message += 'range from 35 to 49, and Level 1 ("green") scores range from 50 to ';
      message += '100.\n\n';

      message += 'XWA provides a framework for Descriptive and Diagnostic analytics, ';
      message += 'and Prescriptive analytics inferences might be made across the fleet ';
      message += 'within its data structure hierarchy.\n\n';

      message += 'All of the visual elements in XWA are turn-key, solution-specific ';
      message += 'components written using the d3.js visualization library with AngularJS ';
      message += 'and JavaScript.\n\n';

      return message;
    };
  });
