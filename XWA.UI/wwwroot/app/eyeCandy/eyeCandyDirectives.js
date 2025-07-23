angular.module('xwa.eyeCandyDirectives', [
  'xwa.d3',
  'xwa.common'
])
  .directive('d3Logo', [
    'd3Launch',
    function (
      d3Launch) {
      return {
        restrict: 'A',
        scope: {},
        link: function (scope, ele, attrs) {
          d3Launch.d3().then(function (d3) {

            var el = d3.select(ele[0]);
            var margin = {
              top: 0,
              right: 0,
              bottom: 0,
              left: 0
            };
            var width = el.node().offsetWidth - (margin.left + margin.right);
            var height = el.node().offsetHeight - (margin.top + margin.bottom);
            el.selectAll('svg').remove();
            var svg = el.append('svg')
              .attr('viewBox', '0 0 ' + width + ' ' + height);
            var group = svg.append('g')
              .attr('transform', 'scale(' + (parseFloat(attrs.logoScale) || 1) + ')');

            var dataPoints = [
              'M 7.42 145.986',
              'C 9.185 99.193 32.899 56.035 76.25 27.516',
              'c 0.128 0.048 1.251 -0.361 0.738 0.61',
              'c -3.434 3.184 -65.172 76.114 -8.344 133.68',
              'c 0 0 29.858 28.704 53.011 1.468',
              'c 0 0 22.847 -29.577 -0.289 -74.413',
              'c 0 0 -5.856 -14.64 -26.955 -23.721',
              'l 16.992 -18.748',
              'c 0 0 14.359 6.161 25.478 22.871',
              'c 0 0 0.593 -17.593 -12.884 -36.34',
              'l 26.345 -29.89',
              'l 26.08 29.609',
              'c 0 0 -11.993 16.991 -12.876 36.902',
              'c 0 0 8.191 -13.477 25.776 -23.151',
              'l 16.686 18.748',
              'c 0 0 -16.045 5.287 -26.794 23.529',
              'c -9.242 16.902 -16.357 53.05 0.416 75.223',
              'c 0 0 18.772 26.618 51.792 -1.571',
              'c 0 0 60.712 -54.399 -6.226 -133.048',
              'c 0 0 -3.658 -3.233 0.449 -1.476',
              'c 29.586 21.54 65.012 49.946 68.67 120.837',
              'c -1.444 85.966 -59.012 147.334 -143.074 147.334',
              'C 68.934 295.968 4.95 227.283 7.42 145.986'
            ];

            group.append('path')
              .attr('d', dataPoints.join(' '))
              .attr('class', attrs.logoClass)
              .append('svg:title')
              .text(function () {
                return 'logo';
              });
          });
        }
      };
    }])
  .directive('d3Resizer', [
    '$window',
    'd3Launch',
    '$timeout',
    function (
      $window,
      d3Launch,
      $timeout) {
      return {
        restrict: 'A',
        scope: {},
        link: function (scope, ele, attrs) {
          d3Launch.d3().then(function (d3) {

            $timeout(function () {
              onResize();
            }, parseInt(attrs.resizeDelay) || 100);

            function onResize() {
              var el = d3.select(ele[0]);
              var margin = {
                top: 0,
                right: 0,
                bottom: 0,
                left: 0
              };
              var width = el.node().offsetWidth - (margin.left + margin.right);
              var height = el.node().offsetHeight - (margin.top + margin.bottom);

              el.selectAll('svg').remove();
              var svg = el.append('svg')
                .attr('viewBox', '0 0 ' + width + ' ' + height);

              // Draw an X to show that the size is correct.
              var lines = svg.selectAll('line').data([
                {x1: 0, y1: 0, x2: width, y2: height},
                {x1: 0, y1: height, x2: width, y2: 0}
              ]);
              lines
                .enter()
                .append('line')
                .style('stroke-width', 50)
                .style('stroke-opacity', 0.4)
                .style('stroke', 'black')
                .merge(lines)
                .attr('x1', function (d) {
                  return d.x1;
                })
                .attr('y1', function (d) {
                  return d.y1;
                })
                .attr('x2', function (d) {
                  return d.x2;
                })
                .attr('y2', function (d) {
                  return d.y2;
                });
            }

            function cleanUp() {
              angular.element($window).off('resize', onResize);
            }

            angular.element($window).on('resize', onResize);

            scope.$on('$destroy', cleanUp);
          });
        }
      };
    }])
  .directive('d3Sphere', [
    'd3Launch',
    '$timeout',
    function (
      d3Launch,
      $timeout) {
      return {
        restrict: 'A',
        scope: {},
        link: function (scope, ele, attrs) {
          d3Launch.d3().then(function (d3) {

            $timeout(function () {
              onLoad();
            }, parseInt(attrs.resizeDelay) || 100);

            function onLoad() {
              var isWireframe = ('true' === attrs.sphereIsWireframe);

              var el = d3.select(ele[0]);
              var margin = {
                top: 0,
                right: 0,
                bottom: 0,
                left: 0
              };
              var width = el.node().offsetWidth - (margin.left + margin.right);
              var height = el.node().offsetHeight - (margin.top + margin.bottom);

              el.selectAll('svg').remove();
              var svg = el.append('svg')
                .attr('viewBox', '0 0 ' + width + ' ' + height);
              var group = svg.append('g');

              var scale = 0.90 * Math.min(width, height) / 2;

              var rotate = [0, 0];
              var velocity = [.006, .006];

              var projection = d3.geoOrthographic()
                .scale(scale)
                .clipAngle(isWireframe ? 0 : 90)
                .translate([width / 2, height / 2]);

              var path = d3.geoPath()
                .projection(projection);

              var graticule = d3.geoGraticule()
                .step([12, 12]);

              var outline = '#4D4D4D';
              var shadow = '#333333';
              var highlight = 'white';
              group.append('circle')
                .attr('cx', width / 2)
                .attr('cy', height / 2)
                .attr('r', scale)
                .attr('class', 'sphere')
                .style('stroke', outline)
                .style('stroke-width', '1px');

              var gradient;
              var defs = svg.append('svg:defs');

              gradient = defs.append('svg:radialGradient')
                .attr('id', 'gradSphere')
                .attr('r', '100%')
                .attr('cx', '50%')
                .attr('cy', '25%');
              gradient.append('svg:stop')
                .attr('offset', '5%')
                .attr('stop-color', highlight)
                .attr('stop-opacity', 1);
              gradient.append('svg:stop')
                .attr('offset', '100%')
                .attr('stop-color', shadow)
                .attr('stop-opacity', 1);

              var feature = group.append('path')
                .style('stroke', outline)
                .style('fill', 'none')
                .datum(graticule)
                .attr('d', path);

              d3.timer(function (elapsed) {
                projection.rotate([rotate[0] + elapsed * velocity[0], rotate[1] + elapsed * velocity[1]]);
                feature.attr('d', path);
              });
            }
          });
        }
      };
    }]);
