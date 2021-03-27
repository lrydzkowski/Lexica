namespace Lexica.MaintainingMode.Config
{
    public class MaintainingSettings
    {
        public bool ResetAfterMistake { get; set; } = false;

        public PronunciationSettings PlayPronuncation { get; set; } = new PronunciationSettings();
    }
}
