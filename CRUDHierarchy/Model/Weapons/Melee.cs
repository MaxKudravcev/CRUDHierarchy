namespace CRUDHierarchy
{
    [ClassDescription("Melee weapon")]
    class Melee : Weapon
    {
        public enum Impact
        {
            Stabbing,
            Piercing,
            Crushing,
            StabbingAndPiercing
        }

        private int length;
        private Impact impact;
    }
}