using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace AirObservationSystem.HHU.Core.Model.Message
{
    public abstract class AMessage : IMessage
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        public string Body { get; set; }

        [Indexed]
        public DateTime DateTime { get; set; }
    }
}
