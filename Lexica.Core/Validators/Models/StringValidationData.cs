namespace Lexica.Core.Validators.Models
{
    public class StringValidationData : GeneralValidationData
    {
        public int MinLength { get; }

        public int MaxLength { get; }

        public StringValidationData(int minLength, int maxLength, bool mandatory, string name) : base(mandatory, name)
        {
            MinLength = minLength;
            MaxLength = maxLength;
        }
    }
}