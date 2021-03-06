using System;
using AirObservationSystem.HHU.Core.Infrastructure;
using AirObservationSystem.HHU.Core.Model.User;
using AirObservationSystem.HHU.Core.Repository.User;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace AirObservationSystem.HHU.Core.Tests.Repository
{
    /// <summary>This class contains parameterized unit tests for RegionRepository</summary>
    [TestFixture]
    [PexClass(typeof(RegionRepository))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class RegionRepositoryTest
    {

        /// <summary>Test stub for .ctor(IDatabaseFactory)</summary>
        [PexMethod]
        public RegionRepository ConstructorTest(IDatabaseFactory dbFactory)
        {
            RegionRepository target = new RegionRepository(dbFactory);
            return target;
            // TODO: add assertions to method RegionRepositoryTest.ConstructorTest(IDatabaseFactory)
        }
    }
}
