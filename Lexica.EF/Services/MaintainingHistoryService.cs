using Lexica.EF.Models;
using Lexica.MaintainingMode.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.EF.Services
{
    class MaintainingHistoryService : IMaintainingHistoryService
    {
        public MaintainingHistoryService(LexicaContext dbContext)
        {
            DbContext = dbContext;
        }

        public LexicaContext DbContext { get; set; }

        public async Task SaveAsync(
            string namespaceName, 
            string fileName, 
            string question, 
            string answer, 
            string properAnswer, 
            bool isCorrect)
        {
            var historyTable = new MaintainingHistoryTable();
            historyTable.Namespace = namespaceName;
            historyTable.Name = fileName;
            historyTable.Question = question;
            historyTable.Answer = answer;
            historyTable.ProperAnswer = properAnswer;
            historyTable.IsCorrect = isCorrect;
            DbContext.MaintainingHistoryRecords.Add(historyTable);
            await DbContext.SaveChangesAsync();
        }
    }
}
