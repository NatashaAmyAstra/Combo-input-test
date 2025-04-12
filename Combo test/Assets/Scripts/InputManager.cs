using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
    public enum ButtonType {
        Primary,
        Alt,
        Special,
        PrimaryAndAlt,
        PrimaryAndSpecial,
        AltAndSpecial
    }

    private struct ButtonData {
        public ButtonType type;
        public float stage;
        public float time;

        public ButtonData(ButtonType type, float stage, float time) {
            this.type = type;
            this.stage = stage;
            this.time = time;
        }
    }

    public static ButtonType MostRecentInput;

    private static List<ButtonData> _buttonQueue = new List<ButtonData>();
    private static CustomInput _userInput = new CustomInput();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Setup() {
        _userInput.Enable();

        _userInput.Player.AttackPrimary.performed += AttackPrimary;
        _userInput.Player.AttackAlt.performed += AttackAlt;
        _userInput.Player.Special.performed += Special;
    }

    private static void AttackPrimary(InputAction.CallbackContext value) {
        UpdateQueue(ButtonType.Primary, value.ReadValue<float>());
    }

    private static void AttackAlt(InputAction.CallbackContext value) {
        UpdateQueue(ButtonType.Alt, value.ReadValue<float>());
    }

    private static void Special(InputAction.CallbackContext value) {
        if(value.ReadValue<float>() == 0)
            return;
        for(int i = 0; i < _buttonQueue.Count; i++)
        {
            ButtonData data = _buttonQueue[i];
            Debug.Log(data.stage == 1f ? 
                $"{data.type.ToString()} pressed at {data.time}":
                $"{data.type.ToString()} released at {data.time}");

        }
    }



    private static void UpdateQueue(ButtonType type, float stage) {
        _buttonQueue.Add(new ButtonData(type, stage, Time.time));
    }
}
