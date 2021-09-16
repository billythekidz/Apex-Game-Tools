namespace Apex.Demo.RTS.AI
{
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(IUnit))]
    public sealed class UnitInitializerComponent : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 10f)]
        private float _startDelay = 1f;

        [SerializeField]
        private bool _startMovement;

        [SerializeField]
        private Transform _moveTarget;

        private IUnit _unit;

        private void OnEnable()
        {
            if (_startMovement && _moveTarget == null)
            {
                throw new System.ArgumentNullException("_moveTarget");
            }

            _unit = this.GetComponent<IUnit>();
            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(_startDelay);
            _unit.controller.units.Add(_unit);
            _unit.controller.defaultGroup.Add(_unit);
            _unit.gameObject.ColorRenderers(_unit.controller.color);

            if (_startMovement)
            {
                _unit.MoveTo(_moveTarget.position);
            }
        }
    }
}