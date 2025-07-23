angular.module('xwa.d3', [
  'xwa.common'
]).factory('d3Launch', [
  '$document',
  '$window',
  '$q',
  '$rootScope',
  'Common',
  function (
    $document,
    $window,
    $q,
    $rootScope,
    common) {

    var _d = $q.defer();
    var _d3Launch = {
      d3: function () {
        return _d.promise;
      }
    };

    _d3Launch.gauge = {
      // Excluding "unknown", the "pct" property should add up to 100.
      level3: {
        idxGauge: 1,
        idxRing: 3,
        pct: common.maxLevelScore.level3,
        class: 'ringLevel3'
      },
      level2: {
        idxGauge: 2,
        idxRing: 2,
        pct: common.maxLevelScore.level2 - common.maxLevelScore.level3,
        class: 'ringLevel2'
      },
      level1: {
        idxGauge: 3,
        idxRing: 1,
        pct: 100 - common.maxLevelScore.level2,
        class: 'ringLevel1'
      },
      unknown: {
        idx: -1,
        pct: -100,
        class: undefined
      }
    };

    _d3Launch.provision = {
      shield: {
        x: 242,
        y: 426
      },
      power: {
        x: 242,
        y: 371
      },
      droid: {
        x: 242,
        y: 343
      },
      servo: {
        x: 242,
        y: 476
      },
      sensor: {
        x: 242,
        y: 35
      }
    };

    function onScriptLoad() {
      $rootScope.$apply(function () {
        _d.resolve($window.d3);
      });
    }

    var scriptTag = $document[0].createElement('script');
    scriptTag.type = 'text/javascript';
    scriptTag.async = true;
    scriptTag.src = 'resources/d3/d3.min.js';
    scriptTag.onreadystatechange = function () {
      if ('complete' === this.readyState)
        onScriptLoad();
    };
    scriptTag.onload = onScriptLoad;

    var s = $document[0].getElementsByTagName('body')[0];
    s.appendChild(scriptTag);

    return _d3Launch;
  }]);
