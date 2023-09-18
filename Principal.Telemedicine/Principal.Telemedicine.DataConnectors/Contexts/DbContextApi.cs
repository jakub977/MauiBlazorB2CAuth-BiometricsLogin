using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Principal.Telemedicine.DataConnectors.Models.Shared;
using Principal.Telemedicine.PenelopeData.Models;
using Principal.Telemedicine.Shared.Models;



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

    [NotMapped]
    public virtual DbSet<UserCalendarWithMeasuredValuesDataModel> UserCalendarWithMeasuredValuesDataModels { get; set; }

    [NotMapped]
    public virtual DbSet<CalendarWithMeasuredValuesDataModel> CalendarWithMeasuredValuesDataModels { get; set; }

    [NotMapped]
    public virtual DbSet<ScheduledActivitiesDataModel> ScheduledActivitiesDataModels { get; set; }


    public virtual DbSet<AddressCity> AddressCities { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<EffectiveUser> EffectiveUsers { get; set; }

    public virtual DbSet<GenderType> GenderTypes { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupEffectiveMember> GroupEffectiveMembers { get; set; }

    public virtual DbSet<GroupPermission> GroupPermissions { get; set; }

    public virtual DbSet<HealthCareInsurer> HealthCareInsurers { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<PasswordFormatType> PasswordFormatTypes { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionCategory> PermissionCategories { get; set; }

    public virtual DbSet<PermissionType> PermissionTypes { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<ProfessionType> ProfessionTypes { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleCategory> RoleCategories { get; set; }

    public virtual DbSet<RoleCategoryCombination> RoleCategoryCombinations { get; set; }

    public virtual DbSet<RoleMember> RoleMembers { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<RoleSubCategory> RoleSubCategories { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<SubjectAllowedToOrganization> SubjectAllowedToOrganizations { get; set; }

    public virtual DbSet<SubjectAllowedToProvider> SubjectAllowedToProviders { get; set; }

    public virtual DbSet<SubjectType> SubjectTypes { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    public virtual DbSet<MediaStorage> MediaStorages { get; set; }

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
        modelBuilder.UseCollation("SQL_Latin1_General_CP1250_CI_AS");
        modelBuilder.Entity<DiseaseDetectionResultFromMLItemDataModel>().HasNoKey();
        modelBuilder.Entity<DiseaseOriginDetectionResultFromMLItemDataModel>().HasNoKey();
        modelBuilder.Entity<DiseaseDetectionKeyInputsToMLItemDataModel>().HasNoKey();
        modelBuilder.Entity<VirtualSurgeryBasicOverviewDataModel>().HasNoKey();
        modelBuilder.Entity<AvailableDeviceListItemDataModel>().HasNoKey();
        modelBuilder.Entity<ScheduledActivitiesDataModel>().HasNoKey();
        modelBuilder.Entity<UserCalendarWithMeasuredValuesDataModel>().HasNoKey();
        modelBuilder.Entity<CalendarWithMeasuredValuesDataModel>().HasNoKey(); 

        modelBuilder.Entity<AddressCity>(entity =>
        {
            entity.Property(e => e.Id).HasComment("Primary identifier of city");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if city is active");
            entity.Property(e => e.Code).HasComment("City code");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of city creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if city is deleted");
            entity.Property(e => e.ExtendedName).HasComputedColumnSql("([dbo].[f_GetAddressCityExtendedName]([Id]))", false);
            entity.Property(e => e.IdAddressDistrict).HasComment("Link to dbo.AddressDistrict as a parent district of city");
            entity.Property(e => e.IdAddressMunicipalityWithExtendedCompetence).HasComment("Link to dbo.AddressMunicipalityWithExtendedCompetence as a parent municipality with extended competence of city");
            entity.Property(e => e.IdAddressRegion).HasComment("Link to dbo.AddressRegion as a parent region of city");
            entity.Property(e => e.Name).HasComment("City name");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of city update, using coordinated universal time");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer", tb => tb.HasComment("Table of users"));

            entity.Property(e => e.Id).HasComment("Primary identifier of an user");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if an user is active");
            entity.Property(e => e.AddressLine).HasComment("Address line of an user (street, land registry number or house number, city)");
            entity.Property(e => e.AdminComment).HasComment("Comment of a super admin");
            entity.Property(e => e.ApiloginEnabled)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if login to API is enabled");
            entity.Property(e => e.ApiloginToken).HasComment("Login token issued when login to API");
            entity.Property(e => e.BirthIdentificationNumber).HasComment("Birth identification number of an user");
            entity.Property(e => e.Birthdate).HasComment("Birthdate of an user");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates an user");
            entity.Property(e => e.CreatedByProviderId).HasComment("Link to dbo.Provider as a provider which creates an user and where user is registered");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of user creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if an user is deleted");
            entity.Property(e => e.Email).HasComment("User's e-mail address");
            entity.Property(e => e.EmployerName).HasComment("Employer of user");
            entity.Property(e => e.FirstName).HasComment("First name of an user");
            entity.Property(e => e.FriendlyName)
                .HasComputedColumnSql("(([FirstName]+' ')+[LastName])", true)
                .HasComment("Friendly name, i.e. full name of an user");
            entity.Property(e => e.GenderTypeId).HasComment("Link to dbo.GenderType as a gender type of an user");
            entity.Property(e => e.GlobalId)
                .HasDefaultValueSql("(newid())")
                .HasComment("Global identifier of user, used for synchronization between dedicated DBs and central Azure DB");
            entity.Property(e => e.HealthCareInsurerCode).HasComment("Code of health care insurer of an user");
            entity.Property(e => e.HealthCareInsurerId).HasComment("Link to dbo.HealthCareInsurer as a health care insurer of an user");
            entity.Property(e => e.InvalidLoginsCount).HasComment("Number of failed login attempts");
            entity.Property(e => e.IsOrganizationAdminAccount).HasComment("Bit identifier if an account is the organization admin account");
            entity.Property(e => e.IsProviderAdminAccount).HasComment("Bit identifier if an account is the provider admin account");
            entity.Property(e => e.IsRiskPatient).HasDefaultValueSql("((0))");
            entity.Property(e => e.IsSuperAdminAccount).HasComment("Bit identifier if an account is the super admin account");
            entity.Property(e => e.IsSystemAccount).HasComment("Bit identifier if an account is the system account");
            entity.Property(e => e.LastActivityDateUtc).HasComment("Last date of activity, using coordinated universal time");
            entity.Property(e => e.LastApiloginDateTime).HasComment("Last date of login to API, using coordinated universal time");
            entity.Property(e => e.LastIpAddress).HasComment("Last IP address related to an user");
            entity.Property(e => e.LastLoginDateUtc).HasComment("Last date of login, using coordinated universal time");
            entity.Property(e => e.LastName).HasComment("Last name of an user");
            entity.Property(e => e.Note).HasComment("Note to an user");
            entity.Property(e => e.OrganizationId).HasComment("Link to dbo.Organization as an organization in which an user is registered");
            entity.Property(e => e.Password).HasComment("Password related to the user account");
            entity.Property(e => e.PasswordFormatTypeId).HasComment("Password format identifier");
            entity.Property(e => e.PasswordSalt).HasComment("Salt of password");
            entity.Property(e => e.PersonalIdentificationNumber).HasComment("Personal identification number of an user");
            entity.Property(e => e.PictureId).HasComment("Link to dbo.Picture as a photo of an user");
            entity.Property(e => e.PostalCode).HasComment("Postal code");
            entity.Property(e => e.ProfessionTypeId).HasComment("Link to dbo.ProfessionType as a profession of user");
            entity.Property(e => e.PublicIdentifier).HasComment("Public identifier of an user");
            entity.Property(e => e.TelephoneNumber).HasComment("Telephone number of an user");
            entity.Property(e => e.TelephoneNumber2).HasComment("Second telephone number of an user");
            entity.Property(e => e.TitleAfter).HasComment("Title after user's name");
            entity.Property(e => e.TitleBefore).HasComment("Title before user's name");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of user update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates an user");

            entity.HasOne(d => d.City).WithMany(p => p.Customers).HasConstraintName("FK_Customer_AddressCity");

            entity.HasOne(d => d.CreatedByProvider).WithMany(p => p.Customers).HasConstraintName("FK_Customer_Provider");

            entity.HasOne(d => d.GenderType).WithMany(p => p.Customers).HasConstraintName("FK_Customer_GenderType");

            entity.HasOne(d => d.HealthCareInsurer).WithMany(p => p.Customers).HasConstraintName("FK_Customer_HealthCareInsurer");

            entity.HasOne(d => d.Organization).WithMany(p => p.Customers).HasConstraintName("FK_Customer_Organization");

            entity.HasOne(d => d.PasswordFormatType).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customer_PasswordFormatType");

            entity.HasOne(d => d.Picture).WithMany(p => p.Customers).HasConstraintName("FK_Customer_Picture");

            entity.HasOne(d => d.ProfessionType).WithMany(p => p.Customers).HasConstraintName("FK_Customer_ProfessionType");
        });

        modelBuilder.Entity<EffectiveUser>(entity =>
        {
            entity.ToTable("EffectiveUser", tb => tb.HasComment("Table of effective users, i.e. members of provider. We also distinguish direct users, who are members of an organization only and not of a provider (these are users in dbo.Customer without row in this table)."));

            entity.Property(e => e.Id).HasComment("Primary identifier of an effective user");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if effective user is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates effective user");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of effective user creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if effective user is deleted");
            entity.Property(e => e.ProviderId).HasComment("Link to dbo.Provider as a provider of which user is member");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of effective user update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates effective user");
            entity.Property(e => e.UserId).HasComment("Link to dbo.Customer as an user who is effective user, i.e. member of provider");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.EffectiveUserCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Provider).WithMany(p => p.EffectiveUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EffectiveUser_Provider");

            entity.HasOne(d => d.User).WithMany(p => p.EffectiveUserUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EffectiveUser_Customer");
        });

        modelBuilder.Entity<GenderType>(entity =>
        {
            entity.ToTable("GenderType", tb => tb.HasComment("System lookup table of types of gender"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a gender type");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a gender type is active");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of gender type creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a gender type is deleted");
            entity.Property(e => e.Name).HasComment("Nname of a gender type");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Group", tb => tb.HasComment("Table of groups, third level of organization hierarchy"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a group");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a group is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates a group");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of group creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a group is deleted");
            entity.Property(e => e.GroupTagTypeId).HasComment("Link to dbo.GroupTagType as a tag type of a group");
            entity.Property(e => e.IsRiskGroup).HasComment("Bit identifier if group is risk group");
            entity.Property(e => e.Name).HasComment("Name of a group");
            entity.Property(e => e.ParentGroupId).HasComment("Link to dbo.Group as a parent group of a group");
            entity.Property(e => e.PictureId).HasComment("Link to dbo.Picture as a photo of a group");
            entity.Property(e => e.ProviderId).HasComment("Link to dbo.Provider as a parent provider");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of group update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates a group");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.GroupCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.ParentGroup).WithMany(p => p.InverseParentGroup).HasConstraintName("FK_Group_Group");

            entity.HasOne(d => d.Picture).WithMany(p => p.Groups).HasConstraintName("FK_Group_Picture");

            entity.HasOne(d => d.Provider).WithMany(p => p.Groups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Group_Provider");
        });

        modelBuilder.Entity<GroupEffectiveMember>(entity =>
        {
            entity.ToTable("GroupEffectiveMember", tb => tb.HasComment("Table of effective users who are members of groups"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a group member");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a group member is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates a group member");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of group member creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a group member is deleted");
            entity.Property(e => e.EffectiveUserId).HasComment("Link to dbo.EffectiveUser as an effective user (i.e. user who is member of a directory and not only of an organization) who is a member of group");
            entity.Property(e => e.GroupId).HasComment("Link to dbo.Group as a group of which user is a member");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of group member update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates a group member");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.GroupEffectiveMemberCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.EffectiveUser).WithMany(p => p.GroupEffectiveMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupEffectiveMember_EffectiveUser");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupEffectiveMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupEffectiveMember_Group");
        });

        modelBuilder.Entity<GroupPermission>(entity =>
        {
            entity.ToTable("GroupPermission", tb => tb.HasComment("Table of permissions of groups"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a group permission");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a group permission is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates a group permission");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of group permission creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a group permission is deleted");
            entity.Property(e => e.GroupId).HasComment("Link to dbo.Group as a group to which permission is assigned");
            entity.Property(e => e.PermissionId).HasComment("Link to dbo.Permission as a permission which is assigned to group");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of group permission update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates a group permission");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.GroupPermissionCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Group).WithMany(p => p.GroupPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupPermission_Group");

            entity.HasOne(d => d.Permission).WithMany(p => p.GroupPermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupPermission_Permission");
        });

        modelBuilder.Entity<HealthCareInsurer>(entity =>
        {
            entity.ToTable("HealthCareInsurer", tb => tb.HasComment("Table of health care insurers"));

            entity.Property(e => e.Id).HasComment("Primary identifier of an insurer");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if an insurer is active");
            entity.Property(e => e.Code).HasComment("Code of an insurer");
            entity.Property(e => e.CountryId).HasComment("Link to dbo.Country as a country of insurer");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates an insurer");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of insurance company creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if an insurer is deleted");
            entity.Property(e => e.Name).HasComment("Name of an insurer");
            entity.Property(e => e.ShortName).HasComment("Short name of an insurer");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of insurance company update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates an insurer");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.HealthCareInsurerCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("Organization", tb => tb.HasComment("Table of organizations, the highest level of organization hierarchy"));

            entity.Property(e => e.Id).HasComment("Primary identifier of an organization");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if an organization is active");
            entity.Property(e => e.AddressLine).HasComment("Address line of an organization (street, land registry number or house number, city)");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of organization creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if an organization is deleted");
            entity.Property(e => e.IdentificationNumber).HasComment("Identification number of an organization");
            entity.Property(e => e.Name).HasComment("Name of an organization");
            entity.Property(e => e.PostalCode).HasComment("Postal code of an organization");
            entity.Property(e => e.TaxIdentificationNumber).HasComment("Tax identification number of an organization");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of organization update, using coordinated universal time");
        });

        modelBuilder.Entity<PasswordFormatType>(entity =>
        {
            entity.ToTable("PasswordFormatType", tb => tb.HasComment("System lookup table of password formats"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a password format");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a password format is active");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of password format creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a password format is deleted");
            entity.Property(e => e.Name).HasComment("Name of a password format");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("Permission", tb => tb.HasComment("Table of permissions on subjects"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a permission");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a permission is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates a permission");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of permission creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a permission is deleted");
            entity.Property(e => e.Description).HasComment("Detailed description of a permission");
            entity.Property(e => e.Name).HasComment("Name of a permission");
            entity.Property(e => e.ParentPermissionId).HasComment("Link to dbo.Permission as a parent permission, used for hierarchy of permissions");
            entity.Property(e => e.PermissionCategoryId).HasComment("Link to dbo.PermissionCategory as a category of permission");
            entity.Property(e => e.PermissionTypeId).HasComment("Link to dbo.PermissionType as a permission type");
            entity.Property(e => e.SubjectId).HasComment("Link to dbo.Subject as a subject of the application");
            entity.Property(e => e.SystemName).HasComment("System name of a permission");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of permission update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates a permission");

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
            entity.ToTable("PermissionCategory", tb => tb.HasComment("Table of categories of permissions"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a category");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if category is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates category");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of category creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if category is deleted");
            entity.Property(e => e.Description).HasComment("Detailed description of a category");
            entity.Property(e => e.Name).HasComment("Name of a category");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of category update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates category");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.PermissionCategoryCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PermissionType>(entity =>
        {
            entity.ToTable("PermissionType", tb => tb.HasComment("System lookup table of permission types (primarly CRUD)"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a permission type");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a permission type is active");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of permission type creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a permission type is deleted");
            entity.Property(e => e.Description).HasComment("Detailed description of a permission type");
            entity.Property(e => e.Name).HasComment("Name of a permission type");
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Picture");

            entity.ToTable("Picture", tb => tb.HasComment("Table of pictures and their metadata."));

            entity.Property(e => e.Id).HasComment("Primary identifier of a picture");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a picture is active");
            entity.Property(e => e.ConvertedSizeInkB).HasComment("Size of converted picture, in kB.");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates a picture");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of picture creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a picture is deleted");
            entity.Property(e => e.DiseaseSymptomCategoryId).HasComment("Link to dbo.DiseaseSymptomCategory as a category of disease symptom, using for ML");
            entity.Property(e => e.FriendlyName).HasComment("Friendly name of a picture");
            entity.Property(e => e.Height).HasComment("Height of a picture");
            entity.Property(e => e.IsConverted).HasComment("Bit identifier if a picture was successfully converted");
            entity.Property(e => e.IsNew).HasComment("Bit identifier if a picture is new");
            entity.Property(e => e.IsPublic).HasComment("Bit identifier if a picture is also public for another user");
            entity.Property(e => e.IsTransient).HasComment("Bit identifier if a picture is transient");
            entity.Property(e => e.MediaStorageId).HasComment("Link to dbo.MediaStorage as hex of a picture");
            entity.Property(e => e.MimeType).HasComment("Mime type of a picture");
            entity.Property(e => e.OriginalSizeInkB).HasComment("Size of original picture, in kB.");
            entity.Property(e => e.SeoFilename).HasComment("SEO file name of a picture");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates a picture");
            entity.Property(e => e.UpdatedOnUtc).HasComment("Date of picture update, using coordinated universal time");
            entity.Property(e => e.UserId).HasComment("Link to dbo.Customer as an user to whom picture relates. Used when column CreatedByCustomerId is different to this column: e.g. a doctor imports a picture of a patient (CreatedByCustomerId = doctor, UserId = patient)");
            entity.Property(e => e.Width).HasComment("Width of a picture");

            entity.HasOne(d => d.MediaStorage).WithMany(p => p.Pictures).HasConstraintName("FK_dbo.Picture_dbo.MediaStorage_MediaStorageId");
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

        modelBuilder.Entity<RoleCategory>(entity =>
        {
            entity.ToTable("RoleCategory", tb => tb.HasComment("Table of categories of roles"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a category");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if category is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates category");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of category creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if category is deleted");
            entity.Property(e => e.Description).HasComment("Detailed description of a category");
            entity.Property(e => e.Name).HasComment("Name of a category");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of category update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates category");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.RoleCategoryCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);
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

            entity.HasOne(d => d.RoleCategory).WithMany(p => p.RoleCategoryCombinations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleCategoryCombination_RoleCategory");

            entity.HasOne(d => d.RoleSubCategory).WithMany(p => p.RoleCategoryCombinations).HasConstraintName("FK_RoleCategoryCombination_RoleSubCategory");
        });

        modelBuilder.Entity<RoleMember>(entity =>
        {
            entity.ToTable("RoleMember", tb => tb.HasComment("Table of (effective and direct) users who are members of roles"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a role member");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a role member is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates a role member");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of role member creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a role member is deleted");
            entity.Property(e => e.DirectUserId).HasComment("Link to dbo.Customer as an direct user (i.e. user who is member only of an organization and not of a directory) who is a member of a role");
            entity.Property(e => e.EffectiveUserId).HasComment("Link to dbo.EffectiveUser as an effective user (i.e. user who is member of a directory and not only of an organization) who is a member of a role");
            entity.Property(e => e.RoleId).HasComment("Link to dbo.Role as a role which is grant to a (direct or effective) user");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of role member update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates a role member");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.RoleMemberCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.DirectUser).WithMany(p => p.RoleMemberDirectUsers).HasConstraintName("FK_RoleMember_Customer_DirectUser");

            entity.HasOne(d => d.EffectiveUser).WithMany(p => p.RoleMembers).HasConstraintName("FK_RoleMember_EffectiveUser");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleMember_Role");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("RolePermission", tb => tb.HasComment("Table of permissions of roles"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a role permission");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a role permission is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates a role permission");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of role permission creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a role permission is deleted");
            entity.Property(e => e.PermissionId).HasComment("Link to dbo.Permission as a permission which is assigned to role");
            entity.Property(e => e.RoleId).HasComment("Link to dbo.Role as a role to which permission is assigned");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of role permission update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates a role permission");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.RolePermissionCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Permission");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePermission_Role");
        });

        modelBuilder.Entity<RoleSubCategory>(entity =>
        {
            entity.ToTable("RoleSubCategory", tb => tb.HasComment("Table of subcategories of roles"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a subcategory");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if subcategory is active");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates subcategory");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of subcategory creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if subcategory is deleted");
            entity.Property(e => e.Description).HasComment("Detailed description of a subcategory");
            entity.Property(e => e.Name).HasComment("Name of a subcategory");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of subcategory update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates subcategory");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.RoleSubCategoryCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);
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

            entity.HasOne(d => d.SubjectType).WithMany(p => p.Subjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subject_SubjectType");
        });

        modelBuilder.Entity<SubjectAllowedToOrganization>(entity =>
        {
            entity.ToTable("SubjectAllowedToOrganization", tb => tb.HasComment("Table of subjects (modules) allowed to organization"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a subject");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if subject is active");
            entity.Property(e => e.AllowedFromDateUtc).HasComment("Date from which subject (module) is allowed to organization, using coordinated universal time");
            entity.Property(e => e.AllowedToDateUtc).HasComment("Date to which subject (module) is allowed to organization, using coordinated universal time");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates subject");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of subject creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if subject is deleted");
            entity.Property(e => e.OrganizationId).HasComment("Link to dbo.Organization as an organization to which subjects are allowed");
            entity.Property(e => e.SubjectId).HasComment("Link to dbo.Subject as a specific subject (module) which is allowed to organization");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of subject update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates subject");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.SubjectAllowedToOrganizationCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Organization).WithMany(p => p.SubjectAllowedToOrganizations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubjectAllowedToOrganization_Organization");

            entity.HasOne(d => d.Subject).WithMany(p => p.SubjectAllowedToOrganizations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubjectAllowedToOrganization_Subject");
        });

        modelBuilder.Entity<SubjectAllowedToProvider>(entity =>
        {
            entity.ToTable("SubjectAllowedToProvider", tb => tb.HasComment("Table of subjects (modules) allowed to organization"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a subject");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if subject is active");
            entity.Property(e => e.AllowedFromDateUtc).HasComment("Date from which subject (module) is allowed to organization, using coordinated universal time");
            entity.Property(e => e.AllowedToDateUtc).HasComment("Date to which subject (module) is allowed to organization, using coordinated universal time");
            entity.Property(e => e.CreatedByCustomerId).HasComment("Link to dbo.Customer as an user who creates subject");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of subject creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if subject is deleted");
            entity.Property(e => e.ProviderId).HasComment("Link to dbo.Provider as an organization to which subjects are allowed");
            entity.Property(e => e.SubjectAllowedToOrganizationId).HasComment("Link to dbo.SubjectAllowedToOrganization as set of subjects (modules) which are allowed to organization and from which subjects (modules) can be allowed to specific provider");
            entity.Property(e => e.UpdateDateUtc).HasComment("Date of subject update, using coordinated universal time");
            entity.Property(e => e.UpdatedByCustomerId).HasComment("Link to dbo.Customer as an user who updates subject");

            entity.HasOne(d => d.CreatedByCustomer).WithMany(p => p.SubjectAllowedToProviderCreatedByCustomers).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Provider).WithMany(p => p.SubjectAllowedToProviders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubjectAllowedToProvider_Provider");

            entity.HasOne(d => d.SubjectAllowedToOrganization).WithMany(p => p.SubjectAllowedToProviders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SubjectAllowedToProvider_SubjectAllowedToOrganization");
        });

        modelBuilder.Entity<SubjectType>(entity =>
        {
            entity.ToTable("SubjectType", tb => tb.HasComment("System lookup table of subject types (e.g. module or part of module), also used to specify hierarchy of subjects"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a subject type");
            entity.Property(e => e.Active)
                .HasDefaultValueSql("((1))")
                .HasComment("Bit identifier if a subject type is active");
            entity.Property(e => e.CreatedDateUtc)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of subject type creation, using coordinated universal time");
            entity.Property(e => e.Deleted).HasComment("Bit identifier if a subject type is deleted");
            entity.Property(e => e.Description).HasComment("Detailed description of a subject type");
            entity.Property(e => e.Name).HasComment("Name of a subject type");
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

        modelBuilder.Entity<MediaStorage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MediaStorage");

            entity.ToTable("MediaStorage", tb => tb.HasComment("Table of binary data of multimedia"));

            entity.Property(e => e.Id).HasComment("Primary identifier of a multimedia");
            entity.Property(e => e.Data).HasComment("Binary data of a multimedia");
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
            T obj = default;
            while (result.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!Equals(result[prop.Name], DBNull.Value))
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
