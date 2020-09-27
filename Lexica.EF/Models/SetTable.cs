using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lexica.EF.Models
{
    public class SetTable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(400)]
        public string Namespace { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<EntryTable> Entries { get; set; }

        public ImportHistoryTable Import { get; set; }
    }
}
