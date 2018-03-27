using AirObservationSystem.HHU.Core.PlatformInterface;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    internal class Algorithm
    {
        public string Name;
        public ICrc32 Hash;

        public override string ToString()
        {
            return Name;
        }

        public Algorithm(string aName, ICrc32 aHash)
        {
            Name = aName;
            Hash = aHash;
        }
    }
}
