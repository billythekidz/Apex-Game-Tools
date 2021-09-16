namespace Apex.Demo.RTS.AI.Editor
{
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Visualizer for the AI Controller. Purely for editor debug purposes. Uses Gizmos and GUI drawing.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(AIController))]
    public sealed class AIControllerVisualizer : MonoBehaviour
    {
        [Header("GUI")]
        public bool drawGUI = true;

        public bool rightAlignGUI;

        [Range(10, 20)]
        public int fontSize = 15;

        public Rect guiRect = new Rect(5f, 5f, 200f, 110f);

        [Header("Strategy")]
        [ReadOnly]
        public StrategyType currentStrategy;

        public Color predictedEnemyBasePositionColor = Color.red;

        [Range(0.5f, 4f)]
        public float predictedEnemyBaseSphereSize = 2f;

        public bool drawPredictedEnemyBasePosition = true;

        [Header("Resources")]
        [ReadOnly]
        public int currentMetal;

        [ReadOnly]
        public int currentEnergy;

        [Header("Entity Counts")]
        [ReadOnly]
        public int unitsCount;

        [ReadOnly]
        public int workerCount;

        [ReadOnly]
        public int structureCount;

        [ReadOnly]
        public int groupsCount;

        [ReadOnly]
        public int defaultGroupUnitsCount;

        [Header("Observations")]
        [ReadOnly]
        public int observationsCount;

        [ReadOnly]
        public int enemyObservationsCount;

        [ReadOnly]
        public int resourceObservationsCount;

        public bool drawObservations;

        [Range(0.1f, 2f)]
        public float observationsSphereSize = 0.5f;

        [Header("Structure Grid")]
        [ReadOnly]
        public int occupiedCells;

        [ReadOnly]
        public int vacantCells;

        public bool drawStructureGrid = true;

        [Header("Map Grid")]
        [ReadOnly]
        public int cellsCount;

        public Color mapGridColor = Color.cyan;

        [Range(0.1f, 0.99f)]
        public float threatAlpha = 0.5f;

#if UNITY_EDITOR
        public bool writeMapThreat = true;
        public bool addLastSeenTimestamp = true;
#endif
        public bool drawMapGrid = true;

        private AIController _controller;
        private GUIStyle _style;

        private AIController _enemyController;

        private void OnEnable()
        {
            _controller = this.GetComponent<AIController>();
            _enemyController = FindObjectsOfType<AIController>().SingleOrDefault(c => c.gameObject != this.gameObject);
        }

        private void Update()
        {
            if (_controller == null)
            {
                return;
            }

            this.currentMetal = _controller.GetCurrentResource(ResourceType.Metal);
            this.currentEnergy = _controller.GetCurrentResource(ResourceType.Energy);

            this.unitsCount = _controller.units.Count;
            this.workerCount = _controller.units.Count(u => u.unitType == UnitType.Worker);
            this.structureCount = _controller.structures.Count;
            this.groupsCount = _controller.groups.Count;
            this.defaultGroupUnitsCount = _controller.defaultGroup.count;

            this.observationsCount = _controller.observations.Count;
            this.enemyObservationsCount = _controller.enemyObservations.Count;
            this.resourceObservationsCount = _controller.observations.Where(o => o.entity.entityType == EntityType.Resource).Count();

            var occupied = 0;
            var cells = _controller.structureGrid.cells;
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].occupied)
                {
                    occupied++;
                }
            }

            this.occupiedCells = occupied;
            this.vacantCells = cells.Length - occupied;

            this.cellsCount = _controller.mapGrid.cells.Length;

            this.currentStrategy = _controller.strategy;
        }

        private void OnGUI()
        {
            if (_controller == null)
            {
                return;
            }

            // need the precompilation flag because Selection is in the UnityEditor namespace
#if UNITY_EDITOR
            if (this.writeMapThreat && _controller.mapGrid != null)
            {
                if (UnityEditor.Selection.activeGameObject == this.gameObject)
                {
                    var cells = _controller.mapGrid.cells;
                    for (int i = 0; i < cells.Length; i++)
                    {
                        var pos = Camera.main.WorldToScreenPoint(cells[i].center);
                        pos.y = Screen.height - pos.y;
                        var threat = cells[i].threat;
                        if (Mathf.Abs(threat) < 1f)
                        {
                            GUI.color = Color.black;
                        }
                        else if (threat < 0f)
                        {
                            GUI.color = Color.green;
                        }
                        else
                        {
                            GUI.color = Color.red;
                        }

                        var text = this.addLastSeenTimestamp ? string.Concat(threat.ToString("F2"), "\n", cells[i].lastSeen.ToString("F1")) : threat.ToString("F2");
                        GUI.Label(new Rect(pos.x, pos.y, 100f, 100f), text);
                    }
                }
            }
#endif

            if (!this.drawGUI)
            {
                return;
            }

            if (_style == null || _style.fontSize != this.fontSize)
            {
                _style = new GUIStyle(GUI.skin.box)
                {
                    wordWrap = true,
                    fontSize = this.fontSize,
                    alignment = TextAnchor.MiddleLeft
                };
            }

            GUI.color = _controller.color;
            if (_enemyController != null && !_enemyController.gameObject.activeSelf)
            {
                GUI.Box(new Rect(Screen.width * 0.5f - 100f, Screen.height * 0.5f - 50f, 200f, 100f), "AI DEFEATED... OTHER AI!", _style);
                return;
            }

            var rect = this.guiRect;
            if (this.rightAlignGUI)
            {
                rect.x = Screen.width - this.guiRect.x - this.guiRect.width;
            }

            GUI.Box(rect, string.Concat(
                            "Center HP: ", _controller.center.currentHealth.ToString("F0"), " / ", _controller.center.maxHealth.ToString("F0"), "\n",
                            "Structures: ", _controller.structures.Count, "\n",
                            "Units: ", _controller.units.Count, "\n",
                            "Observed Enemies: ", _controller.enemyObservations.Count, "\n",
                            "Metal: ", _controller.GetCurrentResource(ResourceType.Metal), "\n",
                            "Energy: ", _controller.GetCurrentResource(ResourceType.Energy)), _style);
        }

        private void OnDrawGizmosSelected()
        {
            if (_controller == null)
            {
                return;
            }

            if (this.drawStructureGrid && _controller.structureGrid != null)
            {
                var cells = _controller.structureGrid.cells;
                for (int i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    if (cell.occupied)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                    }

                    Gizmos.DrawCube(cell.center, cell.bounds.size);
                }
            }

            if (this.drawMapGrid && _controller.mapGrid != null)
            {
                var cells = _controller.mapGrid.cells;
                var minThreat = float.MaxValue;
                var maxThreat = float.MinValue;
                for (int i = 0; i < cells.Length; i++)
                {
                    var threat = cells[i].threat;
                    if (threat < minThreat)
                    {
                        minThreat = threat;
                    }
                    else if (threat > maxThreat)
                    {
                        maxThreat = threat;
                    }
                }

                var diffThreat = maxThreat - minThreat;
                for (int i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    var threat = cell.threat;

                    var color = this.mapGridColor;
                    if (Mathf.Abs(threat) < 1f)
                    {
                        color = Color.grey;
                    }
                    else if (diffThreat != 0f)
                    {
                        var normScore = threat - minThreat;
                        var quotient = normScore / diffThreat;
                        color = new Color(quotient, 1f - quotient, 0f);
                    }

                    color.a = this.threatAlpha;
                    Gizmos.color = color;

                    Gizmos.DrawCube(cell.center, cell.bounds.size);
                }
            }

            if (this.drawObservations && _controller.observations != null)
            {
                var heightVector = new Vector3(0f, 1f, 0f); // we need a small height buffer to ensure that lines and spheres are drawn above ground
                var observations = _controller.observations;
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
                        if (_controller.IsEnemy(obs))
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
                    Gizmos.DrawLine(_controller.center.position, obsPos);
                    Gizmos.DrawSphere(obsPos, this.observationsSphereSize);
                }
            }

            if (this.drawPredictedEnemyBasePosition && _controller != null)
            {
                Gizmos.color = this.predictedEnemyBasePositionColor;
                Gizmos.DrawWireSphere(_controller.predictedEnemyBasePosition, this.predictedEnemyBaseSphereSize);
            }
        }
    }
}