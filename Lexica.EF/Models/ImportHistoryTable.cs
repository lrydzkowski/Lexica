using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lexica.EF.Models
{
    public class ImportHistoryTable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long SetId { get; set; }
        private SetTable? _set;
        public SetTable Set {
            get => _set ?? throw new Exception("Uninitialized property: " + nameof(Set));
            set => _set = value; 
        }

        [Required]
        public DateTime ExecutedDate { get; set; }
    }
}
