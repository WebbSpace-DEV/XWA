angular.module('xwa.analysisPersistenceFactory', []).factory('AnalysisPersistenceFactory', [
  function () {

    var _entity = {
      data: {
        analysis: [],
        fleet: {},
        platforms: [],
        squadrons: [],
        airframes: [],
        provisions: [],
        airfields: [],
        flights: [],
        summaries: []
      },
      filter: {
        selectedPlatform: {},
        selectedSquadron: {},
        selectedAirframe: {},
        selectedAirfield: {},
        selectedSummary: {}
      }
    };

    return _entity;
  }]);
