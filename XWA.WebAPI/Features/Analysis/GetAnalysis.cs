using Microsoft.Extensions.Options;
using XWA.Core.Constants;
using XWA.WebAPI.Features.Airfield;
using XWA.WebAPI.Features.Airframe;
using XWA.WebAPI.Features.Fleet;
using XWA.WebAPI.Features.Flight;
using XWA.WebAPI.Features.Platform;
using XWA.WebAPI.Features.Provision;
using XWA.WebAPI.Features.Squadron;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Analysis;

/// <summary>
/// This procedure decomposes the fleet in order to aggregate metrics for analysis.
/// </summary>
public class GetAnalysis()
{
    private static readonly FleetAnalysisResponse _fleet = new();
    private static readonly IList<PlatformAnalysisResponse> _platforms = [];
    private static readonly IList<SquadronAnalysisResponse> _squadrons = [];
    private static readonly IList<AirframeAnalysisResponse> _airframes = [];
    private static readonly IList<ProvisionAnalysisResponse> _provisions = [];
    private static readonly IList<AirfieldAnalysisResponse> _airfields = [];
    private static readonly IList<FlightAnalysisResponse> _flights = [];

    // Create heap collections for "flattening" the hierarchy.
    private static readonly IList<PlatformHierarchyResponse> _platformsHeap = [];
    private static readonly IList<ProvisionHierarchyResponse> _provisionsHeap = [];

    private static FleetHierarchyResponse _fleetPrototype = new([]);
    /// <summary>
    /// The hierarchy-based analysis execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="platformsPrototype">The collection of hierarchy-based prototype platform objects.</param>
    /// <param name="provisionsPrototype">The collection of hierarchy-based prototype provision objects.</param>
    /// <param name="airfieldsPrototype">The collection of hierarchy-based prototype airfield objects.</param>
    /// <param name="collectionSizeOptions">The bar-delimited randomizable range of collection sizes from the appsettings.json file.</param>
    /// <param name="provisionScoreOptions">The bar-delimited randomizable range of provision scores from the appsettings.json file.</param>
    /// <returns>The analysis response model.</returns>
    public static async Task<AnalysisResponse> Execute(
        IList<PlatformHierarchyResponse> platformsPrototype,
        IList<ProvisionHierarchyResponse> provisionsPrototype,
        IList<AirfieldHierarchyResponse> airfieldsPrototype,
        IOptions<CollectionSizeOptions> collectionSizeOptions,
        IOptions<ProvisionScoreOptions> provisionScoreOptions)
    {
        // Get the fleet prototype.
        _fleetPrototype = await GetFleet.Execute(platformsPrototype, provisionsPrototype, airfieldsPrototype, collectionSizeOptions, provisionScoreOptions);

        ProcessSquadronsAndBelow();

        PostProcessPlatforms();

        PostProcessProvisions(provisionsPrototype);

        _ = _platforms.OrderBy(r => r.Id);
        _ = _squadrons.OrderBy(r => r.Id);
        _ = _airframes.OrderBy(r => $"{r.Platform}.{r.Squadron}.{r.Id}");
        _ = _flights.OrderBy(r => $"{r.Orig}.{r.Dest}");

        // The following console outputs are used for confirming functionality.
        Console.WriteLine($"{"Fleet",10}{_fleet}");

        Console.WriteLine($"\nPlatforms");
        foreach (PlatformAnalysisResponse platform in _platforms)
        {
            Console.WriteLine($"{platform}");
        }

        Console.WriteLine($"\nSquadrons");
        foreach (SquadronAnalysisResponse squadron in _squadrons)
        {
            Console.WriteLine($"{squadron}");
        }

        Console.WriteLine($"\nAirframes");
        foreach (AirframeAnalysisResponse airframe in _airframes)
        {
            Console.WriteLine($"{airframe}");
        }

        // This console output is used for confirming functionality.
        Console.WriteLine($"\nProvisions");
        foreach (ProvisionAnalysisResponse provision in _provisions)
        {
            Console.WriteLine($"{provision}");
        }

        Console.WriteLine($"\nAirfields");
        foreach (AirfieldAnalysisResponse airfield in _airfields)
        {
            Console.WriteLine($"{airfield}");
        }

        Console.WriteLine($"\nFlights");
        foreach (FlightAnalysisResponse flight in _flights)
        {
            Console.WriteLine($"{flight}");
        }

        return new(
            _fleetPrototype,
            _fleet,
            _platforms,
            _squadrons,
            _airframes,
            _provisions,
            _airfields,
            _flights);
    }

