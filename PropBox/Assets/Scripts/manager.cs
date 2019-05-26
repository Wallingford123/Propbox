using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manager : MonoBehaviour {
    public List<GameObject> pedestals;
    public GameObject key;
    public GameObject gem;
    public GameObject gemPed;
    public GameObject demoGem;
    public GameObject door;
    public List<Material> colors;
    public GameObject cam;
    mainScript emptyHand;
    public float leeway;
    bool lol = false;

    public float collisionWidth;
    mainScript g, k, demo;

    AABB keyBox;
    AABB gemBox;
    AABB camBox;
    AABB demoGemBox;
    AABB doorBox;

    public class AABB
    {
        public float left;
        public float right;
        public float front;
        public float back;
        public float top;
        public float bottom;
        public AABB(Vector3 pos, float width)
        {

            left = pos.x - width;
            right = pos.x + width;
            front = pos.z + width;
            back = pos.z - width;
            top = pos.y + width;
            bottom = pos.y - width;
        }

    }


    int activeKey, activeGem, activePed1, activePed2;
    void Start()
    {
        ResetGame();   
    }
    
    //function to 'replace' start function, so I can reset level without requiring scene reload;
    void ResetGame()
    {
        setRandomComponents();
        setDemo();
        setKey();
        setGem();
        setStaticBounds();
    }
	
	void Update () {
        setDemoRotator();
        testCheats();
        setBounds();
        testCam();
        testKey();
        testGem();

        checkReset();
    }

    //function to set the dynamic bounding boxes;
    void setBounds()
    {
        if (!lol) gemBox = new AABB(new Vector3(g.pos.x, g.pos.y, g.pos.z + 2.2f), collisionWidth);
        else gemBox = new AABB(new Vector3(g.pos.x, g.pos.y, g.pos.z - 2.2f), collisionWidth);
        if (!lol) keyBox = new AABB(new Vector3(k.pos.x, k.pos.y, k.pos.z + 2.2f), collisionWidth);
        else keyBox = new AABB(new Vector3(k.pos.x, k.pos.y, k.pos.z - 2.2f), collisionWidth);
        camBox = new AABB(cam.transform.position, collisionWidth);
    }

    //function to test if the key is in the correct orientation and translation to 'place' it in the door;
    void testKey()
    {
        if (k.isPicked)
        {
            if (keyBox.right >= doorBox.left && keyBox.left <= doorBox.right && keyBox.top >= doorBox.bottom && keyBox.bottom <= doorBox.top && keyBox.front >= doorBox.back && keyBox.back <= doorBox.front)
            {
                if (k.q.x < 0.1f && k.q.x > -0.1f)
                    if ((k.q.y > 0.585f && k.q.y < 0.65f) || (k.q.y < -0.585f && k.q.y > -0.65f))
                        if (k.q.z > -0.1f)
                            if (k.q.w > 0.6f || k.q.w < 0.01f)
                            {
                                //sets variables appropriately to disable user input movement to keep it in place;
                                emptyHand.pos = k.pos;
                                emptyHand.y = k.y;
                                emptyHand.g = k.g;
                                k.placed = true;
                                k.isPicked = false;
                                emptyHand.isPicked = true;
                            }
            }
        }
    }

    //function to test if the gem is in the correct orientation, translation and scale to place it on the pedestal;
    void testGem()
    {
        if (g.isPicked)
        {
            if (gemBox.right >= demoGemBox.left && gemBox.left <= demoGemBox.right && gemBox.top >= demoGemBox.bottom && gemBox.bottom <= demoGemBox.top && gemBox.front >= demoGemBox.back && gemBox.back <= demoGemBox.front)
            {
                if (g.q.x > demo.q.x - leeway && g.q.x < demo.q.x + leeway &&
                    g.q.y > demo.q.y - leeway && g.q.y < demo.q.y + leeway &&
                    g.q.z > demo.q.z - leeway && g.q.z < demo.q.z + leeway &&
                    g.q.w > demo.q.w - leeway && g.q.w < demo.q.w + leeway)
                {
                    if (g.scaleMul > demo.scaleMul - 0.5f && g.scaleMul < demo.scaleMul + 0.5f)
                    {
                        g.placed = true;
                        g.isPicked = false;
                        emptyHand.pos = g.pos;
                        emptyHand.y = g.y;
                        emptyHand.g = g.g;
                        g.pos = new Vector3(gemPed.transform.position.x + 0.34f, gemPed.transform.position.y, gemPed.transform.position.z + 1.87f);
                        demoGem.SetActive(false);
                        emptyHand.isPicked = true;
                        g.notSet = true;
                    }
                }
            }
        }
    }

    //function to test if the camera bounding box is overlapping with either the key or gem in order to pick it up;
    void testCam()
    {
        if (camBox.right >= gemBox.left && camBox.left <= gemBox.right && camBox.top >= gemBox.bottom && camBox.bottom <= gemBox.top && camBox.front >= gemBox.back && camBox.back <= gemBox.front && !g.isPicked && !k.isPicked && !g.placed)
        {
            emptyHand.isPicked = false;
            g.pos = cam.transform.position;
            g.g = emptyHand.g;
            g.y = emptyHand.y;
            g.isPicked = true;
        }
        if (camBox.right >= keyBox.left && camBox.left <= keyBox.right && camBox.top >= keyBox.bottom && camBox.bottom <= keyBox.top && camBox.front >= keyBox.back && camBox.back <= keyBox.front && !g.isPicked && !k.isPicked && !k.placed)
        {
            emptyHand.isPicked = false;
            k.pos = cam.transform.position;
            k.g = emptyHand.g;
            k.y = emptyHand.y;
            k.isPicked = true;
        }
    }

    //function to cheat the maze and teleport to the objects and central area;
    void testCheats()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            emptyHand.pos = g.pos;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            emptyHand.pos = k.pos;
        }
        if (Input.GetKeyDown(KeyCode.N) && g.isPicked)
        {
            g.pos = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.H) && k.isPicked)
        {
            k.pos = new Vector3(0, 0, 0);
        }
    }

    //function to set the object that is currently controlling the camera so the demo gem can rotate to face the player;
    void setDemoRotator()
    {
        if (emptyHand.isPicked) demo.g = emptyHand.g;
        if (k.isPicked) demo.g = k.g;
        if (g.isPicked) demo.g = g.g;
    }

    //function to check if both objects are placed, and if so reset the game;
    void checkReset()
    {
        if (k.placed && g.placed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                pedestals[activePed1].SetActive(false);
                pedestals[activePed2].SetActive(false);
                demoGem.SetActive(true);
                lol = true;
                ResetGame();
            }
        }
    }

    //function to set random elements of the game (color of gem/key and position of it in the maze);
    void setRandomComponents()
    {
        activeKey = Random.Range(0, colors.Capacity);
        activeGem = Random.Range(0, colors.Capacity);
        while (activeKey == activeGem)
        {
            activeGem = Random.Range(0, colors.Capacity);
        }
        activePed1 = Random.Range(0, pedestals.Capacity);
        activePed2 = Random.Range(0, pedestals.Capacity);
        while (activePed2 == activePed1)
        {
            activePed2 = Random.Range(0, pedestals.Capacity);
        }
        key.GetComponent<Renderer>().material = colors[activeKey];
        gem.GetComponent<Renderer>().material = colors[activeGem];
        pedestals[activePed1].SetActive(true);
        pedestals[activePed2].SetActive(true);
    }

    //function to set the bounding boxes of objects that will not change (demo gem and door bounding boxes);
    void setStaticBounds()
    {
        if (!lol) demoGemBox = new AABB(new Vector3(demo.pos.x, demo.pos.y, demo.pos.z + 2.2f), collisionWidth);
        else demoGemBox = new AABB(new Vector3(demo.pos.x, demo.pos.y, demo.pos.z - 2.2f), collisionWidth);
        if (!lol) doorBox = new AABB(new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z + 4.4f), collisionWidth);
        else doorBox = new AABB(new Vector3(door.transform.position.x, door.transform.position.y, door.transform.position.z), collisionWidth);
    }

    //function to set the initial values of the key object
    void setDemo()
    {
        demo = demoGem.GetComponent<mainScript>();
        demo.pos = gemPed.transform.position;
        demo.pos.y = -1.8f;
        demo.pos.z = gemPed.transform.position.z;
        demo.yawAng = Random.Range(0.0f, Mathf.PI * 2);
        demo.pitchAng = Random.Range(0.0f, Mathf.PI * 2);
        float scale = Random.Range(0.5f, 3.0f);
        demo.scaleMul = scale;
        demo.isKey = false;
        demo.notSet = true;
    }

    //function to set the initial values of the demo gem object
    void setKey()
    {
        k = key.GetComponent<mainScript>();
        k.scaleMul = 1;
        k.isKey = true;
        k.notSet = true;
        k.placed = false;
        k.pos = pedestals[activePed1].transform.position;
        k.pos.y = -1.8f;
        if (!lol) k.pos.z = k.pos.z - 2.2f;
        else k.pos.z = k.pos.z + 2.2f;
        k.q = new mainScript.Quat(1, new Vector3(0, 0, 0));
    }

    //function to set the initial values of the gem object
    void setGem()
    {
        g = gem.GetComponent<mainScript>();
        g.isKey = false;
        g.notSet = true;
        g.placed = false;
        g.pos = pedestals[activePed2].transform.position;
        g.pos.y = -1.8f;
        if (!lol) g.pos.z = g.pos.z - 2.2f;
        else g.pos.z = g.pos.z + 2.2f;
        g.scaleMul = 1;
        g.q = new mainScript.Quat(1, new Vector3(0, 0, 0));
        emptyHand = cam.GetComponentInChildren<mainScript>();
    }
}
