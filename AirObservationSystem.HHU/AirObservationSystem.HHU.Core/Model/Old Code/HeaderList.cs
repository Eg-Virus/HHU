using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    public class HeaderList
    {
        private readonly List<Header> _headerList = new List<Header>();

        public HeaderList()
        {

        }


        /// <summary>
        /// Constructor that takes byte array and converts it to Headers List
        /// </summary>
        /// <param name="data">the byte array to be converted</param>
        /// <author>Feras 13 Fab. 10</author>
        /// <remarks>Precondition: The byte array must include headers count</remarks>
        public HeaderList(byte[] data)
        {
            var i = 0;
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                var count = (int)reader.ReadByte();

                while (i < count)
                {
                    var size = (int)reader.ReadByte();
                    var name = reader.ReadBytes(size);
                    size = (int)reader.ReadByte();
                    var value = reader.ReadBytes(size);
                    //AddHeader(Encoding.ASCII.GetString(name, 0, name.Length),
                    //    Encoding.ASCII.GetString(value, 0, value.Length));
                    //TODO: need to be checked
                    AddHeader(Encoding.UTF8.GetString(name, 0, name.Length),
                        Encoding.UTF8.GetString(value, 0, value.Length));
                    i++;
                }
            }

        }

        /// <summary>
        /// Adds header to the list
        /// </summary>
        /// <param name="name">The name of the header</param>
        /// <param name="value">The value of the header</param>
        /// <author>Feras 13 Fab. 10</author>
        public void AddHeader(string name, string value)
        {
            _headerList.Add(new Header(name, value));
        }

        /// <summary>
        /// Clears all the headers from the list
        /// </summary>
        /// <author>Feras 13 Fab. 10</author>
        public void ClearAll()
        {
            _headerList?.Clear();
        }


        public bool ContainsName(string name)
        {
            var header = GetByName(name);

            return header != null;
        }

        public Header GetByName(string name)
        {
            foreach (var header in _headerList)
            {
                if (header.Name == name)
                    return header;
            }

            return null;
        }


        /// <summary>
        /// returns all the headers of the array list and convert to byte[]
        /// </summary>
        /// <Author>MOHAMED SAKR 20 Jan. 10</Author>
        public byte[] AsByteArray()
        {
            var retBytes = new List<byte>();

            if (_headerList != null)
            {
                retBytes.Add((byte)_headerList.Count);

                foreach (var h in _headerList)
                    retBytes.AddRange(h.AsByteArray());
            }

            return retBytes.ToArray();
        }

        public int Count => _headerList.Count;

        public override string ToString()
        {
            return _headerList.Aggregate("Headers: \n", (current, header) => current + header.ToString());
        }
    }
}
