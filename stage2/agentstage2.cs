using MLAgents;
using UnityEngine;

public class agentstage2 : Agent
{
    public Transform Target;
    public blocks Wall;
    private Rigidbody rb_bouncer;
    stage2Academy m_Academy;
    public float diffculty;

    public override void InitializeAgent()
    {
        rb_bouncer = GetComponent<Rigidbody>();
        m_Academy = FindObjectOfType<stage2Academy>();
    }
    Vector3 debugPoint;
    public override void CollectObservations()
    {
        AddVectorObs((transform.localPosition) / 5f);
        AddVectorObs((Target.localPosition) / 5f);

        AddVectorObs(rb_bouncer.velocity.x);
        AddVectorObs(rb_bouncer.velocity.z);

        Vector3 p0 = this.transform.parent.InverseTransformPoint(Wall.wallbricks[Wall.hole0].position);
        Vector3 p1 = this.transform.parent.InverseTransformPoint(Wall.wallbricks[Wall.hole1].position);
        AddVectorObs(p0 / 5f);
        AddVectorObs(p1 / 5f);
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

    void dif_reward()
    {
        AddReward(-0.0005f);
        switch(diffculty)
        {
            //过墙
            default:
            case 1:{
                    if (Vector3.Dot(transform.localPosition - Wall.transform.localPosition, Wall.transform.forward) > 0)
                    {
                        SetReward(1.0f);
                        Done();
                    }
                } break;
            //找到target
            case 2:{
                    if (Vector3.Dot(transform.localPosition - Wall.transform.localPosition, Wall.transform.forward) > 0)
                    {
                        AddReward(-0.0001f);
                    }
                } break;
            //找到空中的target
            case 3:{
                } break;
            //限制跳跃
            case 4:{
                    if(!IsOnGround())
                    {
                        AddReward(-0.1f);
                    }
                } break;
        }

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        if (transform.localPosition.y < 0f)
        {
            Done();
        }
    }


    public float speed = 3.0f;
    public override void AgentAction(float[] vectorAction)
    {
        Vector3 dir = new Vector3(vectorAction[0], 0, vectorAction[1]);

        rb_bouncer.AddForce(dir * speed);

        if (IsOnGround() && vectorAction[2] > 0.0f)
        {
            Jump();
        }

        dif_reward();
    }

    void dif_reset(int dif)
    {
        switch (dif)
        {
            default:
            case 1:
                Target.localPosition = Wall.transform.right * Random.Range(1, 6) + Wall.transform.forward * Random.Range(1, 6) + Vector3.up * 2.0f;
                break;
            case 2:
                Target.localPosition = Wall.transform.right * Random.Range(1, 6) + Wall.transform.forward * Random.Range(1, 6) + Vector3.up * 0.5f;
                break;
            case 3:
                Target.localPosition = Wall.transform.right * Random.Range(1, 6) + Wall.transform.forward * Random.Range(1, 6) + Vector3.up * 2.0f;
                break;
            case 4:
                Target.localPosition = Wall.transform.right * Random.Range(1, 6) + Wall.transform.forward * Random.Range(1, 6) + Vector3.up * 2.0f;
                break;
        }
    }

    public override void AgentReset()
    {
        Wall.setwall();

        transform.localPosition = Wall.transform.right * Random.Range(-1, -6) + Wall.transform.forward * Random.Range(-1, -6) + Vector3.up * 0.5f;
        transform.localRotation = Quaternion.identity;

        //diffculty = m_Academy.FloatProperties.GetPropertyWithDefault("diffculty", 4);
        diffculty = 1.0f;
        dif_reset(Mathf.RoundToInt(diffculty));

        rb_bouncer.velocity = Vector3.zero;
        rb_bouncer.angularVelocity = Vector3.zero;
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
