using System;
using UnityEngine;

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        private Vector2 uiMoveDirection = Vector2.zero;

        // Touch input handling
        private int touchFingerId = -1;
        private Vector2 touchStartPosition;

        public float touchDeadZone = 20f;
        public float maxMoveDistance = 100f;

        void Update()
        {
            HandleTouchMovement();

            if (uiMoveDirection != Vector2.zero)
            {
                move = uiMoveDirection;
            }
            else if (move != Vector2.zero)
            {
                move = Vector2.zero;
            }
        }

        private void HandleTouchMovement()
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            // Start tracking the first finger
                            if (touchFingerId == -1)
                            {
                                touchFingerId = touch.fingerId;
                                touchStartPosition = touch.position;
                            }
                            break;

                        case TouchPhase.Moved:
                        case TouchPhase.Stationary:
                            if (touch.fingerId == touchFingerId)
                            {
                                Vector2 delta = touch.position - touchStartPosition;

                                if (delta.magnitude > touchDeadZone)
                                {
                                    Vector2 normalizedDelta = Vector2.ClampMagnitude(delta, maxMoveDistance) / maxMoveDistance;
                                    uiMoveDirection = new Vector2(normalizedDelta.x, normalizedDelta.y);
                                }
                                else
                                {
                                    uiMoveDirection = Vector2.zero;
                                }
                            }
                            break;

                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            if (touch.fingerId == touchFingerId)
                            {
                                touchFingerId = -1;
                                uiMoveDirection = Vector2.zero;
                            }
                            break;
                    }
                }
            }
            else
            {
                uiMoveDirection = Vector2.zero;
                touchFingerId = -1;
            }
        }

        public void MoveInput(Vector2 newMoveDirection) => move = newMoveDirection;
        public void LookInput(Vector2 newLookDirection) => look = newLookDirection;
        public void JumpInput(bool newJumpState) => jump = newJumpState;
        public void SprintInput(bool newSprintState) => sprint = newSprintState;

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        // Optional button methods for touch UI
        public void MoveUp() => StartMoving(Vector2.up);
        public void MoveDown() => StartMoving(Vector2.down);
        public void MoveLeft() => StartMoving(Vector2.left);
        public void MoveRight() => StartMoving(Vector2.right);
        public void StopMoving() => StartMoving(Vector2.zero);

        private void StartMoving(Vector2 direction)
        {
            uiMoveDirection = direction;
        }
    }
}