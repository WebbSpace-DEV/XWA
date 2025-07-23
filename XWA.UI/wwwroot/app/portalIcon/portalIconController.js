angular.module('xwa.portalIcon', [
  'ui.router',
  'ui.bootstrap',
  'xwa.portalIconDirectives',
  'xwa.portalIconService',
  'xwa.portalIconPersistenceFactory',
  'xwa.sessionFactory',
  'xwa.eyeCandyDirectives'
]).controller('PortalIconController',
  function (
    $scope,
    $state,
    $window,
    PortalIconService,
    PortalIconPersistenceFactory,
    SessionFactory) {

    $scope.sessionFactory = SessionFactory;

    $scope.init = function () {
      $scope.$state = $state;
      $scope.icons = PortalIconPersistenceFactory.icons;
    };

    $scope.load = function () {
      //if ('-1' !== $scope.sessionFactory.user.id && $scope.isEmptyArray($scope.icons)) {
      if ($scope.isEmptyArray($scope.icons)) {
        PortalIconService.getIcons()
          .then(function (response) {
            $scope.icons = response.data;
            PortalIconPersistenceFactory.icons = $scope.icons;
          });
      }
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

    $scope.doIconClick = function (url) {
      console.log('Navigate to \'' + url + '\'');
    };

    $scope.isEmpty = function (obj) {
      return (null === obj || undefined === obj);
    };

    $scope.isEmptyArray = function (arr) {
      return ($scope.isEmpty(arr) || 0 === arr.length);
    };
  });
