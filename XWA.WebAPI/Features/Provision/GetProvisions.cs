using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;
using XWA.Core.Constants;
using XWA.WebAPI.Options;

namespace XWA.WebAPI.Features.Provision;

/// <summary>
/// This procedure gets the provision information, typically consumed during airframe processing.
/// </summary>
public class GetProvisions()
{
    /// <summary>
    /// The hierarchy-based provision execution method.
    /// 
    /// Unit testing can be performed and the complete user experience can be
    /// demonstrated in the absence of "real" data.
    /// </summary>
    /// <param name="csvProvisionFile">The provision CSV file path and name acquired from the upstream Options Pattern object.</param>
    /// <param name="provisionBiasOptions">The pilot-survivability provision bias values from the appsettings.json file.</param>
    /// <returns>The collection of provision response models.</returns>
    public static async Task<IList<ProvisionHierarchyResponse>> Execute(
        string csvProvisionFile,
        IOptions<ProvisionBiasOptions> provisionBiasOptions)
    {
        IList<ProvisionHierarchyResponse> results = [];
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
        using (StreamReader reader = new(csvProvisionFile))
        {
            using CsvReader csv = new(reader, config);

            IAsyncEnumerable<ProvisionHierarchyResponse> records = csv.GetRecordsAsync<ProvisionHierarchyResponse>();

            await foreach (ProvisionHierarchyResponse record in records)
            {
                switch (record.Type)
                {
                    case ProvisionTypes.SENSOR:
                        record.Bias = provisionBiasOptions.Value.Sensor;
                        break;
                    case ProvisionTypes.SERVO:
                        record.Bias = provisionBiasOptions.Value.Servo;
                        break;
                    case ProvisionTypes.DROID:
                        record.Bias = provisionBiasOptions.Value.Droid;
                        break;
                    case ProvisionTypes.POWER:
                        record.Bias = provisionBiasOptions.Value.Power;
                        break;
                    case ProvisionTypes.SHIELD:
                        record.Bias = provisionBiasOptions.Value.Shield;
                        break;
                    default:
                        // Do nothing.
                        break;
                }
                results.Add(record);
            }
        }

        // Do not sort the list in order to use the sort order embedded in the CSV.

        return results;
    }
}
