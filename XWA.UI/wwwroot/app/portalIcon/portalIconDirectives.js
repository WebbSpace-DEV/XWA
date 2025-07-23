angular.module('xwa.portalIconDirectives', [
  'xwa.d3',
  'xwa.common'
])
  .directive('d3PortalIcon', [
    'd3Launch',
    '$timeout',
    function (
      d3Launch,
      $timeout) {
      return {
        restrict: 'A',
        scope: {
          data: '=iconData'
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

              $timeout(function () {
                onLoad();
              }, parseInt(attrs.resizeDelay) || 100);

              function onLoad() {
                var el = d3.select(ele[0]);
                var padding = 2;
                var canvas = {
                  width: el.node().offsetWidth,
                  height: el.node().offsetHeight
                };
                var graphic = {
                  width: canvas.width - (2 * padding),
                  height: canvas.width - (2 * padding)
                };
                el.selectAll('svg').remove();
                var svg = el.append('svg')
                  .attr('viewBox', '0 0 ' + canvas.width + ' ' + canvas.height);
                var icon = svg.append('g');
                var defs = svg.append('svg:defs');

                var shadow = '#999';
                var highlight = '#EEE';
                var gradient = defs.append('svg:linearGradient')
                  .attr('gradientUnits', 'objectBoundingBox')
                  .attr('id', 'virtualLight')
                  .attr('x1', 0)
                  .attr('y1', 0)
                  .attr('x2', 0)
                  .attr('y2', 1);
                gradient.append('svg:stop')
                  .attr('offset', 0)
                  .attr('stop-color', highlight)
                  .attr('stop-opacity', 0.6);
                gradient.append('svg:stop')
                  .attr('offset', 0.4)
                  .attr('stop-color', shadow)
                  .attr('stop-opacity', 0.5);

                icon.append('rect')
                  .attr('rx', 8)
                  .attr('ry', 8)
                  .attr('x', padding)
                  .attr('y', padding)
                  .attr('width', graphic.width)
                  .attr('height', graphic.height)
                  .attr('class', 'portalIcon')
                  .style('fill', data.color);

                icon.append('rect')
                  .attr('class', 'portalIconLight')
                  .attr('rx', 6)
                  .attr('ry', 6)
                  .attr('x', padding + 2)
                  .attr('y', padding + 2)
                  .attr('width', graphic.width - 4)
                  .attr('height', graphic.height - 4);

                var words;
                words = data.shortTitle.trim().split(/\s+/);
                var xPos = parseInt((graphic.width + (2 * padding)) / 2);
                var yPos = parseInt(((graphic.height - 15) / (words.length + 1)));
                if (yPos > 0) {
                  var text = icon.append('text')
                    .attr('x', xPos)
                    .attr('y', yPos)
                    .attr('text-anchor', 'middle')
                    .attr('class', 'portalIconText');
                  words.forEach(function (word) {
                    text.append('tspan')
                      .attr('x', xPos)
                      .attr('dy', '0.9em')
                      .text(word);
                  });
                }
                words = data.longTitle.trim().split(/\s+/);
                var showLabel = canvas.height >= (graphic.height + (15 * words.length) + 7.5);
                if (showLabel) {
                  var xPos = parseInt((graphic.width + (2 * padding)) / 2);
                  var yPos = graphic.height + 7.5;
                  if (yPos > 0) {
                    var text = icon.append('text')
                      .attr('x', xPos)
                      .attr('y', yPos)
                      .attr('text-anchor', 'middle')
                      .attr('class', 'portalIconLabel');
                    words.forEach(function (word) {
                      text.append('tspan')
                        .attr('x', xPos)
                        .attr('dy', '1.1em')
                        .text(word);
                    });
                  }
                }
              }
            };
          });
        }
      };
    }]);
