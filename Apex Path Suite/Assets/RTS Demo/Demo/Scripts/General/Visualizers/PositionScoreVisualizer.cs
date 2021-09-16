namespace Apex.Demo.RTS.AI.Editor
{
    using System.Collections.Generic;
    using Apex.AI;
    using Apex.AI.Visualization;
    using UnityEngine;

    /// <summary>
    /// Position score visualizer which visualizes MoveToBestPosition calls scoring. Purely for editor debug purposes. Uses Gizmos and GUI drawing.
    /// </summary>
    /// <seealso cref="Apex.AI.Visualization.ActionWithOptionsVisualizerComponent{Apex.Demo.RTS.AI.MoveToBestPosition, UnityEngine.Vector3}" />
    public sealed class PositionScoreVisualizer : ActionWithOptionsVisualizerComponent<MoveToBestPosition, Vector3>
    {
        [Range(0.1f, 2f)]
        public float sphereSize = 0.25f;

        [Range(0.01f, 0.99f)]
        public float sphereAlpha = 0.5f;

        protected override void DrawGUI(IList<ScoredOption<Vector3>> scoredOptions)
        {
            if (scoredOptions == null || scoredOptions.Count == 0)
            {
                return;
            }

            var cam = Camera.main;
            if (cam == null)
            {
                return;
            }

            var count = scoredOptions.Count;
            for (int i = 0; i < count; i++)
            {
                var pos = scoredOptions[i].option;
                var score = scoredOptions[i].score;

                var p = cam.WorldToScreenPoint(pos);
                p.y = Screen.height - p.y;

                if (score < 0f)
                {
                    GUI.color = Color.red;
                }
                else if (score == 0f)
                {
                    GUI.color = Color.black;
                }
                else
                {
                    GUI.color = Color.green;
                }

                var content = new GUIContent(score.ToString("F0"));
                var size = new GUIStyle(GUI.skin.label).CalcSize(content);
                GUI.Label(new Rect(p.x, p.y, size.x, size.y), content);
            }
        }

        protected override void DrawGizmos(IList<ScoredOption<Vector3>> scoredOptions)
        {
            if (scoredOptions == null || scoredOptions.Count == 0)
            {
                return;
            }

            var max = float.MinValue;
            var min = float.MaxValue;

            var count = scoredOptions.Count;
            for (int i = 0; i < count; i++)
            {
                var score = scoredOptions[i].score;
                if (score > max)
                {
                    max = score;
                }
                else if (score < min)
                {
                    min = score;
                }
            }

            var diff = max - min;
            for (int i = 0; i < count; i++)
            {
                var pos = scoredOptions[i].option;
                var score = scoredOptions[i].score;

                var normScore = score - min;
                Gizmos.color = GetColor(normScore, diff);
                Gizmos.DrawSphere(pos, this.sphereSize);
            }
        }

        private Color GetColor(float score, float maxScore)
        {
            if (maxScore <= 0f)
            {
                return Color.green;
            }

            if (score == maxScore)
            {
                return Color.cyan;
            }

            var quotient = score / maxScore;
            return new Color((1f - quotient), quotient, 0f, this.sphereAlpha);
        }

        protected override IList<Vector3> GetOptions(IAIContext context)
        {
            return ((UnitContext)context).sampledPositions;
        }
    }
}