using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

using morrisTestAPI.Context;
namespace morrisTestAPI.Models
{
    [Table("Agencies", Schema = "pli")]
    public partial class Agency
    {
        public Agency()
        {
     //       LandInventory = new HashSet<LandInventory>();
        }

        [Key]
        [Column("AgencyID")]
        [StringLength(10)]
        public string AgencyId { get; set; }
        [Required]
        [StringLength(125)]
        public string AgencyName { get; set; }
        [StringLength(125)]
        public string AgencyLabel { get; set; }
        [StringLength(50)]
        public string AgencyAbbreviation { get; set; }
        [Required]
        [StringLength(15)]
        public string AgencyType { get; set; }
        [Required]
        public bool? IsFundingAgency { get; set; }
        [Required]
        public bool? IsLandOwnerAgency { get; set; }
        [Required]
        [Column("bbox")]
        [StringLength(100)]
        public string Bbox { get; set; }

      //  [InverseProperty(nameof(LandInventory.LandOwner))]
      //  public virtual ICollection<LandInventory> LandInventory { get; set; }
    }
}
