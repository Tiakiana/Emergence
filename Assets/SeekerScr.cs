using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekerInfo
{
    public BotScr Home, Origin, Current, Endtarget;
    public List<BotScr> AlreadyVisited;
    public int Resources;
}

public class SeekerScr : MonoBehaviour
{
    public GameObject Seeker;

    // Start is called before the first frame update
    void Start()
    {
        Seeker = PathfindingControl.inst.Seeker;
    }



    public void StartSeeking(BotScr home, BotScr origin, BotScr current, BotScr endtarget, List<BotScr> alreadyVisited, int resources)
    {

        StartCoroutine("Seeking", new SeekerInfo() { Home = home, AlreadyVisited = alreadyVisited, Current = current, Endtarget = endtarget, Origin = origin , Resources = resources});

    }
    public IEnumerator Seeking(SeekerInfo info)
    {
        while (Vector3.Distance(transform.position, info.Current.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.transform.position, info.Current.transform.position, 0.1f);
            yield return new WaitForSeconds(.01f);
        }


        info.AlreadyVisited.Add(info.Current);

        if (info.Current.Equals(info.Endtarget))
        {
            info.Home.ComparePathways(info.Endtarget, info.AlreadyVisited);
            Destroy(gameObject);
        }
        else
        {
            List<BotScr> nonintersect = info.Current.GetBotScrsOfNeighbours.Except(info.AlreadyVisited).ToList();
            if (nonintersect.Count > 0)
            {
                A:
                if (info.Resources>0)
                {

                var item = nonintersect[Random.Range(0, nonintersect.Count)];
                GameObject go = Instantiate(Seeker, transform.position, Quaternion.identity);
                SeekerScr scr = go.GetComponent<SeekerScr>();
                List<BotScr> already = new List<BotScr>();
                foreach (BotScr botscr in info.AlreadyVisited)
                {
                    already.Add(botscr);
                }
                    info.Resources -= 1;
                scr.StartSeeking(info.Home, info.Current, item, info.Endtarget, already,info.Resources);
                    if (Random.Range(0,2) ==1)
                    {
                        goto A;
                    }
                }

                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }




}
