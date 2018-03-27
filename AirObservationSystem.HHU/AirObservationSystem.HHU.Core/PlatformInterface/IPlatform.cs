
using System.Threading.Tasks;

namespace AirObservationSystem.HHU.Core.PlatformInterface
{
    public interface IPlatform 
    {
        #region Platform Info
        string GetModel();
        string GetVersion();
        string GetDbPath(string dbName);
        #endregion

        #region Battery
        //int RemainingChargePercent { get; }
        //BatteryStatus Status { get; }
        //PowerSource PowerSource { get; }
        #endregion

        void SetStatusBar(bool show);

        void CloseApp();


    }
}
