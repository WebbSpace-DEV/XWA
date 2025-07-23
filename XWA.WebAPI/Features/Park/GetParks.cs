using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using XWA.Core.Constants;

namespace XWA.WebAPI.Features.Park;

/// <summary>
/// This procedure gets the park information consumed for park visits.
/// </summary>
public class GetParks()
{
    /// <summary>
    /// The park execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="csvParkFile">The park comma-separated-values data file.</param>
    /// <param name="oconusFips">The bar-delimited collection of FIPS outside the continental US.</param>
    /// <returns>The collection of park response models.</returns>
    public static async Task<IList<ParkResponse>> Execute(
        string csvParkFile,
        string oconusFips)
    {
        IList<ParkResponse> results = [];

        IList<string> excludedFipsFactory = [.. oconusFips.Split(Global.ArrayDelimiter)];

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
        using (StreamReader reader = new(csvParkFile))
        {
            using CsvReader csv = new(reader, config);
            StringBuilder sb = new();
            IAsyncEnumerable<ParkResponse> records = csv.GetRecordsAsync<ParkResponse>();
            bool isExcludedFips;
            await foreach (ParkResponse record in records)
            {
                // Determine if this park should be OconusFips from the response.
                isExcludedFips = false;

                // The FIPS field in the CSV file may have multiple delimited entries.
                sb.Remove(0, sb.Length);
                sb.Append(record.Fips);
                foreach (string fipsCode in sb.ToString().Split(Global.ArrayDelimiter))
                {
                    isExcludedFips = excludedFipsFactory.Contains(fipsCode);
                    if (isExcludedFips)
                    {
                        break;
                    }
                }

                if (!isExcludedFips)
                {
                    // The region should be the friendly name, not the code -- but whatever.
                    ParkResponse parkResponse = new(
                        record.Id,
                        record.Name.Replace("National Park", "NP").Replace(" and Preserve", string.Empty),
                        record.Type,
                        record.Region,
                        record.Fips,
                        record.Visitors,
                        record.Acreage,
                        record.Latitude,
                        record.Longitude
                        );

                    results.Add(parkResponse);
                }
            }
        }

        return [.. results.OrderBy(r => r.Id)];
    }
}
