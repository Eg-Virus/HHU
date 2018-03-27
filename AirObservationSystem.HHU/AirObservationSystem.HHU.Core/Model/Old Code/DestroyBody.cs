using System;
using System.Collections.Generic;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class DestroyBody : IBody
    {
        public DestroyBody() { }

        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.Destroy;

        public byte[] AsByteArray()
        {
            var desArray = new List<byte>();
            desArray.AddRange(BitConverter.GetBytes((ushort)2));
            desArray.AddRange(BitConverter.GetBytes((ushort)TypeFlag));
            return desArray.ToArray();
        }

        #endregion
    }
}
