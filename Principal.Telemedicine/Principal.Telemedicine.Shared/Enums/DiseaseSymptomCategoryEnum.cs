using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Principal.Telemedicine.Shared.Enums
{
    public enum DiseaseSymptomCategoryEnum
    {
        /// <summary>
        /// Neurčeno
        /// </summary>
        None = 0,
        /// <summary>
        /// Denní režim
        /// </summary>
        //[ImageFileName("fa-tired")]
        DailyProgram = 1,
        /// <summary>
        /// Dýchání
        /// </summary>
        //[ImageFileName("fa-wind")]
        Respiration = 2,
        /// <summary>
        /// Kašel
        /// </summary>
        //[ImageFileName("fa-head-side-cough")]
        Cough = 3,
        /// <summary>
        /// Bolesti
        /// </summary>
        //[ImageFileName("fa-bolt")]
        Pain = 4,
        /// <summary>
        /// Krk a dutina ústní
        /// </summary>
        //[ImageFileName("fa-lips")]
        NeckAndOralCavity = 5,
        /// <summary>
        /// Krk a dutina ústní
        /// </summary>
        //[ImageFileName("fa-ellipsis-h")]
        OtherDifficulties = 6
    }
}
