using MLAgents;
using UnityEngine;
using System.Linq;

public class agentstage5 : Agent
{
    public Transform Target;
    private Rigidbody rb_bouncer;
    private float speed = 3f;

    public override void InitializeAgent()
    {
        rb_bouncer = GetComponent<Rigidbody>();
    }

    const int k_NoAction = 0;  // do nothing!
    const int k_Up = 1;
    const int k_Down = 2;
    const int k_Left = 3;
    const int k_Right = 4;

    public override void CollectObservations()
    {
    }

    public float JumpSpeed = 5.0f;
    private void Jump()
    {
        rb_bouncer.velocity += Vector3.up * JumpSpeed;
    }

    public override void AgentAction(float[] vectorAction)
    {

        AddReward(-0.01f);
        var action = Mathf.FloorToInt(vectorAction[0]);
        var curspeed = speed * Time.deltaTime;
        var targetPos = transform.position;
        switch (action)
        {
            case k_NoAction:
                // do nothing
                break;
            case k_Right:
                targetPos = transform.position + new Vector3(1f, 0, 0f) * curspeed;
                break;
            case k_Left:
                targetPos = transform.position + new Vector3(-1f, 0, 0f) * curspeed;
                break;
            case k_Up:
                targetPos = transform.position + new Vector3(0f, 0, 1f) * curspeed;
                break;
            case k_Down:
                targetPos = transform.position + new Vector3(0f, 0, -1f) * curspeed;
                break;
            default:
                throw new System.ArgumentException("Invalid action value");
        }

        transform.position = targetPos;

        var hit = Physics.OverlapBox(
            targetPos, new Vector3(0.3f, 0.3f, 0.3f), transform.rotation);

        if(hit.Where(col => col.gameObject.CompareTag("goal")).ToArray().Length == 1)
        {
            Done();
            SetReward(1f);
        }

        if (Mathf.Abs(transform.localPosition.x) > 5f || Mathf.Abs(transform.localPosition.z) > 5f)
        {
            Done();
            SetReward(-1.0f);
        }
    }

    public override void AgentReset()
    {
        transform.localPosition = new Vector3(Random.Range(-1, -5), 0.5f, Random.Range(-1, -5));
        transform.localRotation = Quaternion.identity;

        Target.localPosition = new Vector3(Random.Range(-1, -5), 0.5f, Random.Range(-1, -5));
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
