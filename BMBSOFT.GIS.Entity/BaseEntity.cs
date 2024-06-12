using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASE.Entity
{
    public class BaseEntity
    {
        [Column("created_by")]
        public string CreatedBy { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("modified_by")]
        public string ModifiedBy { get; set; }

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
    public class BaseEntity<T>
    {
        [Key]
        [Column("id")]
        public T Id { get; set; }

    }
}
