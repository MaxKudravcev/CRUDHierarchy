namespace CRUDHierarchy
{
    abstract class Weapon : CRUD
    {
        public enum Purpose
        {
            Combat,
            Huntsman,
            Sports
        }

        protected Purpose purpose;
        protected bool isLethal;
        protected int weight;
        protected string model;
        
        public override string GetName() { return model; }
    }
}
