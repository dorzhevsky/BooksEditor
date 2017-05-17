using NUnit.Framework;
using System.Collections.Generic;
using Web.Infrastructure;

namespace Web.Tests
{
    [TestFixture]
    public class NonEmptyListAttributeTest
    {
        [Test]
        public void ShouldValidateEmptyList()
        {
            NonEmptyListAttribute attribute = new NonEmptyListAttribute();
            bool isValid = attribute.IsValid(new List<object>());

            Assert.IsFalse(isValid);
        }

        [Test]
        public void ShouldValidateNonEmptyList()
        {
            NonEmptyListAttribute attribute = new NonEmptyListAttribute();
            bool isValid = attribute.IsValid(new List<object>{new object()});

            Assert.IsTrue(isValid);
        }
    }
}