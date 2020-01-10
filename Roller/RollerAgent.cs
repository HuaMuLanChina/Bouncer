using MLAgents;
using UnityEngine;
using System.Collections.Generic;

public class RollerAgent : Agent
{
    Rigidbody rBody;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;

    public override void AgentReset()
    {
        if(this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        }

        Target.localPosition = new Vector3(Random.value * 8 -4, 0.5f, Random.value * 8 -4);
    }

    public override void CollectObservations()
    {
        AddVectorObs(Target.localPosition);
        AddVectorObs(transform.localPosition);

        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);
    }


    public float speed = 10;
    public override void AgentAction(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * speed);

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        if(distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        if(this.transform.localPosition.y < 0)
        {
            Done();
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[2];

        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");

        return action;
    }
}
