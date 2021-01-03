using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{

    public GameController manager;
    public float moveForce;
    // space needed b.w. rope links:
    public float linkSpace;
    // distance b.w. player and rope to which the rope is then deleted:
    public float ropeDeleteDistance;
    // list of currently active rope links:
    List<GameObject> currRope = new List<GameObject>();

    Rigidbody2D body;
    private Camera mainCamera;
    // detects whether 
    GameObject ropeStart = null;


      // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        body = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, transform.position.y - 5, mainCamera.transform.position.z);
        if (Input.GetKey(KeyCode.D))
        {
            body.AddForce(Vector3.right * moveForce);
        }
        if (Input.GetKey(KeyCode.A))
        {
            body.AddForce(Vector3.left * moveForce);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 0.15f;
            manager.sloMoStart();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Time.timeScale = 1f;
            manager.sloMoEnd();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            float distance = transform.position.z - mainCamera.transform.position.z;
            Vector3 pos = ray.GetPoint(distance);
            createRopeLink(pos);
        }
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("TomerScene");
        }

    }

    void createRopeLink(Vector3 pos)
    {
        GameObject newLink = manager.getLink();
        newLink.transform.position = pos;
        newLink.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        newLink.GetComponent<Collider2D>().enabled = false;
        if (ropeStart is null)
        {
            ropeStart = newLink;
        }
        else
        {
            generateRope(ropeStart, newLink);
            ropeStart = null;
        }
    }

    void generateRope(GameObject p1, GameObject p2)
    {
        currRope.Add(p1);
        Vector3 pos1 = p1.transform.position;
        Vector3 pos2 = p2.transform.position;
        Vector3 linkGap = (pos2 - pos1).normalized;
        linkGap *= linkSpace;
        Vector3 newPos = pos1 + linkGap;
        GameObject newLink = p1;
        GameObject prevLink = p1;
        int i = 1;
        while ((pos1 - pos2).magnitude >= (pos1 - newPos).magnitude)
        {
            currRope.Add(manager.getLink());
            currRope[i].transform.position = newPos;
            currRope[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            currRope[i - 1].GetComponent<SpringJoint2D>().connectedBody = currRope[i].GetComponent<Rigidbody2D>();
            newPos += linkGap;
            i++;
        }
        currRope[i - 1].GetComponent<SpringJoint2D>().connectedBody = p2.GetComponent<Rigidbody2D>();
        currRope.Add(p2);
    }

    IEnumerator waitForRopeRemove(Vector3 ropePos)
    {
        float playerRopeDistance = (transform.position - ropePos).magnitude;
        float duration = 0;
        while (playerRopeDistance < ropeDeleteDistance && duration < 3)
        {
            yield return new WaitForSeconds(0.5f);
            duration += 0.5f;
            playerRopeDistance = (transform.position - ropePos).magnitude;
        }
        for (int i = 0; i < currRope.Count; i++)
        {
            manager.resetLink(currRope[i]);
        }
        currRope.Clear();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Rope")
        {
            if (currRope.Count > 0)
            {
                Vector3 boost = (currRope[0].transform.position - currRope[currRope.Count - 1].transform.position);
                boost = new Vector3(boost.x, boost.y, 0).normalized;
                boost = (boost.x >= 0) ? Quaternion.Euler(0, 0, -90) * boost : Quaternion.Euler(0, 0, 90) * boost;
                boost *= 200;
                body.AddForce(boost);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Rope")
        {
            StartCoroutine(waitForRopeRemove(collision.collider.transform.position));
            //if (currRope.Count > 0)
            //{
            //    Vector3 boost = (currRope[0].transform.position - currRope[currRope.Count - 1].transform.position).normalized;
            //    Debug.Log(boost);
            //    boost = (boost.x >= 0) ? Quaternion.Euler(0, 0, -90) * boost : Quaternion.Euler(0, 0, 90) * boost;
            //    Debug.Log(boost);
            //    boost *= 300;
            //    body.AddForce(boost);
            //}
            
        }
    }

}
