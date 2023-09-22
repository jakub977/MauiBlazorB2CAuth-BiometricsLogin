using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Principal.Telemedicine.DataConnectors.Models.Shared;

/// <summary>
/// Table of logs of application.
/// </summary>
[Table("Log")]
public partial class Log
{
    /// <summary>
    /// Primary identifier of logs
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Level of log (enum column)
    /// </summary>
    public int LogLevelId { get; set; }

    /// <summary>
    /// Short message of log
    /// </summary>
    [StringLength(4000)]
    public string ShortMessage { get; set; } = null!;

    /// <summary>
    /// 
    /// Full message of log
    /// </summary>
    public string? FullMessage { get; set; }

    /// <summary>
    /// 
    /// IP address of log
    /// </summary>
    [StringLength(200)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// 
    /// Link to dbo.Customer as an user to whom log is associated
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// 
    /// URL page where log was created
    /// </summary>
    [StringLength(1500)]
    public string? PageUrl { get; set; }

    /// <summary>
    /// 
    /// Referrer URL of log
    /// </summary>
    [StringLength(1500)]
    public string? ReferrerUrl { get; set; }

    /// <summary>
    /// 
    /// Date of log creation, using coordinated universal time
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// 
    /// What record log
    /// </summary>
    [StringLength(400)]
    public string Logger { get; set; } = null!;

    /// <summary>
    /// 
    /// Type of HTTP method
    /// </summary>
    [StringLength(10)]
    public string? HttpMethod { get; set; }

    /// <summary>
    /// 
    /// User name of log
    /// </summary>
    [StringLength(100)]
    public string? UserName { get; set; }

    /// <summary>
    /// Trace request information.
    /// </summary>
    [StringLength(500)]
    public string? CorrelationGuid { get; set; }
}
