using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lexica.EF.Models
{
    public class EntryTable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RecId { get; set; }

        [Required]
        public long SetId { get; set; }
        public SetTable Set { get; set; }

        [Required]
        public int EntryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Word { get; set; }

        [Required]
        [MaxLength(50)]
        public string Translation { get; set; }
    }
}
