using Newtonsoft.Json;
using System;

namespace CodeChallenge.Models
{
    [Serializable]
    public class Compensation
    {
        [JsonProperty("employee")]
        public Employee Employee { get; set; }

        [JsonProperty("salary")]
        public double Salary { get; set; }

        [JsonProperty("effectiveDate")]
        public DateTime EffectiveDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Compensation()
        {
            EndDate = null;
        }

    }
}
