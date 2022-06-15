using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using morrisTestAPI.Models;
namespace morrisTestAPI.Context
{
    public partial class PLIDbContext : DbContext
    {
        public PLIDbContext()
        {
        }

        public PLIDbContext(DbContextOptions<PLIDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Agency> Agency { get; set; }
        public virtual DbSet<LandInventory> LandInventory { get; set; }
        public virtual DbSet<Muni> Muni { get; set; }
        public virtual DbSet<Tract> Tract { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
   //         if (!optionsBuilder.IsConfigured)
   //         {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
   //             optionsBuilder.UseSqlServer("Data Source=mcgisdb;Initial Catalog=PublicLandInventoryDB;Persist Security Info=True;User ID=mcprima;Password=G!S#17:2:db:mcprima");
   //         }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agency>(entity =>
            {
                entity.Property(e => e.AgencyId).IsUnicode(false);

                entity.Property(e => e.AgencyType).IsUnicode(false);

                entity.Property(e => e.Bbox)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('512464,749910,524419,759452')");

                entity.Property(e => e.IsFundingAgency).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsLandOwnerAgency).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<LandInventory>(entity =>
            {
                entity.HasIndex(e => e.EsmntId)
                    .HasName("IX_Easement");

                entity.HasIndex(e => e.LandOwnerId)
                    .HasName("IX_LandOwner");

                entity.HasIndex(e => e.MunicipalId)
                    .HasName("IX_Municipality");

                entity.HasIndex(e => e.PamsPin)
                    .HasName("IX_PAMS_PIN");

                entity.HasIndex(e => e.TractId)
                    .HasName("IX_Tract");

                entity.Property(e => e.GlobalId).HasDefaultValueSql("(CONVERT([uniqueidentifier],CONVERT([binary](10),newid())+CONVERT([binary](6),getdate())))");

                entity.Property(e => e.AccessType)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('U')");

                entity.Property(e => e.Block).IsUnicode(false);

                entity.Property(e => e.County)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Morris')");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EncumbranceTypeCode)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FeeSimpleorEasement)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('parcel')");

                entity.Property(e => e.FundType).IsUnicode(false);

                entity.Property(e => e.Geometry).IsUnicode(false);

                entity.Property(e => e.Gisacres).HasDefaultValueSql("((0))");

                entity.Property(e => e.LandOwnerId)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((9999999))");

                entity.Property(e => e.LandOwnerNotes).IsUnicode(false);

                entity.Property(e => e.LastEditDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastEditor).IsUnicode(false);

                entity.Property(e => e.Lot).IsUnicode(false);

                entity.Property(e => e.MunicipalId).IsUnicode(false);

                entity.Property(e => e.Municipality).IsUnicode(false);

                entity.Property(e => e.ObjectId).ValueGeneratedOnAdd();

                entity.Property(e => e.OpenSpaceClass).HasDefaultValueSql("('u')");

                entity.Property(e => e.PresTrustProjectName).IsUnicode(false);

                entity.Property(e => e.PrgrmEncmbrdType)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('UNK')");

                entity.Property(e => e.PrimaryUseCode).HasDefaultValueSql("((99))");

                entity.Property(e => e.ProtectionStatus)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('unknown')");

                entity.Property(e => e.QualificationCode).IsUnicode(false);

                entity.Property(e => e.RecordedAcreage).HasDefaultValueSql("((0))");

                entity.Property(e => e.TractId).HasDefaultValueSql("((3575))");

          //      entity.HasOne(d => d.LandOwner)
          //          .WithMany(p => p.LandInventory)
          //          .HasForeignKey(d => d.LandOwnerId)
          //          .HasConstraintName("FK_LandInventory_LandOwner");

          //      entity.HasOne(d => d.Municipal)
          //          .WithMany(p => p.LandInventory)
          //          .HasForeignKey(d => d.MunicipalId)
          //          .OnDelete(DeleteBehavior.ClientSetNull)
          //          .HasConstraintName("FK_LandInventory_Lkup_Municipality");

           //     entity.HasOne(d => d.Tract)
           //         .WithMany(p => p.LandInventory)
            //        .HasForeignKey(d => d.TractId)
           //         .OnDelete(DeleteBehavior.ClientSetNull)
           //         .HasConstraintName("FK_LandInventory_Tracts");
            });

            modelBuilder.Entity<Muni>(entity =>
            {
                entity.Property(e => e.MunicipalId).IsUnicode(false);

                entity.Property(e => e.Gisacres).HasDefaultValueSql("((0))");

                entity.Property(e => e.GissqMiles).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<Tract>(entity =>
            {
                entity.HasIndex(e => e.ManagerId)
                    .HasName("IX_TractManagers");

                entity.Property(e => e.AltNames).IsUnicode(false);

                entity.Property(e => e.Bbox).IsUnicode(false);

                entity.Property(e => e.Geometry).IsUnicode(false);

                entity.Property(e => e.ManagerId)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((9999999))");

                entity.Property(e => e.MunicipalIdlist).IsUnicode(false);

                entity.Property(e => e.PrimaryUse).IsUnicode(false);

                entity.Property(e => e.TractLabel).IsUnicode(false);

                entity.Property(e => e.TractName).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
