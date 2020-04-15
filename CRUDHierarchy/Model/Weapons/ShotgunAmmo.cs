using System;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [Serializable]
    [ClassDescription("Shotgun ammo")]
    [DataContract]
    class ShotgunAmmo : Ammo
    {
        public enum ShotgunAmmoType
        {
            Fleshettes,
            Buckshot,
            Bullet,
            Combined
        }

        [DataMember]
        private ShotgunAmmoType type;
    }
}
