using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour {

    public List<Player> players;
    
    public List<int> assignedControllers;

    private void Awake()
    {
        assignedControllers = new List<int>();
    }

    private void Update()
    {
        for (int i = 1; i <= 4; i++)
        {
            // If not assigned and pressing Jump
            if (!assignedControllers.Contains(i)
                && Input.GetButtonDown("J" + i + " Jump"))
            {
                foreach (Player player in players)
                {
                    if (!player.ControllerAssigned)
                    {
                        // If assign Controller
                        assignedControllers.Add(i);
                        player.AssignVirtualInput(new ControllerInput(i));
                        Debug.Log("Controller " + i + " assigned to " + player.name);

                        CheckConnected();
                        break;
                    }
                }
            }
        }
    }

    private void CheckConnected()
    {
        if (assignedControllers.Count >= players.Count)
        {
            enabled = false;
        }
    }
}
