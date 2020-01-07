using MLAgents;
using UnityEngine;

public class agentstage1 : Agent
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
    //stage1
    //public override void AgentAction(float[] vectorAction)
    //{
    //    Vector3 dir = new Vector3(vectorAction[0], 0, vectorAction[1]);

    //    transform.localPosition += dir * speed * Time.deltaTime;

    //    if (IsOnGround() && vectorAction[2] > 0.0f)
    //    {
    //        Jump();
    //        SetReward(-0.1f);
    //    }

    //    if (transform.localPosition.y < 0f)
    //    {
    //        Done();
    //    }
    //}

    //stage1.1
    //public override void AgentAction(float[] vectorAction)
    //{
    //    SetReward(-0.0005f);
    //    Vector3 dir = new Vector3(vectorAction[0], 0, vectorAction[1]);

    //    transform.localPosition += dir * speed * Time.deltaTime;

    //    if (IsOnGround() && vectorAction[2] > 0.0f)
    //    {
    //        Jump();
    //        SetReward(-0.1f);
    //    }

    //    if (transform.localPosition.y < 0f)
    //    {
    //        Done();
    //    }
    //}

    //stage1.2
    //public override void AgentAction(float[] vectorAction)
    //{
    //    SetReward(-0.0005f);
    //    Vector3 dir = new Vector3(vectorAction[0], 0, vectorAction[1]);

    //    transform.localPosition += dir * speed * Time.deltaTime;

    //    if (IsOnGround() && vectorAction[2] > 0.0f)
    //    {
    //        Jump();
    //        SetReward(-0.3f);
    //    }

    //    if (transform.localPosition.y < 0f)
    //    {
    //        Done();
    //    }
    //}

    //stage1.3
    public override void AgentAction(float[] vectorAction)
    {
        SetReward(-0.0005f);
        Vector3 dir = new Vector3(vectorAction[0], 0, vectorAction[1]);

        transform.localPosition += dir * speed * Time.deltaTime;

        if (IsOnGround() && vectorAction[2] > 0.0f)
        {
            Jump();
            //SetReward(-0.3f);//1.3.1
            //SetReward(-0.003f);//1.3.2
            //SetReward(-0.03f);//1.3.3强调平移后再跳跃而不是跳着走
            SetReward(-0.3f);//1.3.4更加强调平移
        }

        if (transform.localPosition.y < 0f)
        {
            Done();
            //SetReward(-0.3f);//1.3.1
            SetReward(-1.0f);
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
