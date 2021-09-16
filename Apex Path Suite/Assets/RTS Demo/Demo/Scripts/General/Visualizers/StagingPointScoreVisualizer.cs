namespace Apex.Demo.RTS.AI.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using Apex.AI;
    using UnityEngine;

    /// <summary>
    /// Specialized visualizer for staging point scoring, as a part of offensive group orders. Purely for editor debug purposes. Uses Gizmos and GUI drawing.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public sealed class StagingPointScoreVisualizer : MonoBehaviour
    {
        private static IList<ScoredOption<StagingPoint>> _scores = new List<ScoredOption<StagingPoint>>(100);

        [SerializeField]
        private bool _drawGizmos;

        [SerializeField]
        private bool _writeGUI;

        [SerializeField]
        private float _sphereSize = 2f;

        [SerializeField]
        private float _sphereAlpha = 0.5f;

        public static void AddScores(IList<ScoredOption<StagingPoint>> stagingPointsScored)
        {
            _scores.Clear();
            _scores.AddRange(stagingPointsScored);
        }

        private void OnGUI()
        {
            if (_scores.Count == 0 || !_writeGUI)
            {
                return;
            }

            var cam = Camera.main;
            if (cam == null)
            {
                return;
            }

            var count = _scores.Count;
            for (int i = 0; i < count; i++)
            {
                var pos = _scores[i].option.position;
                var score = _scores[i].score;

                var p = cam.WorldToScreenPoint(pos);
                p.y = Screen.height - p.y;

                if (Mathf.Abs(score) < 1f)
                {
                    GUI.color = Color.black;
                }
                else if (score < 0f)
                {
                    GUI.color = Color.red;
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

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !this.enabled || !_drawGizmos)
            {
                return;
            }

            if (_scores.Count == 0)
            {
                return;
            }

            var max = _scores.Max(s => s.score);
            var min = _scores.Min(s => s.score);

            var diff = max - min;
            var count = _scores.Count;
            for (int i = 0; i < count; i++)
            {
                var pos = _scores[i].option.position;
                var score = _scores[i].score;

                var normScore = score - min;
                Gizmos.color = GetColor(normScore, diff);
                Gizmos.DrawSphere(pos, _sphereSize);
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
            return new Color((1f - quotient), quotient, 0f, _sphereAlpha);
        }
    }
}