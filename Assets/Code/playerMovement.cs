using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public GameController manager;
    public float moveForce;
    public float linkSpace;
    Rigidbody2D body;
    private Camera mainCamera;
    GameObject ropeStart = null;

      // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 2f;
        mainCamera = Camera.main;
        moveForce = 2f;
        body = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, transform.position.y - 5, mainCamera.transform.position.z);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            body.AddForce(Vector3.right * moveForce);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.AddForce(Vector3.left * moveForce);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 0.3f;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Time.timeScale = 1f;
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            float distance = transform.position.z - mainCamera.transform.position.z;
            Vector3 pos = ray.GetPoint(distance);
            createRopeLink(pos);
        }
    }

    void createRopeLink(Vector3 pos)
    {
        GameObject newLink = manager.getLink();
        newLink.transform.position = pos;
        newLink.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
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
        Vector3 pos1 = p1.transform.position;
        Vector3 pos2 = p2.transform.position;
        Vector3 linkGap = (pos2 - pos1).normalized;
        linkGap *= linkSpace;
        Vector3 newPos = pos1 + linkGap;
        GameObject newLink = p1;
        GameObject prevLink = p1;
        while ((pos1 - pos2).magnitude >= (pos1 - newPos).magnitude)
        {
            newLink = manager.getLink();
            newLink.transform.position = newPos;
            newLink.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            prevLink.GetComponent<SpringJoint2D>().connectedBody = newLink.GetComponent<Rigidbody2D>();
            newPos += linkGap;
            prevLink = newLink;
        }
        newLink.GetComponent<SpringJoint2D>().connectedBody = p2.GetComponent<Rigidbody2D>();
    }

}
