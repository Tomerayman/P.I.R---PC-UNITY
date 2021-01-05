using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    Camera mainCamera;
    GameObject player;
    public Image sloMoSign;
    public GameObject linkPrefab;
    public GameObject obstaclePrefab;
    public GameObject boundaries;
    private GameObject[] linkPool;
    private GameObject[] obstaclesPool;
    private int currLinkIdx;
    private float frameHeight;
    private float frameWidth;
    private float parallaxObstaclesFactor;
    private float minObstaclesGap;
    private float maxObstaclesGap;
    private float speed;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        sloMoSign.enabled = false;
        frameHeight = 32f;
        frameWidth = 18f;
        parallaxObstaclesFactor = 0.9f;
        minObstaclesGap = 0.5f;
        minObstaclesGap = 2.5f;
        speed = 0.05f;
        // Initializing the chains.
        linkPool = new GameObject[50];
        for (int i = 0; i < 50; i++)
        {
            linkPool[i] = Instantiate(linkPrefab);
            //linkPool[i].transform.parent = this.transform;
            linkPool[i].SetActive(false);
        }
        currLinkIdx = 0;
        // Inintializing the obstacles.
        obstaclesPool = new GameObject[5];
        for (int i = 0; i < 5; i++)
        {
            obstaclesPool[i] = Instantiate(obstaclePrefab);
            obstaclesPool[i].transform.position = new Vector3(-20, 0, -1);
            obstaclesPool[i].SetActive(true);
        }
    }

    public GameObject getLink()
    {
        GameObject link = linkPool[currLinkIdx];
        currLinkIdx++;
        if (currLinkIdx > 49)
        {
            currLinkIdx = 0;
        }
        link.SetActive(true);
        
        return link;
    }

    public void resetLink(GameObject link)
    {
        link.GetComponent<SpringJoint2D>().connectedBody = null;
        link.GetComponent<Collider2D>().enabled = true;
        link.SetActive(false);
    }

    public void sloMoStart()
    {
        sloMoSign.enabled = true;
    }
    public void sloMoEnd()
    {
        sloMoSign.enabled = false;
    }

    private void SetObstacles()
    {
        for (int i = 0; i < obstaclesPool.Length; ++i)
        {
            GameObject obstacle = obstaclesPool[i];
            float camPosY = mainCamera.transform.position.y;
            if (obstacle.transform.position.y - camPosY > frameHeight)
            {
                float curPosX = -(i - 1) * frameWidth * 0.375f;
                float curPosY = camPosY - frameHeight * 0.75f - i * 10f;
                float curPosZ = obstacle.transform.position.z;
                obstacle.transform.position = new Vector3(curPosX, curPosY, curPosZ);
            }
        }
    }

    private void setBoundaries()
    {
        boundaries.transform.position = new Vector3(boundaries.transform.position.x, mainCamera.transform.position.y, boundaries.transform.position.z);
    }
    
    // Update is called once per frame
    void Update()
    {
        SetObstacles();
        setBoundaries();
    }
}
