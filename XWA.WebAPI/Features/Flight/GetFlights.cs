using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using XWA.Core.Constants;

namespace XWA.WebAPI.Features.Flight;

/// <summary>
/// This procedure gets the flight information, typically consumed during airfield processing.
/// </summary>
public class GetFlights()
{
    /// <summary>
    /// The hierarchy-based flight execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="csvFlightFile">The flight comma-separated-values data file.</param>
    /// <returns>The collection of flight response models.</returns>
    public static async Task<IList<FlightHierarchyResponse>> Execute(
        string csvFlightFile)
    {
        IList<FlightHierarchyResponse> results = [];
        /*
         *
         * After toying with mapper classes, I chose to use the CsvHelper
         * [Name] attribute in the response class instead. This approach
         * seems more consistent with the "minimal" approach.
         *
         * Additional information about mapping CSV column headers with
         * the class fields can be found here:
         *
         * https://foxlearn.com/csharp/csvhelper-header-with-name-not-found-8672.html
         *
         */
        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
            HasHeaderRecord = true,
            Delimiter = Global.CsvDelimiter.ToString()
        };

        // Note that reading from a FileSystemObject (StreamReader), as implemented, is unwise.
        using (StreamReader reader = new(csvFlightFile))
        {
            using CsvReader csv = new(reader, config);
            IAsyncEnumerable<FlightHierarchyResponse> records = csv.GetRecordsAsync<FlightHierarchyResponse>();
            await foreach (FlightHierarchyResponse record in records)
            {
                results.Add(record);
            }
        }

        return [.. results.OrderBy(r => $"{r.Orig,-3}.{r.Dest,-3}")];
    }
}
