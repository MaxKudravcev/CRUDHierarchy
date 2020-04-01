namespace CRUDHierarchy
{
    [ClassDescription("Handgun")]
    class Handgun : Firearm
    {
        public enum Type
        {
            Multicharged,
            SemiAutomatic,
            Automatic
        }

        private RegularAmmo ammo;
        private Caliber caliber;
        private Type type;
    }
}
