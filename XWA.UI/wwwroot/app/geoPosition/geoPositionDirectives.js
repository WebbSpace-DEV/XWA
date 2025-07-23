angular.module('xwa.geoPositionDirectives', [
  'xwa.d3',
  'xwa.common'
])
  .directive('d3GeoPosition', [
    'd3Launch',
    '$timeout',
    'Common',
    function (
      d3Launch,
      $timeout,
      common) {
      return {
        restrict: 'A',
        scope: {
          geoPosData: '=geoPosData',
          airfieldData: '=airfieldData',
          flightData: '=flightData'
        },
        link: function (scope, ele, attrs) {
          d3Launch.d3().then(function (d3) {
            scope.$watch('geoPosData', function (data) {
              scope.render(data);
            }, true);

            scope.render = function (geoPosData) {
              if (!geoPosData) {
                return;
              }

              $timeout(function () {
                onLoad();
              }, parseInt(attrs.resizeDelay) || 100);

              function onLoad() {
                var el = d3.select(ele[0]);
                var margin = {
                  top: 0,
                  right: 0,
                  bottom: 0,
                  left: 0
                };
                var width = el.node().parentElement.offsetWidth - (margin.left + margin.right);
                var height = el.node().parentElement.offsetHeight - (margin.top + margin.bottom);

                var projection = d3.geoAlbersUsa()
                  .scale(1000 * (parseFloat(attrs.geoPositionScale) || 1))
                  .translate([width / 2, height / 2]);

                el.selectAll('svg').remove();
                var svg = el.append('svg')
                  .attr('viewBox', '0 0 ' + width + ' ' + height)
                  .attr('preserveAspectRatio', 'xMidYMid meet');

                var path = d3.geoPath()
                  .projection(projection)
                  .pointRadius(2.5);

                var voronoi = d3.voronoi()
                  .extent([[-1, -1], [width + 1, height + 1]]);

                var urlAirfields = common.getServiceUrl('analysis/airfields');

                // Having experimented with various ways to get the JSON
                // payload, nothing works 100% except to use the endpoint URL.
                d3.queue()
                  .defer(d3.json, 'resources/d3/topo/us.json')
                  .defer(d3.json, urlAirfields)
                  .await(ready);

                function ready(error, us, airfields) {
                  if (error)
                    throw error;

                  var flights = scope.flightData;

                  airfields.forEach(function (r) {
                    typeAirfield(r);
                  });

                  flights.forEach(function (r) {
                    typeFlight(r);
                  });

                  var airfieldByIata = d3.map(airfields, function (d) {
                    return d.id;
                  });

                  var airfieldDomainByIata = d3.map(scope.airfieldData, function (d) {
                    return d.id;
                  });

                  flights.forEach(function (flight) {
                    var source = airfieldByIata.get(flight.orig);
                    var target = airfieldByIata.get(flight.dest);
                    source.arcs.coordinates.push([source, target]);
                    target.arcs.coordinates.push([target, source]);
                  });

                  airfields = airfields.filter(function (d) {
                    return d.arcs.coordinates.length;
                  });

                  svg.append('g')
                    .attr('class', 'states')
                    .selectAll('.states')
                    .data(topojson.feature(us, us.objects.states).features.filter(function (d) {
                      return common.isOconus(d.id);
                    }))
                    .enter()
                    .append('path', '.states')
                    .attr('d', path);

                  svg.append('g')
                    .attr('class', 'state-borders')
                    .append('path')
                    .attr('d', path(topojson.mesh(us, us.objects.states, function (a, b) {
                      return a !== b;
                    })));

                  var max = 0;
                  airfields.forEach(function (airfield) {
                    var val = parseInt(airfield.arcs.coordinates.length || 0);
                    max = val > max ? val : max;
                  });

                  var radiusAnchor = d3.scaleSqrt()
                    .domain([0, max])
                    .range([0, 20]);

                  svg.append('g')
                    .attr('class', 'anchored')
                    .selectAll('.anchored')
                    .data(airfields)
                    .enter()
                    .append('circle', '.anchored')
                    .attr('cx', function (d) {
                      return projection([d.longitude, d.latitude])[0];
                    })
                    .attr('cy', function (d) {
                      return projection([d.longitude, d.latitude])[1];
                    })
                    .attr('r', function (d) {
                      return radiusAnchor(d.arcs.coordinates.length);
                    })
                    .sort(function (a, b) {
                      return b.arcs.coordinates.length - a.arcs.coordinates.length;
                    });

                  svg.append('g')
                    .attr('class', 'filtered')
                    .selectAll('.filtered')
                    .data(geoPosData)
                    .enter()
                    .append('path', '.filtered')
                    .attr('class', function (d) {
                      return 'level' + common.getLevelByScore(d.score);
                    })
                    .attr('d', d3.symbol().type(d3.symbolStar).size(16 * 16))  // "size" is an area in square pixels
                    .attr('transform', function (d) {
                      return 'translate(' + projection([d.longitude, d.latitude]) + ')';
                    })
                    .sort(function (a, b) {
                      return (100 - b.score) - (100 - a.score);
                    });

                  var airfieldRegion = svg.append('g')
                    .attr('class', 'region');

                  airfieldRegion
                    .append('path')
                    .datum({type: 'MultiPoint', coordinates: airfields})
                    .attr('d', path);

                  var airfieldPin = airfieldRegion.selectAll('.pinned')
                    .data(airfields)
                    .enter()
                    .append('g', '.pinned')
                    .attr('class', 'pinned');

                  airfieldPin.append('title')
                    .text(function (d) {
                      var s = '';
                      if (airfieldDomainByIata.has(d.id)) {
                        var airfield = scope.airfieldData.filter(function (lookup) {
                          return lookup.id === d.id;
                        });
                        s += 'Squadron ' + airfield[0].squadron + ': ';
                      }
                      s += d.arcs.coordinates.length + ' flights ';
                      s += 'to/from ' + d.city + ', ' + d.state + ' ';
                      s += '(' + d.id + ')';
                      return s;
                    });

                  airfieldPin.append('path')
                    .attr('class', function (d) {
                      return (airfieldDomainByIata.has(d.id) ? 'squadron' : 'airfield') + '-connector';
                    })
                    .attr('d', function (d) {
                      return path(d.arcs);
                    });

                  airfieldPin.append('path')
                    .data(voronoi.polygons(airfields.map(projection)))
                    .attr('class', 'cell')
                    .attr('d', function (d) {
                      return d ? 'M' + d.join('L') + 'Z' : null;
                    });
                }

                function typeAirfield(r) {
                  r[0] = +r.longitude;
                  r[1] = +r.latitude;
                  r.arcs = { type: 'MultiLineString', coordinates: [] };
                  return r;
                }

                function typeFlight(r) {
                  r.count = +r.count;
                  return r;
                }
              }
            };
          });
        }
      };
    }]);
