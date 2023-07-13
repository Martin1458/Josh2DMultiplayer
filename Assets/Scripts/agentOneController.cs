using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;

public class agentOneController : Agent
{

    public GameObject bulletOne;
    public GameObject agentTwo;
    public GameObject[] myBullets;
    public int runSpeed;
    public Vector3 rotationSpeed = new Vector3(0, 50, 0);
    // This bool says if agent is able to shoot
    private bool ableToShoot = true;

    void Start()
    {
        // Finding the correct second Agent
        Transform parent = transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject sibling = parent.GetChild(i).gameObject;
            if (sibling.tag == "Agent2")
            {
                Debug.Log("Donee");
                agentTwo = sibling;
            }
        }
        
    }
    public override void OnEpisodeBegin()
    {
        ableToShoot = true;
    }
    void Update()
    {
        
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector2)transform.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation((Vector2)agentTwo.transform.localPosition);
        sensor.AddObservation(agentTwo.transform.localRotation);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        // -1 = left; 0 = stay; 1 = right
        int moveX = actions.DiscreteActions[0];
        // -1 = down; 0 = stay; 1 = up
        int moveY = actions.DiscreteActions[1];
        // -1 = left; 0 = stay; 1 = right
        int rotate = actions.DiscreteActions[2];
        // 0 = nothing; 1 = fire
        int fire = actions.DiscreteActions[3];

        // Do the moving
        //transform.localPosition += Vector3.right * moveX * Time.deltaTime * runSpeed;
        //transform.localPosition += Vector3.up * moveY * Time.deltaTime * runSpeed;

        transform.Translate(Vector3.right * moveX * Time.deltaTime * runSpeed);
        transform.Translate(Vector3.up * moveY * Time.deltaTime * runSpeed);

        transform.Rotate(rotationSpeed * Time.deltaTime * rotate);

        // Do the shooting
        if(fire == 1 && ableToShoot) 
        { 
            ableToShoot = false;
            Fire(); 
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> DiscreteActions = actionsOut.DiscreteActions;

        // -1 = left; 0 = stay; 1 = right
        DiscreteActions[0] = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        // -1 = down; 0 = stay; 1 = up
        DiscreteActions[1] = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));

        // -1 = left; 0 = stay; 1 = right
        DiscreteActions[2] = 0;
        if (Input.GetKey(KeyCode.F))
        {
            DiscreteActions[2] += 1;
        } 
        if(Input.GetKey(KeyCode.J))
        {
            DiscreteActions[2] -= 1;
        }

        // 0 = nothing; 1 = fire
        if (Input.GetKey(KeyCode.Space)) 
        {
            DiscreteActions[3] = 1;  
        } else {
            DiscreteActions[3] = 0;
        }
 
    }
    private void Fire()
    {
        GameObject bulletInstance = Instantiate(bulletOne, transform.localPosition, transform.rotation, transform.parent);
        myBullets.Append(bulletInstance);
        StartCoroutine(Reload());
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(4f);
        ableToShoot = true;
    }
    public void OpponentKilled()
    {
        AddReward(10f);
    }
    public void Missed()
    {
        AddReward(-.5f);
    }
}
