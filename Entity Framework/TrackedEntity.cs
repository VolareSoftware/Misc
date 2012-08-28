using System;

namespace Core
{
    public class TrackedEntity : Entity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}