using MLAgents;
using UnityEngine;

public class bouncerAgent : Agent
{
    public Transform Target;
    public Transform[] blocks;
    private Rigidbody rb_bouncer;


    public override void InitializeAgent()
    {
        rb_bouncer = GetComponent<Rigidbody>();
    }


    public override void CollectObservations()
    {
        AddVectorObs((transform.localPosition) / 5);
        AddVectorObs((Target.localPosition) / 5);
        AddVectorObs(IsOnGround() ? 1 : 0);
    }

    private bool IsOnGround()
    {
        bool HeightOnGround = Mathf.Abs(transform.localPosition.y - 0.5f) < 0.00001f;
        bool XInsideGround = Mathf.Abs(transform.localPosition.x) < 5f;
        bool YInsideGround = Mathf.Abs(transform.localPosition.z) < 5f;
        return HeightOnGround && XInsideGround && YInsideGround;
    }


    public float JumpSpeed = 5.0f;
    private void Jump()
    {
        rb_bouncer.velocity += Vector3.up * JumpSpeed;
    }


    public float speed = 3.0f;
    public override void AgentAction(float[] vectorAction)
    {
        SetReward(-0.0005f);
        Vector3 dir = new Vector3(vectorAction[0], 0 , vectorAction[1]);

        transform.localPosition += dir * speed * Time.deltaTime;

        if(IsOnGround() && vectorAction[2] > 0.0f)
        {
            Jump();
            SetReward(-0.1f);
        }

        if(transform.localPosition.y < 0f)
        {
            Done();
        }
    }

    private void resetblocks()
    {
        if (blocks != null)
        {
            foreach (Transform t in blocks)
            {
                t.localPosition = new Vector3(Random.Range(-5.0f, 5.0f), 0.5f, Random.Range(-5.0f, 5.0f));
            }
        }
    }

    public override void AgentReset()
    {
        transform.localPosition = Vector3.up * 0.5f;
        transform.localRotation = Quaternion.identity;

        Target.localPosition = new Vector3( Random.Range(-5.0f, 5.0f), 1.83f, Random.Range(-5.0f, 5.0f));
        rb_bouncer.velocity = Vector3.zero;

        resetblocks();
    }


    public override float[] Heuristic()
    {
        var action = new float[3];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        action[2] = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;

        return action;
    }
}
