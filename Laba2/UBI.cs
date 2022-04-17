using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba2
{
    public class UBI
    {
        internal static Dictionary<int,UBI> CurrentUBIS = new Dictionary<int, UBI>();
        internal static Dictionary<int,UBI> OldUBIS = new Dictionary<int, UBI>();

        public int ThreatID { get; set; }
        public string ThreatName { get; set; }
        public string ThreatDescription { get; set; }
        public string ThreatSource { get; set; }
        public string ThreatObject { get; set; }
        public string ConfViolation { get; set; }
        public string IntegrityViolation { get; set; }
        public string AccessViolation { get; set; }
        public string Status { get; set; }

        public UBI(string threatID, string threatName, string threatDescription, string threatSource, string threatObject, string confViolation, string integrityViolation, string accessViolation)
        {
            ThreatID = Int32.Parse(threatID);
            ThreatName = threatName;
            ThreatDescription = threatDescription;
            ThreatSource = threatSource;
            ThreatObject = threatObject;
            ConfViolation = confViolation;
            IntegrityViolation = integrityViolation;
            AccessViolation = accessViolation;
        }
    }
}
