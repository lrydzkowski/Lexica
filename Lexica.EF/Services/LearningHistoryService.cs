using Lexica.EF.Models;
using Lexica.LearningMode.Services;
using Lexica.MaintainingMode.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.EF.Services
{
    class LearningHistoryService : ILearningHistoryService
    {
        public LearningHistoryService(LexicaContext dbContext)
        {
            DbContext = dbContext;
        }

        public LexicaContext DbContext { get; set; }

        public async Task SaveAsync(
            string namespaceName, 
            string fileName, 
            string question, 
            string questionType,
            string answer, 
            string properAnswer, 
            bool isCorrect)
        {
            var historyTable = new LearningHistoryTable();
            historyTable.Namespace = namespaceName;
            historyTable.Name = fileName;
            historyTable.Question = question;
            historyTable.QuestionType = questionType;
            historyTable.Answer = answer;
            historyTable.ProperAnswer = properAnswer;
            historyTable.IsCorrect = isCorrect;
            DbContext.LearningHistoryRecords.Add(historyTable);
            await DbContext.SaveChangesAsync();
        }
    }
}
