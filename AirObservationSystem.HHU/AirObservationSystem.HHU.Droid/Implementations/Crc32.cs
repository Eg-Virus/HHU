using System.Collections;
using System.IO;
using System.Security.Cryptography;
using AirObservationSystem.HHU.Core.PlatformInterface;
using AirObservationSystem.HHU.Droid.Implementations;
using Xamarin.Forms;

[assembly: Dependency(typeof(Crc32))]
namespace AirObservationSystem.HHU.Droid.Implementations
{
    internal sealed class Crc32 : HashAlgorithm, ICrc32
    {
        private static uint AllOnes = 0xffffffff;
        private static readonly Hashtable CachedCrc32Tables;

        private readonly uint[] _crc32Table;
        private uint _mCrc;

        /// <summary>
        /// Returns the default polynomial (used in WinZip, Ethernet, etc)
        /// </summary>
        public static uint DefaultPolynomial => 0x04C11DB7;

        /// <summary>
        /// Gets or sets the auto-cache setting of this class.
        /// </summary>
        public static bool AutoCache { get; set; }

        /// <summary>
        /// Initialize the cache
        /// </summary>
        static Crc32()
        {
            CachedCrc32Tables = Hashtable.Synchronized(new Hashtable());
            AutoCache = true;
        }

        public static void ClearCache()
        {
            CachedCrc32Tables.Clear();
        }


        /// <summary>
        /// Builds a crc32 table given a polynomial
        /// </summary>
        /// <param name="ulPolynomial"></param>
        /// <returns></returns>
        private static uint[] BuildCrc32Table(uint ulPolynomial)
        {
            var table = new uint[256];

            // 256 values representing ASCII character codes. 
            for (int i = 0; i < 256; i++)
            {
                var dwCrc = (uint)i;
                for (int j = 8; j > 0; j--)
                {
                    if ((dwCrc & 1) == 1)
                        dwCrc = (dwCrc >> 1) ^ ulPolynomial;
                    else
                        dwCrc >>= 1;
                }
                table[i] = dwCrc;
            }

            return table;
        }


        /// <summary>
        /// Creates a CRC32 object using the DefaultPolynomial
        /// </summary>
        public Crc32() : this(DefaultPolynomial)
        {
        }

        /// <summary>
        /// Creates a CRC32 object using the specified Creates a CRC32 object 
        /// </summary>
        public Crc32(uint aPolynomial) : this(aPolynomial, AutoCache)
        {
        }

        /// <summary>
        /// Construct the 
        /// </summary>
        public Crc32(uint aPolynomial, bool cacheTable)
        {
            HashSize = 32;

            _crc32Table = (uint[])CachedCrc32Tables[aPolynomial];
            if (_crc32Table == null)
            {
                _crc32Table = BuildCrc32Table(aPolynomial);
                if (cacheTable)
                    CachedCrc32Tables.Add(aPolynomial, _crc32Table);
            }
            Initialize();
        }

        public override int HashSize { get; }

        /// <summary>
        /// Initializes an implementation of HashAlgorithm.
        /// </summary>
        public override void Initialize()
        {
            _mCrc = AllOnes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        protected override void HashCore(byte[] buffer, int offset, int count)
        {
            // Save the text in the buffer. 
            for (int i = offset; i < count; i++)
            {
                ulong tabPtr = (_mCrc & 0xFF) ^ buffer[i];
                _mCrc >>= 8;
                _mCrc ^= _crc32Table[tabPtr];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override byte[] HashFinal()
        {
            byte[] finalHash = new byte[4];
            ulong finalCrc = _mCrc ^ AllOnes;

            finalHash[0] = (byte)((finalCrc >> 24) & 0xFF);
            finalHash[1] = (byte)((finalCrc >> 16) & 0xFF);
            finalHash[2] = (byte)((finalCrc >> 8) & 0xFF);
            finalHash[3] = (byte)((finalCrc >> 0) & 0xFF);

            return finalHash;
        }

        /// <summary>
        /// Computes the hash value for the specified Stream.
        /// </summary>
        public new byte[] ComputeHash(Stream inputStream)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, 4096)) > 0)
            {
                HashCore(buffer, 0, bytesRead);
            }
            return HashFinal();
        }


        /// <summary>
        /// Overloaded. Computes the hash value for the input data.
        /// </summary>
        public new byte[] ComputeHash(byte[] buffer)
        {
            return ComputeHash(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Overloaded. Computes the hash value for the input data.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public new byte[] ComputeHash(byte[] buffer, int offset, int count)
        {
            HashCore(buffer, offset, count);
            return HashFinal();
        }
    }

    
}
