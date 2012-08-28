using System.Linq;

namespace Core
{
    public class Entity
    {
        public int Id { get; set; }

        public bool IsExisting
        {
            get { return Id != default(int); }
        }
    }
}