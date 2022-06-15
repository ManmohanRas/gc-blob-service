using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using morrisTestAPI.Context;
namespace morrisTestAPI.Models
{
    [Table("Tracts", Schema = "pli")]
    public partial class Tract
    {
        public Tract()
        {
     //       LandInventory = new HashSet<LandInventory>();
        }

        [Key]
        [Column("TractID")]
        public int TractId { get; set; }
        [StringLength(50)]
        public string TractName { get; set; }
        [StringLength(50)]
        public string TractLabel { get; set; }
        [StringLength(255)]
        public string AltNames { get; set; }
        [Required]
        [Column("ManagerID")]
        [StringLength(10)]
        public string ManagerId { get; set; }
        [Column("MunicipalIDlist")]
        [StringLength(50)]
        public string MunicipalIdlist { get; set; }
        [StringLength(50)]
        public string PrimaryUse { get; set; }
        public bool IsSubTract { get; set; }
        [Column("ParentTractID")]
        public int? ParentTractId { get; set; }
        [Column("bbox")]
        [StringLength(255)]
        public string Bbox { get; set; }
        [Column("geometry")]
        public string Geometry { get; set; }

     //   [InverseProperty(nameof(LandInventory.Tract))]
     //   public virtual ICollection<LandInventory> LandInventory { get; set; }
    }
}
