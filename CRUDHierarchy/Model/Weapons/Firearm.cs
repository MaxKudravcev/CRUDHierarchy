namespace CRUDHierarchy
{
    abstract class Firearm : Weapon
    {
        public enum Barrel
        {
            Smooth,
            Threaded
        }

        public enum Caliber
        {
            _9mm,
            _45ACP,
            _357Magnum,
            _5_56x45mm,
            _7_62x39mm,
            _7_62NATO,
            _50BM
        }

        protected int rateOfFire;
        protected Barrel barrel;
        protected int magCapacity;
    }
}
