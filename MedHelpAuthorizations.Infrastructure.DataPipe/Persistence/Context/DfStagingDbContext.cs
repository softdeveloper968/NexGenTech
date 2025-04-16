using MedHelpAuthorizations.Application.Options;
using MedHelpAuthorizations.Infrastructure.DataPipe.Contracts;
using MedHelpAuthorizations.Infrastructure.DataPipe.Entities;
using MedHelpAuthorizations.Infrastructure.DataPipe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Context;

public partial class DfStagingDbContext : DbContext
{
	//protected readonly ICurrentUserService _currentUserService;
	private readonly DatabaseSettings _dbSettings;

	public DfStagingDbContext(DbContextOptions<DfStagingDbContext> options, IOptions<DatabaseSettings> dbSettings) : base(options)
	{
		_dbSettings = dbSettings.Value;
	}

	public virtual DbSet<TblAdjustmentCode> TblAdjustmentCodes { get; set; }

	public virtual DbSet<TblCardHolder> TblCardHolders { get; set; }

	public virtual DbSet<TblCharge> TblCharges { get; set; }

	public virtual DbSet<TblClaimAdjustment> TblClaimAdjustments { get; set; }

	public virtual DbSet<TblClaimPayment> TblClaimPayments { get; set; }

	public virtual DbSet<TblInsurance> TblInsurances { get; set; }

	public virtual DbSet<TblLocation> TblLocations { get; set; }

	public virtual DbSet<TblLogDetail> TblLogDetails { get; set; }

	public virtual DbSet<TblMapping> TblMappings { get; set; }

	public virtual DbSet<TblPatient> TblPatients { get; set; }

	public virtual DbSet<TblPatientInsuranceCard> TblPatientInsuranceCards { get; set; }

	public virtual DbSet<TblPlaceOfService> TblPlaceOfServices { get; set; }

	public virtual DbSet<TblProvider> TblProviders { get; set; }

	public virtual DbSet<TblProviderLocation> TblProviderLocations { get; set; }

	public virtual DbSet<TblRemittance> TblRemittances { get; set; }

