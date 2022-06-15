using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using morrisTestAPI.Context;
namespace morrisTestAPI.Models
{
    [Table("Lkup_Municipality", Schema = "pli")]
    public partial class Muni
    {
        public Muni()
        {
     //       LandInventory = new HashSet<LandInventory>();
        }

        [Key]
        [Column("MunicipalID")]
        [StringLength(4)]
        public string MunicipalId { get; set; }
        [Required]
        [StringLength(10)]
        public string County { get; set; }
        [StringLength(35)]
        public string MunLabel { get; set; }
        [StringLength(12)]
        public string MunicipalType { get; set; }
        [StringLength(40)]
        public string Municipality { get; set; }
        [Column("GISAcres", TypeName = "numeric(10, 1)")]
        public decimal? Gisacres { get; set; }
        [Column("GISSqMiles", TypeName = "numeric(10, 2)")]
        public decimal? GissqMiles { get; set; }

     //   [InverseProperty(nameof(LandInventory.Municipal))]
     //   public virtual ICollection<LandInventory> LandInventory { get; set; }
    }
}
