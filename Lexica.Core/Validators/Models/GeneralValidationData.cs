namespace Lexica.Core.Validators.Models
{
    public class GeneralValidationData
    {
        public bool Mandatory { get; }

        public string Name { get; }

        public GeneralValidationData(bool mandatory, string name)
        {
            Mandatory = mandatory;
            Name = name;
        }
    }
}