    private static void ProcessSquadronsAndBelow()
    {
        decimal squadronScore = 0M;
        decimal squadronCount = 0M;

        foreach (SquadronHierarchyResponse squadron in _fleetPrototype.Squadrons)
        {
            // Process the level-above fleet analysis.
            squadronScore += _fleetPrototype.Squadrons.Where(r => r.Id == squadron.Id).Sum(r => r.Score);
            squadronCount += _fleetPrototype.Squadrons.Where(r => r.Id == squadron.Id).Count();
            _fleet.Level1Count += _fleetPrototype.Squadrons.Where(r => r.Id == squadron.Id && r.Score > MaxLevelScore.Level_2 && r.Score <= MaxLevelScore.Level_1).Count();
            _fleet.Level2Count += _fleetPrototype.Squadrons.Where(r => r.Id == squadron.Id && r.Score > MaxLevelScore.Level_3 && r.Score <= MaxLevelScore.Level_2).Count();
            _fleet.Level3Count += _fleetPrototype.Squadrons.Where(r => r.Id == squadron.Id && r.Score > MaxLevelScore.Unknown && r.Score <= MaxLevelScore.Level_3).Count();

            _fleet.Level1Percent = Math.Round(_fleet.Level1Count * 100 / squadronCount, 0);
            _fleet.Level2Percent = Math.Round(_fleet.Level2Count * 100 / squadronCount, 0);
            _fleet.Level3Percent = Math.Round(_fleet.Level3Count * 100 / squadronCount, 0);

            _fleet.Score = Math.Round(squadronScore / squadronCount, 0);

            // Scaffold the this-level squadron analysis item(s).
            SquadronAnalysisResponse squadronAnalysis = new(squadron.Id);

            ProcessAirframesAndBelow(squadron, squadronAnalysis);

            _squadrons.Add(squadronAnalysis);

            // This console output is used for confirming functionality.
            Console.WriteLine($"{"Squadron",10}{squadronAnalysis}");

            ProcessAirfieldsAndBelow(squadron, squadronAnalysis);
        }
    }

    private static void ProcessAirframesAndBelow(SquadronHierarchyResponse squadron, SquadronAnalysisResponse squadronAnalysis)
    {
        decimal airframeScore = 0M;
        decimal airframeCount = 0M;

        foreach (AirframeHierarchyResponse airframe in squadron.Airframes)
        {
            // Process the level-above squadron analysis.
            airframeScore += squadron.Airframes.Where(r => r.Id == airframe.Id).Sum(r => r.Score);
            airframeCount += squadron.Airframes.Where(r => r.Id == airframe.Id).Count();
            squadronAnalysis.Level1Count += squadron.Airframes.Where(r => r.Id == airframe.Id && r.Score > MaxLevelScore.Level_2 && r.Score <= MaxLevelScore.Level_1).Count();
            squadronAnalysis.Level2Count += squadron.Airframes.Where(r => r.Id == airframe.Id && r.Score > MaxLevelScore.Level_3 && r.Score <= MaxLevelScore.Level_2).Count();
            squadronAnalysis.Level3Count += squadron.Airframes.Where(r => r.Id == airframe.Id && r.Score > MaxLevelScore.Unknown && r.Score <= MaxLevelScore.Level_3).Count();

            squadronAnalysis.Level1Percent = Math.Round(squadronAnalysis.Level1Count * 100 / airframeCount, 0);
            squadronAnalysis.Level2Percent = Math.Round(squadronAnalysis.Level2Count * 100 / airframeCount, 0);
            squadronAnalysis.Level3Percent = Math.Round(squadronAnalysis.Level3Count * 100 / airframeCount, 0);

            squadronAnalysis.Score = Math.Round(airframeScore / airframeCount, 0);

            // Scaffold the this-level airframe analysis item(s).
            AirframeAnalysisResponse airframeAnalysis = new(airframe.Id, airframe.Platform)
            {
                Squadron = squadron.Id,
                Airfield = squadron.Airfield.Id,
                Score = airframe.Score
            };

            PreProcessProvisions(airframe, airframeAnalysis);

            PreProcessPlatforms(airframe);

            _airframes.Add(airframeAnalysis);

            // This console output is used for confirming functionality.
            Console.WriteLine($"{"Airframe",10}{airframeAnalysis}");
        }
    }

