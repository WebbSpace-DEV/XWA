namespace XWA.Core.Constants;

public struct Global
{
    public const short InvalidId = 0;
    public const string InvalidCode = "0";
    public const string InvalidGuid = "00000000-0000-0000-0000-000000000000";
    public const string InvalidUri = "https://localhost";
    public const string DateTemplate = "yyyy-MM-dd";
    public const string XmlTrue = "true";
    public const string NewLine = "<br/>";
    public const char ArrayDelimiter = '|';
    public const char CsvDelimiter = ',';
    public const string WhiteList = "AllowAll";
}

public struct MaxLevelScore
{
    public const int Unknown = -1;
    public const int Level_3 = 34;
    public const int Level_2 = 49;
    public const int Level_1 = 100;
}

public enum ProvisionTypes
{
    SENSOR,
    SERVO,
    DROID,
    POWER,
    SHIELD
}

public enum SortTypes
{
    Unknown,
    Asc,
    Desc
}

public enum PayloadTypes
{
    Unknown,
    Json,
    Xml,
    Csv
}
