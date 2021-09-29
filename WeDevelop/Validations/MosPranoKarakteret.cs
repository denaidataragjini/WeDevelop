using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WeDevelop.Validations
{
    public class MosPranoKarakteret : ValidationAttribute
    {
        public MosPranoKarakteret(string karakteret)
        {
            _karakteret = karakteret;
        }
        private string _karakteret;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();
                foreach(char k in _karakteret)
                {
                    if (valueAsString.Contains(k))
                    {
                        return new ValidationResult("Nuk lejohet karakteri " + k);
                    }
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Vlera eshte null");
        }
    }
}