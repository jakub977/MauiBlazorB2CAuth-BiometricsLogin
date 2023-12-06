using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of LocaleStringResource
/// </summary>
[Table("LocaleStringResource")]
public partial class LocaleStringResource
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int LanguageId { get; set; }

    [StringLength(200)]
    [Required]
    public string ResourceName { get; set; } = null!;

    public string? ResourceValue { get; set; }

    public bool? IsFromPlugin { get; set; } = null;

    public bool? IsTouched { get; set; } = null;
}
