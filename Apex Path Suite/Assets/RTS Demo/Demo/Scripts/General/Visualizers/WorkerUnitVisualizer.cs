namespace Apex.Demo.RTS.AI.Editor
{
    using UnityEngine;

    /// <summary>
    /// Specialized unit visualizers for workers. In addition to normal unit visualization, also shows details about fleeing, harvesting and constructing. Purely for editor debug purposes. Uses Gizmos and GUI drawing.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.Editor.UnitVisualizer" />
    [RequireComponent(typeof(WorkerUnit))]
    public sealed class WorkerUnitVisualizer : UnitVisualizer
    {
        [Header("Worker States")]
        [ReadOnly]
        public bool isFleeing;

        [ReadOnly]
        public bool isBuilding;

        [Space]
        [ReadOnly]
        public BuildTarget buildTarget;

        public Color buildTargetColor = Color.cyan;
        public bool drawBuildTarget;

        [Header("Resources")]
        public bool drawResourceObservations;

        public bool drawResourceTarget;

        public Color resourcesColor = Color.magenta;

        [ReadOnly]
        public float currentCarriedResources;

        [ReadOnly]
        public ResourceBase resourceTarget;

        private WorkerUnit _worker;

        protected override void OnEnable()
        {
            base.OnEnable();

            _worker = this.GetComponent<WorkerUnit>();
        }

        protected override void Update()
        {
            base.Update();

            if (_worker == null)
            {
                return;
            }

            this.currentCarriedResources = _worker.currentCarriedResources;
            this.isBuilding = _worker.isBuilding;
            this.isFleeing = _worker.isFleeing;
            this.resourceTarget = _worker.resourceTarget as ResourceBase;
            this.buildTarget = _worker.context.buildTarget;
        }

        protected override void OnGUI()
        {
            if (!this.guiDrawOrders)
            {
                return;
            }

            _sb.Length = 0; // reset string builder

            if (_unit.currentlyExecuting != null)
            {
                _sb.Append(this.currentlyExecuting);

                if (_unit.currentlyExecuting.orderType == OrderType.BuildStructure)
                {
                    var buildOrder = (BuildStructureOrder)_unit.currentlyExecuting;
                    _sb.Append(" => ");
                    _sb.AppendLine(buildOrder.structureType.ToString());
                }
            }
            else if (this.isFleeing)
            {
                _sb.AppendLine("Fleeing");
            }
            else if (this.isBuilding)
            {
                _sb.AppendLine("Constructing");
            }
            else if (this.resourceTarget != null)
            {
                _sb.AppendLine("Harvesting");
            }

            var pos = Camera.main.WorldToScreenPoint(_unit.position);
            var rect = new Rect(pos.x, Screen.height - pos.y, 500f, 250f);
            GUI.Label(rect, _sb.ToString());
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            if (_worker == null || _worker.context == null)
            {
                return;
            }

            if (this.drawResourceObservations)
            {
                Gizmos.color = resourcesColor;

                var observations = _worker.observations;
                var count = observations.Count;
                for (int i = 0; i < count; i++)
                {
                    var obs = observations[i];

                    if (obs.entity.entityType != EntityType.Resource)
                    {
                        continue;
                    }

                    Gizmos.DrawLine(_worker.position, obs.position);
                    Gizmos.DrawSphere(obs.position, this.observationsSphereSize);
                }
            }

            if (this.drawResourceTarget && this.resourceTarget != null)
            {
                Gizmos.color = this.resourcesColor;
                Gizmos.DrawLine(_worker.position, this.resourceTarget.position);
                Gizmos.DrawSphere(this.resourceTarget.position, this.observationsSphereSize);
            }

            if (this.drawBuildTarget)
            {
                var buildTarget = _unit.context.buildTarget;
                if (buildTarget != null)
                {
                    Gizmos.color = this.buildTargetColor;
                    Gizmos.DrawLine(_unit.position, buildTarget.position);
                    Gizmos.DrawWireSphere(buildTarget.position, this.targetsSphereSize);
                }
            }
        }
    }
}