using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Models;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace Principal.Telemedicine.DataConnectors.Contexts;

public partial class DbContextApi : DbContext
{
    public DbContextApi()
    {
    }

    public DbContextApi(DbContextOptions<DbContextApi> options)
        : base(options)
    {
    }

    [NotMapped]
    public virtual DbSet<DiseaseDetectionResultFromMLItemDataModel> DetectionResultFromMlItemDataModels { get; set; }

    [NotMapped]
    public virtual DbSet<DiseaseOriginDetectionResultFromMLItemDataModel> DiseaseOriginDetectionResultFromMLItemDataModels { get; set; }

    [NotMapped]
    public virtual DbSet<DiseaseDetectionKeyInputsToMLItemDataModel> DiseaseDetectionKeyInputsToMLItemDataModels { get; set; }

    [NotMapped]
    public virtual DbSet<VirtualSurgeryBasicOverviewDataModel> VirtualSurgeryBasicOverviewDataModels { get; set; }

    [NotMapped]
    public virtual DbSet<AvailableDeviceListItemDataModel> AvailableDeviceListItemDataModels { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<EffectiveUser> EffectiveUsers { get; set; }

    public virtual DbSet<GroupEffectiveMember> GroupEffectiveMembers { get; set; }

    public virtual DbSet<RoleMember> RoleMembers { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=tmworkstoresqlserver.database.windows.net;Initial Catalog=VANDA_TEST;Application Name=VANDA_TEST_AZURE;Integrated Security=False;User ID=TM_DB_dev;Password=Ap7M9$eWSj8TUQ734FcGdqnfHkqw$BHf;Persist Security Info=True;Enlist=False;Pooling=True;Min Pool Size=1;Max Pool Size=100;Connect Timeout=45;User Instance=False;MultipleActiveResultSets=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1250_CI_AS");
        modelBuilder.Entity<DiseaseDetectionResultFromMLItemDataModel>().HasNoKey();
        modelBuilder.Entity<DiseaseOriginDetectionResultFromMLItemDataModel>().HasNoKey();
        modelBuilder.Entity<DiseaseDetectionKeyInputsToMLItemDataModel>().HasNoKey();
        modelBuilder.Entity<VirtualSurgeryBasicOverviewDataModel>().HasNoKey();
        modelBuilder.Entity<AvailableDeviceListItemDataModel>().HasNoKey();

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.Id);
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.AddressLine);
            entity.Property(e => e.AdminComment);
            entity.Property(e => e.ApiloginEnabled)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.ApiloginToken);
            entity.Property(e => e.BirthIdentificationNumber);
            entity.Property(e => e.Birthdate);
            entity.Property(e => e.CreatedByCustomerId);
            entity.Property(e => e.CreatedByProviderId);
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deleted);
            entity.Property(e => e.Email);
            entity.Property(e => e.EmployerName);
            entity.Property(e => e.FirstName);
            entity.Property(e => e.FriendlyName)
                .HasComputedColumnSql("(([FirstName]+' ')+[LastName])", true);
            entity.Property(e => e.GenderTypeId);
            entity.Property(e => e.GlobalId)
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.HealthCareInsurerCode);
            entity.Property(e => e.HealthCareInsurerId);
            entity.Property(e => e.InvalidLoginsCount);
            entity.Property(e => e.IsOrganizationAdminAccount);
            entity.Property(e => e.IsProviderAdminAccount);
            entity.Property(e => e.IsRiskPatient).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsSuperAdminAccount);
            entity.Property(e => e.IsSystemAccount);
            entity.Property(e => e.LastActivityDateUtc);
            entity.Property(e => e.LastApiloginDateTime);
            entity.Property(e => e.LastIpAddress);
            entity.Property(e => e.LastLoginDateUtc);
            entity.Property(e => e.LastName);
            entity.Property(e => e.Note);
            entity.Property(e => e.OrganizationId);
            entity.Property(e => e.Password);
            entity.Property(e => e.PasswordFormatTypeId);
            entity.Property(e => e.PasswordSalt);
            entity.Property(e => e.PersonalIdentificationNumber);
            entity.Property(e => e.PictureId);
            entity.Property(e => e.PostalCode);
            entity.Property(e => e.ProfessionTypeId);
            entity.Property(e => e.PublicIdentifier);
            entity.Property(e => e.TelephoneNumber);
            entity.Property(e => e.TelephoneNumber2);
            entity.Property(e => e.TitleAfter);
            entity.Property(e => e.TitleBefore);
            entity.Property(e => e.UpdateDateUtc);
            entity.Property(e => e.UpdatedByCustomerId);
        });

        modelBuilder.Entity<EffectiveUser>(entity =>
        {
            entity.ToTable("EffectiveUser");

            entity.Property(e => e.Id);
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.CreatedByCustomerId);
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deleted);
            entity.Property(e => e.ProviderId);
            entity.Property(e => e.UpdateDateUtc);
            entity.Property(e => e.UpdatedByCustomerId);
            entity.Property(e => e.UserId);

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.EffectiveUserCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.EffectiveUserUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EffectiveUser_Customer");
        });

        modelBuilder.Entity<GroupEffectiveMember>(entity =>
        {
            entity.ToTable("GroupEffectiveMember");

            entity.Property(e => e.Id);
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.CreatedByCustomerId);
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deleted);
            entity.Property(e => e.EffectiveUserId);
            entity.Property(e => e.GroupId);
            entity.Property(e => e.UpdateDateUtc);
            entity.Property(e => e.UpdatedByCustomerId);

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.GroupEffectiveMemberCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.EffectiveUser).WithMany(p => p.GroupEffectiveMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupEffectiveMember_EffectiveUser");
        });

        modelBuilder.Entity<RoleMember>(entity =>
        {
            entity.ToTable("RoleMember");

            entity.Property(e => e.Id);
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.CreatedByCustomerId);
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deleted);
            entity.Property(e => e.DirectUserId);
            entity.Property(e => e.EffectiveUserId);
            entity.Property(e => e.RoleId);
            entity.Property(e => e.UpdateDateUtc);
            entity.Property(e => e.UpdatedByCustomerId);

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.RoleMemberCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.DirectUser).WithMany(p => p.RoleMemberDirectUsers).HasConstraintName("FK_RoleMember_Customer_DirectUser");

            entity.HasOne(d => d.EffectiveUser).WithMany(p => p.RoleMembers).HasConstraintName("FK_RoleMember_EffectiveUser");
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.ToTable("UserPermission");
            entity.Property(e => e.Id);
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.CreatedByCustomerId);
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deleted);
            entity.Property(e => e.IsDeniedPermission);
            entity.Property(e => e.PermissionId);
            entity.Property(e => e.ProviderId);
            entity.Property(e => e.UpdateDateUtc);
            entity.Property(e => e.UpdatedByCustomerId);
            entity.Property(e => e.UserId);

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.UserPermissionCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.UserPermissionUsers).OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    /// <summary>
    /// Pomocná metoda vykonávající query a vracející list objektů.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <returns> List objektů </returns>
    public List<T> ExecSqlQuery<T>(string query)
    {
        using var command = Database.GetDbConnection().CreateCommand();

        command.CommandText = query;
        command.CommandType = CommandType.Text;
        Database.OpenConnection();

        List<T> list = new List<T>();
        using (var result = command.ExecuteReader())
        {
            T obj = default(T);
            while (result.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(result[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, result[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
        }
        Database.CloseConnection();
        return list;

    }
}
