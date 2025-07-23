namespace XWA.WebAPI.Features.Flight;

/// <summary>
/// The flight analysis response model class.
/// </summary>
public class FlightAnalysisResponse : FlightBase
{
    /// <summary>
    /// The constructor for the flight analysis response model.
    /// </summary>
    /// <param name="orig">The origination IATA for the flight.</param>
    /// <param name="dest">The destination IATA for the flight.</param>
    /// <param name="count">The count of flights between the origination IATA and the destination IATA.</param>
    public FlightAnalysisResponse(
        string orig,
        string dest,
        int count) : base(orig, dest, count)
    {
        Orig = orig;
        Dest = dest;
        Count = count;
    }

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        return base.ToString();
    }
}
