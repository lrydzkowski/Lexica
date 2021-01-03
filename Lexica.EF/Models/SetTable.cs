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
        public long SetId { get; set; }

        [MaxLength(400)]
        public string Namespace { get; set; } = "";

        [MaxLength(100)]
        public string Name { get; set; } = "";

        private ICollection<EntryTable>? _entries;
        public ICollection<EntryTable> Entries {
            get => _entries ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Entries));
            set => _entries = value;
        }

        private ImportHistoryTable? _import;
        public ImportHistoryTable Import {
            get => _import ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Import));
            set => _import = value;
        }
    }
}
