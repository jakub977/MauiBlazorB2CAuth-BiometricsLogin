namespace Principal.Telemedicine.Shared.Enums;

/// <summary>
/// Dvoupísmenový ISO kód požadovaného jazyka převedený na integer
/// Hodnoty odpovídají tabulce LanguageType
/// </summary>
public enum LanguageCodeEnum
{
    None = 0,
    /// <summary>
    /// angličtina
    /// </summary>
    en = 1,

    /// <summary>
    /// čeština
    /// </summary>
    cs = 2,

    /// <summary>
    /// němčina
    /// </summary>
    de = 3,

    /// <summary>
    /// ukrajinština
    /// </summary>
    uk = 4,
}
