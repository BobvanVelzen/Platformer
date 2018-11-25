using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    HORIZONTAL,
    VERTICAL
}

public enum Button
{
    ACTION,
    JUMP
}

public class VirtualInput {

    private Dictionary<Axis, string> inputAxis;
    private Dictionary<Button, string> inputButtons;

    public VirtualInput()
    {
        inputButtons = new Dictionary<Button, string>();
        inputAxis = new Dictionary<Axis, string>();

        //InitTestInput();
    }

    private void InitTestInput()
    {
        AssignInput(Axis.HORIZONTAL, "Horizontal");
        AssignInput(Axis.VERTICAL, "Vertical");
        AssignInput(Button.JUMP, "Jump");
        AssignInput(Button.ACTION, "Fire1");
    }

    protected void AssignInput(Axis axis, string name)
    {
        // Removes input if assigned
        if (inputAxis.ContainsKey(axis))
        {
            inputAxis.Remove(axis);
        }
        inputAxis.Add(axis, name);
    }

    protected void AssignInput(Button button, string name)
    {
        // Removes input if assigned
        if (inputButtons.ContainsKey(button))
        {
            inputButtons.Remove(button);
        }
        inputButtons.Add(button, name);
    }

    public bool GetButtonDown(Button button)
    {
        if (!inputButtons.ContainsKey(button)) return false;
        return Input.GetButtonDown(inputButtons[button]);
    }

    public bool GetButton(Button button)
    {
        if (!inputButtons.ContainsKey(button)) return false;
        return Input.GetButton(inputButtons[button]);
    }

    public bool GetButtonUp(Button button)
    {
        if (!inputButtons.ContainsKey(button)) return false;
        return Input.GetButtonUp(inputButtons[button]);
    }

    public float GetAxis(Axis axis)
    {
        if (!inputAxis.ContainsKey(axis)) return 0;
        return Input.GetAxis(inputAxis[axis]);
    }

    public float GetAxisRaw(Axis axis)
    {
        if (!inputAxis.ContainsKey(axis)) return 0;
        return Input.GetAxisRaw(inputAxis[axis]);
    }
}
