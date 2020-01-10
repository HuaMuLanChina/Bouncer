using MLAgents;
using UnityEngine;

public class agentstage4 : Agent
{
    public Transform Target;
    public blocks Wall;
    private Rigidbody rb_bouncer;

    public override void InitializeAgent()
    {
        rb_bouncer = GetComponent<Rigidbody>();
    }

    public override void CollectObservations()
    {
        AddVectorObs((transform.localPosition) / 5f);
        AddVectorObs((Target.localPosition) / 5f);
        Vector3 hpos0 = Wall.wallbricks[Wall.hole0].position;
        Vector3 hpos1 = Wall.wallbricks[Wall.hole1].position;
        hpos0 = transform.parent.InverseTransformPoint(hpos0);
        hpos1 = transform.parent.InverseTransformPoint(hpos1);
        AddVectorObs(hpos0 / 5f);
        AddVectorObs(hpos1 / 5f);
        AddVectorObs(IsOnGround() ? 1 : -1);
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
        
        Vector3 dir = new Vector3(vectorAction[0], 0, vectorAction[1]);

        transform.localPosition += dir * speed * Time.deltaTime;

        if (IsOnGround() && vectorAction[2] > 0.0f)
        {
            Jump();
            //AddReward(-0.003f);
        }

        if (transform.localPosition.y < 0f)
        {
            Done();
            SetReward(-1.0f);
        }
    }

    bool fisr_set = true;
    public override void AgentReset()
    {
        if(fisr_set)
        {
            Wall.setwall();
            fisr_set = false;
        }

        transform.localPosition = Wall.transform.right * Random.Range(-1, -6) + Wall.transform.forward * Random.Range(-1, -6) + Vector3.up * 0.5f;
        transform.localRotation = Quaternion.identity;

        Target.localPosition = Wall.transform.right * Random.Range(1, 6) + Wall.transform.forward * Random.Range(1, 6) + Vector3.up * 1.83f;

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
