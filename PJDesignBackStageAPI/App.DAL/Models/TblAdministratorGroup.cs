using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App.DAL.Models
{
    [Table("tblAdministratorGroup")]
    public partial class TblAdministratorGroup
    {
        [Key]
        [Column("cId")]
        public int CId { get; set; }
        [Column("cAdministratorId")]
        public int CAdministratorId { get; set; }
        [Column("cGroupId")]
        public int CGroupId { get; set; }
    }
}
