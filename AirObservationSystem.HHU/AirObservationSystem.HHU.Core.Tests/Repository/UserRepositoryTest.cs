using System;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.User;
using AirObservationSystem.HHU.Core.Repository.User;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace AirObservationSystem.HHU.Core.Tests.Repository
{
    /// <summary>This class contains parameterized unit tests for UserRepository</summary>
    [TestFixture]
    [PexClass(typeof(UserRepository))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class UserRepositoryTest
    {

        /// <summary>Test stub for .ctor(IDatabaseFactory)</summary>
        [PexMethod]
        public UserRepository ConstructorTest(IDatabaseFactory dbFactory)
        {
            UserRepository target = new UserRepository(dbFactory);
            return target;
            // TODO: add assertions to method UserRepositoryTest.ConstructorTest(IDatabaseFactory)
        }
    }
}
