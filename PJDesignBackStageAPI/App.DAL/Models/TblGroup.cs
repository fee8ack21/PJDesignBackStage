using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App.DAL.Models
{
    [Table("tblGroup")]
    public partial class TblGroup
    {
        [Key]
        [Column("cId")]
        public int CId { get; set; }
        [Required]
        [Column("cName")]
        [StringLength(50)]
        public string CName { get; set; }
    }
}
