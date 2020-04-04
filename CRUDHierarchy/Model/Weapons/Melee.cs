using System;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [Serializable]
    [ClassDescription("Melee weapon")]
    [DataContract]
    class Melee : Weapon
    {
        public enum Impact
        {
            Stabbing,
            Piercing,
            Crushing,
            StabbingAndPiercing
        }

        [DataMember]
        private int length;
        [DataMember]
        private Impact impact;
    }
}