using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using XWA.Core.Constants;
using XWA.WebAPI.Features.Flight;

namespace XWA.WebAPI.Features.Airfield;

/// <summary>
/// This procedure gets airfield information, typically consumed during squadron processing.
/// </summary>
public class GetAirfields()
{
    /// <summary>
    /// The hierarchy-based airfield execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="csvAirfieldFile">The airfield comma-separated-values data file.</param>
    /// <param name="flightsPrototype">The collection of hierarchy-based prototype flight objects.</param>
    /// <returns>The collection of airfield response models.</returns>
    public static async Task<IList<AirfieldHierarchyResponse>> Execute(
        string csvAirfieldFile,
        IList<FlightHierarchyResponse> flightsPrototype)
    {
        IList<AirfieldHierarchyResponse> results = [];

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
         * For Blazor pages, an alternative can be found here:
         * 
         * https://medium.com/@sametkayikci/automating-anti-forgery-protection-in-asp-net-8-mvc-applications-9d06252c24f5
         * 
         */
        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
            HasHeaderRecord = true,
            Delimiter = Global.CsvDelimiter.ToString()
        };

        // Note that reading from a FileSystemObject (StreamReader), as implemented, is unwise.
        using (StreamReader reader = new(csvAirfieldFile))
        {
            using CsvReader csv = new(reader, config);
            IAsyncEnumerable<AirfieldBase> records = csv.GetRecordsAsync<AirfieldBase>();
            await foreach (AirfieldBase record in records)
            {
                // Only include flights that originate from or terminate at this airfield.
                IList<FlightHierarchyResponse> flights = [.. flightsPrototype.Where(r =>
                    r.Orig == record.Id ||
                    r.Dest == record.Id)];
                if (flights.Any())
                {
                    _ = flights.OrderBy(r => $"{r.Orig,-3}.{r.Dest,-3}");

                    results.Add(new(
                        record.Id,
                        record.Name,
                        record.City,
                        record.State,
                        record.Country,
                        record.Latitude,
                        record.Longitude,
                        flights
                        ));
                }
            }
        }

        return [.. results.OrderBy(r => r.Id)];
    }
}
