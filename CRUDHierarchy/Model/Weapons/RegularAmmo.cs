namespace CRUDHierarchy
{
    [ClassDescription("Pistol & Rifle ammo")]
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

        private AmmoType type;
    }
}
