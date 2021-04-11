using Lexica.EF.Models;
using Lexica.Learning.Services;
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

        public async Task SaveRecordToDbAsync(
            string namespaceName, 
            string fileName,
            string mode,
            string question, 
            string questionType,
            string answer, 
            string properAnswer, 
            bool isCorrect)
        {
            var historyTable = new LearningHistoryTable
            {
                Namespace = namespaceName,
                Name = fileName,
                Mode = mode,
                Question = question,
                QuestionType = questionType,
                Answer = answer,
                ProperAnswer = properAnswer,
                IsCorrect = isCorrect
            };
            await DbContext.LearningHistoryRecords.AddAsync(historyTable);
            await DbContext.SaveChangesAsync();
        }
    }
}
