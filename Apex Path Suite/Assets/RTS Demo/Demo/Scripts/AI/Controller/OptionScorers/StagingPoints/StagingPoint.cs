namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    public struct StagingPoint
    {
        public MapGridCell cell;
        public Vector3 attackPoint;
        public IHasHealth attackTarget;

        public StagingPoint(MapGridCell cell, Vector3 attackPoint, IHasHealth attackTarget)
        {
            this.cell = cell;
            this.attackPoint = attackPoint;
            this.attackTarget = attackTarget;
        }

        public Vector3 position
        {
            get { return this.cell.center; }
        }
    }
}