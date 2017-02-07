using System.Runtime.Serialization;

namespace QOAM.Core
{
    //[DataContract(IsReference = false)]
    public abstract class Entity
    {
        public int Id { get; set; }
    }
}