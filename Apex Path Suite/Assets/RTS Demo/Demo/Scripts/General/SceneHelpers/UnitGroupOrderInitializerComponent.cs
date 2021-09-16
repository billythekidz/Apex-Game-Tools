namespace Apex.Demo.RTS.AI
{
    using System;
    using System.Collections;
    using System.Linq;
    using UnityEngine;

    public sealed class UnitGroupOrderInitializerComponent : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 10f)]
        private float _startDelay = 1f;

        [SerializeField]
        private AIController _controller;

        [SerializeField]
        private HasHealthBase _attackTarget;

        [SerializeField]
        private Transform[] _waypoints = new Transform[2];

        private IUnit[] _units;

        private void OnEnable()
        {
            if (_controller == null)
            {
                throw new ArgumentNullException("_controller");
            }

            if (_attackTarget == null)
            {
                throw new ArgumentNullException("_attackTarget");
            }

            if (_waypoints == null || _waypoints.Length < 1)
            {
                throw new ArgumentNullException("_waypoints");
            }

            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(_startDelay);

            var wps = _waypoints.Select(w => w.position).ToList();
            var order = new WeakestPointAttackOrder(1, _attackTarget, wps[0], wps.GetRange(1, wps.Count - 1));

            var newGroup = _controller.defaultGroup.Split();
            var count = newGroup.count;
            for (int i = 0; i < count; i++)
            {
                newGroup[i].orders.Add(order);
            }
        }
    }
}