using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// This class is responsible for controlling the camera.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public float cameraSensitivity;

        private float _cameraRotationX;
        private float _cameraRotationY;

        /// <summary>
        /// Updates the camera's rotation and position based on user input.
        /// If the middle mouse button is pressed, the camera's rotation is updated based on the mouse's X and Y axes,
        /// multiplied by the camera's sensitivity. The rotation on the X axis is clamped between -90 and 90 degrees
        /// to prevent overspinning. The up and down arrow keys control the camera distance from the player.
        /// </summary>
        private void Update()
        {
            if (Mouse.current.middleButton.isPressed)
            {
                var mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
                var mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;

                _cameraRotationY -= mouseX;
                _cameraRotationX -= mouseY;
                _cameraRotationX = Mathf.Clamp(_cameraRotationX, -90f, 90f);
            }

            if (Keyboard.current.downArrowKey.isPressed)
                transform.Translate(new Vector3(0, 0, -0.001f));
            else if (Keyboard.current.upArrowKey.isPressed)
                transform.Translate(new Vector3(0, 0, 0.001f), Space.Self);
        }

        /// <summary>
        /// The actual transform operation of the camera happens here to avoid micro stuttering in the animation.
        /// </summary>
        private void LateUpdate() =>
            transform.parent.rotation = Quaternion.Euler(_cameraRotationX, -_cameraRotationY, 0f);
    }
}
