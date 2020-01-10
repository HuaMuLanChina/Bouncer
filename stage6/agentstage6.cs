using MLAgents;
using UnityEngine;

public class agentstage6 : Agent
{
    public Transform Target;
    //public blocks Wall;
    stage6Academy m_Academy;
    private Rigidbody rb_bouncer;

    public override void InitializeAgent()
    {
        rb_bouncer = GetComponent<Rigidbody>();
        m_Academy = FindObjectOfType<stage6Academy>();
    }

    public override void CollectObservations()
    {
        AddVectorObs(rb_bouncer.velocity.x);
        AddVectorObs(rb_bouncer.velocity.z);
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

    public float speed = 1.0f;
    public override void AgentAction(float[] vectorAction)
    {
        Vector3 dir = new Vector3(vectorAction[0], 0, vectorAction[1]);

        rb_bouncer.AddForce(dir.normalized * speed);
        
        if (IsOnGround() && vectorAction[2] > 0.0f)
        {
            Jump();
            //AddReward(-0.003f);
        }

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        if (Mathf.Abs(transform.localPosition.x) > 5f || Mathf.Abs(transform.localPosition.z) > 5)
        {
            Done();
            SetReward(-1.0f);
        }
    }

    //void dif_lession(int dif)
    //{
    //    switch(dif)
    //    {
    //        default:
    //        case 1:
    //            Target.localPosition = Wall.transform.right * Random.Range(-1, -6) + Wall.transform.forward * Random.Range(-1, -6) + Vector3.up * 0.5f;
    //            break;
    //        case 2:
    //            Target.localPosition = Wall.transform.right * Random.Range(-1, -6) + Wall.transform.forward * Random.Range(-1, -6) + Vector3.up * 2.0f;
    //            break;
    //        case 3:
    //            {
    //                Target.position = (Wall.wallbricks[Wall.hole0].transform.position + Wall.wallbricks[Wall.hole1].position) / 2 + Vector3.up * 2.0f;
    //            }
    //            break;
    //        case 4:
    //            Target.localPosition = Wall.transform.right * Random.Range(1, 6) + Wall.transform.forward * Random.Range(1, 6) + Vector3.up * 2.0f;
    //            break;
    //    }
    //}

    public override void AgentReset()
    {
        //Wall.setwall();

        //transform.localPosition = Wall.transform.right * Random.Range(-1, -6) + Wall.transform.forward * Random.Range(-1, -6) + Vector3.up * 0.5f;
        //transform.localRotation = Quaternion.identity;

        //float dif = m_Academy.FloatProperties.GetPropertyWithDefault("diffculty", 4);
        //dif_lession(Mathf.RoundToInt(dif));

        transform.localPosition = default(Vector3) + Vector3.up * 0.5f;
        transform.localRotation = Quaternion.identity;
        Target.localPosition = Vector3.right * Random.Range(-4, 5) + Vector3.forward * Random.Range(-4, 5) + Vector3.up * 0.5f;
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
