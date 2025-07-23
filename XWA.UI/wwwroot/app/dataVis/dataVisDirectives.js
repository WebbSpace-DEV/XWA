angular.module('xwa.dataVisDirectives', [
  'xwa.d3',
  'xwa.common'
])
  .directive('d3Summary', [
    'd3Launch',
    '$timeout',
    'Common',
    function (
      d3Launch,
      $timeout,
      common) {
      return {
        // Restrict to an attribute type.
        restrict: 'A',
        scope: {
          data: '=summaryData'
        },
        // If the element must have ng-model attribute (e.g., "blur" events)
        // require: 'ngModel',
        // scope = the parent scope
        // elem = the element the directive is on
        // attr = a dictionary of attributes on the element
        // ctrl = the controller for ngModel.
        link: function (scope, ele, attrs) {
          d3Launch.d3().then(function (d3) {
            scope.$watch('data', function (newData) {
              scope.render(newData);
            }, true);

            scope.render = function (data) {
              if (!data) {
                return;
              }

              $timeout(function () {
                onLoad();
              }, parseInt(attrs.resizeDelay) || 100);

              function onLoad() {

                var summaryType = attrs.summaryType.toLowerCase();
                var isGauge = ('airframes' === summaryType);

                var el = d3.select(ele[0]);
                var numSections = 3;
                var widgetInset = isGauge ? 20 : 25;
                var widgetWidth = widgetInset;

                /*
                 * In svg, 0%/100% is 0-degrees straight up, with the
                 * arc coordinates increasing clockwise.
                 */
                var ttlPct = isGauge ? 0.75 : 0;
                var margin = {
                  top: 0,
                  right: 0,
                  bottom: 0,
                  left: 0
                };
                var width = el.node().offsetWidth - (margin.left + margin.right);
                var height = el.node().offsetHeight - (margin.top + margin.bottom);
                var radius = width / (isGauge ? 1.75 : 2);
                var pctToDeg = function (pct) {
                  return pct * 360;
                };
                var pctToRad = function (pct) {
                  return degToRad(pctToDeg(pct));
                };
                var degToRad = function (deg) {
                  return deg * Math.PI / 180;
                };
                el.selectAll('svg').remove();
                var svg = el.append('svg')
                  .attr('width', width + margin.left + margin.right)
                  .attr('height', height + margin.top + margin.bottom);
                var dataVis = svg.append('g')
                  .attr('transform', 'translate(' + ((width + margin.left) / 2) + ', ' + ((height + margin.top) / 2) + ')');
                var arc;
                var arcStartRad;
                var arcEndRad;
                var i;
                var ref;
                var sectIdx;
                var sectPct;
                var sectMaxLevelScore;
                var pctOfCircle = isGauge ? 0.5 : 1.0;
                var sectCount;

                var gradient;
                var defs = svg.append('svg:defs');

                var shadow;
                var highlight;
                for (i = 1; i <= numSections; i++) {
                  switch (i) {
                    case 1:
                      shadow = isGauge ? 'red' : 'darkgreen';
                      highlight = isGauge ? '#FF6666' : '#00DD00';
                      break;
                    case 2:
                      shadow = isGauge ? '#FFCC00' : '#FFCC00';
                      highlight = isGauge ? '#FFE066' : '#FFE066';
                      break;
                    case 3:
                      shadow = isGauge ? 'darkgreen' : 'red';
                      highlight = isGauge ? '#00DD00' : '#FF6666';
                      break;
                    default:
                      break;
                  }
                  gradient = defs.append('svg:radialGradient')
                    .attr('gradientUnits', 'userSpaceOnUse')
                    .attr('id', 'gradRingLevel' + i)
                    .attr('r', '100%')
                    .attr('cx', '0')
                    .attr('cy', '0');
                  gradient.append('svg:stop')
                    .attr('offset', isGauge ? '31%' : '15%')
                    .attr('stop-color', shadow)
                    .attr('stop-opacity', 1);
                  gradient.append('svg:stop')
                    .attr('offset', isGauge ? '36%' : '25%')
                    .attr('stop-color', highlight)
                    .attr('stop-opacity', 1);
                  gradient.append('svg:stop')
                    .attr('offset', isGauge ? '41%' : '30%')
                    .attr('stop-color', shadow)
                    .attr('stop-opacity', 1);
                  if (!isGauge) {
                    gradient = defs.append('svg:radialGradient')
                      .attr('gradientUnits', 'userSpaceOnUse')
                      .attr('id', 'gradSphereLevel' + i)
                      .attr('r', '100%')
                      .attr('cx', '0')
                      .attr('cy', '0');
                    gradient.append('svg:stop')
                      .attr('offset', '5%')
                      .attr('stop-color', highlight)
                      .attr('stop-opacity', 1);
                    gradient.append('svg:stop')
                      .attr('offset', '12%')
                      .attr('stop-color', shadow)
                      .attr('stop-opacity', 1);
                  }
                }

                var cellColor;
                for (sectIdx = i = 1, ref = numSections; 1 <= ref ? i <= ref : i >= ref; sectIdx = 1 <= ref ? ++i : --i) {
                  switch (true) {
                    case (sectIdx === (isGauge ? d3Launch.gauge.level3.idxGauge : d3Launch.gauge.level3.idxRing)):
                      sectPct = isGauge ? d3Launch.gauge.level3.pct : data.level3Percent;
                      sectMaxLevelScore = common.maxLevelScore.level3;
                      if (!isGauge) {
                        sectCount = data.level3Count;
                      }
                      break;
                    case (sectIdx === (isGauge ? d3Launch.gauge.level2.idxGauge : d3Launch.gauge.level2.idxRing)):
                      sectPct = isGauge ? d3Launch.gauge.level2.pct : data.level2Percent;
                      sectMaxLevelScore = common.maxLevelScore.level2;
                      if (!isGauge) {
                        sectCount = data.level2Count;
                      }
                      break;
                    case (sectIdx === (isGauge ? d3Launch.gauge.level1.idxGauge : d3Launch.gauge.level1.idxRing)):
                      sectPct = isGauge ? d3Launch.gauge.level1.pct : data.level1Percent;
                      sectMaxLevelScore = common.maxLevelScore.level1;
                      if (!isGauge) {
                        sectCount = data.level1Count;
                      }
                      break;
                    default:
                      sectPct = d3Launch.gauge.unknown.pct;
                      sectMaxLevelScore = common.maxLevelScore.unknown;
                      if (!isGauge) {
                        sectCount = 0;
                      }
                      break;
                  }
                  sectPct = (sectPct / 100) * pctOfCircle;
                  if (sectPct >= 0) {
                    arcStartRad = pctToRad(ttlPct);
                    arcEndRad = arcStartRad + pctToRad(sectPct);
                    ttlPct += sectPct;
                    arc = d3.arc()
                      .outerRadius(radius - widgetInset)
                      .innerRadius(radius - (widgetInset + widgetWidth))
                      .startAngle(arcStartRad)
                      .endAngle(arcEndRad);
                    cellColor = isGauge ? common.getColorByScore(sectMaxLevelScore) : common.getColorByLevel(sectIdx);
                    dataVis.append('path')
                      .attr('class', 'ringLevel' + sectIdx)
                      .attr('d', arc)
                      .append('svg:title')
                      .text(function () {
                        return cellColor + (isGauge ? '' : ' count: ' + sectCount);
                      });
                  }
                }

                var widgetScore = parseInt(data.score) || 0;
                if (!isGauge) {
                  var cssClass = 'level' + common.getLevelByScore(widgetScore);

                  gradient = defs.append('svg:radialGradient')
                    .attr('gradientUnits', 'userSpaceOnUse')
                    .attr('id', 'gradSphereLevel0')
                    .attr('r', '100%')
                    .attr('cx', '0')
                    .attr('cy', '0');
                  gradient.append('svg:stop')
                    .attr('offset', '0%')
                    .attr('stop-color', '#F2F2F2')
                    .attr('stop-opacity', 1);
                  gradient.append('svg:stop')
                    .attr('offset', '25%')
                    .attr('stop-color', '#4D4D4D')
                    .attr('stop-opacity', 1);

                  dataVis.append('circle')
                    .attr('r', radius - (widgetInset + widgetWidth + 3))
                    .attr('cx', 0)
                    .attr('cy', 0)
                    .attr('class', 'sphereLevel' + (common.getLevelByScore(widgetScore) || 0))
                    .append('svg:title')
                    .text(function () {
                      return 'Average Score: ' + widgetScore;
                    });
                  dataVis.append('text')
                    .attr('x', 0)
                    .attr('y', 5)
                    .attr('text-anchor', 'middle')
                    .attr('class', cssClass + 'Text')
                    .text(widgetScore)
                    .append('svg:title')
                    .text(function () {
                      return 'Average Score: ' + widgetScore;
                    });
                  dataVis.append('text')
                    .attr('x', 0)
                    .attr('y', parseInt(widgetInset * 2.5))
                    .attr('text-anchor', 'middle')
                    .attr('class', 'widgetText')
                    .text(data.id);
                } else {
                  var Needle;
                  var needleLength = radius - widgetInset;
                  var needleRadius = parseInt((radius - widgetInset) / 10);

                  Needle = (function () {
                    function Needle(len, rad) {
                      this.len = len;
                      this.radius = rad;
                    }

                    Needle.prototype.drawOn = function (el, score) {
                      var pct = score / 100;
                      var needleClass = 'needle';
                      if (pct < 0 || pct > 1) {
                        needleClass += 'Warning';
                      }
                      el.append('circle')
                        .attr('r', this.radius)
                        .attr('cx', 0)
                        .attr('cy', 0)
                        .attr('class', needleClass + 'Center');
                      /*
                       * Since animateOn will position the needle,
                       * position it initially at 0.
                       */
                      return el.append('path')
                        .attr('class', needleClass)
                        .attr('d', this.calcPos(0));
                    };

                    Needle.prototype.animateOn = function (el, score) {
                      var pct = score / 100;
                      var self;
                      self = this;
                      return el.transition()
                        .duration(1000)
                        .delay(500)
                        .ease(d3.easeElasticOut)
                        .selectAll('.needle')
                        .tween('progress', function () {
                          return function (step) {
                            var progress;
                            progress = step * pct;
                            return el.selectAll('.needle')
                              .attr('d', self.calcPos(progress))
                              .append('svg:title')
                              .text(function () {
                                return common.getColorByScore(score) + ' Score: ' + score;
                              });
                          };
                        });
                    };

                    Needle.prototype.calcPos = function (pct) {
                      // Assures that the value is between 0 and 1.
                      switch (true) {
                        case (pct < 0):
                          pct = 0;
                          break;
                        case (pct > 1):
                          pct = 1;
                          break;
                        default:
                          // Do nothing.
                          break;
                      }
                      var thetaRad = pctToRad(pct / 2);
                      var centerX = 0;
                      var centerY = 0;
                      var topX = centerX - this.len * Math.cos(thetaRad);
                      var topY = centerY - this.len * Math.sin(thetaRad);
                      var leftX = centerX - this.radius * Math.cos(thetaRad - Math.PI / 2);
                      var leftY = centerY - this.radius * Math.sin(thetaRad - Math.PI / 2);
                      var rightX = centerX - this.radius * Math.cos(thetaRad + Math.PI / 2);
                      var rightY = centerY - this.radius * Math.sin(thetaRad + Math.PI / 2);
                      return 'M ' + leftX + ' ' + leftY + ' L ' + topX + ' ' + topY + ' L ' + rightX + ' ' + rightY;
                    };

                    return Needle;
                  })();

                  var needle = new Needle(needleLength, needleRadius);
                  needle.drawOn(dataVis, widgetScore);
                  needle.animateOn(dataVis, widgetScore);
                  dataVis.append('text')
                    .attr('x', 0)
                    .attr('y', radius - parseInt(widgetInset * 2.5))
                    .attr('text-anchor', 'middle')
                    .attr('class', 'widgetText')
                    .text(data.id);
                  dataVis.append('text')
                    .attr('x', 0)
                    .attr('y', radius - parseInt(widgetInset * 1.5))
                    .attr('text-anchor', 'middle')
                    .attr('class', 'widgetCallOut')
                    .text(data.airfield);

                }
              }
            };
          });
        }
      };
    }])
  .directive('d3Provision', [
    'd3Launch',
    'Common',
    function (
      d3Launch,
      common) {
      return {
        restrict: 'A',
        scope: {
          data: '=provisions'
        },
        link: function (scope, ele, attrs) {
          d3Launch.d3().then(function (d3) {

            scope.$watch('data', function (newData) {
              scope.render(newData);
            }, true);

            scope.render = function (data) {
              if (!data) {
                return;
              }

              var el = d3.select(ele[0]);
              el.selectAll('svg').remove();
              var svg = el.append('svg')
                .attr('width', el.node().offsetWidth)
                .attr('height', el.node().offsetHeight);

              angular.forEach(data, function (item) {
                var x;
                var y;
                var score = parseInt(item.score) || 0;
                switch (item.provision) {
                  case ('shield'):
                    x = d3Launch.provision.shield.x;
                    y = d3Launch.provision.shield.y;
                    break;
                  case ('power'):
                    x = d3Launch.provision.power.x;
                    y = d3Launch.provision.power.y;
                    break;
                  case ('droid'):
                    x = d3Launch.provision.droid.x;
                    y = d3Launch.provision.droid.y;
                    break;
                  case ('servo'):
                    x = d3Launch.provision.servo.x;
                    y = d3Launch.provision.servo.y;
                    break;
                  case ('sensor'):
                    x = d3Launch.provision.sensor.x;
                    y = d3Launch.provision.sensor.y;
                    break;
                  default:
                    x = undefined;
                    y = undefined;
                    break;
                }

                var provision = svg.append('g')
                  .attr('transform', 'translate(' + x + ', ' + y + ')');

                var cssClass = 'level' + common.getLevelByScore(score);
                var provisionSize = parseInt(attrs.provisionRadius) + parseInt((100 - score) / 4);
                provision.append('circle')
                  .attr('r', provisionSize)
                  .attr('cx', 0)
                  .attr('cy', 0)
                  .attr('class', cssClass + ' provision');
                provision.append('text')
                  .attr('x', 0)
                  .attr('y', parseInt(provisionSize / 4))
                  .attr('text-anchor', 'middle')
                  .attr('class', cssClass + 'Text')
                  .style('font-size', provisionSize + 'px')
                  .text(score + '');
                provision.append('svg:title')
                  .text(function () {
                    return item.provision.toUpperCase() + ' ' + common.getColorByScore(score) + ' Score: ' + score;
                  });
              });
            };
          });
        }
      };
    }]);
