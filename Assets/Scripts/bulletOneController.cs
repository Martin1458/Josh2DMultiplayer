using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletOneController : MonoBehaviour
{
    public int myAgentNum;
    public GameObject myAgentObj;
    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
