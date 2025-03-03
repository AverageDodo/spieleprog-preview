using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /// <summary>
    /// This class is responsible for controlling the players movement by capturing Keyboard inputs and translating
    /// them to appropriate <c>AddForce</c> calls to be applied to the player objects rigidbody.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public float playerSpeedModifier;
        public float jumpHeightModifier;
        public float maxDistanceToGround;
    
        private bool _isGrounded;
        private Camera _mainCamera;
        private Rigidbody _playerRigidbody;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _playerRigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var lookDirection = transform.position - _mainCamera.transform.position;
            lookDirection.Scale(new Vector3(1, 0, 1));
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, maxDistanceToGround);

            if (Keyboard.current.wKey.isPressed)
            {
                _playerRigidbody.AddForce(lookDirection * playerSpeedModifier);
            }
            if (Keyboard.current.sKey.isPressed)
            {
                _playerRigidbody.AddForce(-lookDirection * playerSpeedModifier);
            }
            if (Keyboard.current.aKey.isPressed)
            {
                _playerRigidbody.AddForce((Quaternion.Euler(0, -90, 0) * lookDirection) * playerSpeedModifier);
            }
            if (Keyboard.current.dKey.isPressed)
            {
                _playerRigidbody.AddForce((Quaternion.Euler(0, 90, 0) * lookDirection) * playerSpeedModifier);
            }
            if (Keyboard.current.spaceKey.isPressed && _isGrounded)
            {
                _playerRigidbody.AddForce(0, 1f * jumpHeightModifier, 0, ForceMode.Impulse);
            }
        }
    }
}
