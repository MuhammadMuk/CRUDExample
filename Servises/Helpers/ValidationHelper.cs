﻿
using ServiceContracts.DTO;
using System.ComponentModel.DataAnnotations;

namespace Servises.Helpers
{
	public class ValidationHelper
	{
		internal static void ModelValidation(object obj)
		{
			ValidationContext validationContext = new ValidationContext(obj);
			List<ValidationResult> validationResults = new List<ValidationResult>();
			bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
			if (isValid == false)
			{
				throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
			}
		}
	}
}
