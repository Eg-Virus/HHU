using System;
using System.Collections.Generic;
using System.IO;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
    /// <summary>
    /// Represents the login response body
    /// </summary>
    /// <author>Feras 13 Fab. 10</author>
    public class LoginResponseBody : IBody
    {
        public int UserId { get; }

        public int LoginRequestId { get; }

        /// <summary>
        /// Constructor that takes the basic elements in the object
        /// </summary>
        /// <param name="userId">The returned userID, -1 is returned if not valid</param>
        /// <param name="loginRequestId"></param>
        /// <author>Feras 13 Fab. 10</author>
        public LoginResponseBody(int userId, int loginRequestId)
        {
            UserId = userId;
            LoginRequestId = loginRequestId;
        }

        /// <summary>
        /// Constructor that takes byte array and converts it to Login Response
        /// </summary>
        /// <param name="data">the byte array to be converted</param>
        /// <author>Feras 13 Fab. 10</author>
        /// <remarks>Precondition: The byte array must not include the body size and flag</remarks>
        public LoginResponseBody(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                UserId = reader.ReadInt32();
                LoginRequestId = reader.ReadInt32();
            }
        }

        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.LoginResponse;

        /// <summary>
        /// Converts the body to byte array
        /// </summary>
        /// <returns>body byte array</returns>
        /// <author>Feras 13 Fab. 10</author>
        public byte[] AsByteArray()
        {
            var retBytes = new List<byte>();
            retBytes.AddRange(BitConverter.GetBytes((ushort)10));
            retBytes.AddRange(BitConverter.GetBytes((ushort)TypeFlag));
            retBytes.AddRange(BitConverter.GetBytes(UserId));
            retBytes.AddRange(BitConverter.GetBytes(LoginRequestId));
            return retBytes.ToArray();
        }

        #endregion

        public override string ToString()
        {
            return "UserID: "+UserId.ToString() + "\n";
        }
    }
}
