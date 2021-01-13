using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstSearch : MonoBehaviour
{
    public List<Material> Materials = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("d"))
        {

            FindAggregates();
        }
        if (Input.GetKeyUp("g"))
        {
            FindBridges();
        }
        if (Input.GetKeyUp("h"))
        {
            FindArticulationPoints();
        }

    }
    private void OnGUI()
    {
        
    }
    public int numberOfNodesInGraph;
    public List<BotScr> adjacencyList = new List<BotScr>();
    public List<BotScr> visited = new List<BotScr>();
    public int count = 0;
    public List<List<BotScr>> Aggregates = new List<List<BotScr>>();
    //God til at finde ting i et netværk, der ikke er forbundet med noget. 
    //Depth first searchen går bare igennem ting uden at gøre noget, den er ligesom en cykel noget funktionalitet kan bruge til at besøge et helt netværk.
    public void FindAggregates()
    {
        Aggregates.Clear();
        adjacencyList.Clear();
        numberOfNodesInGraph = GUIController.inst.AllObjects.Count;
        count = -1;
        GUIController.inst.AllObjects.ForEach(x => adjacencyList.Add(x.GetComponent<BotScr>()));
        visited.Clear();
        BotScr startNode = adjacencyList[0];
        MarkStructures();

        for (int i = 0; i < Aggregates.Count; i++)
        {

            foreach (var bots in Aggregates[i])
            {
                bots.GetComponent<Renderer>().material = Materials[i];
            }
        }
    }

    private void MarkStructures()
    {
        foreach (var item in adjacencyList)
        {
            if (!visited.Contains(item))
            {
                count++;
                Aggregates.Add(new List<BotScr>());
                dfs(item);
            }
        }
    }

    private void dfs(BotScr at)
    {
        if (visited.Contains(at))
        {
            return;
        }
        else
        {
            visited.Add(at);
            Aggregates[count].Add(at);
            List<BotScr> neighbours = at.GetBotScrsOfNeighbours;
            foreach (BotScr item in neighbours)
            {
                dfs(item);
            }
        }
    }

    int ID = 0;
    int outEdgeCount = 0;
    Dictionary<BotScr, int> ids = new Dictionary<BotScr, int>();
    Dictionary<BotScr, int> low = new Dictionary<BotScr, int>();

    //Find broer
    public void FindBridges()
    {
        ID = -1;
        ids.Clear();
        low.Clear();
        adjacencyList.Clear();
        GUIController.inst.AllObjects.ForEach(x => adjacencyList.Add(x.GetComponent<BotScr>()));
        visited.Clear();
        List<BotScr> bridges = new List<BotScr>();

        foreach (var item in adjacencyList)
        {
            if (!visited.Contains(item))
            {
                dfs(item, null, bridges);
            }
        }

        foreach (BotScr item in bridges)
        {
            item.GetComponent<Renderer>().material = Materials[0];
        }

    }

    public void FindArticulationPoints()
    {
        ID = -1;
        ids.Clear();
        low.Clear();
        adjacencyList.Clear();
        outEdgeCount = 0;
        GUIController.inst.AllObjects.ForEach(x => adjacencyList.Add(x.GetComponent<BotScr>()));
        visited.Clear();
        //   BotScr startNode = adjacencyList[Random.Range(0, adjacencyList.Count)];
        List<BotScr> ArticulationPoints = new List<BotScr>();
        foreach (var item in adjacencyList)
        {
            if (!visited.Contains(item))
            {
                outEdgeCount = 0;
                dfs(item, item, null, ArticulationPoints);
                if (outEdgeCount > 1)
                {
                    ArticulationPoints.Add(item);
                }
            }
        }
        foreach (BotScr item in ArticulationPoints)
        {
            item.GetComponent<Renderer>().material = Materials[0];
        }
    }


    private void dfs(BotScr at, BotScr parent, List<BotScr> bridges)
    {

        if (visited.Contains(at))
        {
            return;
        }
        else
        {
            visited.Add(at);
            ID += 1;
            ids.Add(at, ID);
            low.Add(at, ID);

            //Aggregates[count].Add(at);
            List<BotScr> neighbours = at.GetBotScrsOfNeighbours;
            foreach (BotScr to in neighbours)
            {
                if (to == parent)
                {
                    continue;
                }
                if (!visited.Contains(to))
                {
                    dfs(to, at, bridges);
                    low[at] = System.Math.Min(low[at], low[to]);
                    if (ids[at] < low[to])
                    {
                        Debug.Log("Yo here bridge man!");
                        bridges.Add(at);
                        bridges.Add(to);
                    }

                }
                else
                {
                    low[at] = System.Math.Min(low[at], ids[to]);
                }
            }
        }
    }

    private void dfs(BotScr root, BotScr at, BotScr parent, List<BotScr> artpoints)
    {

        if (root == parent)
        {
            outEdgeCount++;
        }
        if (visited.Contains(at))
        {
            return;
        }

        else
        {
            visited.Add(at);
            ID += 1;
            ids.Add(at, ID);
            low.Add(at, ID);

            //Aggregates[count].Add(at);
            List<BotScr> neighbours = at.GetBotScrsOfNeighbours;
            foreach (BotScr to in neighbours)
            {
                if (to == parent)
                {
                    continue;
                }
                if (!visited.Contains(to))
                {
                    dfs(root, to, at, artpoints);
                    low[at] = System.Math.Min(low[at], low[to]);
                    if (ids[at] <= low[to])
                    {
                        Debug.Log("Yo here articulationpoint man!");
                        artpoints.Add(at);
                    }

                }
                else
                {
                    low[at] = System.Math.Min(low[at], ids[to]);
                }
            }
        }
    }



}
