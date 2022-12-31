using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App.DAL.Models
{
    [Table("tblContact")]
    public partial class TblContact
    {
        [Key]
        [Column("cId")]
        public int CId { get; set; }
        [Required]
        [Column("cName")]
        [StringLength(50)]
        public string CName { get; set; }
        [Required]
        [Column("cEmail")]
        [StringLength(200)]
        public string CEmail { get; set; }
        [Required]
        [Column("cPhone")]
        [StringLength(200)]
        public string CPhone { get; set; }
        [Required]
        [Column("cContent")]
        [StringLength(200)]
        public string CContent { get; set; }
        [Column("cCreateDt", TypeName = "datetime")]
        public DateTime CCreateDt { get; set; }
        [Column("cAutoReplyDt", TypeName = "datetime")]
        public DateTime? CAutoReplyDt { get; set; }
        [Column("cAutoReplyStatus")]
        public byte CAutoReplyStatus { get; set; }
    }
}
