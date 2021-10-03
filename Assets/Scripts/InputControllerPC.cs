using UnityEngine;

internal class InputControllerPC : MonoBehaviour, IInputController
{
    public bool GetInput()
    {
        return Input.anyKeyDown;
    }
}