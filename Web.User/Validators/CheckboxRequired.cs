using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Web.User.Validators
{
    public class CheckboxRequired : ValidationAttribute, IClientModelValidator
    {
        private readonly bool _checkValue;

        public CheckboxRequired(bool CheckValue)
        {
            _checkValue = CheckValue;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val-checkboxrequired", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        }

        public override bool IsValid(object? value)
        {
            if(value is bool)
            {
                return (bool)value == _checkValue;
            }
            return false;
        }
    }
}
