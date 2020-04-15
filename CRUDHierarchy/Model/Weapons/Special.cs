using System;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [Serializable]
    [ClassDescription("Special weapon")]
    [DataContract]
    class Special : Weapon
    {
        public enum Type
        {
            Tazer,
            Shocker,
            Tranqulizer
        }

        [DataMember]
        private Type type;
        [DataMember]
        private int range;
    }
}
