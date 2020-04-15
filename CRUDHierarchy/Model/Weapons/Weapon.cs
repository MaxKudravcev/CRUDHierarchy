using System;
using System.Runtime.Serialization;

namespace CRUDHierarchy
{
    [Serializable]
    [DataContract]
    abstract class Weapon : CRUD
    {
        public enum Purpose
        {
            Combat,
            Huntsman,
            Sports
        }

        [DataMember]
        protected Purpose purpose;
        [DataMember]
        protected bool isLethal;
        [DataMember]
        protected int weight;
        [DataMember]
        protected string model;
        
        public override string GetName() { return model; }
    }
}
