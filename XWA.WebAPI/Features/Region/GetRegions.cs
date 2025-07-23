using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text;
using XWA.Core.Constants;

namespace XWA.WebAPI.Features.Region;

/// <summary>
/// This procedure gets the region information consumed in park visits.
/// </summary>
public class GetRegions()
{
    /// <summary>
    /// The region execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="csvRegionFile">The region comma-separated-values data file.</param>
    /// <returns>The collection of region response models.</returns>
    public static async Task<IList<RegionResponse>> Execute(
        string csvRegionFile)
    {
        IList<RegionResponse> results = [];

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
        using (StreamReader reader = new(csvRegionFile))
        {
            using CsvReader csv = new(reader, config);
            IAsyncEnumerable<RegionResponse> records = csv.GetRecordsAsync<RegionResponse>();
            await foreach (RegionResponse record in records)
            {
                results.Add(record);
            }
        }

        return [.. results.OrderBy(r => r.Name)];
    }
}
