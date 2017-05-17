using NUnit.Framework;
using Web.Infrastructure;

namespace Web.Tests
{
    [SetUpFixture]
    public class Setup
    {
        [SetUp]
        public void DoSetup()
        {
            Mappings.CreateMappings();
        }
    }
}