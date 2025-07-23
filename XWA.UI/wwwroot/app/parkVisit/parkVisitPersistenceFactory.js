angular.module('xwa.parkVisitPersistenceFactory', []).factory('ParkVisitPersistenceFactory', [
  function () {

    var _entity = {
      data: {
        parks: [],
        regions: []
      },
      filter: {
        selectedRegion: {}
      }
    };

    return _entity;
  }]);
