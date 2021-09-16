namespace Apex.Demo.RTS.AI
{
    using UnityEngine;

    /// <summary>
    /// Simple example of a component for facilitating simple RTS-style control of the camera, without mouse edge-scrolling however (keys only).
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SingletonMonoBehaviour{Apex.Demo.RTS.AI.CameraController}" />
    public sealed class CameraController : SingletonMonoBehaviour<CameraController>
    {
        [SerializeField, Range(1f, 1000f), Tooltip("How fast the camera moves.")]
        private float _moveSpeed = 100f;

        [SerializeField, Range(1f, 1000f), Tooltip("How fast the camera zooms.")]
        private float _scrollSpeed = 200f;

        [SerializeField, Range(1f, 100f), Tooltip("The minimum zoom level (Y-position) for the camera.")]
        private float _minZoom = 10f;

        [SerializeField, Range(10f, 1000f), Tooltip("The maximum zoom level (Y-position) for the camera.")]
        private float _maxZoom = 150f;

        private void Update()
        {
            MoveOnKeys();
            ZoomOnScroll();
        }

        private void ZoomOnScroll()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll == 0f)
            {
                return;
            }

            var y = Mathf.Clamp(this.transform.position.y + (-Mathf.Sign(scroll) * _scrollSpeed * Time.unscaledDeltaTime), _minZoom, _maxZoom);
            this.transform.position = new Vector3(this.transform.position.x, y, this.transform.position.z);
        }

        private void MoveOnKeys()
        {
            var vector = Vector3.zero;

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                vector += Vector3.forward;
            }

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                vector += Vector3.back;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                vector += Vector3.right;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                vector += Vector3.left;
            }

            MoveCamera(vector);
        }

        private void MoveCamera(Vector3 direction)
        {
            this.transform.position += direction.normalized * _moveSpeed * Time.unscaledDeltaTime;
        }
    }
}