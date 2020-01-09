using UnityEngine;
using MLAgents;

public class avoidwall : MonoBehaviour
{
    public Transform agent;
    private Agent target;

    private void Start()
    {
        target = agent.GetComponent<agentstage3a>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Agent")
        {
            target.AddReward(-0.3f);

        }
    }
}
