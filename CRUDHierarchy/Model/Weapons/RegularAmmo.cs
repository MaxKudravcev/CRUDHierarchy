using System;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [Serializable]
    [ClassDescription("Pistol & Rifle ammo")]
    [DataContract]
    class RegularAmmo : Ammo
    {
        public enum AmmoType
        {
            Regular,
            Tracing,
            ArmorPiercing,
            Incendiary,
            Expansive
        }

        [DataMember]
        private AmmoType type;
    }
}
