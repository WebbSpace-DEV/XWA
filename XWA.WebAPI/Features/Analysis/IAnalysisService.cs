namespace XWA.WebAPI.Features.Analysis;

/// <summary>
/// The analysis service interface.
/// </summary>
public interface IAnalysisService
{
    /// <summary>
    /// The analysis interface signature.
    /// </summary>
    /// <returns>The analysis response model.</returns>
    Task<AnalysisResponse> GetAnalysisAsync();
}
