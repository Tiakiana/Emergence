using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIController : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> AllObjects = new List<GameObject>();
    public bool showRadius = false;
    public bool LightPlay = true;
    public bool blinking = true;
    public static GUIController inst;

    public float Influence = .1f;

    private void Awake()
    {
        inst = this;
    }

    private void OnGUI()
    {



        //if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
        //    Debug.Log("Clicked the button with text");


        if (GUI.Button(new Rect(100, 70, 150, 30), "Randomize Blinking"))
        {
            if (AllObjects.Count > 0)
            {

                foreach (var item in AllObjects)
                {
                    item.GetComponent<BotScr>().ResetPower();
                }
            }
        }
        if (GUI.Button(new Rect(100, 102, 150, 30), "Create Disk"))
        {
            GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            go.GetComponent<BotScr>().ToggleRadius(showRadius);
            go.GetComponent<BotScr>().IsColouring = LightPlay;
            go.GetComponent<BotScr>().ToggleBlinking(blinking);
            AllObjects.Add(go);
        }
        if (GUI.Button(new Rect(100, 134, 150, 30), "Delete Disk"))
        {
            if (AllObjects.Count > 0)
            {
                Destroy(AllObjects[0]);
                AllObjects.RemoveAt(0);
            }
        }

        if (GUI.Button(new Rect(100, 166, 150, 30), "Toggle Radius"))
        {
            if (showRadius)
            {
                showRadius = false;
            }
            else
            {
                showRadius = true;
            }

            if (AllObjects.Count > 0)
            {
                foreach (var item in AllObjects)
                {
                    item.GetComponent<BotScr>().ToggleRadius(showRadius);
                }
            }
        }

        if (GUI.Button(new Rect(100, 198, 150, 30), "Randomize Light"))
        {
            if (AllObjects.Count > 0)
            {
                foreach (var item in AllObjects)
                {
                    item.GetComponent<BotScr>().SetRandomLightColour();
                }
            }
        }

        if (GUI.Button(new Rect(100, 230, 170, 30), "Toggle lights interact: " + LightPlay))
        {
            if (LightPlay)
            {
                LightPlay = false;
            }
            else
            {
                LightPlay = true;
            }

            if (AllObjects.Count > 0)
            {
                foreach (var item in AllObjects)
                {
                    item.GetComponent<BotScr>().IsColouring = LightPlay;
                }
            }
        }

        if (GUI.Button(new Rect(100, 262, 170, 30), "Toggle blinking: " + blinking))
        {
            if (blinking)
            {
                blinking = false;
            }
            else
            {
                blinking = true;
            }

            if (AllObjects.Count > 0)
            {
                foreach (var item in AllObjects)
                {
                    item.GetComponent<BotScr>().ToggleBlinking(blinking);
                }
            }
        }

        if (GUI.Button(new Rect(100, 294, 170, 30), "Create Board"))
        {
            for (int i = -4; i < 5; i++)
            {
                for (int y = 4; y >= -5; y--)
                {
                    GameObject go = Instantiate(prefab, new Vector3(i, 2, y), Quaternion.identity);
                    go.GetComponent<BotScr>().ToggleRadius(showRadius);
                    go.GetComponent<BotScr>().IsColouring = LightPlay;
                    go.GetComponent<BotScr>().ToggleBlinking(blinking);
                    AllObjects.Add(go);
                }
            }
        }

        if (GUI.Button(new Rect(100, 326, 170, 30), "Create spaced board"))
        {
            for (int i = -3; i < 4; i++)
            {
                for (int y = 3; y >= -3; y--)
                {
                    GameObject go = Instantiate(prefab, new Vector3(i * 1.6f, 2, y * 1.6f), Quaternion.identity);
                    go.GetComponent<BotScr>().ToggleRadius(showRadius);
                    go.GetComponent<BotScr>().IsColouring = LightPlay;
                    go.GetComponent<BotScr>().ToggleBlinking(blinking);
                    AllObjects.Add(go);
                }
            }
        }

        if (GUI.Button(new Rect(100, 358, 170, 30), "Delete All Disks"))
        {

            if (AllObjects.Count > 0)
            {
                for (int i = AllObjects.Count - 1; i >= 0; i--)
                {
                    Destroy(AllObjects[i]);
                    AllObjects.RemoveAt(i);
                }
            }

        }

        Influence = GUI.HorizontalSlider(new Rect(100, 400, 100, 30), Influence, 0.0F, 1f);
        GUI.Label(new Rect(100, 420, 100, 30), "" + Influence);

        if (GUI.Button(new Rect(100, 442, 170, 30), "Snap to " + (float)(snapto + 1) / 10))
        {
            snapto++;
            if (snapto > 10)
            {
                snapto = 1;
            }
            Influence = (float)snapto / 10;
        }
        if (GUI.Button(new Rect(100, 500, 170, 50), "Reset"))
        {
            SceneManager.LoadScene(0);
        }


        GUI.Label(new Rect(100, 600, 100, 200), "Tryk \n b = breadth first search \n d= find aggregates \n g= findbridges \n h= find articulation points");

    }
    int snapto = 1;
    public bool ShowConnections = false;



    void FixedUpdate()
    {
        // always draw a 5-unit colored line from the origin

        if (PathfindingControl.inst.from && PathfindingControl.inst.to)
        {
            if (PathfindingControl.inst.from.Pathways.ContainsKey(PathfindingControl.inst.to))
            {
                if (PathfindingControl.inst.from.Pathways[PathfindingControl.inst.to].Count > 0)
                {

                    List<BotScr> liste = PathfindingControl.inst.from.Pathways[PathfindingControl.inst.to];
                    for (int i = 0; i < liste.Count - 1; i++)
                    {
                        Debug.DrawLine(liste[i].transform.position, liste[i + 1].transform.position, Color.white);
                    }
                }
            }

        }

        if (ShowConnections)
        {
            foreach (var item in AllObjects)
            {
                BotScr scr =  item.GetComponent<BotScr>();
                if (scr)
                {
                    foreach (var neig in scr.GetBotScrsOfNeighbours)
                    {
                        Debug.DrawLine(item.transform.position,neig.gameObject.transform.position, Color.white);
                    }
                }
            }
        }
    }



}
