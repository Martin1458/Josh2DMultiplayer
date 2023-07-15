using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;
using UnityEditor;

public class agentTwoController : Agent
{
    public GameObject bulletTwoPrefab;
    private GameObject bulletOne;
    private GameObject bulletTwo;
    private GameObject agentOne;
    private int runSpeed = 10;
    private Vector3 rotationSpeed = new Vector3(0, 0, 120);
    // This bool says if agent is able to shoot
    private bool ableToShoot = true;
    private GameObject floorObj;
    private SpriteRenderer floorRenderer;
    private Color loseColor = new Color(8f, 115f, 255f);
    private Color winColor = new Color(255f, 70f, 33f);
    private Color baseColor = new Color(0f, 0f, 0f);


    void Start()
    {
        // Finding the correct second Agent
        Transform parent = transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject sibling = parent.GetChild(i).gameObject;
            if (sibling.tag == "Agent1")
            {
                agentOne = sibling;
            }
            else if (sibling.tag == "Floor")
            {
                floorObj = sibling;
            }
        }

        if (floorObj != null)
        {
            floorRenderer = floorObj.GetComponent<SpriteRenderer>();
            Debug.Log(floorRenderer);
        }
        else
        {
            Debug.Log("The floorObj was not found");
        }

    }
    public override void OnEpisodeBegin()
    {
        ableToShoot = true;
        floorRenderer.color = new Color(45f, 0f, 0f);
        // jdi na nahodne misto prosim
        transform.localPosition = new Vector2(UnityEngine.Random.Range(-7f, 7f), UnityEngine.Random.Range(-7f, 7f));
    }
    void Update()
    {

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector2)transform.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation((Vector2)agentOne.transform.localPosition);
        sensor.AddObservation(agentOne.transform.localRotation);

        // Check if one or more bullets exist
        Transform parent = transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject sibling = parent.GetChild(i).gameObject;
            if (sibling.tag == "BulletOne")
            {
                bulletOne = sibling;
            }
            else if (sibling.tag == "BulletTwo")
            {
                bulletTwo = sibling;
            }
        }


        if (bulletTwo != null)
        {
            sensor.AddObservation((Vector2)bulletTwo.transform.localPosition);
            sensor.AddObservation(bulletTwo.transform.localRotation);
        }
        else
        {
            sensor.AddObservation((Vector2)new Vector2(0, 0));
            sensor.AddObservation(0);
        }

        if (bulletOne != null)
        {
            sensor.AddObservation((Vector2)bulletOne.transform.localPosition);
            sensor.AddObservation(bulletOne.transform.localRotation);
        }
        else
        {
            sensor.AddObservation((Vector2)new Vector2(0, 0));
            sensor.AddObservation(0);
        }
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
        if (fire == 1 && ableToShoot)
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
        if (Input.GetKey(KeyCode.J))
        {
            DiscreteActions[2] -= 1;
        }

        // 0 = nothing; 1 = fire
        if (Input.GetKey(KeyCode.Space))
        {
            DiscreteActions[3] = 1;
        }
        else
        {
            DiscreteActions[3] = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            floorRenderer.color = loseColor;
            EndEpisode();
        }
        if (other.gameObject.tag == "Agent1")
        {
            SetReward(0f);
            EndEpisode();
        }
    }
    private void Fire()
    {
        Instantiate(bulletTwoPrefab, transform.localPosition, transform.rotation, transform.parent);
        StartCoroutine(Reload());
    }
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(4f);
        ableToShoot = true;
    }
    public void OpponentKilled()
    {
        SetReward(10f);
        floorRenderer.color = winColor;
        EndEpisode();
    }
    public void Missed()
    {
        AddReward(-.5f);
    }
}
