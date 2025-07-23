angular.module('xwa.sessionFactory', []).factory('SessionFactory', [
  '$http',
  function (
    $http) {

    var _metadata = {
      baseAddress: '',
      user: {
        id: '',
        email: ''
      },
      version: ''
    };

    this.initialize = function () {
      jQuery.ajax({ TYPE: 'GET', url: 'api/sessionFactory' })
        .then(function (response) {
          _metadata.baseAddress = response.baseAddress;
          _metadata.user.id = response.user.id;
          _metadata.user.email = response.user.email;
          _metadata.version = response.version;
          localStorage.setItem('jwt_token', response.jwtToken);
        })
    };

    this.initialize();

    return _metadata;

  }]);
