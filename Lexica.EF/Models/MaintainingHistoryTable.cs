using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lexica.EF.Models
{
    public class MaintainingHistoryTable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OperationId { get; set; }

        [ForeignKey("Entry")]
        public long EntryRecId { get; set; }
        private EntryTable? _entry;
        public EntryTable Entry
        {
            get => _entry ?? throw new InvalidOperationException("Unintialized property: " + nameof(Entry));
            set => _entry = value;
        }

        public bool IsWord { get; set; }

        public bool IsTranslation { get; set; }

        public long NumOfCorrectAnswers { get; set; }

        public long NumOfMistakes { get; set; }
    }
}
