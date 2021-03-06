using System;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.User;
using AirObservationSystem.HHU.Core.Repository.User;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace AirObservationSystem.HHU.Core.Tests.Repository
{
    /// <summary>This class contains parameterized unit tests for UserRegionRepository</summary>
    [TestFixture]
    [PexClass(typeof(UserRegionRepository))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class UserRegionRepositoryTest
    {

        /// <summary>Test stub for .ctor(IDatabaseFactory)</summary>
        [PexMethod]
        public UserRegionRepository ConstructorTest(IDatabaseFactory dbFactory)
        {
            UserRegionRepository target = new UserRegionRepository(dbFactory);
            return target;
            // TODO: add assertions to method UserRegionRepositoryTest.ConstructorTest(IDatabaseFactory)
        }
    }
}
