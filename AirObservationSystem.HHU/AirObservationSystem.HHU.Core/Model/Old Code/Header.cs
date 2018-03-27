using System;
using System.Collections.Generic;
using System.Text;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class Header
    {
        /// <summary>
        /// The name of the header
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value of the header
        /// </summary>
        public string Value { get; set; }

        public Header(string name, string value)
        {
            //TODO: Valdaition
            //if (Encoding.UTF8.GetByteCount(name) > (int)byte.MaxValue)
            if (Encoding.UTF8.GetByteCount(name) > (int)byte.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(name), name);
            Name = name;
            if (Encoding.UTF8.GetByteCount(value) > (int)byte.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), value);
            Value = value;
        }

        /// <summary>
        /// returns collection pairs of one header and convert to byte[]
        /// </summary>
        /// <Author>MOHAMED SAKR 20 Jan. 10</Author>
        public byte[] AsByteArray()
        {
            //TODO:  Need to be checked
            var headerBytes = new List<byte> {(byte) Name.Length};
           
            // Here the number of character == numbe of bytes since its ascii
            //headerBytes.AddRange(Encoding.UTF8.GetBytes(name));
            headerBytes.AddRange(Encoding.UTF8.GetBytes(Name));
            headerBytes.Add((byte)Value.Length); // Here the number of character == numbe of bytes since its unicode
            //headerBytes.AddRange(Encoding.UTF8.GetBytes(value));
            headerBytes.AddRange(Encoding.UTF8.GetBytes(Value));
            return headerBytes.ToArray();
        }

        public override string ToString()
        {
            return "Name: " + Name + "| Value:" + Value + "\n";
        }

    }
}
