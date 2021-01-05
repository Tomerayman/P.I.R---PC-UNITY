using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    private Transform cam;
    public GameObject[] backgroundElements;
    private float parallaxFactor = 0.75f;
    private float speed = 0.01f;
    private float frameHeight;

    private void Awake()
    {
        cam = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        frameHeight = backgroundElements[0].transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        PositionBackgrounds();
    }

    private void PositionBackgrounds()
    {
        for (int i = 0; i < backgroundElements.Length; ++i)
        {
            float curPosX = backgroundElements[i].transform.position.x;
            float curPosY = backgroundElements[i].transform.position.y;
            float curPosZ = backgroundElements[i].transform.position.z;
            float camBgDis = Mathf.Abs(cam.position.y - curPosY);
            if (camBgDis > frameHeight)
            {
                curPosY = backgroundElements[(i + 1) % backgroundElements.Length].transform.position.y - frameHeight;
                backgroundElements[i].transform.position = new Vector3(curPosX, curPosY, curPosZ);
            }
        }
    }
}
