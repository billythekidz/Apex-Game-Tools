namespace Apex.Demo.RTS.AI
{
    using System;

    /// <summary>
    /// Helper class for getting costs of structures and units.
    /// </summary>
    public static class CostHelper
    {
        /// unit costs

        private static readonly ResourceCost workerCost = new ResourceCost(0.5f, new ResourceCostItem(ResourceType.Metal, 60));
        private static readonly ResourceCost meleeCost = new ResourceCost(1f, new ResourceCostItem(ResourceType.Metal, 100));
        private static readonly ResourceCost rangedCost = new ResourceCost(1.1f, new ResourceCostItem(ResourceType.Metal, 110));
        private static readonly ResourceCost siegeCost = new ResourceCost(2f, new ResourceCostItem(ResourceType.Energy, 50), new ResourceCostItem(ResourceType.Metal, 120));

        /// structure costs

        private static readonly ResourceCost centerCost = new ResourceCost(12f, new ResourceCostItem(ResourceType.Energy, 250), new ResourceCostItem(ResourceType.Metal, 250));
        private static readonly ResourceCost factoryCost = new ResourceCost(7f, new ResourceCostItem(ResourceType.Energy, 250), new ResourceCostItem(ResourceType.Metal, 125));
        private static readonly ResourceCost cannonCost = new ResourceCost(4f, new ResourceCostItem(ResourceType.Energy, 150), new ResourceCostItem(ResourceType.Metal, 75));
        private static readonly ResourceCost radarCost = new ResourceCost(4f, new ResourceCostItem(ResourceType.Energy, 75), new ResourceCostItem(ResourceType.Metal, 50));
        private static readonly ResourceCost energyCost = new ResourceCost(5.5f, new ResourceCostItem(ResourceType.Metal, 100));

        /// <summary>
        /// Gets the resource cost for the specified type of structure.
        /// </summary>
        /// <param name="type">The structure type.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">The structure type specified, {type}, has not been mapped to a cost</exception>
        public static ResourceCost GetCost(StructureType type)
        {
            switch (type)
            {
                case StructureType.Center:
                {
                    return centerCost;
                }

                case StructureType.Factory:
                {
                    return factoryCost;
                }

                case StructureType.Cannon:
                {
                    return cannonCost;
                }

                case StructureType.Radar:
                {
                    return radarCost;
                }

                case StructureType.EnergyGenerator:
                {
                    return energyCost;
                }

                default:
                {
                    throw new NotImplementedException("The structure type specified, " + type + ", has not been mapped to a cost");
                }
            }
        }

        /// <summary>
        /// Gets the resource cost for the specified type of unit.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">The unit type specified, {type}, has not been mapped to a cost</exception>
        public static ResourceCost GetCost(UnitType type)
        {
            switch (type)
            {
                case UnitType.Worker:
                {
                    return workerCost;
                }

                case UnitType.Melee:
                {
                    return meleeCost;
                }

                case UnitType.Ranged:
                {
                    return rangedCost;
                }

                case UnitType.Siege:
                {
                    return siegeCost;
                }

                default:
                {
                    throw new NotImplementedException("The unit type specified, " + type + ", has not been mapped to a cost");
                }
            }
        }
    }
}