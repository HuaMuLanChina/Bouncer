using UnityEngine;
using MLAgents;

public class target2 : MonoBehaviour
{
    public Transform agent;
    private Agent target;

    private void Start()
    {
        target = agent.GetComponent<agentstage2>();
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
