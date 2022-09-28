using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : FSMState
{
    private Vector3 destPos;
    private float curRotSpeed = 1.0f;
    private float curSpeed = 10.0f;

    public ChaseState(Transform[] wp)
    {
        stateID = FSMStateID.Chasing;
    }

    public override void CheckTransitionRules(Transform player, Transform npc)
    {
        //Set the target position as the player position
        destPos = player.position;

        //Check the distance with player tank
        //When the distance is near, transition to attack state
        float dist = Vector3.Distance(npc.position, destPos);
        if (dist <= 20.0f)
        {
            Debug.Log("Switch to Attack state");
            npc.GetComponent<EnemyController>().SetTransition(Transition.ReachPlayer);
        }
        //Go back to patrol is it become too far
        else if (dist >= 10.0f)
        {
            Debug.Log("Switch to Patrol state");
            npc.GetComponent<EnemyController>().SetTransition(Transition.LostPlayer);
        }
    }

    public override void RunState(Transform player, Transform npc)
    {
        //Rotate to the target point
        destPos = player.position;

        Quaternion targetRotation = Quaternion.LookRotation(destPos - npc.position);
        npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

        //Go Forward
        npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }
}
