angular.module('xwa.parkVisitDirectives', [
  'xwa.d3',
  'xwa.common'
])
  .directive('d3ParkVisit', [
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
          filteredParks: '=parkVisitParks'
        },
        link: function (scope, ele, attrs) {
          d3Launch.d3().then(function (d3) {
            scope.$watch('filteredParks', function (data) {
              scope.render(data);
            }, true);

            scope.render = function (filteredParks) {

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
                  .scale(1000 * (parseFloat(attrs.parkVisitScale) || 1))
                  .translate([width / 2, height / 2]);

                var domainType = attrs.parkVisitDomain.toLowerCase(); // Acceptible values are "acreage" and "visitors".

                el.selectAll('svg').remove();
                var svg = el.append('svg')
                  .attr('viewBox', '0 0 ' + width + ' ' + height)
                  .attr('preserveAspectRatio', 'xMidYMid meet');

                var path = d3.geoPath()
                  .projection(projection)
                  .pointRadius(2.5);

                var voronoi = d3.voronoi()
                  .extent([[-1, -1], [width + 1, height + 1]]);

                var urlParkVisits = common.getServiceUrl('parkVisit/parkVisits');

                // Having experimented with various ways to get the JSON
                // payload, nothing works 100% except to use the endpoint URL.
                d3.queue()
                  .defer(d3.json, 'resources/d3/topo/us.json')
                  .defer(d3.json, urlParkVisits)
                  .await(ready);

                function ready(error, us, parkVisits) {
                  if (error)
                    throw error;

                  var parks = parkVisits.parks;

                  processParks(parks);

                  var max = 0;
                  parks.forEach(function (park) {
                    var val = 0;
                    switch (domainType) {
                      case 'visitors':
                        val = parseInt(park.visitors || 0);
                        break;
                      case 'acreage':
                        val = parseInt(park.acreage || 0);
                        break;
                      default:
                        // Do nothing.
                        break;
                    }
                    max = val > max ? val : max;
                  });

                  var radiusAnchor = d3.scaleSqrt()
                    .domain([0, max])
                    .range([0, 20]);

                  svg.append('g')
                    .attr('class', 'states')
                    .selectAll('.states')
                    .data(topojson.feature(us, us.objects.states).features
                      .filter(function (d) {
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

                  svg.append('g')
                    .attr('class', 'anchored')
                    .selectAll('.anchored')
                    .data(parks)
                    .enter()
                    .append('circle', '.anchored')
                    .attr('class', function (d) {
                      return 'park-size';
                    })
                    .attr('cx', function (d) {
                      return projection([d.longitude, d.latitude])[0];
                    })
                    .attr('cy', function (d) {
                      return projection([d.longitude, d.latitude])[1];
                    })
                    .attr('r', function (d) {
                      switch (domainType) {
                        case 'visitors':
                          return radiusAnchor(parseInt(d.visitors || 0));
                          break;
                        case 'acreage':
                          return radiusAnchor(parseInt(d.acreage || 0));
                          break;
                        default:
                          // Do nothing.
                          break;
                      }
                    })
                    .sort(function (a, b) {
                      switch (domainType) {
                        case 'visitors':
                          return parseInt(b.visitors || 0) - parseInt(a.visitors || 0);
                          break;
                        case 'acreage':
                          return parseInt(b.acreage || 0) - parseInt(a.acreage || 0);
                          break;
                        default:
                          // Do nothing.
                          break;
                      }
                    });

                  var selectedParks = [];

                  if (!common.isEmptyArray(filteredParks)) {
                    filteredParks.forEach(function (park) {
                      if (park['selected']) {
                        selectedParks.push(park);
                      }
                    });
                    filteredParks = filteredParks
                      .filter(function (d) {
                        return d.filtered;
                      });

                    svg.append('g')
                      .attr('class', 'filtered')
                      .selectAll('.filtered')
                      .data(filteredParks)
                      .enter()
                      .append('path', '.filtered')
                      .attr('class', 'filtered')
                      .attr('d', d3.symbol().type(d3.symbolCircle).size(8 * 8))  // "size" is an area in square pixels
                      .attr('transform', function (d) {
                        return 'translate(' + projection([d.longitude, d.latitude]) + ')';
                      })
                      .sort(function (a, b) {
                        switch (domainType) {
                          case 'visitors':
                            return parseInt(b.visitors || 0) - parseInt(a.visitors || 0);
                            break;
                          case 'acreage':
                            return parseInt(b.acreage || 0) - parseInt(a.acreage || 0);
                            break;
                          default:
                            // Do nothing.
                            break;
                        }
                      });
                  }

                  if (!common.isEmptyArray(selectedParks)) {
                    selectedParks.forEach(function (park) {
                      park['sort'] = String(parseInt(Math.abs(park.latitude * park.longitude) || 0));
                    });
                    selectedParks
                      .sort(function (a, b) {
                        return b.sort.localeCompare(a.sort);
                      });
                    var stepNmbr = 0;
                    var totlDist = 0;
                    var initName = '';
                    var initLat = Infinity;
                    var initLon = Infinity;
                    var origName = '';
                    var origLat = Infinity;
                    var origLon = Infinity;
                    var destName = '';
                    var destLat = Infinity;
                    var destLon = Infinity;
                    selectedParks.forEach(function (park) {
                      destName = park.name;
                      destLat = park.latitude;
                      destLon = park.longitude;
                      switch (true) {
                        case (Infinity === initLat):
                        case (Infinity === initLon):
                          initName = park.name;
                          initLat = park.latitude;
                          initLon = park.longitude;
                          break;
                        case (Infinity === origLat):
                        case (Infinity === origLon):
                        case (Infinity === destLat):
                        case (Infinity === destLon):
                          // Do nothing.
                          break;
                        default:
                          var calcDist = getCoordDist(origLat, origLon, destLat, destLon);
                          totlDist += calcDist;
                          stepNmbr += 1;
                          console.log('[' + String('00' + parseInt(stepNmbr)).slice(-2) + '] ' + String('0000' + parseInt(calcDist)).slice(-4) + ' miles from ' + origName + ' to ' + destName + '.');
                          break;
                      }
                      // Reset for next iteration.
                      origName = park.name;
                      origLat = destLat;
                      origLon = destLon;
                    });
                    switch (true) {
                      case (Infinity === initLat):
                      case (Infinity === initLon):
                      case (Infinity === origLat):
                      case (Infinity === origLon):
                      case (Infinity === destLat):
                      case (Infinity === destLon):
                        // Do nothing.
                        break;
                      default:
                        var calcDist = getCoordDist(destLat, destLon, initLat, initLon);
                        totlDist += calcDist;
                        stepNmbr += 1;
                        if (totlDist > 0) {
                          console.log('[' + String('00' + parseInt(stepNmbr)).slice(-2) + '] ' + String('0000' + parseInt(calcDist)).slice(-4) + ' miles from ' + destName + ' to ' + initName + '.');
                        }
                        break;
                    }
                    if (0 !== totlDist) {
                      console.log('==> Total distance : ' + parseInt(totlDist) + ' miles.');
                    }

                    svg.append('g')
                      .attr('class', 'selected')
                      .selectAll('.selected')
                      .data(selectedParks)
                      .enter()
                      .append('path', '.selected')
                      .attr('class', 'selected')
                      .attr('d', d3.symbol().type(d3.symbolStar).size(16 * 16))  // "size" is an area in square pixels
                      .attr('transform', function (d) {
                        return 'translate(' + projection([d.longitude, d.latitude]) + ')';
                      });
                  }

                  var parkRegion = svg.append('g')
                    .attr('class', 'region');

                  parkRegion
                    .append('path')
                    .datum({ type: 'MultiPoint', coordinates: parks })
                    .attr('d', path);

                  var parkPin = parkRegion.selectAll('.pinned')
                    .data(parks)
                    .enter()
                    .append('g', '.pinned')
                    .attr('class', 'pinned');

                  parkPin.append('title')
                    .text(function (d) {
                      var s = '';
                      s += d.name;
                      s += ' [ ';
                      switch (domainType) {
                        case 'visitors':
                          s += parseInt(d.visitors).toLocaleString() + ' visitors';
                          break;
                        case 'acreage':
                          s += parseInt(d.acreage).toLocaleString() + ' acres';
                          break;
                        default:
                          s += 'undefined';
                          break;
                      }
                      s += ' ]';
                      return s;
                    });

                  parkPin.append('path')
                    .data(voronoi.polygons(parks.map(projection)))
                    .attr('class', 'cell')
                    .attr('d', function (d) {
                      return d ? 'M' + d.join('L') + 'Z' : null;
                    });
                }

                function processParks(d) {
                  d.forEach(function (r) {
                    typePark(r);
                  });
                  return d;
                }

                function typePark(d) {
                  d[0] = +d.longitude;
                  d[1] = +d.latitude;
                  return d;
                }

                function getCoordDist(alphaLat, alphaLon, omegaLat, omegaLon) {
                  var degToRad = function (deg) {
                    return deg * Math.PI / 180;
                  };
                  var kmToMiles = function (km) {
                    return km * 0.621371;
                  };
                  var earthRadius = 6371; // Radius of the earth in kilometers.
                  var radLat = degToRad(omegaLat - alphaLat);
                  var radLon = degToRad(omegaLon - alphaLon);
                  // Application of the Haversine Formula...
                  var a =
                    Math.sin(radLat / 2) * Math.sin(radLat / 2) +
                    Math.cos(degToRad(alphaLat)) * Math.cos(degToRad(omegaLat)) *
                    Math.sin(radLon / 2) * Math.sin(radLon / 2);
                  // Return the distance in kilometers.
                  return kmToMiles(earthRadius * (2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a))));
                }
              }
            };
          });
        }
      };
    }]);
