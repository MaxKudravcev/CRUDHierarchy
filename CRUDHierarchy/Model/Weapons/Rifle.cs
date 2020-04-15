using System;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [Serializable]
    [ClassDescription("Rifle")]
    [DataContract]
    class Rifle : Firearm
    {
        public enum Type
        {
            NonAutomatic,
            SemiAutomatic,
            Automatic
        }

        [DataMember]
        private RegularAmmo ammo;
        [DataMember]
        private Caliber caliber;
        [DataMember]
        private Type type;
    }
}
