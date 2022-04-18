using System.Data;
using System.Threading.Tasks;
using Dapper;
using Lexica.Learning.Services;

namespace Lexica.Database.Services
{
    internal class LearningHistoryService : ILearningHistoryService
    {
        public LearningHistoryService(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public IDbConnection DbConnection { get; }

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
            await DbConnection.ExecuteAsync(
                @"insert into answer (
                    folder_path,
                    set_file_name,
                    mode,
                    question,
                    question_type,
                    answer,
                    proper_answer,
                    is_correct
                ) values (
                    @folder_path,
                    @set_file_name,
                    @mode,
                    @question,
                    @question_type,
                    @answer,
                    @proper_answer,
                    @is_correct
                )",
                new
                {
                    folder_path = namespaceName,
                    set_file_name = fileName,
                    mode,
                    question,
                    question_type = questionType,
                    answer,
                    proper_answer = properAnswer,
                    is_correct = isCorrect
                }
            );
        }
    }
}