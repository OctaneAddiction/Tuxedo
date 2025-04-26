using System.ComponentModel;

namespace Tuxedo.Shared.Enums;

public enum EndCondition
{
    [Description("No end (continuous)")]
    NoEndDate = 0,
    
    [Description("On specific date")]
    EndDate = 1
}
