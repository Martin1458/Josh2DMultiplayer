using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bulletOneController : MonoBehaviour
{
    //public int myAgentNum;
    public GameObject myAgentObj;
    void Start()
    {
        // Finding the correct Agent obj
        Transform parent = transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject sibling = parent.GetChild(i).gameObject;
            if (sibling.tag == "Agent1")
            {
                Debug.Log("Doneerrrr");
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
        if (other.gameObject.tag == "Agent2")
        {
            Destroy(other.gameObject);
            if (myAgentObj != null)
            {    
                myAgentObj.GetComponent<agentOneController>().OpponentKilled();
            }
            Destroy(this.gameObject);
        }
        if(other.gameObject.tag == "Wall")
        {
            if (myAgentObj != null)
            {
                myAgentObj.GetComponent<agentOneController>().Missed();
            }
            Destroy(this.gameObject);
        }
    }
}
