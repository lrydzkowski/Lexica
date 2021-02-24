using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lexica.EF.Models
{
    public class EntryTable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("rec_id")]
        public long RecId { get; set; }

        [Column("set_id")]
        public long SetId { get; set; }
        private SetTable? _set;
        public SetTable Set {
            get => _set ?? throw new InvalidOperationException("Unintialized property: " + nameof(Set));
            set => _set = value;
        }

        [Column("entry_id")]
        public int EntryId { get; set; }

        [MaxLength(50), Column("word")]
        public string Word { get; set; } = "";

        [MaxLength(50), Column("translation")]
        public string Translation { get; set; } = "";

        private MaintainingHistoryTable? _history;
        public MaintainingHistoryTable History
        {
            get => _history ?? throw new InvalidOperationException("Uninitialized property: " + nameof(History));
            set => _history = value;
        }
    }
}
