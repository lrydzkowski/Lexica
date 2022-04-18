using Lexica.Core.Models;

namespace Lexica.Core.Validators
{
    public interface IValidator<T1>
    {
        public OperationResult Validate(T1 data);
    }

    public interface IValidator<T1, T2>
    {
        public OperationResult Validate(T1 data, T2 additionalInfo);
    }
}