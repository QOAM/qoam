namespace QOAM.Core.Helpers
{
    using System.ComponentModel.DataAnnotations;

    public static class ValidatorExtensions
    {
        public static void Validate(this object obj)
        {
            Validator.ValidateObject(obj, new ValidationContext(obj));
        }

        public static bool IsValid(this object obj)
        {
            return Validator.TryValidateObject(obj, new ValidationContext(obj), null, true);
        }
    }
}