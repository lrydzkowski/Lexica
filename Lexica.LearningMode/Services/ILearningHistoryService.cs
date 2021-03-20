using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.LearningMode.Services
{
    public interface ILearningHistoryService
    {
        public Task SaveAsync(
            string namespaceName, 
            string fileName, 
            string question,
            string questionType,
            string answer, 
            string properAnswer, 
            bool isCorrect
        );
    }
}