    private static void PreProcessProvisions(AirframeHierarchyResponse airframe, AirframeAnalysisResponse airframeAnalysis)
    {
        foreach (ProvisionHierarchyResponse provision in airframe.Provisions)
        {
            // Process the provision heap collection, which is already scaffolded.

            // Add all provisions to the heap.
            _provisionsHeap.Add(provision);

            switch (provision.Type)
            {
                case ProvisionTypes.SENSOR:
                    airframeAnalysis.Sensor = provision.Score;
                    break;
                case ProvisionTypes.SERVO:
                    airframeAnalysis.Servo = provision.Score;
                    break;
                case ProvisionTypes.DROID:
                    airframeAnalysis.Droid = provision.Score;
                    break;
                case ProvisionTypes.POWER:
                    airframeAnalysis.Power = provision.Score;
                    break;
                case ProvisionTypes.SHIELD:
                    airframeAnalysis.Shield = provision.Score;
                    break;
                default:
                    // Do nothing.
                    break;
            }

            // Since the fleet object is complicated, this console output is used for atomic confirmation of functionality, generally in Excel.
            Console.WriteLine($"{"Provision",10}{airframe.Squadron,10}{airframe.Id,10}{provision}");
        }
    }

    private static void PreProcessPlatforms(AirframeHierarchyResponse airframe)
    {
        _platformsHeap.Add(new(airframe.Platform)
        {
            Score = airframe.Score
        });
    }

    private static void ProcessAirfieldsAndBelow(SquadronHierarchyResponse squadron, SquadronAnalysisResponse squadronAnalysis)
    {
        // Scaffold the this-level airfield analysis item using values from the squadron analysis, which is at the level above.
        AirfieldAnalysisResponse airfieldAnalysis = new(
            squadron.Airfield.Id,
            squadron.Airfield.Name,
            squadron.Airfield.City,
            squadron.Airfield.State,
            squadron.Airfield.Country,
            squadron.Airfield.Latitude,
            squadron.Airfield.Longitude)
        {
            Squadron = squadron.Id,
            Score = squadronAnalysis.Score,
            Level1Count = squadronAnalysis.Level1Count,
            Level2Count = squadronAnalysis.Level2Count,
            Level3Count = squadronAnalysis.Level3Count,
            Level1Percent = squadronAnalysis.Level1Percent,
            Level2Percent = squadronAnalysis.Level2Percent,
            Level3Percent = squadronAnalysis.Level3Percent
        };

        // The this-level airfield analysis needs no additional processing since the airframes took care of that already.

        ProcessFlights(squadron.Airfield);

        _airfields.Add(airfieldAnalysis);

        // This console output is used for confirming functionality.
        Console.WriteLine($"{"Airfield",10}{airfieldAnalysis}");
    }

    private static void ProcessFlights(AirfieldHierarchyResponse airfield)
    {
        foreach (FlightHierarchyResponse flight in airfield.Flights)
        {
            // The this-level flight collection needs no scaffolding.
            if (!_flights.Any(r => r.Orig == flight.Orig && r.Dest == flight.Dest))
            {
                _flights.Add(new(
                    flight.Orig,
                    flight.Dest,
                    flight.Count));
            }
        }
    }

