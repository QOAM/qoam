namespace QOAM.Website.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.Helpers;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IssnsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            
            var issns = value.ToString().ToLinesSet();

            foreach (var issn in issns)
            {
                if (!issn.IsValidISSN())
                {
                    return new ValidationResult("\"" + issn + "\" is not a valid ISSN.");
                }
            }

            return ValidationResult.Success;
        }
    }
}