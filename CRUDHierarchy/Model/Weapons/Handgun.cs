using System;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [Serializable]
    [ClassDescription("Handgun")]
    [DataContract]
    class Handgun : Firearm
    {
        public enum Type
        {
            Multicharged,
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
