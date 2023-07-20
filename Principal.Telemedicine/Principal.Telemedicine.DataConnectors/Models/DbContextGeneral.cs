using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Principal.Telemedicine.DataConnectors.Models;

/// <summary>
/// Db context TMAZWorkStore
/// </summary>
public partial class DbContextGeneral : DbContext
{
    private readonly string _connectionString;

    public DbContextGeneral()
    {
    }

    public DbContextGeneral(DbContextOptions<DbContextGeneral> options)
        : base(options)
    {
    }

    public DbContextGeneral(string connectionString)
    {
        _connectionString = connectionString;
    }

    public virtual DbSet<Log> Logs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connectionString))
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1250_CI_AS");

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC0771B54912");

            entity.Property(e => e.CreatedDateUtc).HasDefaultValueSql("(getutcdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}