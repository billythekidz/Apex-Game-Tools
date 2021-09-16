namespace Apex.Demo.RTS.AI.Editor
{
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Visualizer for units, showing unit stats and drawing. Purely for editor debug purposes. Uses Gizmos and GUI drawing.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(IUnit))]
    public class UnitVisualizer : MonoBehaviour
    {
        [Header("Controller")]
        [ReadOnly]
        public AIController controller;

        [Header("State")]
        [ReadOnly]
        public bool isInGroup;

        [ReadOnly]
        public bool isMoving;

        [Header("Health")]
        [ReadOnly]
        public bool isDead;

        [ReadOnly]
        public float currentHealth;

        [ReadOnly]
        public float healthPercentage;

        [Header("Observations")]
        [ReadOnly]
        public int observationsCount;

        [ReadOnly]
        public int enemyObservationsCount;

        public bool drawObservations;

        [Range(0.1f, 2f)]
        public float observationsSphereSize = 0.5f;

        public Color scanRadiusColor = Color.yellow;
        public bool drawScanRadius;

        public Color attackRadiusColor = Color.red;
        public bool drawAttackRadius;

        [Header("Orders")]
        public bool guiDrawOrders;

        [ReadOnly]
        public OrderType currentOrder;

        [ReadOnly]
        public OrderType currentlyExecuting;

        public bool drawWaypoints;

        [Range(0.1f, 2f)]
        public float waypointsSphereSize = 0.5f;

        public Color waypointsColor = Color.yellow;

        [ReadOnly]
        public int currentWaypoints;

        [Header("Targets")]
        [Range(0.1f, 2f)]
        public float targetsSphereSize = 1f;

        [Space]
        [ReadOnly]
        public Vector3 moveTarget;

        public Color moveTargetColor = Color.blue;
        public bool drawMoveTarget;

        [Space]
        [ReadOnly]
        public HasHealthBase attackTarget;

        public Color attackTargetColor = Color.red;
        public bool drawAttackTarget;

        [Space]
        [ReadOnly]
        public HasHealthBase temporaryTarget;

        public Color temporaryTargetColor = new Color(0.5f, 0f, 0f, 1f);
        public bool drawTemporaryTarget;

        [Space]
        [ReadOnly]
        public StructureBase defendTarget;

        public Color defendTargetColor = Color.white;
        public bool drawDefendTarget;

        protected UnitBase _unit;
        protected StringBuilder _sb = new StringBuilder();

        protected virtual void OnEnable()
        {
            _unit = this.GetComponent<UnitBase>();
        }

        protected virtual void Update()
        {
            if (_unit == null || _unit.context == null)
            {
                return;
            }

            var context = _unit.context;

            this.controller = _unit.controller;
            this.isInGroup = _unit.group != null;
            this.isMoving = _unit.isMoving;

            this.isDead = _unit.isDead;
            this.currentHealth = _unit.currentHealth;
            this.healthPercentage = (this.currentHealth / _unit.maxHealth) * 100f;

            this.observationsCount = context.unit.observations.Count;
            this.enemyObservationsCount = context.unit.enemyObservations.Count;

            this.currentOrder = _unit.currentOrder != null ? _unit.currentOrder.orderType : OrderType.None;
            this.currentlyExecuting = _unit.currentlyExecuting != null ? _unit.currentlyExecuting.orderType : OrderType.None;

            this.currentWaypoints = context.waypoints.Count;

            this.moveTarget = context.moveTarget.HasValue ? context.moveTarget.Value : Vector3.zero;
            this.attackTarget = context.attackTarget as HasHealthBase;
            this.temporaryTarget = context.temporaryTarget as HasHealthBase;
            this.defendTarget = context.defendTarget as StructureBase;
        }

        protected virtual void OnGUI()
        {
            if (!this.guiDrawOrders)
            {
                return;
            }

            _sb.Length = 0; // reset string builder
            if (this.temporaryTarget != null)
            {
                _sb.Append("Temporary Combat");
            }
            else
            {
                _sb.Append(this.currentlyExecuting);
            }

            var pos = Camera.main.WorldToScreenPoint(_unit.position);
            var rect = new Rect(pos.x, Screen.height - pos.y, 500f, 250f);
            GUI.Label(rect, _sb.ToString());
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (_unit == null || _unit.context == null)
            {
                return;
            }

            if (this.drawObservations)
            {
                var heightVector = new Vector3(0f, 1f, 0f); // we need a small height buffer to ensure that lines and spheres are drawn above ground
                var observations = _unit.observations;
                var count = observations.Count;
                for (int i = 0; i < count; i++)
                {
                    var obs = observations[i];
                    if (obs.entity.entityType == EntityType.Resource)
                    {
                        Gizmos.color = Color.magenta;
                    }
                    else if (obs.entity.entityType == EntityType.Unit || obs.entity.entityType == EntityType.Structure)
                    {
                        var hasHealth = obs.GetEntity<IHasHealth>();
                        if (_unit.IsEnemy(hasHealth))
                        {
                            Gizmos.color = Color.red;
                        }
                        else
                        {
                            Gizmos.color = Color.green;
                        }
                    }
                    else
                    {
                        // should never happen, but if somehow this happens we want to see it
                        Gizmos.color = Color.gray;
                    }

                    var obsPos = obs.position + heightVector;
                    Gizmos.DrawLine(_unit.position, obsPos);
                    Gizmos.DrawSphere(obsPos, this.observationsSphereSize);
                }
            }

            if (this.drawScanRadius)
            {
                Gizmos.color = this.scanRadiusColor;
                Gizmos.DrawSphere(_unit.position, _unit.scanRadius);
            }

            if (this.drawAttackRadius)
            {
                Gizmos.color = this.attackRadiusColor;
                Gizmos.DrawSphere(_unit.position, _unit.attackRadius);
            }

            if (this.drawMoveTarget)
            {
                var moveTarget = _unit.context.moveTarget;
                if (moveTarget.HasValue)
                {
                    Gizmos.color = this.moveTargetColor;
                    Gizmos.DrawLine(_unit.position, moveTarget.Value);
                    Gizmos.DrawWireSphere(moveTarget.Value, this.targetsSphereSize);
                }
            }

            if (this.drawAttackTarget)
            {
                var attackTarget = _unit.context.attackTarget;
                if (attackTarget != null)
                {
                    Gizmos.color = this.attackTargetColor;
                    Gizmos.DrawLine(_unit.position, attackTarget.position);
                    Gizmos.DrawWireSphere(attackTarget.position, this.targetsSphereSize);
                }
            }

            if (this.drawTemporaryTarget)
            {
                var tempTarget = _unit.context.temporaryTarget;
                if (tempTarget != null)
                {
                    Gizmos.color = this.temporaryTargetColor;
                    Gizmos.DrawLine(_unit.position, tempTarget.position);
                    Gizmos.DrawWireSphere(tempTarget.position, this.targetsSphereSize);
                }
            }

            if (this.drawDefendTarget)
            {
                var defendTarget = _unit.context.defendTarget;
                if (defendTarget != null)
                {
                    Gizmos.color = this.defendTargetColor;
                    Gizmos.DrawLine(_unit.position, defendTarget.position);
                    Gizmos.DrawWireSphere(defendTarget.position, this.targetsSphereSize);
                }
            }

            var wps = _unit.context.waypoints;
            if (this.drawWaypoints && wps.Count > 0)
            {
                Gizmos.color = this.waypointsColor;
                var prevPos = _unit.context.moveTarget.HasValue ? _unit.context.moveTarget.Value : _unit.position;
                foreach (var wp in wps)
                {
                    Gizmos.DrawLine(prevPos, wp);
                    Gizmos.DrawWireSphere(wp, this.waypointsSphereSize);

                    prevPos = wp;
                }
            }
        }
    }
}