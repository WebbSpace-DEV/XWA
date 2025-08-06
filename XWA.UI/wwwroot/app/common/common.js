angular.module('xwa.common', [
  'xwa.sessionFactory'
]).factory('Common', [
  'SessionFactory',
  function (
    sessionFactory) {

    var _common = {};

    _common.getServiceUrl = function (fragment, isNoCache = false) {

      var url = '';
      url += sessionFactory.baseAddress;

      fragment.replaceAll('\\', '/');
      if (fragment.slice(1) !== '/') {
        url += '/'
      };
      url += fragment;

      if (isNoCache) {
        url += '?nocache=' + new Date().getTime();
      }
      return url;
    };

    _common.maxLevelScore = {
      unknown: -1,
      level3: 34,
      level2: 49,
      level1: 100
    };

    _common.maxColorScore = {
      unknown: _common.maxLevelScore.unknown,
      red: _common.maxLevelScore.level3,
      amber: _common.maxLevelScore.level2,
      green: _common.maxLevelScore.level1
    };

    _common.getColorByScore = function (value) {
      var retVal;
      switch (true) {
        case (value <= _common.maxColorScore.red):
          return 'red';
          break;
        case (value <= _common.maxColorScore.amber):
          return 'amber';
          break;
        case (value <= _common.maxColorScore.green):
          return 'green';
          break;
        default:
          retVal = undefined;
          break;
      }
      return retVal;
    };

    _common.getLevelByScore = function (value) {
      var retVal;
      switch (true) {
        case (value <= _common.maxColorScore.red):
          return 3;
          break;
        case (value <= _common.maxColorScore.amber):
          return 2;
          break;
        case (value <= _common.maxColorScore.green):
          return 1;
          break;
        default:
          retVal = undefined;
          break;
      }
      return retVal;
    };

    _common.getColorByLevel = function (value) {
      var retVal;
      switch (value) {
        case (3):
          retVal = 'red';
          break;
        case (2):
          retVal = 'amber';
          break;
        case (1):
          retVal = 'green';
          break;
        default:
          retVal = undefined;
          break;
      }
      return retVal;
    };

    _common.isOconus = function (value) {
      var oconusFips = [2, 3, 7, 14, 15, 43, 52, 60, 64, 66, 67, 68, 69, 70, 71, 72, 74, 76, 78, 79, 81, 84, 86, 89, 95];
      return -1 === oconusFips.indexOf(value);
    };

    _common.isEmpty = function (obj) {
      return (null === obj || undefined === obj);
    }

    _common.isEmptyArray = function (arr) {
      return (this.isEmpty(arr) || 0 === arr.length);
    }

    _common.titleCase = function (str) {
      return str.toLowerCase().split(' ').map(function (word) {
        return word.replace(word[0], word[0].toUpperCase());
      }).join(' ');
    };

    return _common;

  }]);