	public virtual DbSet<TblResponsibleParty> TblResponsibleParties { get; set; }

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
	{
		foreach (var entry in ChangeTracker.Entries<IDfStagingAuditableEntity>().ToList())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.StgCreatedOn = DateTime.UtcNow;
					break;

				case EntityState.Modified:
					entry.Entity.StgLastModifiedOn = DateTime.UtcNow;
					break;
			}
		}
		return await base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		// TODO: We want this only for development probably... maybe better make it configurable in logger.json config?
		optionsBuilder.EnableSensitiveDataLogging();

		// If you want to see the sql queries that efcore executes:

		// Uncomment the next line to see them in the output window of visual studio
		// optionsBuilder.LogTo(m => Debug.WriteLine(m), LogLevel.Information);

		// Or uncomment the next line if you want to see them in the console
		// optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
		optionsBuilder.UseSqlServer(_dbSettings.DataPipeConnectionString, b => { b.MigrationsAssembly(typeof(DfStagingDbContext).Assembly.FullName);  b.EnableRetryOnFailure(); });
		//optionsBuilder.UseDatabase(_dbSettings.DBProvider!, _dbSettings.DataPipeConnectionString);
	}
	//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
	//        => optionsBuilder.UseSqlServer("Server=tcp:df-stagingdb.database.windows.net,1433;User ID=df-admin;Password=@dm!n@123#$;Database=df_StagingDb;Trusted_Connection=False;");


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<TblAdjustmentCode>(entity =>
		{
			entity
				.ToTable("tbl_AdjustmentCodes");

			entity.Property(e => e.Name)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.Type)
				.HasMaxLength(1)
				.IsUnicode(false)
				.IsFixedLength();
		});

		modelBuilder.Entity<TblCardHolder>(entity =>
		{
			entity
				.ToTable("tbl_CardHolders");

			entity.Property(e => e.Address)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.City)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.DateOfBirth)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.Employer)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.FirstName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.Gender)
				.HasMaxLength(1)
				.IsUnicode(false)
				.IsFixedLength();
			entity.Property(e => e.LastName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.PostalCode)
				.HasMaxLength(10)
				.IsUnicode(false);
			entity.Property(e => e.State)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblCharge>(entity =>
		{
			entity
				.ToTable("tbl_Charges");

			entity.Property(e => e.ChargeAmount).HasColumnType("decimal(10, 2)");
			entity.Property(e => e.DateOfServiceFrom)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.DateOfServiceTo)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.PatientFirstBillDate)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.PatientLastBillDate)
				.HasMaxLength(24)
				.IsUnicode(false);
            entity.Property(e => e.InsuranceFirstBilledOn)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.InsuranceLastBilledOn)
                .HasMaxLength(24)
                .IsUnicode(false);
            entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblClaimAdjustment>(entity =>
		{
			entity
				.ToTable("tbl_ClaimAdjustments");

			entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
			entity.Property(e => e.EntryDate)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.ModifiedDate)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(255)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblClaimPayment>(entity =>
		{
			entity
				.ToTable("tbl_ClaimPayments");

			entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
			entity.Property(e => e.EntryDate)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.ModifiedDate)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(255)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblInsurance>(entity =>
		{
			entity
				.ToTable("tbl_Insurances");

			entity.Property(e => e.Active)
				.HasMaxLength(5)
				.IsUnicode(false);
			entity.Property(e => e.AddressStreetLine1)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.AddressStreetLine2)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.City)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.Name)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.Phone)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.PostalCode)
				.HasMaxLength(15)
				.IsUnicode(false);
			entity.Property(e => e.State)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblLocation>(entity =>
		{
			entity
				.ToTable("tbl_Locations");

			entity.Property(e => e.AddressStreetLine1).HasMaxLength(100);
			entity.Property(e => e.AddressStreetLine2).HasMaxLength(100);
			entity.Property(e => e.City).HasMaxLength(50);
			entity.Property(e => e.LocationName).HasMaxLength(50);
			entity.Property(e => e.PostalCode).HasMaxLength(10);
			entity.Property(e => e.State).HasMaxLength(2);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblLogDetail>(entity =>
		{
			entity.HasKey(e => e.LogId).HasName("PK__tbl_LogD__5E5486485FE466E3");

			entity.ToTable("tbl_LogDetails");

			entity.Property(e => e.ActivityName)
				.HasMaxLength(500)
				.IsUnicode(false);
			entity.Property(e => e.CreatedOn).HasColumnType("datetime");
			entity.Property(e => e.EndTime).HasColumnType("datetime");
			entity.Property(e => e.PipeLineName)
				.HasMaxLength(500)
				.IsUnicode(false);
			entity.Property(e => e.StartTime).HasColumnType("datetime");
			entity.Property(e => e.Status)
				.HasMaxLength(50)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblMapping>(entity =>
		{
			entity
				.HasNoKey()
				.ToTable("tbl_Mappings");

			entity.Property(e => e.Source)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.SourceFileName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.TableId).HasMaxLength(255);
			entity.Property(e => e.TableName).HasMaxLength(50);
			entity.Property(e => e.TableSchema).HasMaxLength(50);
			//entity.Property(e => e.TenantClientString)
			//	.HasMaxLength(100)
			//	.IsUnicode(false);
		});

		modelBuilder.Entity<TblPatient>(entity =>
		{
			entity
				.ToTable("tbl_Patients");

			entity.Property(e => e.AddressStreetLine1)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.City)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.CreatedOn)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.DateOfBirth)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.ExternalId)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.FirstName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.Gender)
				.HasMaxLength(1)
				.IsUnicode(false);
			entity.Property(e => e.HomePhoneNumber)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.LastModifiedOn)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.LastName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.MiddleName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.PostalCode)
				.HasMaxLength(10)
				.IsUnicode(false);
			entity.Property(e => e.SocialSecurityNumber)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.State)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblPatientInsuranceCard>(entity =>
		{
			entity
				.ToTable("tbl_PatientInsuranceCards");

			entity.Property(e => e.ActiveDate)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.CardHolderRelationship)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.CoInsurance).HasColumnType("decimal(10, 2)");
			entity.Property(e => e.CoPay).HasColumnType("decimal(10, 2)");
			entity.Property(e => e.EffectiveEndDate)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.EffectiveStartDate)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.InactiveDate)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.InsuranceCardOrder)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.PlanType)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblPlaceOfService>(entity =>
		{
			entity
				.ToTable("tbl_PlaceOfServices");

			entity.Property(e => e.Address)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.City)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.FaxPhone)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.Name)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.Npi).HasColumnName("NPI");
			entity.Property(e => e.OfficePhone)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.PostalCode)
				.HasMaxLength(10)
				.IsUnicode(false);
			entity.Property(e => e.State)
				.HasMaxLength(2)
				.IsUnicode(false);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblProvider>(entity =>
		{
			entity
				.ToTable("tbl_Providers");

			entity.Property(e => e.Address)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.City)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.ExternalId)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.FaxNumber)
				.HasMaxLength(15)
				.IsUnicode(false);
			entity.Property(e => e.FirstName)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.FullName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.IsActive)
				.HasMaxLength(5)
				.IsUnicode(false)
				.IsFixedLength();
			entity.Property(e => e.IsPhysiciansAssistant)
				.HasMaxLength(5)
				.IsUnicode(false)
				.IsFixedLength();
			entity.Property(e => e.LastName)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.LicenseNumber)
				.HasMaxLength(50)
				.IsUnicode(false);
			entity.Property(e => e.Npi)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.OfficePhone)
				.HasMaxLength(15)
				.IsUnicode(false);
			entity.Property(e => e.PostalCode)
				.HasMaxLength(10)
				.IsUnicode(false);
			entity.Property(e => e.SocialSecurityNumber)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.State)
				.HasMaxLength(2)
				.IsUnicode(false);
			entity.Property(e => e.TaxId)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblProviderLocation>(entity =>
		{
			entity
				.ToTable("tbl_ProviderLocations");

			entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		modelBuilder.Entity<TblRemittance>(entity =>
		{
			entity
				.ToTable("tbl_Remittances");

			entity.Property(e => e.CheckDate)
				.HasMaxLength(24)
				.IsUnicode(false);
			entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10, 2)");
			entity.Property(e => e.RemittanceFormType)
				.HasMaxLength(10)
				.IsUnicode(false);
			entity.Property(e => e.RemittanceSource)
				.HasMaxLength(10)
				.IsUnicode(false);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
			entity.Property(e => e.UndistributedAmount).HasColumnType("decimal(10, 2)");
		});

		modelBuilder.Entity<TblResponsibleParty>(entity =>
		{
			entity
				.ToTable("tbl_ResponsibleParties");

			entity.Property(e => e.AddressStreetLine1)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.AddressStreetLine2)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.City)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.Email)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.FirstName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.LastName)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.MobilePhone)
				.HasMaxLength(20)
				.IsUnicode(false);
			entity.Property(e => e.PostalCode)
				.HasMaxLength(10)
				.IsUnicode(false);
			entity.Property(e => e.State)
				.HasMaxLength(255)
				.IsUnicode(false);
			entity.Property(e => e.TenantClientString)
				.HasMaxLength(100)
				.IsUnicode(false);
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
