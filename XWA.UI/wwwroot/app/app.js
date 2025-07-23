var xwa = angular.module('xwa', [
  'ui.router',
  'ui.bootstrap',
  'ui.grid',
  'ui.grid.resizeColumns',
  'ui.grid.autoResize',
  'ngSanitize',
  'xwa.overview',
  'xwa.analysis',
  'xwa.parkVisit',
  'xwa.portalIcon',
  'xwa.d3',
  'xwa.dataVisDirectives',
  'xwa.geoPositionDirectives',
  'xwa.parkVisitDirectives',
  'xwa.portalIconDirectives',
  'xwa.eyeCandyDirectives'
]);

xwa.config([
  '$urlRouterProvider',
  '$stateProvider',
  function (
    $urlRouterProvider,
    $stateProvider) {
    $urlRouterProvider.otherwise('/overview/');

    $stateProvider
      .state('#', {
        url: '/#/',
        templateUrl: 'app/overview/overview.html',
        controller: 'OverviewController'
      })
      .state('overview', {
        url: '/overview/',
        templateUrl: 'app/overview/overview.html',
        controller: 'OverviewController'
      })
      .state('tabular', {
        url: '/tabular/',
        templateUrl: 'app/tabular/tabular.html',
        controller: 'AnalysisController'
      })
      .state('analysis', {
        url: '/analysis/',
        templateUrl: 'app/analysis/analysis.html',
        controller: 'AnalysisController'
      })
      .state('geoPosition', {
        url: '/geoPosition/',
        templateUrl: 'app/geoPosition/geoPosition.html',
        controller: 'AnalysisController'
      })
      .state('parkVisit', {
        url: '/parkVisit/',
        templateUrl: 'app/parkVisit/parkVisit.html',
        controller: 'ParkVisitController'
      })
      .state('portalIcon', {
        url: '/portalIcon/',
        templateUrl: 'app/portalIcon/portalIcon.html',
        controller: 'PortalIconController'
      });

  }]);

xwa.controller('NavController',
  function (
    $scope,
    $state) {
    $scope.init = function () {
      $scope.$state = $state;
    };
  });

xwa.provider('requestNotification',
  function () {
    // This is where we keep subscribed listeners...
    var onRequestStartedListeners = [];
    var onRequestEndedListeners = [];

    // This is a utility to easily increment the request count...
    var count = 0;
    var requestCounter = {
      increment: function () {
        count++;
      },
      decrement: function () {
        if (count > 0) {
          count--;
        }
      },
      getCount: function () {
        return count;
      }
    };

    // Subscribe to be notified when request starts.
    this.subscribeOnRequestStarted = function (listener) {
      onRequestStartedListeners.push(listener);
    };

    // Tell the provider that the request has started.
    this.fireRequestStarted = function (request) {
      // Increment the request count.
      requestCounter.increment();
      // Run each subscribed listener.
      angular.forEach(onRequestStartedListeners, function (listener) {
        // Call the listener with request argument.
        listener(request);
      });
      return request;
    };

    // This is a complete analogy to the Request START.
    this.subscribeOnRequestEnded = function (listener) {
      onRequestEndedListeners.push(listener);
    };

    this.fireRequestEnded = function () {
      requestCounter.decrement();
      var passedArgs = arguments;
      angular.forEach(onRequestEndedListeners, function (listener) {
        listener.apply(this, passedArgs);
      });
      return arguments[0];
    };

    this.getRequestCount = requestCounter.getCount;

    this.isErrorModalOpen = false;
    this.isXsrfExpiredModalOpen = false;

    // This will be returned as a service.
    this.$get = function () {
      var that = this;
      // Just pass it all...
      return {
        subscribeOnRequestStarted: that.subscribeOnRequestStarted,
        subscribeOnRequestEnded: that.subscribeOnRequestEnded,
        fireRequestEnded: that.fireRequestEnded,
        fireRequestStarted: that.fireRequestStarted,
        getRequestCount: that.getRequestCount,
        isErrorModalOpen: that.isErrorModalOpen,
        isXsrfExpiredModalOpen: that.isXsrfExpiredModalOpen
      };
    };
  });

