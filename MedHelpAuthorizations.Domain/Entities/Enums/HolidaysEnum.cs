using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Shared.Attributes;
using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum HolidaysEnum
    {
        [Description("New Year's Day (January 1)")]
        [Month(1)]
        NewYearsDay = 1,

        [Description("Birthday of Martin Luther King, Jr. (Third Monday in January)")]
        [Month(1)]
        MLKDay,

        [Description("Inauguration Day (January 20, every 4 years following a presidential election)")]
        [Month(1)]
        InaugurationDay,

        [Description("Washington's Birthday (Also known as Presidents Day; third Monday in February)")]
        [Month(2)]
        WashingtonsBirthday,

        [Description("Memorial Day (Last Monday in May)")]
        [Month(5)]
        MemorialDay,

        [Description("Juneteenth National Independence Day (June 19)")]
        [Month(6)]
        Juneteenth,

        [Description("Independence Day (July 4)")]
        [Month(7)]
        IndependenceDay,

        [Description("Labor Day (First Monday in September)")]
        [Month(9)]
        LaborDay,

        [Description("Columbus Day (Second Monday in October)")]
        [Month(10)]
        ColumbusDay,

        [Description("Veterans Day (November 11)")]
        [Month(11)]
        VeteransDay,

        [Description("Thanksgiving Day (Fourth Thursday in November)")]
        [Month(11)]
        ThanksgivingDay,

        [Description("Christmas Day (December 25)")]
        [Month(12)]
        ChristmasDay
    }
}
