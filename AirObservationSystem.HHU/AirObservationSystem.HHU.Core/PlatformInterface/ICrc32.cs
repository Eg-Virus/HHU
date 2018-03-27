
using System.IO;

namespace AirObservationSystem.HHU.Core.PlatformInterface
{
    public interface ICrc32
    {
        void Initialize();
        byte[] ComputeHash(Stream inputStream);
        byte[] ComputeHash(byte[] buffer);
        byte[] ComputeHash(byte[] buffer, int offset, int count);
    }
}
