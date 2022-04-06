namespace Lexica.Learning.Config
{
    public class LearningSettings
    {
        public int NumOfOpenQuestions { get; set; } = 2;

        public bool ResetAfterMistake { get; set; } = false;

        public PronunciationSettings PlayPronuncation { get; set; } = new PronunciationSettings();

        public bool SaveDebugLogs { get; set; } = false;
    }
}