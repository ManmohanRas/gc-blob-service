using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using morrisTestAPI.Context;
namespace morrisTestAPI.Models
{
    [Table("LandInventory", Schema = "pli")]
    public partial class LandInventory
    {
        [Key]
        [Column("GlobalID")]
        public Guid GlobalId { get; set; }
        [Column("ObjectID")]
        public long ObjectId { get; set; }
        [StringLength(10)]
        public string FeeSimpleorEasement { get; set; }
        [Required]
        [Column("PAMS_PIN")]
        [StringLength(38)]
        public string PamsPin { get; set; }
        [Column("EsmntID")]
        [StringLength(50)]
        public string EsmntId { get; set; }
        [Required]
        [StringLength(25)]
        public string County { get; set; }
        [Required]
        [Column("MunicipalID")]
        [StringLength(4)]
        public string MunicipalId { get; set; }
        [StringLength(50)]
        public string Municipality { get; set; }
        [StringLength(10)]
        public string Block { get; set; }
        [StringLength(10)]
        public string Lot { get; set; }
        [StringLength(11)]
        public string QualificationCode { get; set; }
        [Column("TractID")]
        public int TractId { get; set; }
        [Column("LandOwnerID")]
        [StringLength(10)]
        public string LandOwnerId { get; set; }
        public bool MultipleLandOwnersFlag { get; set; }
        public string LandOwnerNotes { get; set; }
        [Required]
        [StringLength(20)]
        public string ProtectionStatus { get; set; }
        [Required]
        [StringLength(10)]
        public string AccessType { get; set; }
        [StringLength(50)]
        public string OpenSpaceClass { get; set; }
        public int? PrimaryUseCode { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal? RecordedAcreage { get; set; }
        [StringLength(4)]
        public string RecordedAcreageSourceType { get; set; }
        [Column("GISAcres", TypeName = "numeric(10, 3)")]
        public decimal? Gisacres { get; set; }
        public int? StatCode { get; set; }
        [StringLength(3)]
        public string PrgrmEncmbrdType { get; set; }
        [StringLength(1)]
        public string EncumbranceTypeCode { get; set; }
        [Column("FUND_TYPE")]
        public string FundType { get; set; }
        [Column("FundingMCMUA")]
        public bool FundingMcmua { get; set; }
        [Column("FundingMCPC")]
        public bool FundingMcpc { get; set; }
        [Column("FundingFarmPT")]
        public bool FundingFarmPt { get; set; }
        [Column("FundingFloodMP")]
        public bool FundingFloodMp { get; set; }
        [Column("FundingOSTF")]
        public bool FundingOstf { get; set; }
        [Column("FundingHistPT")]
        public bool FundingHistPt { get; set; }
        [Column("FundingTrailPT")]
        public bool FundingTrailPt { get; set; }
        [StringLength(75)]
        public string PresTrustProjectName { get; set; }
        public string Notes { get; set; }
        [Column("geometry")]
        public string Geometry { get; set; }
        [Column("Created_Date")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(35)]
        public string LastEditor { get; set; }
        public DateTime? LastEditDate { get; set; }
        public DateTime? DesignationDate { get; set; }

     //   [ForeignKey(nameof(LandOwnerId))]
     //   [InverseProperty(nameof(Agency.LandInventory))]
     //   public virtual Agency LandOwner { get; set; }
    //    [ForeignKey(nameof(MunicipalId))]
     //   [InverseProperty(nameof(Muni.LandInventory))]
     //   public virtual Muni Municipal { get; set; }
     // //  [ForeignKey(nameof(TractId))]
      //  [InverseProperty(nameof(Tract.LandInventory))]
      //  public virtual Tract Tract { get; set; }
    }
}
