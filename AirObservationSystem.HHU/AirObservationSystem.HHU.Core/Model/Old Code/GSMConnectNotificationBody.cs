using System;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    /// <summary>
    /// This describes the currently avaliable connection type
    /// </summary>
    public enum GSMConnectionType : byte
    {
        None,
        Normal,
        Thuriya,
    }
    public enum ActiveCentral : byte
    {
        None,
        TK, // Tabuk
        TF, // Taif
        KM  // Khamees
    }

    public class GSMConnectNotificationBody : IBody
    {
        public GSMConnectNotificationBody(GSMConnectionType gsmConnType, ActiveCentral activeCentral)
        {
            if (Enum.IsDefined(typeof(GSMConnectionType), gsmConnType))
            {
                GsmConnType = gsmConnType;
                ActiveCentral = activeCentral;
            }
            else
                throw new ArgumentOutOfRangeException(nameof(gsmConnType), "Invalid Enamuration Value");
        }
        public GSMConnectNotificationBody(byte[] data)
        {
            GsmConnType = (GSMConnectionType)data[0];
            ActiveCentral = (ActiveCentral)data[1];
        }

        public ActiveCentral ActiveCentral { get; }

        public GSMConnectionType GsmConnType { get; }

        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.GsmConnNotification;

        public byte[] AsByteArray()
        {
            var bodyBytes = new byte[6];


            Buffer.BlockCopy(BitConverter.GetBytes((ushort)4), 0, bodyBytes, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)BodyFlag.GsmConnNotification), 0, bodyBytes, 2, 2);
            bodyBytes[4] = (byte)GsmConnType;
            bodyBytes[5] = (byte)ActiveCentral;
            return bodyBytes;
        }

        #endregion

        public override string ToString()
        {
            return "GSM Connection Type: " + GsmConnType.ToString() + Environment.NewLine +
                   "Active Central is: " + ActiveCentral.ToString() + Environment.NewLine;

        }
    }
}
