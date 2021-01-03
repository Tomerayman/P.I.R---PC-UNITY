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
    private GameObject[] linkPool;
    private int currLinkIdx;

    // Start is called before the first frame update
    void Start()
    {
        sloMoSign.enabled = false;
        linkPool = new GameObject[50];
        for (int i = 0; i < 50; i++)
        {
            linkPool[i] = Instantiate(linkPrefab);
            //linkPool[i].transform.parent = this.transform;
            linkPool[i].SetActive(false);
        }
        currLinkIdx = 0;
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

    // Update is called once per frame
    void Update()
    {
    }
}
