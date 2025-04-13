using System.ComponentModel;

namespace Tuxedo.Shared.Enums
{
    public enum Status
    {
        [Description("Forecasted")]
        Forecasted = 1,
        [Description("Confirmed")]
        Confirmed = 2,
        [Description("Cancelled")]
        Cancelled = 3
    }
}