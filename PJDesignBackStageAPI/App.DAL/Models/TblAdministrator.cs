using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App.DAL.Models
{
    [Table("tblAdministrator")]
    public partial class TblAdministrator
    {
        [Key]
        [Column("cId")]
        public int CId { get; set; }
        [Required]
        [Column("cAccount")]
        [StringLength(50)]
        public string CAccount { get; set; }
        [Required]
        [Column("cPassword")]
        [StringLength(50)]
        public string CPassword { get; set; }
        [Required]
        [Column("cName")]
        [StringLength(50)]
        public string CName { get; set; }
        [Column("cCreateDt", TypeName = "datetime")]
        public DateTime CCreateDt { get; set; }
        [Required]
        [Column("cIsEnabled")]
        public bool? CIsEnabled { get; set; }
        [Column("cLoginAttemptCount")]
        public int CLoginAttemptCount { get; set; }
    }
}
