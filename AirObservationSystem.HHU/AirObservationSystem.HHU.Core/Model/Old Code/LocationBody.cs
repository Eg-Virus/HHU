using System;
using System.Collections.Generic;
using System.IO;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{

    /// <summary>
    /// Used to construct Location Notification body 
    /// </summary>
    /// <author>TuaimiAA 3 May. 11</author>
    public class LocationBody : IBody
    {
        public double Longtitude { get; }

        public double Latitude { get; }

        public DateTime ChangeLocationTime { get; }

        /// <summary>
        /// Constructor that takes the basic elements in the object
        /// </summary>
        /// <author>TuaimiAA 3 May 11</author>
        public LocationBody(double longtitude,double latitude,DateTime date)
        {
            Longtitude = longtitude;
            Latitude = latitude;
            ChangeLocationTime = date;
        }

        /// <summary>
        /// Constructor that takes byte array and converts it to Text Observation
        /// </summary>
        /// <param name="data">the byte array to be converted</param>
        /// <author>TuaimiAA 3 May 11</author>
        /// <remarks>Precondition: The byte array must not include the body size and flag</remarks>
        public LocationBody(byte[] data)
        {
            var reader = new BinaryReader(new MemoryStream(data));

            Longtitude = reader.ReadDouble();
            Latitude = reader.ReadDouble();
            ChangeLocationTime = new DateTime(reader.ReadInt64());
        }

        #region IBody Members
        public BodyFlag TypeFlag => BodyFlag.Location;

        /// <summary>
        /// Converts the body to byte array
        /// </summary>
        /// <returns>body byte array</returns>
        /// <author>TuaimiAA 3 May 11</author>
        public byte[] AsByteArray()
        {
            var bodyArray = new List<byte>();
            var bodyBytes = new List<byte>();

            //
            // Add body type to the fisrt byte array
            //
            var body = new byte[2];
            Array.Copy(BitConverter.GetBytes((ushort)TypeFlag), 0, body, 0, 2);
            bodyBytes.AddRange(body);

            //
            // Fill the basic data following the body type
            //
            bodyBytes.AddRange(BitConverter.GetBytes(Longtitude));
            bodyBytes.AddRange(BitConverter.GetBytes(Latitude));
            bodyBytes.AddRange(BitConverter.GetBytes(ChangeLocationTime.Ticks));

            //
            // Add the first byte array to the second one after the count
            //
            bodyArray.AddRange(BitConverter.GetBytes(Convert.ToInt16(bodyBytes.Count)));
            bodyArray.AddRange(bodyBytes.ToArray());

            return bodyArray.ToArray();
        }

        #endregion
    }
}
