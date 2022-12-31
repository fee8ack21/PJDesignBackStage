using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App.DAL.Models
{
    [Table("tblPortfolioPhotoBefore")]
    public partial class TblPortfolioPhotoBefore
    {
        [Key]
        [Column("cId")]
        public int CId { get; set; }
        [Required]
        [Column("cUrl")]
        [StringLength(200)]
        public string CUrl { get; set; }
        [Column("cPortfolioId")]
        public int CPortfolioId { get; set; }
    }
}
