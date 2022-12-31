using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App.DAL.Models
{
    [Table("tblCategoryMappingBefore")]
    public partial class TblCategoryMappingBefore
    {
        [Key]
        [Column("cId")]
        public int CId { get; set; }
        [Column("cCategoryId")]
        public int CCategoryId { get; set; }
        [Column("cContentId")]
        public int CContentId { get; set; }
    }
}
