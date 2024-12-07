using SurvayBasket.Contracts.Answer;
using System.ComponentModel.DataAnnotations;

namespace SurvayBasket.Contracts.Validations.AnswerValidation
{
    public sealed class MinimumCountAttribute : ValidationAttribute
    {
        private readonly int _mincount;

        public MinimumCountAttribute(int mincount)
        {
            _mincount = mincount;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var list = value as IList<AnswerResponse>;
            if (list != null && list.Count >= _mincount)
            {
                // Check duplication answers 
                var seenAnswers = new HashSet<string>();
                foreach (var answer in list)
                {
                    if (!seenAnswers.Add(answer.Text))
                    {
                        return new ValidationResult("Duplicate answers are not allowed.");
                    }
                }
                
                return ValidationResult.Success;
                
            }
            return new ValidationResult($"The field {validationContext.DisplayName} must have at least {_mincount} items.");
        }
    }
}
