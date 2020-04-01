namespace CRUDHierarchy
{
    [ClassDescription("Rifle")]
    class Rifle : Firearm
    {
        public enum Type
        {
            NonAutomatic,
            SemiAutomatic,
            Automatic
        }

        private RegularAmmo ammo;
        private Caliber caliber;
        private Type type;
    }
}
