using System.Text;
using XWA.Core.Constants;

namespace XWA.Core.Helpers;

public static class Utility
{
    public static int GetRandomBetween(string delimitedMinMax)
    {
        List<int>? limits = delimitedMinMax?.Split(Global.ArrayDelimiter)?.Select(Int32.Parse)?.ToList();
        int min = 0;
        int max = 0;
        if (limits != null && limits.Count.Equals(2))
        {
            limits.Sort();
            min = limits[0];
            max = limits[1];
            return GetRandomBetween(min, max);
        } else
        {
            return 0;
        }
    }

    public static int GetRandomBetween(int min, int max)
    {
        return (new Random()).Next(min, max++);
    }

    public static string GetMockSignature(int index, int length, bool hasDash = true)
    {
        StringBuilder sb = new();
        string guidAsString = Guid.NewGuid().ToString();
        if (hasDash)
        {
            sb.Append(guidAsString);
        }
        else
        {
            sb.Append(guidAsString.Replace("-", GetRandomBetween(min: 0, max: 9).ToString()));
        }
        return sb.ToString().Substring(index, length).ToUpperInvariant();
    }

    public static string GetLevel(decimal score)
    {
        string level = string.Empty;

        if (score is > MaxLevelScore.Unknown and <= MaxLevelScore.Level_3)
        {
            level = "L3";
        }
        if (score is > MaxLevelScore.Level_3 and <= MaxLevelScore.Level_2)
        {
            level = "L2";
        }
        if (score is > MaxLevelScore.Level_2 and <= MaxLevelScore.Level_1)
        {
            level = "L1";
        }

        return level;
    }
}
