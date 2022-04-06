using System.Threading.Tasks;

namespace Lexica.Learning.Services
{
    public interface ILearningHistoryService
    {
        public Task SaveRecordToDbAsync(
            string namespaceName,
            string fileName,
            string mode,
            string question,
            string questionType,
            string answer,
            string properAnswer,
            bool isCorrect
        );
    }
}