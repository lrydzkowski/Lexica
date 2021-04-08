using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.EF.Models
{
    public class LearningHistoryTable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("operation_id")]
        public long OperationId { get; set; }

        [Column("namespace")]
        public string Namespace { get; set; } = "";

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("mode")]
        public string Mode { get; set; } = "";

        [Column("question")]
        public string Question { get; set; } = "";

        [Column("question_type")]
        public string QuestionType { get; set; } = "";

        [Column("answer")]
        public string Answer { get; set; } = "";

        [Column("proper_answer")]
        public string ProperAnswer { get; set; } = "";

        [Column("is_correct")]
        public bool IsCorrect { get; set; } = false;
    }
}