xwa.directive('loadingWidget', [
  'requestNotification',
  '$timeout',
  function (
    requestNotification,
    $timeout) {
    return {
      restrict: 'AC',
      link: function (scope, element) {
        var timer;

        requestNotification.subscribeOnRequestStarted(function () {
          if (requestNotification.getRequestCount() === 1) {
            timer = $timeout(function () {
              var documentHeight = jQuery(document).height();
              var bodyHeight = jQuery(document.body).height();
              // If the document height goes crazy, as it does in
              // Internet Explorer for some reason, then we just
              // set the overlay height to twice the body height
              // instead.
              jQuery(element).height(documentHeight > 100000 ? bodyHeight * 2 : documentHeight);
              element.show();
            }, 0);
          }
        });

        requestNotification.subscribeOnRequestEnded(function () {
          if (requestNotification.getRequestCount() === 0) {
            $timeout.cancel(timer);
            element.hide();
          }
        });
      }
    };
  }]);

xwa.factory('redirectInterceptor',
  function (
    $q,
    $window) {
    return {
      'response': function (response) {
        return response;
      },
      'responseError': function (rejection) {
        /* In IE9, when the timeout happens we get an empty error response with a status of 0 for some reason. In IE11 we get -1.
         * By refreshing the page, we then actually get redirected to the login page.
         */
        if (rejection.status === 0 || rejection.status === -1) {
          $window.location.reload();
        }
        return $q.reject(rejection);
      }
    };
  });

xwa.factory('authInterceptor', [
  '$q',
  '$window',
  function (
    $q,
    $window) {
    return {
      request: function (config) {
        config.headers = config.headers || {};
        var token = $window.localStorage.getItem('jwt_token');

        if (token) {
          // Attach the token as a Bearer token in the Authorization header.
          config.headers.Authorization = 'Bearer ' + token;
        }
        return config;
      },
      // Optional: handle response and responseError for token expiration or other errors.
      response: function (response) {
        return response || $q.when(response);
      },
      responseError: function (rejection) {
        // Handle unauthorized errors (e.g., redirect to login).
        if (rejection.status === 401) {
          // Redirect to login page or refresh token.
        }
        return $q.reject(rejection);
      }
    };
  }
]);

xwa.config([
  '$httpProvider',
  function (
    $httpProvider) {
    $httpProvider.interceptors.push('redirectInterceptor');
    $httpProvider.interceptors.push('authInterceptor');
    $httpProvider.interceptors.push(['$q', '$injector', 'requestNotification',
      function (
        $q,
        $injector,
        requestNotification) {

        return {
          'request': function (config) {
            requestNotification.fireRequestStarted();
            return config;
          },
          'response': function (response) {
            requestNotification.fireRequestEnded();
            return response;
          },
          'responseError': function (rejection) {
            requestNotification.fireRequestEnded();

            var status = rejection.status;

            switch (status) {
              case 401 :
                window.location = './index.html';
                break;
              default:
                if (!requestNotification.isErrorModalOpen) {
                  var modalInstance = $injector.get('$uibModal').open({
                    templateUrl: 'app/modalPopup/errorModalTemplate.html',
                    controller: 'ErrorModalController',
                    size: 'sm',
                    keyboard: false,
                    resolve: {}
                  });
                  modalInstance.result.then(
                    function success(result) {
                    },
                    function failure() {
                    }
                  );

                  requestNotification.isErrorModalOpen = true;
                }
                break;
            }
            // Otherwise...
            return $q.reject(rejection);
          }
        };
      }]);

    $httpProvider.interceptors.push(
      function () {
        return {
          request: function (config) {
            if ('GET' !== config.method) {
              return config;
            }

            if (config.url.match(/(js\/|css\/|img\/|font\/)/g)) {
              var separator = config.url.indexOf('?') === -1 ? '?' : '&';
              config.url = config.url + separator + 'v=VERSION_TOKEN';
            }
            return config;
          }
        };
      });
  }]);
