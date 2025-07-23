namespace XWA.WebAPI.Options;

/// <summary>
/// The class for accessing the pilot-survivability provision bias values
/// from appsettings.json consumed using the Options Pattern.
/// </summary>
public class ProvisionBiasOptions
{
    /// <summary>
    /// This is the literal for the section key in appsettings.json.
    /// </summary>
    public const string Key = "ProvisionBias";

    /// <summary>
    /// The pilot-survivability bias for the sensor provision.
    /// </summary>
    public int Sensor { get; set; } = 1;

    /// <summary>
    /// The pilot-survivability bias for the servo provision.
    /// </summary>
    public int Servo { get; set; } = 1;

    /// <summary>
    /// The pilot-survivability bias for the astromech droid provision.
    /// </summary>
    public int Droid { get; set; } = 1;

    /// <summary>
    /// The pilot-survivability bias for the power provision.
    /// </summary>
    public int Power { get; set; } = 1;

    /// <summary>
    /// The pilot-survivability bias for the shield provision.
    /// </summary>
    public int Shield { get; set; } = 1;
}
