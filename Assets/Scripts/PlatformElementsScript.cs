using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformElementsScript : MonoBehaviour
{
    Transform mainCamTrans; // Transform component of the main camera.

    private bool doneStart = false;
    
    private int platmormsNum = 5;
    private int platformTypeNum = 4;
    private int barrelsNum = 5;
    private int enemiesNum = 5;
    private int[] activPlatforms;
    private GameObject[,] platforms;
    private GameObject[] barrels;
    private GameObject[] centrifuges;

    private float xPosRange;
    private float yPosRange;
    private float minPlatformSpace = 1f;

    private void Awake()
    {
        mainCamTrans = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        activPlatforms = new int[platmormsNum];
        platforms = new GameObject[platmormsNum, platformTypeNum];
        barrels = new GameObject[platmormsNum];
        centrifuges = new GameObject[platmormsNum];
        
        xPosRange = ParallaxScript.frameWidth * 0.5f; // 18f * 0.5f;
        yPosRange = ParallaxScript.frameHeight / (float)platmormsNum; // 32f / (float)platmormsNum;
        
        // Creating a pool of subplatforms.
        for (int i = 0; i < platmormsNum; ++i)
        {
            activPlatforms[i] = 0;
            for (int j = 0; j < platformTypeNum; ++j)
            {
                GameObject platform = Instantiate(Resources.Load("Platform" + (j + 2))) as GameObject;
                platform.SetActive(false);
                platforms[i, j] = platform;
            }
            barrels[i] = Instantiate(Resources.Load("Acid Barrel")) as GameObject;
            centrifuges[i] = Instantiate(Resources.Load("Centrifuge")) as GameObject;
            RecreatePlatform(i);
        }

        doneStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (doneStart)
        {
            // If a platform leaves the frame it is repositioned.
            for (int i = 0; i < platmormsNum; ++i)
            {
                if (platforms[i, activPlatforms[i]].transform.position.y - mainCamTrans.position.y > ParallaxScript.frameHeight) // The platform left the frame.
                {
                    RecreatePlatform(i);
                }
            }
        }
    }

    // Repositioning and setting the charactaristics of the platform. Attributes are randomly generated.
    private void RecreatePlatform(int platformIdx)
    {
        // Size.
        platforms[platformIdx, activPlatforms[platformIdx]].SetActive(false); // Disabling the current platform.
        int size = Random.Range(0, platformTypeNum); // Choosing a new platform size.
        activPlatforms[platformIdx] = size;
        platforms[platformIdx, size].SetActive(true); // Enabling the new platform.
        

        // Repositioning.
        float xPos = Random.Range(-xPosRange, xPosRange);
        float yPos = mainCamTrans.position.y - (ParallaxScript.frameHeight + Random.Range(minPlatformSpace, yPosRange) + platformIdx * yPosRange);
        platforms[platformIdx, size].transform.position = new Vector3(xPos, yPos, -1f);
        
        // Platform type.
        int platformType = Random.Range(0, 3); // 0 for Empty, 1 for accid barrel, 2 for enemy.

        // Cleaning the platform.
        barrels[platformIdx].SetActive(false);
        centrifuges[platformIdx].SetActive(false);

        if (platformType == 1)
        {

            GameObject barrel = barrels[platformIdx];
            barrel.SetActive(true);
            float barrelPosX = xPos + platforms[platformIdx, size].transform.localScale.x * Random.Range(-0.75f, 0.75f);
            float BarrelPosY = yPos + 2 * platforms[platformIdx, size].transform.localScale.y + barrel.transform.localScale.y;
            barrel.transform.position = new Vector3(barrelPosX, BarrelPosY, -1);
        }
        else if (platformType == 2)
        {

            GameObject centrifuge = centrifuges[platformIdx];
            centrifuge.SetActive(true);
            float centrifugePosX = xPos + platforms[platformIdx, size].transform.localScale.x * Random.Range(-0.75f, 0.75f);
            float centrifugePosy = yPos + platforms[platformIdx, size].transform.localScale.y + centrifuge.transform.localScale.y;
            centrifuge.transform.position = new Vector3(centrifugePosX, centrifugePosy, -1);
        }
    }
}
