namespace CRUDHierarchy
{
    abstract class Ammo : CRUD
    {
        protected int speed;
        protected int effectiveRange;
        protected string name;

        public override string GetName() { return name; }
    }
}
