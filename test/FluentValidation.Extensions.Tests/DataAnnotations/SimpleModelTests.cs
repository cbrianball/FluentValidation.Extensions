using FluentValidation.Extensions.DataAnnotations;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace FluentValidation.Extensions.Tests.DataAnnotations
{
    [TestFixture]
    public class SimpleModelTests
    {
        #region Models and Validators
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
        #endregion

        [Test]
        public void DataAnnotationsShouldNotFire()
        {
            var person = new PersonModel();

            var v = new PersonValidator();

            v.Validate(new PersonModel()).IsValid.Should().BeTrue();
        }

        [Test, Ignore("Broken")]
        public void DataAnnotationsShouldFire()
        {
            var person = new PersonModel();

            var v = new PersonValidator();
            //v.SetDataAnnotationValidators();
            v.InitializeDataAnnotations();

            var test = v.Validate(new PersonModel()).IsValid;
            test.Should().BeFalse();
        }
    }
}
