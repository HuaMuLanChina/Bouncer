using UnityEngine;
using MLAgents;

public class target0 : MonoBehaviour
{
    public Transform agent;
    private Agent target;

    private void Start()
    {
        target = agent.GetComponent<agentstage0>();
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
