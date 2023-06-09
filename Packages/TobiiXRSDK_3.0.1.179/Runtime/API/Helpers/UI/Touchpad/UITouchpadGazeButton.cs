﻿// Copyright © 2018 – Property of Tobii AB (publ) - All Rights Reserved

using Tobii.G2OM;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tobii.XR
{
    /// <summary>
    /// A gaze aware button that is interacted with the touchpad button on the Vive controller.
    /// </summary>
    [RequireComponent(typeof(UIGazeButtonGraphics))]
    public class UITouchpadGazeButton : MonoBehaviour, IGazeFocusable
    {
        // Event called when the button is clicked.
        public UIButtonEvent OnButtonClicked;

        // The touchpad button on the Vive controller.
        private const ControllerButton TouchpadButton = ControllerButton.Touchpad;

        // The normalized (0 to 1) haptic strength.
        private const float HapticStrength = 0.1f;

        // The state the button is currently  in.
        private ButtonState _currentButtonState = ButtonState.Idle;

        // Private fields.
        private bool _hasFocus;
        private UIGazeButtonGraphics _uiGazeButtonGraphics;

        public Material skyboxMaterialStar;
        public Material skyboxMaterialStarNames;
        public Material skyboxMaterialStarBoxes;

        private void Start()
        {
            // Store the graphics class.
            _uiGazeButtonGraphics = GetComponent<UIGazeButtonGraphics>();

            // Initialize click event.
            if (OnButtonClicked == null)
            {
                OnButtonClicked = new UIButtonEvent();
            }
        }

        private void Update()
        {
            // When the button is being focused and the interaction button is pressed down, set the button to the PressedDown state.
            if (_currentButtonState == ButtonState.Focused &&
                ControllerManager.Instance.GetButtonPressDown(TouchpadButton))
            {
                UpdateState(ButtonState.PressedDown);
            }
            // When the button is pressed down and the interaction button is released, call the click method and update the state.
            else if (_currentButtonState == ButtonState.PressedDown &&
                     ControllerManager.Instance.GetButtonPressUp(TouchpadButton))
            {
                // Invoke click event.
                if (OnButtonClicked != null)
                {
                    OnButtonClicked.Invoke(gameObject);
                }

                ControllerManager.Instance.TriggerHapticPulse(HapticStrength);

                // Set the state depending on if it has focus or not.
                UpdateState(_hasFocus ? ButtonState.Focused : ButtonState.Idle);
            }
        }

        /// <summary>
        /// Updates the button state and starts an animation of the button.
        /// </summary>
        /// <param name="newState">The state the button should transition to.</param>
        private void UpdateState(ButtonState newState)
        {
            var oldState = _currentButtonState;
            _currentButtonState = newState;

            // Variables for when the button is pressed or clicked.
            var buttonPressed = newState == ButtonState.PressedDown;
            var buttonClicked = (oldState == ButtonState.PressedDown && newState == ButtonState.Focused);

            // If the button is being pressed down or clicked, animate the button click.
            if (buttonPressed || buttonClicked)
            {
                _uiGazeButtonGraphics.AnimateButtonPress(_currentButtonState);
            }
            // In all other cases, animate the visual feedback.
            else
            {
                _uiGazeButtonGraphics.AnimateButtonVisualFeedback(_currentButtonState);
            }
        }

        /// <summary>
        /// Method called by Tobii XR when the gaze focus changes by implementing <see cref="IGazeFocusable"/>.
        /// </summary>
        /// <param name="hasFocus"></param>
        public void GazeFocusChanged(bool hasFocus)
        {
            // Don't use this method if the component is disabled.
            if (!enabled) return;

            _hasFocus = hasFocus;

            UpdateState(hasFocus ? ButtonState.Focused : ButtonState.Idle);
        }

        public void Reset()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Scene resetted");
        }
        public void OnClickStarSkybox()
        {
            RenderSettings.skybox = skyboxMaterialStar;
        }

        public void OnClickStarNamesSkybox()
        {
            RenderSettings.skybox = skyboxMaterialStarNames;
        }

        public void OnClickStarBoxesSkybox()
        {
            RenderSettings.skybox = skyboxMaterialStarBoxes;
        }
    }
}
