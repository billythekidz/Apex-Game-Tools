namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class UnitSetWaypointsFromOrderStagingPoints : ActionBase
    {
        [ApexSerialization(defaultValue = true)]
        public bool overrideExisting = true;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            var order = c.unit.currentOrder;
            if (order == null)
            {
                // unit has no current order
                return;
            }

            if (order.orderCategory != OrderCategoryType.Offensive)
            {
                Debug.LogWarning(this.ToString() + " unsupported order type " + order.orderCategory.ToString() + " : " + order.orderType.ToString());
                return;
            }

            var o = order as OffensiveGroupOrderBase;
            if (o == null)
            {
                // order does not have staging points
                return;
            }

            if (this.overrideExisting && c.waypoints.Count > 0)
            {
                c.waypoints.Clear();
            }

            var stagingPoints = o.stagingPoints;
            if (stagingPoints != null)
            {
                // order has no staging points
                var count = stagingPoints.Count;
                if (count == 0)
                {
                    return;
                }

                for (int i = 0; i < count; i++)
                {
                    c.waypoints.Enqueue(stagingPoints[i]);
                }
            }

            c.waypoints.Enqueue(o.attackPoint);
            c.waypoints.Enqueue(o.target.position);
        }
    }
}