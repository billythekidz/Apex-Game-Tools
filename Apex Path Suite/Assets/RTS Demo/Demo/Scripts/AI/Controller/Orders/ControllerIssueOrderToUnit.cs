namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using Apex.AI;
    using Serialization;
    using UnityEngine;
    using Utilities;

    public sealed class ControllerIssueOrderToUnit : ActionWithOptions<IUnit>
    {
        [ApexSerialization]
        public OrderType orderType = OrderType.BuildStructure;

        [ApexSerialization(defaultValue = 1)]
        public int orderPriority = 1;

        [ApexSerialization, MemberDependency("orderType", (int)OrderType.BuildStructure, MaskMatch.Equals)]
        public StructureType structureType;

        [ApexSerialization, MemberDependency("orderType", (int)OrderType.BuildStructure, MaskMatch.Equals), MemberCategory(null, 10001)]
        public List<IOptionScorer<Vector3>> buildPositionScorers = new List<IOptionScorer<Vector3>>();

        public override void Execute(IAIContext context)
        {
            if (this.orderType == OrderType.None || this.orderType == OrderType.Any)
            {
                return;
            }

            var c = (ControllerContext)context;
            var units = c.controller.units;
            var count = units.Count;
            if (count == 0)
            {
                return;
            }

            var list = ListBufferPool.GetBuffer<IUnit>(units.Count);
            for (int i = 0; i < count; i++)
            {
                if (this.orderType == OrderType.BuildStructure)
                {
                    // only workers are eligible for build structure orders
                    if (units[i].unitType == UnitType.Worker)
                    {
                        list.Add(units[i]);
                    }
                }
                else if (this.orderType == OrderType.Scout)
                {
                    // any unit can scout
                    list.Add(units[i]);
                }
            }

            // find the highest scoring unit to delegate the order to
            var best = this.GetBest(c, list);
            if (best != null)
            {
                var order = CreateOrder(c);
                if (order != null)
                {
                    best.orders.Add(order);
                }
            }

            ListBufferPool.ReturnBuffer(list);
        }

        private IOrder CreateOrder(ControllerContext c)
        {
            IOrder order = null;

            switch (this.orderType)
            {
                case OrderType.BuildStructure:
                {
                    var structureGrid = c.controller.structureGrid;
                    var list = ListBufferPool.GetBuffer<Vector3>(structureGrid.cells.Length);
                    structureGrid.GetAllUnoccupiedCells(c.controller, list);

                    var best = Utils.GetBestFromList(c, list, this.buildPositionScorers);
                    order = new BuildStructureOrder(this.orderPriority, this.structureType, best);

                    ListBufferPool.ReturnBuffer(list);
                    break;
                }

                case OrderType.Scout:
                {
                    order = new ScoutOrder(this.orderPriority);
                    break;
                }

                default:
                {
                    Debug.LogWarning(this.ToString() + " has not been set up to handle order type: " + this.orderType);
                    break;
                }
            }

            return order;
        }
    }
}