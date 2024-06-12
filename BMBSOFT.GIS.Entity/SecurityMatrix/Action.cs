using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BASE.CORE.Helper;

namespace BASE.Entity.SecurityMatrix
{
    [Table(TableFieldNameHelper.Sys.Action, Schema = Constant.Schema.SYS)]
    public class Action
    {
        [Column("id")]
        public int Id { set; get; }
        [Column("code")]
        public string Code { set; get; }
        [Column("name")]
        public string Name { set; get; }
        public ICollection<SecurityMatrix> SecurityMatrices { set; get; } = new List<SecurityMatrix>();
    }
}
