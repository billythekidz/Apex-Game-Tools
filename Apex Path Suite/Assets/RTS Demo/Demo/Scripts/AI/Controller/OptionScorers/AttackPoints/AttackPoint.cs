namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    public struct AttackPoint
    {
        public MapGridCell cell;
        public IHasHealth attackTarget;

        public AttackPoint(MapGridCell cell, IHasHealth attackTarget)
        {
            this.cell = cell;
            this.attackTarget = attackTarget;
        }

        public Vector3 position
        {
            get { return this.cell.center; }
        }
    }
}