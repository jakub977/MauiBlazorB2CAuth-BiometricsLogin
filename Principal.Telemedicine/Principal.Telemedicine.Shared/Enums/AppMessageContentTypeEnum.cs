namespace Principal.Telemedicine.Shared.Enums;

public enum AppMessageContentTypeEnum 
{

    /// <summary>
    /// Aktualizace plánu aktivit, Penelope
    /// </summary>
    P_SCHED_01 = 1,

    /// <summary>
    /// Provozní
    /// </summary>
    U_SERV_01 = 2,

    /// <summary>
    /// Nová zpráva v messengeru
    /// </summary>
    U_MSG_01 = 3,

    /// <summary>
    /// Výzva k doplnění symptomů
    /// </summary>
    V_ML_01 = 4,

    /// <summary>
    /// Primární diagnostické dotazy
    /// </summary>
    V_PDD_01 = 5,

    /// <summary>
    /// ARI/ILI stupeň 2
    /// </summary>
    U_RISKSEV_01 = 6,

    /// <summary>
    /// ARI/ILI stupeň 3
    /// </summary>
    U_RISKSEV_02 = 7,

    /// <summary>
    /// Covid-19 stupeň 2
    /// </summary>
    U_RISKSEV_03 = 8,

    /// <summary>
    /// Covid-19 stupeň 3
    /// </summary>
    U_RISKSEV_04 = 9,

    /// <summary>
    /// Imise stupeň 2
    /// </summary>
    U_RISKSEV_05 = 10,

    /// <summary>
    /// Imise stupeň 3
    /// </summary>
    U_RISKSEV_06 = 11,

    /// <summary>
    /// Výzva k prohlídce
    /// </summary>
    V_EPRACTICE_01 = 12,

    /// <summary>
    /// Doporučená izolace
    /// </summary>
    V_EPRACTICE_02 = 13,

    /// <summary>
    /// Změna doporučené izolace
    /// </summary>
    V_EPRACTICE_03 = 14,

    /// <summary>
    /// Přerušení izolace
    /// </summary>
    V_EPRACTICE_04 = 15,

    /// <summary>
    /// Karanténa
    /// </summary>
    V_EPRACTICE_05 = 16,

    /// <summary>
    /// Změna délky karantény
    /// </summary>
    V_EPRACTICE_06 = 17,

    /// <summary>
    /// Přerušení karantény
    /// </summary>
    V_EPRACTICE_07 = 18
}

