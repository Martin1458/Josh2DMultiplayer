using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletTwoController : MonoBehaviour
{
    //public int myAgentNum;
    private GameObject myAgentObj;
    void Start()
    {
        // Finding the correct Agent obj
        Transform parent = transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject sibling = parent.GetChild(i).gameObject;
            if (sibling.tag == "Agent2")
            {
                myAgentObj = sibling;
            }
        }
    }

    void FixedUpdate()
    {
        if (this != null)
        {
            transform.Translate(0, .3f, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Agent1")
        {
            Destroy(other.gameObject);
            if (myAgentObj != null)
            {
                myAgentObj.GetComponent<agentOneController>().OpponentKilled();
            }
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Wall")
        {
            if (myAgentObj != null)
            {
                agentOneController skriptt = myAgentObj.GetComponent<agentOneController>();
                skriptt.Missed();
            }
            Destroy(this.gameObject);
        }
    }
}
