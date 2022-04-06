namespace Lexica.Core.Validators.Models
{
    public class StringValidationData : GeneralValidationData
    {
        public int MinLength { get; private set; }

        public int MaxLength { get; private set; }

        public StringValidationData(int minLength, int maxLength, bool mandatory, string name) : base(mandatory, name)
        {
            MinLength = minLength;
            MaxLength = maxLength;
        }
    }
}