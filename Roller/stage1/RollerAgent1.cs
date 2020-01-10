using System;
using MLAgents;
using UnityEngine;
using System.Collections.Generic;

public class RollerAgent1 : Agent
{
    Rigidbody rBody;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public Transform Target;

    const int k_NoAction = 0;  // do nothing!
    const int k_Up = 1;
    const int k_Down = 2;
    const int k_Left = 3;
    const int k_Right = 4;

    public override void AgentReset()
    {
        this.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        Target.localPosition = new Vector3(UnityEngine.Random.value * 8 -4, 0.5f, UnityEngine.Random.value * 8 -4);
    }

    public override void CollectObservations()
    {
    }


    public float speed = 10;
    public override void AgentAction(float[] vectorAction)
    {
        AddReward(-0.01f);
        var action = Mathf.FloorToInt(vectorAction[0]);

        var targetPos = transform.position;
        switch (action)
        {
            case k_NoAction:
                // do nothing
                break;
            case k_Right:
                targetPos = transform.position + new Vector3(1f, 0, 0f);
                break;
            case k_Left:
                targetPos = transform.position + new Vector3(-1f, 0, 0f);
                break;
            case k_Up:
                targetPos = transform.position + new Vector3(0f, 0, 1f);
                break;
            case k_Down:
                targetPos = transform.position + new Vector3(0f, 0, -1f);
                break;
            default:
                throw new ArgumentException("Invalid action value");
        }

        transform.position = targetPos;

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        if(distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        if(Mathf.Abs(this.transform.localPosition.x) > 5 || Mathf.Abs(this.transform.localPosition.z) > 5)
        {
            Done();
        }
    }

    public override float[] Heuristic()
    {
        if (Input.GetKey(KeyCode.D))
        {
            return new float[] { k_Right };
        }
        if (Input.GetKey(KeyCode.W))
        {
            return new float[] { k_Up };
        }
        if (Input.GetKey(KeyCode.A))
        {
            return new float[] { k_Left };
        }
        if (Input.GetKey(KeyCode.S))
        {
            return new float[] { k_Down };
        }
        return new float[] { k_NoAction };
    }
}
