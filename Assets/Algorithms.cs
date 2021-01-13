using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartStopNode{
    public BotScr Start;
    public BotScr Stop;
    }


public class Algorithms : MonoBehaviour
{
    public List<Material> Materials = new List<Material>();

    Queue<BotScr> Myqueue = new Queue<BotScr>();
    public List<BotScr> adjacencyList = new List<BotScr>();
    public List<BotScr> visited = new List<BotScr>();

    private void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            // StartCoroutine("BreadthFirstShortestPathSeatch");
            BreadthFirstShortestPathSearch();
            Debug.Log("Done");
        }

    }



    public void BreadthFirstSearch()
    {
        adjacencyList.Clear();
        visited.Clear();
        Myqueue.Clear();
        GUIController.inst.AllObjects.ForEach(x => adjacencyList.Add(x.GetComponent<BotScr>()));
        Myqueue.Enqueue(adjacencyList[0]);
        if (Myqueue.Count > 0)
        {
            StartCoroutine("bfs");
        }

    }
    public void BreadthFirstShortestPathSearch()
    {
        adjacencyList.Clear();
        visited.Clear();
        Myqueue.Clear();
        childParent.Clear(); 
        GUIController.inst.AllObjects.ForEach(x => adjacencyList.Add(x.GetComponent<BotScr>()));
        Myqueue.Enqueue(adjacencyList[0]);
        adjacencyList[0].GetComponent<Renderer>().material = Materials[0];
        adjacencyList[adjacencyList.Count-1].GetComponent<Renderer>().material = Materials[1];
        Target = adjacencyList[adjacencyList.Count - 1];
        if (Myqueue.Count > 0)
        {
            StartCoroutine("bfs_shortest", adjacencyList[0]);
        }
    }
    public BotScr Target;



    public IEnumerator bfs()
    {
        BotScr crnt = Myqueue.Dequeue();
        visited.Add(crnt);
        crnt.transform.Rotate(Vector3.right, 90);
        // Gør hvadend der skal gøres her!
        yield return new WaitForSeconds(.1f);
        Debug.Log("yayaya nu kommer jeg");
        foreach (var item in crnt.GetBotScrsOfNeighbours)
        {
            if (!Myqueue.Contains(item) && !visited.Contains(item))
            {
                Myqueue.Enqueue(item);
            }
        }
        if (Myqueue.Count > 0)
        {
            StartCoroutine("bfs");
        }

    }


    //public List<BotScr> path;
    Dictionary<BotScr, BotScr> childParent = new Dictionary<BotScr, BotScr>();

    public IEnumerator GetPath(StartStopNode stet)
    {
        BotScr crnt = stet.Stop;
        List<BotScr> res = new List<BotScr>();
        while (crnt != stet.Start)
        {
            crnt.transform.Rotate(Vector3.right, -90);
            res.Add(crnt);
            crnt = childParent[crnt];
            yield return new WaitForSeconds(.5f);
        }
        res.Add(stet.Start);
        stet.Start.transform.Rotate(Vector3.right, -90);

        res.Reverse();
       



    }

    public IEnumerator bfs_shortest(BotScr startnode)
    {
        BotScr crnt = Myqueue.Dequeue();
        visited.Add(crnt);
        crnt.transform.Rotate(Vector3.right, 90);
        if (crnt == Target)
        {
            StartCoroutine("GetPath", new StartStopNode() { Start = startnode, Stop = crnt });
        }
        else
        {
            yield return new WaitForSeconds(.3f);
            // Gør hvadend der skal gøres her!
            Debug.Log("yayaya nu kommer jeg");
            foreach (var item in crnt.GetBotScrsOfNeighbours)
            {
                if (!Myqueue.Contains(item) && !visited.Contains(item))
                {
                    Myqueue.Enqueue(item);
                    childParent.Add(item, crnt);
                }
            }
            if (Myqueue.Count > 0)
            {
                StartCoroutine("bfs_shortest", startnode);
            }
        }

    }




}


