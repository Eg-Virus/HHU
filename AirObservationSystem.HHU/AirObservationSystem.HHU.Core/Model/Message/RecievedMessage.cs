using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirObservationSystem.HHU.Core.Helpers;

namespace AirObservationSystem.HHU.Core.Model.Message
{
    public class RecievedMessage : AMessage
    {
        //TODO:Need to one Type :(
        public MsgType MissedType { get; set; }
        public BodyFlag MsgType { get; set; }
    }
}
