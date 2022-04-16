namespace Lexica.CLI.Modes.Learning.Models
{
    internal class ResultStatus
    {
        public ResultStatus(int numOfCorrectAnswers, int numOfQuestions)
        {
            NumOfCorrectAnswers = numOfCorrectAnswers;
            NumOfQuestions = numOfQuestions;
        }

        public int NumOfCorrectAnswers { get; }

        public int NumOfQuestions { get; }

        public override string ToString()
        {
            return $"{NumOfCorrectAnswers}/{NumOfQuestions}";
        }

        public string ToString(int leftPad)
        {
            return NumOfCorrectAnswers.ToString().PadLeft(leftPad) + "/" + NumOfQuestions;
        }
    }
}