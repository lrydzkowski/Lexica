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

        public long SetId { get; set; }
        private SetTable? _set;
        public SetTable Set {
            get => _set ?? throw new InvalidOperationException("Unintialized property: " + nameof(Set));
            set => _set = value;
        }

        public int EntryId { get; set; }

        [MaxLength(50)]
        public string Word { get; set; } = "";

        [MaxLength(50)]
        public string Translation { get; set; } = "";
    }
}
