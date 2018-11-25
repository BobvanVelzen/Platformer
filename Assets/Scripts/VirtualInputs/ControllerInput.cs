using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInput : VirtualInput {

    public ControllerInput(int number)
    {
        SetControllerNumber(number);
    }

    public void SetControllerNumber(int number)
    {
        AssignInput(Axis.HORIZONTAL, "J" + number + " Horizontal");
        AssignInput(Axis.VERTICAL, "J" + number + " Vertical");
        AssignInput(Button.JUMP, "J" + number + " Jump");
        AssignInput(Button.ACTION, "J" + number + " Action");
    }
}
