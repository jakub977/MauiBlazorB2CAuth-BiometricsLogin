using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Enums
{
    /// <summary>
    /// Typy fyziologických dat (např. krevní tlak)
    /// (odpovídá tabulce PhysiologicalDataType)
    /// </summary>
    public enum PhysiologicalDataTypeEnum
    {
        /// <summary>
        /// Tep
        /// </summary>
        //[ImageFileName("fa-heartbeat")]
        HeartRate = 1,

        /// <summary>
        /// SpO2
        /// </summary>
        //[ImageFileName("fa-tint")]
        Spo2 = 2,

        /// <summary>
        /// VO2 max
        /// </summary>
        //[ImageFileName("fa-heart")] //TODO IKONA
        MaximumOxygenVolume = 3,

        /// <summary>
        /// Krevní tlak
        /// </summary>
        //[ImageFileName("fa-heart")]
        BloodPressure = 4,

        /// <summary>
        /// Teplota
        /// </summary>
        //[ImageFileName("fa-thermometer-three-quarters")]
        Temperature = 5,

        /// <summary>
        /// BRPM
        /// </summary>
        //[ImageFileName("fa-wind")]
        Brpm = 6,

        /// <summary>
        /// Bazální metabolismus
        /// </summary>
        //[ImageFileName("fa-child")] //TODO IKONA
        BasalMetabolism = 7,

        /// <summary>
        /// Glykémie
        /// </summary>
        //[ImageFileName("fa-droplet")]
        Glycemia = 8,

        /// <summary>
        /// Krokoměr
        /// </summary>
        //[ImageFileName("fa-walking")] //TODO IKONA
        Steps = 9,

        /// <summary>
        /// CRP
        /// </summary>
        //[ImageFileName("fa-child")] //TODO IKONA
        Crp = 10,

        /// <summary>
        /// BMI
        /// </summary>
        //[ImageFileName("fa-child")] //TODO IKONA
        Bmi = 11,

        /// <summary>
        /// Váha
        /// </summary>
        //[ImageFileName("fa-weight")] //TODO IKONA
        Weight = 12,

        /// <summary>
        /// Leukocity
        /// </summary>
        //[ImageFileName("fa-heart")] //TODO IKONA
        Leukocytes = 13,

        /// <summary>
        /// MAP
        /// </summary>
        //[ImageFileName("fa-heart")]
        MAP = 14,

        /// <summary>
        /// Výška
        /// </summary>
        //[ImageFileName("fa-child")]
        Height = 15,

        /// <summary>
        /// Proteinurie - Bílkoviny v moči
        /// </summary>
        //[ImageFileName("fa-p")]
        Proteinuria = 16,

        /// <summary>
        /// Spánkový cyklus - lehký spánek
        /// </summary>
        //[ImageFileName("fa-p")]
        SleepPeriodLight = 17,

        /// <summary>
        /// Spánkový cyklus - hluboký spánek
        /// </summary>
        //[ImageFileName("fa-p")]
        SleepPeriodDeep = 18,

        /// <summary>
        /// Spánkový cyklus - REM spánek
        /// </summary>
        //[ImageFileName("fa-p")]
        SleepPeriodREM = 19
    }
}
