using System;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [Serializable]
    [DataContract]
    abstract class Ammo : CRUD
    {
        [DataMember]
        protected int speed;
        [DataMember]
        protected int effectiveRange;
        [DataMember]
        protected string name;

        public override string GetName() { return name; }
    }
}
