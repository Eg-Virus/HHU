using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    /// <summary>
    /// used to construct the Observer Status of the body when send (TextObservation)
    /// </summary>
    /// <author>Mohamed Sakr 20 Jan. 10</author>
    
    /// <summary>
    /// Used to construct Text Observation body 
    /// </summary>
    /// <author>Feras 13 Fab. 10</author>
    public class TextObservationBody : IBody
    {
        #region IBody Members

        public string BodyText { get; }

        public string ObserverName { get; }

        public ObserverStatus ObStatus { get; }

        public BodyFlag TypeFlag => BodyFlag.TextObservation;
        public DateTime ObservationTime { get; }

        /// <summary>
        /// Constructor that takes the basic elements in the object
        /// </summary>
        /// <param name="obStatus">Enum that represents the observer</param>
        /// <param name="bodyText">The text sent by the observer</param>
        /// <param name="observerName"></param>
        /// <param name="observationTime"></param>
        /// <author>Feras 13 Fab. 10</author>
        public TextObservationBody(ObserverStatus obStatus, string bodyText,string observerName,
            DateTime observationTime)
        {
            ObservationTime = observationTime;
            if (Enum.IsDefined(typeof(ObserverStatus), obStatus))
                ObStatus = obStatus;
            else
                throw new ArgumentOutOfRangeException(nameof(obStatus), "Invalid Enamuration Value");

            if (Encoding.Unicode.GetByteCount(bodyText) > (int)byte.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(bodyText), "Invalid Enamuration Value");
            BodyText = bodyText;
            if (Encoding.Unicode.GetByteCount(observerName) > (int)byte.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(observerName), "Invalid Enamuration Value");
            ObserverName = observerName;
        }

        /// <summary>
        /// Constructor that takes byte array and converts it to Text Observation
        /// </summary>
        /// <param name="data">the byte array to be converted</param>
        /// <author>Feras 13 Fab. 10</author>
        /// <remarks>Precondition: The byte array must not include the body size and flag</remarks>
        public TextObservationBody(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {

                ObStatus = (ObserverStatus) reader.ReadByte();

                var size = reader.ReadUInt16();
                BodyText = Encoding.Unicode.GetString(reader.ReadBytes(size), 0, size);

                size = reader.ReadUInt16();
                ObserverName = Encoding.Unicode.GetString(reader.ReadBytes(size), 0, size);

                ObservationTime = new DateTime(reader.ReadInt64());
            }
        }

        /// <summary>
        /// Converts the body to byte array
        /// </summary>
        /// <returns>body byte array</returns>
        /// <author>Feras 13 Fab. 10</author>
        public byte[] AsByteArray()
        {
            var bodyArray = new List<byte>();
            var bodyBytes = new List<byte>();
            var unicode = new UnicodeEncoding();
            var ascii = new UTF8Encoding();

            var body = new byte[2];

            Array.Copy(BitConverter.GetBytes((ushort)TypeFlag), 0, body, 0, 2);

            bodyBytes.AddRange(body);

            if (ObStatus == ObserverStatus.Alarm)
                bodyBytes.Add((byte)ObserverStatus.Alarm);
            else if (ObStatus == ObserverStatus.Notification)
                bodyBytes.Add((byte)ObserverStatus.Notification);
            else
                throw new InvalidOperationException("Invalid Observation Status");

            var bodyTextBytes = unicode.GetBytes(BodyText);
            var us = new byte[2];

            us[0] = (byte)(bodyTextBytes.Length);
            us[1] = (byte)((bodyTextBytes.Length) >> 8);

            bodyBytes.AddRange(us);
            bodyBytes.AddRange(bodyTextBytes);

            
            var obsNameBytes = unicode.GetBytes(ObserverName);
            bodyBytes.AddRange(BitConverter.GetBytes((ushort)obsNameBytes.Length));
            bodyBytes.AddRange(obsNameBytes);

            bodyBytes.AddRange(BitConverter.GetBytes(ObservationTime.Ticks));

            bodyArray.AddRange(BitConverter.GetBytes(Convert.ToUInt16(bodyBytes.Count)));
            bodyArray.AddRange(bodyBytes.ToArray());


            return bodyArray.ToArray();
        }

        #endregion

        public override string ToString()
        {
            return "Observer Status: " + ObStatus.ToString() + "\n"+ "Body Text: "+BodyText+"\n";
        }
    }
}
