using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WeDevelop.Models
{
    public class JoKaraktere : ValidationAttribute
    {
        private readonly string _Karakteret;

        public JoKaraktere(string Karakteret)
        {
            _Karakteret = Karakteret;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                for (int i = 0; i < _Karakteret.Length; i++)
                {
                    var vleraNeString = value.ToString();
                    if (vleraNeString.Contains(_Karakteret[i]))
                    {
                        return new ValidationResult("Kujdes");
                    }
                }
            }
            return ValidationResult.Success;
        }

    }
}