using NUnit.Framework;
using Web.Infrastructure;

namespace Web.Tests
{
    [TestFixture]
    public class IsbnValidationAttributeTest
    {
        [Test]
        public void ShouldCheckIsbn10InvalidCase()
        {
            IsbnAttribute attribute = new IsbnAttribute();
            Assert.IsFalse(attribute.IsValid(""));
            Assert.IsFalse(attribute.IsValid("123"));
            Assert.IsFalse(attribute.IsValid("123f"));
            Assert.IsFalse(attribute.IsValid("0-8044-2957-Y"));
        }

        [Test]
        public void ShouldCheckIsbn10ValidCase()
        {
            IsbnAttribute attribute = new IsbnAttribute();

            Assert.IsTrue(attribute.IsValid("99921-58-10-7"));
            Assert.IsTrue(attribute.IsValid("9971-5-0210-0"));
            Assert.IsTrue(attribute.IsValid("960-425-059-0"));
            Assert.IsTrue(attribute.IsValid("80-902734-1-6"));
            Assert.IsTrue(attribute.IsValid("85-359-0277-5"));
            Assert.IsTrue(attribute.IsValid("1-84356-028-3"));
            Assert.IsTrue(attribute.IsValid("0-684-84328-5"));
            Assert.IsTrue(attribute.IsValid("0-8044-2957-X"));
            Assert.IsTrue(attribute.IsValid("0-85131-041-9"));
            Assert.IsTrue(attribute.IsValid("0-943396-04-2"));
            Assert.IsTrue(attribute.IsValid("0-9752298-0-X"));
        }

        [Test]
        public void ShouldCheckIsbn13ValidCase()
        {
            IsbnAttribute attribute = new IsbnAttribute();
            Assert.IsTrue(attribute.IsValid("978-1-86197-876-9"));
        }
    }
}