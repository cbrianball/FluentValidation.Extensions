using FluentValidation.Extensions.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluentValidation.Extensions.Tests.DataAnnotations
{
    public class PersonModel
    {
        public string FirstName { get; set; }
        [Required]
        [StringLength(10)]
        public string LastName { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
    }

    public class PersonValidator : AbstractValidator<PersonModel>
    {
        public void InitializeDataAnnotations()
        {
            this.SetDataAnnotationValidators();
        }
    }
}
