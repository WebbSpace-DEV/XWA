angular.module('xwa.portalIconPersistenceFactory', []).factory('PortalIconPersistenceFactory', [
  function () {

    var _entity = {
      icons: [],
      reset: function () {
        _entity.icons = [];
      }
    };

    return _entity;
  }]);
