using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDHierarchy
{
    [ClassDescription("Shotgun ammo")]
    class ShotgunAmmo : Ammo
    {
        public enum ShotgunAmmoType
        {
            Fleshettes,
            Buckshot,
            Bullet,
            Combined
        }

        private ShotgunAmmoType type;
    }
}
