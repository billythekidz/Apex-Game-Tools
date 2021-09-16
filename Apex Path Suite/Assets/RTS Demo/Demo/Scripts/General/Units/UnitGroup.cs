namespace Apex.Demo.RTS.AI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Represents a group of units. Manages adding and removing from the group and provides convenience methods for getting position and splicing / merging.
    /// </summary>
    public class UnitGroup
    {
        private readonly IList<IUnit> _units;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitGroup"/> class.
        /// </summary>
        /// <param name="preallocation">The preallocation.</param>
        public UnitGroup(int preallocation)
        {
            _units = new List<IUnit>(preallocation);
        }

        /// <summary>
        /// Gets the count of units in the group.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int count
        {
            get { return _units.Count; }
        }

        /// <summary>
        /// Gets the <see cref="IUnit"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="IUnit"/>.
        /// </value>
        /// <param name="idx">The index.</param>
        /// <returns></returns>
        public IUnit this[int idx]
        {
            get { return _units[idx]; }
        }

        /// <summary>
        /// Gets the center of gravity for the group - that is, the average position. Includes all units, even outliers.
        /// </summary>
        /// <value>
        /// The center of gravity.
        /// </value>
        public Vector3 centerOfGravity
        {
            get
            {
                var count = _units.Count;
                if (count == 0)
                {
                    return Vector3.zero;
                }

                if (count == 1)
                {
                    return _units[0].position;
                }

                var center = Vector3.zero;
                for (int i = 0; i < count; i++)
                {
                    center += _units[i].position;
                }

                return center / count;
            }
        }

        /// <summary>
        /// Gets the position of the 0th unit in the group, arbitrarily used as the virtual 'leader'.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 position
        {
            get
            {
                if (_units.Count == 0)
                {
                    return Vector3.zero;
                }

                return _units[0].position;
            }
        }

        /// <summary>
        /// Adds the specified unit to this group. Manages uniqueness and ensures that units only belong to one group at a time.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public void Add(IUnit unit)
        {
            if (unit.unitType == UnitType.Worker)
            {
                // workers cannot be added to any group
                return;
            }

            if (_units.Contains(unit))
            {
                return;
            }

            if (unit.group != null)
            {
                // remove unit from old group
                unit.group.Remove(unit);
            }

            unit.group = this;
            _units.Add(unit);
        }

        /// <summary>
        /// Removes the specified unit from this group.
        /// </summary>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public bool Remove(IUnit unit)
        {
            unit.group = null;
            var result = _units.Remove(unit);
            OnRemoveUnit(unit.controller);

            return result;
        }

        /// <summary>
        /// Removes the at the specified index. Checks that the index is valid.
        /// </summary>
        /// <param name="idx">The index.</param>
        public void RemoveAt(int idx)
        {
            if (idx < 0 || idx >= _units.Count)
            {
                return;
            }

            var unit = _units[idx];
            unit.group = null;
            _units.RemoveAt(idx);

            OnRemoveUnit(unit.controller);
        }

        /// <summary>
        /// Called when units are removed from this group.
        /// </summary>
        /// <param name="controller">The controller.</param>
        private void OnRemoveUnit(AIController controller)
        {
            // remove the group if empty, except if it is the default group
            if (_units.Count == 0 && controller.defaultGroup != this)
            {
                if (controller.groups != null)
                {
                    controller.groups.Remove(this);
                }
            }
        }

        /// <summary>
        /// Merge this group with the specified group. This group persists while the other is cleared.
        /// </summary>
        /// <param name="group">The group.</param>
        public void Merge(UnitGroup group)
        {
            // loop backwards in case the collection is modified (e.g. a unit joining/leaving)
            var count = group.count;
            for (int i = count - 1; i >= 0; i--)
            {
                Add(group[i]);
            }

            group.Clear();
        }

        /// <summary>
        /// Splits this entire group into a new group.
        /// </summary>
        /// <returns></returns>
        public UnitGroup Split()
        {
            return Split(0, _units.Count);
        }

        /// <summary>
        /// Splits this group at the specified start index and ending at the given end index.
        /// </summary>
        /// <param name="startIdx">The start index.</param>
        /// <param name="endIdx">The end index.</param>
        /// <returns></returns>
        public UnitGroup Split(int startIdx, int endIdx)
        {
            var start = Mathf.Max(0, startIdx);
            var end = Mathf.Min(_units.Count - 1, endIdx);
            var count = end - start;
            if (count <= 0)
            {
                return null;
            }

            var newGroup = new UnitGroup(count);
            for (int i = end; i >= start; i--)
            {
                var unit = this[i];
                RemoveAt(i);
                newGroup.Add(unit);
            }

            return newGroup;
        }

        /// <summary>
        /// Clears this unit group for all units.
        /// </summary>
        public void Clear()
        {
            var count = _units.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                Remove(_units[i]);
            }
        }
    }
}