using CsvHelper.Configuration.Attributes;
using System.Text;
using static XWA.Core.Helpers.Utility;

namespace XWA.WebAPI.Features.Airframe;

/// <summary>
/// The airframe analysis response model class.
/// </summary>
public class AirframeAnalysisResponse : AirframeBase
{
    /// <summary>
    /// The constructor for the airframe analysis response model.
    /// </summary>
    /// <param name="id">The id of the airframe.</param>
    /// <param name="platform">The platform on which the airframe is based.</param>
    public AirframeAnalysisResponse(
        string id,
        string platform) : base(id, platform)
    {
        Id = id;
        Platform = platform;
    }

    /// <summary>
    /// The squadron associated with the airframe.
    /// </summary>
    [Name("squadron")]
    public string Squadron { get; set; } = string.Empty;

    /// <summary>
    /// The airfield associated with the airframe.
    /// </summary>
    [Name("airfield")]
    public string Airfield { get; set; } = string.Empty;

    /// <summary>
    /// The sensor provision associated with the airframe.
    /// </summary>
    [Name("sensor")]
    public decimal Sensor { get; set; } = 0M;

    /// <summary>
    /// The servo provision associated with the airframe.
    /// </summary>
    [Name("servo")]
    public decimal Servo { get; set; } = 0M;

    /// <summary>
    /// The astromech droid provision associated with the airframe.
    /// </summary>
    [Name("droid")]
    public decimal Droid { get; set; } = 0M;

    /// <summary>
    /// The power provision associated with the airframe.
    /// </summary>
    [Name("power")]
    public decimal Power { get; set; } = 0M;

    /// <summary>
    /// The shield provision associated with the airframe.
    /// </summary>
    [Name("shield")]
    public decimal Shield { get; set; } = 0M;

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        StringBuilder sb = new(base.ToString());

        sb.Append($"{Squadron,10}");
        sb.Append($"{Airfield,10}");
        sb.Append($"{GetLevel(Sensor),10}");
        sb.Append($"{Sensor,10:N0}");
        sb.Append($"{GetLevel(Servo),10}");
        sb.Append($"{Servo,10:N0}");
        sb.Append($"{GetLevel(Droid),10}");
        sb.Append($"{Droid,10:N0}");
        sb.Append($"{GetLevel(Power),10}");
        sb.Append($"{Power,10:N0}");
        sb.Append($"{GetLevel(Shield),10}");
        sb.Append($"{Shield,10:N0}");

        return sb.ToString();
    }
}
