using System;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Repository.User;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace AirObservationSystem.HHU.Core.Tests.Repository
{
    /// <summary>This class contains parameterized unit tests for IUInfoRepository</summary>
    [TestFixture]
    [PexClass(typeof(IUInfoRepository))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class IUInfoRepositoryTest
    {

        /// <summary>Test stub for .ctor(IDatabaseFactory)</summary>
        [PexMethod]
        public IUInfoRepository ConstructorTest(IDatabaseFactory dbFactory)
        {
            IUInfoRepository target = new IUInfoRepository(dbFactory);
            return target;
            // TODO: add assertions to method IUInfoRepositoryTest.ConstructorTest(IDatabaseFactory)
        }
    }
}
