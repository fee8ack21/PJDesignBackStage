using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App.DAL.Models
{
    [Table("tblUnit")]
    public partial class TblUnit
    {
        [Key]
        [Column("cId")]
        public int CId { get; set; }
        [Required]
        [Column("cName")]
        [StringLength(50)]
        public string CName { get; set; }
        [Column("cBackStageUrl")]
        [StringLength(200)]
        public string CBackStageUrl { get; set; }
        [Column("cTemplateType")]
        public int CTemplateType { get; set; }
        [Column("cFrontStageUrl")]
        [StringLength(200)]
        public string CFrontStageUrl { get; set; }
        [Column("cIsAnotherWindow")]
        public bool CIsAnotherWindow { get; set; }
        [Required]
        [Column("cIsEnabled")]
        public bool? CIsEnabled { get; set; }
        [Column("cCreateDt", TypeName = "datetime")]
        public DateTime CCreateDt { get; set; }
        [Column("cParent")]
        public int? CParent { get; set; }
        [Column("cStageType")]
        public int CStageType { get; set; }
        [Column("cSort")]
        public byte? CSort { get; set; }
    }
}
