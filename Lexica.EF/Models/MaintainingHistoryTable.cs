using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lexica.EF.Models
{
    public class MaintainingHistoryTable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("operation_id")]
        public long OperationId { get; set; }

        [ForeignKey("Entry"), Column("entry_rec_id")]
        public long EntryRecId { get; set; }
        private EntryTable? _entry;
        public EntryTable Entry
        {
            get => _entry ?? throw new InvalidOperationException("Unintialized property: " + nameof(Entry));
            set => _entry = value;
        }

        [Column("is_word")]
        public bool IsWord { get; set; }

        [Column("is_translation")]
        public bool IsTranslation { get; set; }

        [Column("num_of_correct_answers")]
        public long NumOfCorrectAnswers { get; set; }

        [Column("num_of_mistakes")]
        public long NumOfMistakes { get; set; }
    }
}
