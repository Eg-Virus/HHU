using System;
using System.Collections.Generic;
using System.IO;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Old_Code
{
     public class SynchTimeResponse:IBody
    {

         public SynchTimeResponse(DateTime t0, DateTime t1, DateTime t2)
         {
             T0 = t0;
             T1 = t1;
             T2 = t2;
         }

         public SynchTimeResponse(byte[] data)
         {
             using (var reader = new BinaryReader(new MemoryStream(data)))
             {
                 T0 = new DateTime(reader.ReadInt64());
                 T1 = new DateTime(reader.ReadInt64());
                 T2 = new DateTime(reader.ReadInt64());
             }
             //T0 = DateTime.Parse(BitConverter.ToString(data, 0));
             //T1 = DateTime.Parse(BitConverter.ToString(data, 0));
             //T2 = DateTime.Parse(BitConverter.ToString(data, 0));
         }


         public DateTime T0
         { get; private set; }

         public DateTime T1
         {get;private set;}

         public DateTime T2
         { get; private set; }


        #region IBody Members

        public BodyFlag TypeFlag => BodyFlag.SynchTimeResponse;

        public byte[] AsByteArray()
        {

            var byteArray = new List<byte>();
            var synchTimeResp = new List<byte>();

            synchTimeResp.AddRange(BitConverter.GetBytes((ushort)TypeFlag));
            synchTimeResp.AddRange(BitConverter.GetBytes(T0.Ticks));
            synchTimeResp.AddRange(BitConverter.GetBytes(T1.Ticks));
            synchTimeResp.AddRange(BitConverter.GetBytes(T2.Ticks));           

            byteArray.AddRange(BitConverter.GetBytes((ushort)synchTimeResp.Count));
            byteArray.AddRange(synchTimeResp);


            ////List<byte> bodyArray = new List<byte>();
            ////byte[] body = new byte[2];

            //Array.Copy(BitConverter.GetBytes((ushort)this.TypeFlag), 0, body, 0, 2);

            //bodyArray.AddRange(body);
            //bodyArray.AddRange(BitConverter.GetBytes(T0.Ticks));




            return byteArray.ToArray();
        }

        #endregion

        public override string ToString()
        {
            return "Time 0: " + T0 + "\n" + "Time 1 : " + T1 + "\n" + "Time 2 :" + T2 + "\n";
        }
    }
}
