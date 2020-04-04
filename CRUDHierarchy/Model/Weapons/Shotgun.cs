using System;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [Serializable]
    [ClassDescription("Shotgun")]
    [DataContract]
    class Shotgun : Firearm
    {
        public enum Type
        {
            PumpAction,
            LeverAction,
            SemiAutomatic
        }

        [DataMember]
        private ShotgunAmmo ammo;
        [DataMember]
        private Type type;
    }

}
