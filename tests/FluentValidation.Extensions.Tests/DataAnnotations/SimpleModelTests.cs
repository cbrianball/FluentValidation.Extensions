using FluentAssertions;
using NUnit.Framework;

namespace FluentValidation.Extensions.Tests.DataAnnotations
{
    [TestFixture]
    public class SimpleModelTests
    {
        [Test]
        public void DataAnnotationsShouldNotFire()
        {
            var person = new PersonModel();

            var v = new PersonValidator();

            v.Validate(new PersonModel()).IsValid.Should().BeTrue();
        }
    }
}
