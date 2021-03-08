using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.MaintainingMode.Services
{
    public interface IMaintainingHistoryService
    {
        public Task SaveAsync(
            string namespaceName, string fileName, string question, string answer, string properAnswer, bool isCorrect
        );
    }
}
