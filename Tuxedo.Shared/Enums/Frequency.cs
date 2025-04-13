using System.ComponentModel;

namespace Tuxedo.Shared.Enums
{
    public enum Frequency
    {
        [Description("One-off")]
        OneOff = 1,
        Monthly = 2,
        Quarterly = 3,
        Annual = 4,
    }
}
