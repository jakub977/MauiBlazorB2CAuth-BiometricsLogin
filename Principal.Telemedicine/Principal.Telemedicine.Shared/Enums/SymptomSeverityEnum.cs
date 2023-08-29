using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Enums
{
    /// <summary>
    /// Závažnosti symptomů pro filtrování na kartě pacienta
    /// Hodnoty odpovídají tabulce DiseaseSymptomSeverityLevelType
    /// </summary>
    public enum SymptomSeverityEnum
    {
        /// <summary>
        /// Nejednoznačné / nehodnocené
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// Bez (doplňující) klasifikace míry závažnosti
        /// </summary>
        NotClassified = 0,

        /// <summary>
        /// Bez problémů
        /// </summary>
        NoProblems = 1,

        /// <summary>
        /// Mirné - středně intenzivní problémy
        /// </summary>
        Moderate = 2,

        /// <summary>
        /// Závažný stav
        /// </summary>
        Critical = 3,
    }
}
