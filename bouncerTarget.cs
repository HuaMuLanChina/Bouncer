using UnityEngine;
using MLAgents;

public class bouncerTarget : MonoBehaviour
{
    public Transform agent;
    private Agent bouncerAgent;

    private void Start()
    {
        bouncerAgent = agent.GetComponent<bouncerAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Agent")
        {
            bouncerAgent.Done();
            bouncerAgent.SetReward(1.0f);
        }
    }
}
