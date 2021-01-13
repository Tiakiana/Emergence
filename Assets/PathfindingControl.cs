using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingControl : MonoBehaviour
{
    public static PathfindingControl inst;
    public GameObject Seeker;
    public BotScr from, to;
    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("w"))
        {
            if (from && to)
            {
                GameObject go = Instantiate(Seeker, from.transform.position,Quaternion.identity);
                go.GetComponent<SeekerScr>().StartSeeking(from,from,from,to,new List<BotScr>(),10);
            }
            else
            {
                Debug.Log("Pick end point");
            }
        }
    }
}
