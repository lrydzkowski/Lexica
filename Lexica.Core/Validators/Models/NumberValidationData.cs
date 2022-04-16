namespace Lexica.Core.Validators.Models
{
    public class NumberValidationData : GeneralValidationData
    {
        public long MinValue { get; }

        public long MaxValue { get; }

        public NumberValidationData(long minValue, long maxValue, bool mandatory, string name) : base(mandatory, name)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}