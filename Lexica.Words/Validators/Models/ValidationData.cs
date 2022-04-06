using Lexica.Core.Validators.Models;

namespace Lexica.Words.Validators.Models
{
    public class ValidationData
    {
        public StringValidationData EntryWord { get; set; } = new StringValidationData(
            minLength: 1, maxLength: 50, mandatory: true, name: "Word"
        );

        public StringValidationData EntryTranslation { get; set; } = new StringValidationData(
            minLength: 1, maxLength: 200, mandatory: true, name: "Translation"
        );
    }
}