using System.Collections.Generic;
using UnityEngine;

public enum InAxis
{
    HORIZONTAL,
    VERTICAL
}

public enum InButton
{
    ACTION,
    JUMP
}

public class VirtualInput {

    private Dictionary<InAxis, string> inputAxis;
    private Dictionary<InButton, string> inputButtons;

    public VirtualInput()
    {
        inputButtons = new Dictionary<InButton, string>();
        inputAxis = new Dictionary<InAxis, string>();

        InitTestInput();
    }

    private void InitTestInput()
    {
        AssignAxis(InAxis.HORIZONTAL, "Horizontal");
        AssignAxis(InAxis.VERTICAL, "Vertical");
        AssignButton(InButton.JUMP, "Jump");
        AssignButton(InButton.ACTION, "Fire1");
    }

    public void AssignAxis(InAxis axis, string name)
    {
        // Removes input if assigned
        if (inputAxis.ContainsKey(axis))
        {
            inputAxis.Remove(axis);
        }
        inputAxis.Add(axis, name);
    }

    public void AssignButton(InButton button, string name)
    {
        // Removes input if assigned
        if (inputButtons.ContainsKey(button))
        {
            inputButtons.Remove(button);
        }
        inputButtons.Add(button, name);
    }

    public bool GetButtonDown(InButton button)
    {
        return Input.GetButtonDown(inputButtons[button]);
    }

    public bool GetButton(InButton button)
    {
        return Input.GetButton(inputButtons[button]);
    }

    public bool GetButtonUp(InButton button)
    {
        return Input.GetButtonUp(inputButtons[button]);
    }

    public float GetAxis(InAxis axis)
    {
        return Input.GetAxis(inputAxis[axis]);
    }

    public float GetAxisRaw(InAxis axis)
    {
        return Input.GetAxisRaw(inputAxis[axis]);
    }
}
