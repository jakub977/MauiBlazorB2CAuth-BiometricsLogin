using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace Principal.Telemedicine.DataConnectors.Models;

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

    public virtual DbSet<AddressCity> AddressCities { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<EffectiveUser> EffectiveUsers { get; set; }

    public virtual DbSet<GroupEffectiveMember> GroupEffectiveMembers { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionCategory> PermissionCategories { get; set; }

    public virtual DbSet<PermissionType> PermissionTypes { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<ProfessionType> ProfessionTypes { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleCategoryCombination> RoleCategoryCombinations { get; set; }

    public virtual DbSet<RoleMember> RoleMembers { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

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

        modelBuilder.Entity<AddressCity>(entity =>
        {
            entity.Property(e => e.Id);

            modelBuilder.Entity<DiseaseDetectionResultFromMLItemDataModel>().HasNoKey();
            modelBuilder.Entity<DiseaseOriginDetectionResultFromMLItemDataModel>().HasNoKey();
            modelBuilder.Entity<DiseaseDetectionKeyInputsToMLItemDataModel>().HasNoKey();
            modelBuilder.Entity<VirtualSurgeryBasicOverviewDataModel>().HasNoKey();
            modelBuilder.Entity<AvailableDeviceListItemDataModel>().HasNoKey();

            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Code);
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deleted);
            entity.Property(e => e.ExtendedName).HasComputedColumnSql("([dbo].[f_GetAddressCityExtendedName]([Id]))", false);
            entity.Property(e => e.IdAddressDistrict);
            entity.Property(e => e.IdAddressMunicipalityWithExtendedCompetence);
            entity.Property(e => e.IdAddressRegion);
            entity.Property(e => e.Name);
            entity.Property(e => e.UpdateDateUtc);
        });

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

            entity.HasOne(d => d.City).WithMany(p => p.Customers).HasConstraintName("FK_Customer_AddressCity");

            entity.HasOne(d => d.CreatedByProvider).WithMany(p => p.Customers).HasConstraintName("FK_Customer_Provider");

            entity.HasOne(d => d.Organization).WithMany(p => p.Customers).HasConstraintName("FK_Customer_Organization");

            entity.HasOne(d => d.Picture).WithMany(p => p.Customers).HasConstraintName("FK_Customer_Picture");

            entity.HasOne(d => d.ProfessionType).WithMany(p => p.Customers).HasConstraintName("FK_Customer_ProfessionType");
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

            entity.HasOne(d => d.Provider).WithMany(p => p.EffectiveUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EffectiveUser_Provider");

            entity.HasOne(d => d.User).WithMany(p => p.EffectiveUserUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EffectiveUser_Customer");
        });

        modelBuilder.Entity<GroupEffectiveMember>(entity =>
        {
            entity.ToTable("GroupEffectiveMember");

            entity.Property(e => e.Id).HasComment("Primary identifier of a group member");
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

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("Organization");

            entity.Property(e => e.Id);
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.AddressLine);
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deleted);
            entity.Property(e => e.IdentificationNumber);
            entity.Property(e => e.Name);
            entity.Property(e => e.PostalCode);
            entity.Property(e => e.TaxIdentificationNumber);
            entity.Property(e => e.UpdateDateUtc);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permission");

            entity.Property(e => e.Id);
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.CreatedByCustomerId);
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deleted);
            entity.Property(e => e.Description);
            entity.Property(e => e.Name);
            entity.Property(e => e.ParentPermissionId);
            entity.Property(e => e.PermissionCategoryId);
            entity.Property(e => e.PermissionTypeId);
            entity.Property(e => e.SubjectId);
            entity.Property(e => e.SystemName);
            entity.Property(e => e.UpdateDateUtc);
            entity.Property(e => e.UpdatedByCustomerId);

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.PermissionCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ParentPermission).WithMany(p => p.InverseParentPermission).HasConstraintName("FK_Permission_Permission");

            entity.HasOne(d => d.PermissionCategory).WithMany(p => p.Permissions).HasConstraintName("FK_Permission_PermissionCategory");

            entity.HasOne(d => d.PermissionType).WithMany(p => p.Permissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Permission_PermissionType");

            entity.HasOne(d => d.Subject).WithMany(p => p.Permissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Permission_Subject");
        });

        modelBuilder.Entity<PermissionCategory>(entity =>
        {
            entity.ToTable("PermissionCategory");

            entity.Property(e => e.Id);
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.CreatedByCustomerId);
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deleted);
            entity.Property(e => e.Description);
            entity.Property(e => e.Name);
            entity.Property(e => e.UpdateDateUtc);
            entity.Property(e => e.UpdatedByCustomerId);

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.PermissionCategoryCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PermissionType>(entity =>
        {
            entity.ToTable("PermissionType");

            entity.Property(e => e.Id);
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deleted);
            entity.Property(e => e.Description);
            entity.Property(e => e.Name);
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Picture");

            entity.ToTable("Picture");

            entity.Property(e => e.Id);
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.ConvertedSizeInkB);
            entity.Property(e => e.CreatedByCustomerId);
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deleted);
            entity.Property(e => e.DiseaseSymptomCategoryId);
            entity.Property(e => e.FriendlyName).HasComment("Friendly name of a picture");
            entity.Property(e => e.Height).HasComment("Height of a picture");
            entity.Property(e => e.IsConverted).HasComment("Bit identifier if a picture was successfully converted");
            entity.Property(e => e.IsNew).HasComment("Bit identifier if a picture is new");
            entity.Property(e => e.IsPublic).HasComment("Bit identifier if a picture is also public for another user");
            entity.Property(e => e.IsTransient).HasComment("Bit identifier if a picture is transient");
            entity.Property(e => e.MediaStorageId).HasComment("Link to dbo.MediaStorage as hex of a picture");
            entity.Property(e => e.MimeType).HasComment("\nMime type of a picture");
            entity.Property(e => e.OriginalSizeInkB).HasComment("Size of original picture, in kB.");
            entity.Property(e => e.SeoFilename).HasComment("SEO file name of a picture");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates a picture");
            entity.Property(e => e.UpdatedOnUtc).HasComment("Date of picture update, using coordinated universal time");
            entity.Property(e => e.UserId).HasComment("Link to dbo.Customer as an user to whom picture relates. Used when column CreatedByCustomerId is different to this column: e.g. a doctor imports a picture of a patient (CreatedByCustomerId = doctor, UserId = patient)");
            entity.Property(e => e.Width).HasComment("Width of a picture");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.PictureCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ProfessionType>(entity =>
        {
            entity.ToTable("ProfessionType", tb => tb.HasComment("Lookup table of types of professions"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a profession type");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a profession type is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates a profession type");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of profession type creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a profession type is deleted");
            entity.Property(e => e.Description).HasComment("Detailed description of a profession type");
            entity.Property(e => e.Name).HasComment("Name of a profession type");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of profession type update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates a profession type");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.ProfessionTypeCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.ToTable("Provider", tb => tb.HasComment("Table of providers, second level in organization structure"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a provider");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a provider is active");
            entity.Property(e => e.AddressLine).HasComment("Address line of a provider (street, land registry number or house number, city)");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates a provider");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of provider creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a provider is deleted");
            entity.Property(e => e.IdentificationNumber).HasComment("Identification number of an organization");
            entity.Property(e => e.Name).HasComment("Name of a provider");
            entity.Property(e => e.OrganizationId).HasComment("Link to dbo.Organization as a parent organization");
            entity.Property(e => e.PictureId).HasComment("Link to dbo.Picture as a photo of a provider");
            entity.Property(e => e.PostalCode).HasComment("Postal code of a provider");
            entity.Property(e => e.TaxIdentificationNumber).HasComment("Tax identification number of an organization");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of provider update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates a provider");

            entity.HasOne(d => d.City).WithMany(p => p.Providers).HasConstraintName("FK_Provider_AddressCity");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.ProviderCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Organization).WithMany(p => p.Providers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Provider_Organization");

            entity.HasOne(d => d.Picture).WithMany(p => p.Providers).HasConstraintName("FK_Provider_Picture");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role", tb => tb.HasComment("Table of user roles"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a role");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if role is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates role");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of role creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if role is deleted");
            entity.Property(e => e.Description).HasComment("Detailed description of a role");
            entity.Property(e => e.IsGlobal).HasComment("Bit identifier if role is global or custom (0 = global, 1 = custom). Global roles are created by super admins.");
            entity.Property(e => e.Name).HasComment("Name of a role");
            entity.Property(e => e.OrganizationId).HasComment("Link to dbo.Organization as a parent organization");
            entity.Property(e => e.ParentRoleId).HasComment("Link to dbo.Role as a parent role, i.e. reference to original role");
            entity.Property(e => e.ProviderId).HasComment("Link to dbo.Provider as a parent provider");
            entity.Property(e => e.RoleCategoryCombinationId).HasComment("Link to dbo.RoleCategoryCombination as a combination of role category and its subcategory");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of role update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates role");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.RoleCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Organization).WithMany(p => p.Roles).HasConstraintName("FK_Role_Organization");

            entity.HasOne(d => d.ParentRole).WithMany(p => p.InverseParentRole).HasConstraintName("FK_Role_Role");

            entity.HasOne(d => d.Provider).WithMany(p => p.Roles).HasConstraintName("FK_Role_Provider");

            entity.HasOne(d => d.RoleCategoryCombination).WithMany(p => p.Roles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Role_RoleCategoryCombination");
        });

        modelBuilder.Entity<RoleCategoryCombination>(entity =>
        {
            entity.ToTable("RoleCategoryCombination", tb => tb.HasComment("Table of combinations of categories and subcategories of roles"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a combination");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if combination is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates combination");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of combination creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if combination is deleted");
            entity.Property(e => e.Name).HasComment("Name of a combination");
            entity.Property(e => e.RoleCategoryId).HasComment("Link to dbo.RoleCategory as a role category of combination");
            entity.Property(e => e.RoleSubCategoryId).HasComment("Link to dbo.RoleSubCategory as a role subcategory of combination");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of combination update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates combination");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.RoleCategoryCombinationCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);
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

            entity.HasOne(d => d.Role).WithMany(p => p.RoleMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleMember_Role");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subject", tb => tb.HasComment("Table of subjects in the application"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a subject");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a subject is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates a subject");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of subject creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a subject is deleted");
            entity.Property(e => e.IconName).HasComment("Name of subject icon from available web icon set (e.g. Font Awesome)");
            entity.Property(e => e.Name).HasComment("Name of a subject");
            entity.Property(e => e.ParentSubjectId).HasComment("Link to dbo.Subject as a parent subject of subject, used for subject hierarchy");
            entity.Property(e => e.SubjectTypeId).HasComment("Link to dbo.SubjectType as a type of a subject");
            entity.Property(e => e.SystemName).HasComment("System name of a subject");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of subject update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates a subject");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.SubjectCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ParentSubject).WithMany(p => p.InverseParentSubject).HasConstraintName("FK_Subject_Subject");
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.ToTable("UserPermission", tb => tb.HasComment("Table of permissions which are explicitly granted or denied to users beyond permissions already granted based on assigned roles or memberships in any of organizational units"));

            entity.Property(e => e.Id).HasComment("Primary identifier of an user permission");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if user permission is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates user permission");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of user permission creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if user permission is deleted");
            entity.Property(e => e.IsDeniedPermission).HasComment("Bit identifier if permission is denied (1) or granted (0) to user beyond the granted permission based on assigned role or membership in an organizational unit");
            entity.Property(e => e.PermissionId).HasComment("Link to dbo.Permission as a permission which is granted or denied to user");
            entity.Property(e => e.ProviderId).HasComment("Link to dbo.Provider as a provider for which exception is valid");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of user permission update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates user permission");
            entity.Property(e => e.UserId).HasComment("Link to dbo.Customer as an user to whom permission is granted or denied");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.UserPermissionCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Permission).WithMany(p => p.UserPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserPermission_Permission");

            entity.HasOne(d => d.Provider).WithMany(p => p.UserPermissions).HasConstraintName("FK_UserPermission_Provider");

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
