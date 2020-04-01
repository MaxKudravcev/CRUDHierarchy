namespace CRUDHierarchy
{
    [ClassDescription("Special weapon")]
    class Special : Weapon
    {
        public enum Type
        {
            Tazer,
            Shocker,
            Tranqulizer
        }

        private Type type;
        private int range;
    }
}
