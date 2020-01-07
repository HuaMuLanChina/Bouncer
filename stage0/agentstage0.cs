using MLAgents;
using UnityEngine;

public class agentstage0 : Agent
{
    public Transform Target;
    private Rigidbody rb_bouncer;


    public override void InitializeAgent()
    {
        rb_bouncer = GetComponent<Rigidbody>();
    }


    public override void CollectObservations()
    {
        AddVectorObs((transform.localPosition - Target.localPosition) / 5);
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
        Vector3 dir = new Vector3(vectorAction[0], 0, vectorAction[1]);

        transform.localPosition += dir * speed * Time.deltaTime;

        if (IsOnGround() && vectorAction[2] > 0.0f)
        {
            Jump();
        }

        if (transform.localPosition.y < 0f)
        {
            Done();
        }
    }

    public override void AgentReset()
    {
        transform.localPosition = Vector3.up * 0.5f;
        transform.localRotation = Quaternion.identity;

        Target.localPosition = new Vector3(Random.Range(-5.0f, 5.0f), 1.83f, Random.Range(-5.0f, 5.0f));
        rb_bouncer.velocity = Vector3.zero;
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
