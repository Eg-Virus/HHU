using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    /// <summary>
    /// Used to construct Visual Observation body
    /// </summary>
    /// <author>Feras 13 Fab. 10</author>
    public class VisualObservationBody : IBody
    {
        /// <summary>
        /// Constructor that takes the basic elements in the object
        /// </summary>
        /// <param name="noOfTargets">Enum that represents the number of targets seen</param>
        /// <param name="targetType">Type of the target</param>
        /// <param name="altitude">Enum that represents the altitude of the target</param>
        /// <param name="heading">The heading of the target</param>
        /// <param name="observerName"></param>
        /// <param name="observationTime"></param>
        /// <author>Feras 13 Fab. 10</author>
        public VisualObservationBody(NumberofTargets noOfTargets, TargetType targetType,
            Altitude altitude, Heading heading, string observerName,DateTime observationTime)
        {
            ObservationTime = observationTime;
            if (Enum.IsDefined(typeof(NumberofTargets), noOfTargets))
                NoOfTargets = noOfTargets;
            else
                throw new ArgumentOutOfRangeException(nameof(noOfTargets), "Invalid Enamuration Value");

            if (Enum.IsDefined(typeof(TargetType), targetType))
                TargetType = targetType;
            else
                throw new ArgumentOutOfRangeException(nameof(targetType), "Invalid Enamuration Value");

            if (Enum.IsDefined(typeof(Altitude), altitude))
                Altitude = altitude;
            else
                throw new ArgumentOutOfRangeException(nameof(altitude), "Invalid Enamuration Value");

            if (Enum.IsDefined(typeof(Heading), heading))
                Heading = heading;
            else
                throw new ArgumentOutOfRangeException(nameof(heading), "Invalid Enamuration Value");

            if (Encoding.Unicode.GetByteCount(observerName) > (int)ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(observerName), "Invalid Enamuration Value");
            ObserverName = observerName;

        }

        /// <summary>
        /// Constructor that takes byte array and converts it to Visual Observation
        /// </summary>
        /// <param name="data">the byte array to be converted</param>
        /// <author>Feras 13 Fab. 10</author>
        /// <remarks>Precondition: The byte array must not include the body size and flag</remarks>
        public VisualObservationBody(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                NoOfTargets = (NumberofTargets) reader.ReadByte();
                TargetType = (TargetType) reader.ReadByte();
                Altitude = (Altitude) reader.ReadByte();
                Heading = (Heading) reader.ReadByte();
                var size = reader.ReadUInt16();
                ObserverName = Encoding.Unicode.GetString(reader.ReadBytes(size), 0, size);
                ObservationTime = new DateTime(reader.ReadInt64());
            }
        }

        #region IBody Members

        public Altitude Altitude { get; }

        public Heading Heading { get; }

        public NumberofTargets NoOfTargets { get; }

        public string ObserverName { get; }

        public TargetType TargetType { get; }

        public BodyFlag TypeFlag => BodyFlag.VisualObservation;

        public DateTime ObservationTime { get; }

        /// <summary>
        /// Converts the body to byte array
        /// </summary>
        /// <returns>body byte array</returns>
        /// <author>Feras 13 Fab. 10</author>
        public byte[] AsByteArray()
        {
            var byteArray = new List<byte>();
            var audible = new List<byte>();

            var body = new byte[2];

            Array.Copy(BitConverter.GetBytes((ushort)TypeFlag), 0, body, 0, 2);

            audible.AddRange(body);

            audible.Add((byte)NoOfTargets);
            audible.Add((byte)TargetType);
            audible.Add((byte)Altitude);
            audible.Add((byte)Heading);
            var obsNameBytes = Encoding.Unicode.GetBytes(ObserverName);
            audible.AddRange(BitConverter.GetBytes((ushort)obsNameBytes.Length));
            audible.AddRange(obsNameBytes);
            audible.AddRange(BitConverter.GetBytes(ObservationTime.Ticks));
            byteArray.AddRange(BitConverter.GetBytes((ushort)audible.Count));
            byteArray.AddRange(audible.ToArray());
           
            return byteArray.ToArray();
        }

        #endregion

        public override string ToString()
        {
            return "Visual Observation:- \n Number of Targets: " + NoOfTargets.ToString() + "\n" + "Target Type: " + TargetType + "\n" + "Altitude: " + Altitude.ToString() + "\n" + "Heading: " + Heading + "\n";
        }
    }
}
