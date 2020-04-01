namespace CRUDHierarchy
{
    [ClassDescription("Shotgun")]
    class Shotgun : Firearm
    {
        public enum Type
        {
            PumpAction,
            LeverAction,
            SemiAutomatic
        }

        private ShotgunAmmo ammo;
        private Type type;
    }

}
