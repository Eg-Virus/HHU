using System;

namespace AirObservationSystem.HHU.Core.Model.Message
{
    public interface IMessage
    {
         long Id { get; set; }
         string Body { get; set; }
         DateTime DateTime { get; set; }
    }
}