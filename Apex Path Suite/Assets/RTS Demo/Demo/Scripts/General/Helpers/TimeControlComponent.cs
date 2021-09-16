namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Convenience component for adjusting Unity's time scale in Play mode.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SingletonMonoBehaviour{Apex.Demo.RTS.AI.TimeControlComponent}" />
    public sealed class TimeControlComponent : SingletonMonoBehaviour<TimeControlComponent>
    {
        [SerializeField, Range(1f, 128f)]
        private float _maxTimeScale = 16f;

        [SerializeField, Range(0.0001f, 1f)]
        private float _minTimeScale = 0.0625f;

        [SerializeField, Tooltip("The size of the time GUI box.")]
        private Vector2 _guiSize = new Vector2(150f, 40f);

        [SerializeField]
        private bool _writeTime = true;

        private void OnEnable()
        {
            // default time on enable
            Time.timeScale = 1f;
        }

        private void OnDisable()
        {
            // default time on disable
            Time.timeScale = 1f;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Plus) || Input.GetKeyUp(KeyCode.KeypadPlus))
            {
                // increase time scale (up to max) on plus key
                if (Time.timeScale < _maxTimeScale)
                {
                    Time.timeScale *= 2f;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Minus) || Input.GetKeyUp(KeyCode.KeypadMinus))
            {
                // decrease time scale (down to min) on minus key
                if (Time.timeScale > _minTimeScale)
                {
                    Time.timeScale *= 0.5f;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                }
            }
        }

        private void OnGUI()
        {
            if (_writeTime)
            {
                GUI.Box(new Rect((Screen.width * 0.5f) - (_guiSize.x * 0.5f), 5f, _guiSize.x, _guiSize.y),
                    string.Concat("Time Scale: ", Time.timeScale.ToString("F2"), "x\n", "Time Elapsed: ", Time.timeSinceLevelLoad.ToString("F0"), "s"));
            }
        }
    }
}