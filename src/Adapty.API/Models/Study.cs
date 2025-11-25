using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adapty.API.Models
{
    public class Study
    {
        public int Id { get; set; }
        public int DeckId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int TotalMinutes
        {
            get
            {
                if (EndTime.HasValue)
                {
                    return (int)(EndTime.Value - StartTime).TotalMinutes;
                }
                return 0;
            }
        }
    }
}