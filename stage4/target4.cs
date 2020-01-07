using UnityEngine;
using MLAgents;

public class target4 : MonoBehaviour
{
    public Transform agent;
    private Agent target;

    private void Start()
    {
        target = agent.GetComponent<agentstage4>();
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
