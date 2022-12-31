using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App.DAL.Models
{
    [Table("tblQuestionAfter")]
    public partial class TblQuestionAfter
    {
        [Key]
        [Column("cId")]
        public int CId { get; set; }
        [Required]
        [Column("cTitle")]
        [StringLength(50)]
        public string CTitle { get; set; }
        [Column("cContent")]
        public string CContent { get; set; }
        [Column("cEditorId")]
        public int CEditorId { get; set; }
        [Column("cCreateDt", TypeName = "datetime")]
        public DateTime CCreateDt { get; set; }
        [Column("cEditDt", TypeName = "datetime")]
        public DateTime CEditDt { get; set; }
        [Column("cCreatorId")]
        public int CCreatorId { get; set; }
        [Required]
        [Column("cIsEnabled")]
        public bool? CIsEnabled { get; set; }
    }
}
