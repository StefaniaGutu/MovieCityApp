using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.Common.Exceptions
{
    public class ValidationErrorException : Exception
    {
        public readonly ValidationResult validationResult;
        public readonly object Model;

        public ValidationErrorException(ValidationResult result, object model)
        {
            validationResult = result;
            Model = model;
        }
    }
}
