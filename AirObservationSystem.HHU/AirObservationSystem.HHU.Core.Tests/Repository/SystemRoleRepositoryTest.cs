using System;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.User;
using AirObservationSystem.HHU.Core.Repository.User;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace AirObservationSystem.HHU.Core.Tests.Repository
{
    /// <summary>This class contains parameterized unit tests for SystemRoleRepository</summary>
    [TestFixture]
    [PexClass(typeof(SystemRoleRepository))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class SystemRoleRepositoryTest
    {

        /// <summary>Test stub for .ctor(IDatabaseFactory)</summary>
        [PexMethod]
        public SystemRoleRepository ConstructorTest(IDatabaseFactory dbFactory)
        {
            SystemRoleRepository target = new SystemRoleRepository(dbFactory);
            return target;
            // TODO: add assertions to method SystemRoleRepositoryTest.ConstructorTest(IDatabaseFactory)
        }
    }
}
