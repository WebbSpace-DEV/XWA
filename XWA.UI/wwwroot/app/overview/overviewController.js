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
      message += 'The "X-Wing Advisor" web application demonstrates many of the ';
      message += 'capabilities of AngularJS, data visualization, and best-practice ';
      message += 'information assurance and security.\n\n';

      message += 'Data is provided via RESTful web services using Minimal API and ';
      message += 'Vertical Slice Architecture in the orchestration layer. The ';
      message += 'application implements Javascript Web Tokens (JWT) to authenticate ';
      message += 'and authorize access to the web API service endpoints.\n\n';

      message += 'In the sample data, the X-Wing fleet is organized by airframes, ';
      message += 'squadrons, and platforms. Airframes identify specific X-Wing ';
      message += 'vehicles, squadrons identify groups of X-Wing airframes, and ';
      message += 'platforms identify variations of X-Wing airframes.\n\n';

      message += 'Each X-Wing airframe has an identical set of five (5) provisioned ';
      message += 'components: "Sensor Window", "Servo Actuator", "Astromech Droid", ';
      message += '"Power Generator", and "Deflector Shield". The provisions are ';
      message += 'biased linearly according to their criticality. In the module that ';
      message += 'generates "shim" data on the backend of the RESTful web service, ';
      message += 'each of the per-airframe provisions is randomly assigned a weighted ';
      message += 'score, and by applying the provision ordinal-bias business logic, ';
      message += 'an overall score is assigned to the airframe, with score averages ';
      message += 'aggregating forward through the fleet hierarchy.This allows the web ';
      message += 'application user to analyze and visualize the readiness of provisions, ';
      message += 'airframes, squadrons, platforms, airfields, and ultimately the fleet ';
      message += 'itself.\n\n';

      message += 'Level 3 ("red") scores range from 0 to 34, Level 2 ("amber") scores ';
      message += 'range from 35 to 49, and Level 1 ("green") scores range from 50 to ';
      message += '100.\n\n';

      return message;
    };
  });
