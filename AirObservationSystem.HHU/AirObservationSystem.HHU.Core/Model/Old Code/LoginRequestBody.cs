using System;
using System.Collections.Generic;
using System.IO;
using AirObservationSystem.HHU.Core.Helpers;
using System.Text;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    internal class LoginRequestBody : IBody
    {
        public int LoginRequestId { get; }
        public string HHUID { get; }
        public string Username { get; }
        public string Password { get; }

        /// <summary>
        /// Constructor that takes the basic elements in the object
        /// </summary>
        /// <param name="hhuid">The hhu id number</param>
        /// <param name="username">the name of the user</param>
        /// <param name="password">the password of the user</param>
        /// <param name="loginRequestId"></param>
        /// <author>Feras 13 Fab. 10</author>
        public LoginRequestBody(string hhuid, string username, string password, int loginRequestId)
        {
            HHUID = hhuid;
            if (Encoding.UTF8.GetByteCount(username) > byte.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(username), username);
            Username = username;
            if (Encoding.UTF8.GetByteCount(password) > byte.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(password), password);
            Password = password;
            LoginRequestId = loginRequestId;
        }

        /// <summary>
        /// Constructor that takes byte array and converts it to Login Request
        /// </summary>
        /// <param name="data">the byte array to be converted</param>
        /// <author>Feras 13 Fab. 10</author>
        /// <remarks>Precondition: The byte array must not include the body size and flag</remarks>
        public LoginRequestBody(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                //BodyFlag flag = (BodyFlag)reader.ReadInt16();
                int size;
                //BUG: Fix Me.
                HHUID = reader.ReadString();
                size = reader.ReadByte();
                Username = Encoding.UTF8.GetString(reader.ReadBytes(size), 0, size);
                size = reader.ReadByte();
                Password = Encoding.UTF8.GetString(reader.ReadBytes(size), 0, size);

                LoginRequestId = reader.ReadInt32();
            }
        }

        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.LoginRequest;

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
            //TODO: check
            //UTF8Encoding ascii = new UTF8Encoding();

            var body = new byte[2];

            Array.Copy(BitConverter.GetBytes((ushort)TypeFlag), 0, body, 0, 2);

            bodyBytes.AddRange(body);

            bodyBytes.Add((byte)HHUID.Length);
            bodyBytes.AddRange(ascii.GetBytes(HHUID));
            bodyBytes.Add((byte)(Username.Length * 2));
            bodyBytes.AddRange(unicode.GetBytes(Username));
            bodyBytes.Add((byte)(Password.Length * 2));
            bodyBytes.AddRange(unicode.GetBytes(Password));
            bodyBytes.AddRange(BitConverter.GetBytes(LoginRequestId));

            bodyArray.AddRange(BitConverter.GetBytes(Convert.ToInt16(bodyBytes.Count)));
            bodyArray.AddRange(bodyBytes.ToArray());

            return bodyArray.ToArray();
        }
        #endregion

        public override string ToString()
        {
            return "HHUID: " + HHUID + "\n" + "Username: " + Username + "\n" + "Password" + Password + "\n";
        }
    }
}