using System;
using System.Collections.Generic;
using AirObservationSystem.HHU.Core.Model.Message;

namespace AirObservationSystem.HHU.Core.Model.User
{
    public class IUInfo
    {

        public int IUInfoID { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string IUTag { get; set; }
        public string IUName { get; set; }
        public string PhoneNumber { get; set; }
        public bool ReciveEnable { get; set; }
        public Guid SquadCode { get; set; }
        public bool IsCentral { get; set; }
        public string VoipExtenNumber { get; set; }
        public string IUDataCenter { get; set; }
        public string IPAddress { get; set; }
        public int RegionID { get; set; }

        public virtual Region Region { get; set; }

        public object ToObject()
        {
            return new
            {
                IUInfoID = IUInfoID,
                Longitude = Longitude,
                Latitude = Latitude,
                IUTag = IUTag,
                IUName = IUName,
                PhoneNumber = PhoneNumber,
                ReciveEnable = ReciveEnable,
                SquadCode = SquadCode,
                IsCentral = IsCentral,
                VoipExtenNumber = VoipExtenNumber,
                IUDataCenter = IUDataCenter,
                IPAddress = IPAddress,
                RegionID = RegionID
            };
        }

    }
}