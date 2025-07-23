using CsvHelper.Configuration.Attributes;
using System.Text;
using XWA.Core.Constants;

namespace XWA.WebAPI.Features.Provision;

/// <summary>
/// The hierarchy-based provision response model class.
/// </summary>
public class ProvisionHierarchyResponse : ProvisionBase
{
    /// <summary>
    /// The constructor for the hierarchy-based provision response model.
    /// </summary>
    /// <param name="type">The type of the provision.</param>
    /// <param name="name">The name of the provision.</param>
    public ProvisionHierarchyResponse(
        ProvisionTypes type,
        string name) : base(type, name)
    {
        Type = type;
        Name = name;
    }

    /// <summary>
    /// The ordinal pilot-survivability bias of the provision.
    /// </summary>
    [Optional]
    [Name("bias")]
    public int Bias { get; set; }

    /// <summary>
    /// ToString() override, useful in debugging.
    /// </summary>
    public override string ToString()
    {
        StringBuilder sb = new(base.ToString());

        sb.Append($"{Bias,10:N0}");

        return sb.ToString();
    }
}
