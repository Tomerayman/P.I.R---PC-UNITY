using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Camera mainCamera;
    GameObject player;
    public GameObject linkPrefab;
    private GameObject[] linkPool;
    private int currShotIdx;

    // Start is called before the first frame update
    void Start()
    {
        linkPool = new GameObject[50];
        for (int i = 0; i < 50; i++)
        {
            linkPool[i] = Instantiate(linkPrefab);
            //linkPool[i].transform.parent = this.transform;
            linkPool[i].SetActive(false);
        }
        currShotIdx = 0;
    }

    public GameObject getLink()
    {
        GameObject link = linkPool[currShotIdx];
        currShotIdx = (currShotIdx == 49) ? 0 : currShotIdx + 1;
        link.SetActive(true);
        return link;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
