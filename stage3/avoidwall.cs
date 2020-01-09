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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Agent")
        {
            target.AddReward(-0.3f);
            Debug.Log("Wall Hit");
        }
    }
}
