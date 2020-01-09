using UnityEngine;
using MLAgents;

public class target3a : MonoBehaviour
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
            target.Done();
            target.SetReward(1.0f);
        }
    }
}
