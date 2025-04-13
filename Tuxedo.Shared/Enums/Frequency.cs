using System.ComponentModel;

namespace Tuxedo.Shared.Enums
{
    public enum Frequency
    {
        [Description("One-off")]
        OneOff = 1,
        [Description("Monthly")]
        Monthly = 2,
        [Description("Quarterly")]
        Quarterly = 3,
        [Description("Annual")]
        Annual = 4,
    }
}