    private static void PostProcessPlatforms()
    {
        // Scaffold the platform analysis collection, which can only be done after the entire fleet tree has been traversed.
        foreach (PlatformHierarchyResponse platform in _platformsHeap)
        {
            if (!_platforms.Any(r => r.Id == platform.Id))
            {
                _platforms.Add(new(
                    platform.Id)
                {
                    Level1Count = 0,
                    Level2Count = 0,
                    Level3Count = 0,
                    Level1Percent = 0,
                    Level2Percent = 0,
                    Level3Percent = 0
                });
            }
        }

        decimal platformScore = 0M;
        decimal platformCount = 0M;

        foreach (PlatformAnalysisResponse platform in _platforms)
        {
            platformScore = _platformsHeap.Where(r => r.Id == platform.Id).Sum(r => r.Score);
            platform.Level1Count = _platformsHeap.Where(r => r.Id == platform.Id && r.Score > MaxLevelScore.Level_2 && r.Score <= MaxLevelScore.Level_1).Count();
            platform.Level2Count = _platformsHeap.Where(r => r.Id == platform.Id && r.Score > MaxLevelScore.Level_3 && r.Score <= MaxLevelScore.Level_2).Count();
            platform.Level3Count = _platformsHeap.Where(r => r.Id == platform.Id && r.Score > MaxLevelScore.Unknown && r.Score <= MaxLevelScore.Level_3).Count();

            platformCount = platform.Level1Count + platform.Level2Count + platform.Level3Count;

            platform.Level1Percent = Math.Round(platform.Level1Count * 100 / platformCount, 0);
            platform.Level2Percent = Math.Round(platform.Level2Count * 100 / platformCount, 0);
            platform.Level3Percent = Math.Round(platform.Level3Count * 100 / platformCount, 0);

            platform.Score = Math.Round(platformScore / platformCount, 0);
        }
    }

    private static void PostProcessProvisions(IList<ProvisionHierarchyResponse> provisionsPrototype)
    {
        // Scaffold the provision analysis collection, which can only be done after the entire fleet tree has been traversed.
        foreach (ProvisionHierarchyResponse provision in provisionsPrototype)
        {
            _provisions.Add(new(
                provision.Type,
                provision.Name)
            {
                Level1Count = 0,
                Level2Count = 0,
                Level3Count = 0,
                Level1Percent = 0,
                Level2Percent = 0,
                Level3Percent = 0
            });
        }

        decimal provisionScore = 0M;
        decimal provisionCount = 0M;

        foreach (ProvisionAnalysisResponse provision in _provisions)
        {
            provisionScore = _provisionsHeap.Where(r => r.Type == provision.Type).Sum(r => r.Score);
            provision.Level1Count = _provisionsHeap.Where(r => r.Type == provision.Type && r.Score > MaxLevelScore.Level_2 && r.Score <= MaxLevelScore.Level_1).Count();
            provision.Level2Count = _provisionsHeap.Where(r => r.Type == provision.Type && r.Score > MaxLevelScore.Level_3 && r.Score <= MaxLevelScore.Level_2).Count();
            provision.Level3Count = _provisionsHeap.Where(r => r.Type == provision.Type && r.Score > MaxLevelScore.Unknown && r.Score <= MaxLevelScore.Level_3).Count();

            provisionCount = provision.Level1Count + provision.Level2Count + provision.Level3Count;

            provision.Level1Percent = Math.Round(provision.Level1Count * 100 / provisionCount, 0);
            provision.Level2Percent = Math.Round(provision.Level2Count * 100 / provisionCount, 0);
            provision.Level3Percent = Math.Round(provision.Level3Count * 100 / provisionCount, 0);

            provision.Score = Math.Round(provisionScore / provisionCount, 0);
        }
    }
}
