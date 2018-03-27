using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public interface IBody
    {
        BodyFlag TypeFlag { get; }
        byte[] AsByteArray();
    }
}
