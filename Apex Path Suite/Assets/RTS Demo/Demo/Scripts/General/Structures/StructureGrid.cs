namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Utilities;

    /// <summary>
    /// Represents a grid for structures to be built at. Each grid cell can host one structure and is set to occupied when built upon.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.GridBase{Apex.Demo.RTS.AI.StructureGridCell}" />
    public sealed class StructureGrid : GridBase<StructureGridCell>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructureGrid"/> class.
        /// </summary>
        /// <param name="center">The center position of the grid.</param>
        /// <param name="size">The size of the grid around the supplied center.</param>
        /// <param name="spacing">The spacing between individual cells.</param>
        /// <param name="cellSize">The size of cells.</param>
        public StructureGrid(Vector3 center, float size, float spacing, float cellSize)
            : base(center, size, spacing, cellSize)
        {
        }

        /// <summary>
        /// Initialize the grid at the given center.
        /// </summary>
        /// <param name="center"></param>
        protected override void Init(Vector3 center)
        {
            var space = this.spacing + this.cellSize;
            var oneSideSize = Mathf.CeilToInt(this.size / space);
            var list = ListBufferPool.GetBuffer<StructureGridCell>(oneSideSize * oneSideSize);

            var halfSize = size * 0.5f;
            var maxImprecision = spacing * 0.4f;

            // find all resources in the map
            var resourceEntities = EntityManager.GetAllEntities<IResource>();
            var resources = ListBufferPool.GetBuffer<IResource>(resourceEntities.Count());
            resources.AddRange(resourceEntities);
            var resourceCount = resources.Count;

            // construct structure grid cells and check whether they are blocked by resources and navmesh availability
            for (float x = center.x - halfSize; x <= center.x + halfSize; x += space)
            {
                for (float z = center.z - halfSize; z <= center.z + halfSize; z += space)
                {
                    UnityEngine.AI.NavMeshHit hit;
                    if (!UnityEngine.AI.NavMesh.SamplePosition(new Vector3(x, center.y, z), out hit, maxImprecision, UnityEngine.AI.NavMesh.AllAreas))
                    {
                        continue;
                    }

                    var cell = new StructureGridCell(hit.position, this.cellSize);

                    // check for collisions with resource objects
                    for (int i = 0; i < resourceCount; i++)
                    {
                        if (resources[i].cachedCollider.bounds.Intersects(cell.bounds))
                        {
                            cell.occupied = true;
                            break;
                        }
                    }

                    list.Add(cell);
                }
            }

            // convert the list, which must be a list because we cannot be sure how many cells are available due to the navmesh sampling, to an array for later high performance usage
            _cells = list.ToArray();
            ListBufferPool.ReturnBuffer(list);
            ListBufferPool.ReturnBuffer(resources);
        }

        /// <summary>
        /// Gets all unoccupied cells. Also considers build structure orders issued, to avoid two units attempting to build at the same location.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="list">The list.</param>
        public void GetAllUnoccupiedCells(AIController controller, IList<Vector3> list)
        {
            var units = controller.units;
            for (int i = 0; i < _cells.Length; i++)
            {
                if (_cells[i].occupied)
                {
                    continue;
                }

                var pos = _cells[i].center;
                if (IsPositionInUnitOrders(units, pos))
                {
                    continue;
                }

                list.Add(pos);
            }
        }

        /// <summary>
        /// Determines whether the given position is already used by another build structure order, for any of the specified units.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        private bool IsPositionInUnitOrders(IList<IUnit> units, Vector3 position)
        {
            // Check first the currently executing order for each unit, then the current order and finally checks all non-completed orders issued to the unit.
            var count = units.Count;
            for (int i = 0; i < count; i++)
            {
                var unit = units[i];
                if (unit.currentlyExecuting != null && IsPositionInOrder(unit.currentlyExecuting, position))
                {
                    return true;
                }

                if (unit.currentOrder != null && IsPositionInOrder(unit.currentOrder, position))
                {
                    return true;
                }

                var orders = unit.orders;
                if (orders.Count > 0)
                {
                    if (IsPositionInOrders(orders, position))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified position is already used by another build structure order in the given list of orders.
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        private bool IsPositionInOrders(IList<IOrder> orders, Vector3 position)
        {
            var count = orders.Count;
            for (int i = 0; i < count; i++)
            {
                if (IsPositionInOrder(orders[i], position))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the given order is a build structure order with the same position as the specified position argument.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        private bool IsPositionInOrder(IOrder order, Vector3 position)
        {
            // check the order type (very fast)
            if (order.orderType != OrderType.BuildStructure)
            {
                return false;
            }

            // if the type checked out, assume that it can be cast (it would be an error if not possible)
            if (((BuildStructureOrder)order).location == position)
            {
                return true;
            }

            return false;
        }
    }
}