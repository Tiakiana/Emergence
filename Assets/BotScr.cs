using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotScr : MonoBehaviour
{
    public float Power { get; private set; }
    public bool Steadfast { get; private set; }
    public Material white, red;

    public GameObject MyLight;
    public List<GameObject> OtherInstances = new List<GameObject>();
    public int NumberOfConnectedInstances = 0;
    public bool IsColouring = true;
    public float PowerNess = 0.25f;
    public bool isBlinking = true;
    public void ToggleBlinking(bool blinking)
    {
        isBlinking = blinking;
        if (!blinking)
        {
            MyLight.SetActive(true);
        }

    }
    public List<BotScr> GetBotScrsOfNeighbours
    {
        get
        {
            List<BotScr> res = new List<BotScr>();
            foreach (GameObject item in OtherInstances)
            {
                res.Add(item.GetComponent<BotScr>());
            }
            return res;
        }
        
    }

   public Dictionary<BotScr, List<BotScr>> Pathways = new Dictionary<BotScr, List<BotScr>>();

   public void ComparePathways(BotScr target, List<BotScr> newpath)
    {
        if (Pathways.Keys.Contains(target))
        {
            if (Pathways[target].Count>= newpath.Count)
            {
                Pathways[target] = newpath;
            }
        }
        else
        {
            Pathways[target] = newpath;

        }
    }

    void Start()
    {
        gameObject.tag = "Bot";
        StartCoroutine("Beep");
        Power = Random.value;
        SetRandomLightColour();
    }

    void Update()
    {

       
        if (Input.GetKey("r"))
        {
            ResetPower();
        }
        OtherInstances.Clear();
        Collider[] cols = Physics.OverlapSphere(transform.position, 1.5f).Where(x => x.gameObject.tag == "Bot").ToArray();
        foreach (var item in cols)
        {
            if (!item.gameObject.Equals(this.gameObject))
            {

                OtherInstances.Add(item.gameObject);
            }
        }
        NumberOfConnectedInstances = OtherInstances.Count;
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1))
        {
            ToggleSteadfast();
        }


        if (Input.GetMouseButtonUp(2))
        {
            GUIController.inst.AllObjects.Remove(gameObject);
            Destroy(gameObject);
        }
        if (Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.LeftControl))
        {
                SetRandomLightColour();
        }

        if (Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.LeftShift))
        {
            PathfindingControl.inst.from = this;
        }
        if (Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.RightShift))
        {
            PathfindingControl.inst.to = this;
        }

    }
    private void ToggleSteadfast()
    {
        if (Steadfast)
        {
            Steadfast = false;
            GetComponent<MeshRenderer>().material = red;
            PowerNess = 0.25f;
        }
        else
        {
            Steadfast = true;
            GetComponent<MeshRenderer>().material = white;
            PowerNess = .4f;

        }
    }

    IEnumerator Beep()
    {
        //Denne coroutine kører for så længe programmet kører og for hver instans af Disk.
        while (true)
        {
            Power += 0.01f;

            //power sættes til 0 hvis mere end 1
            if (Power >= 1)
            {
                Power = 0;
                if (OtherInstances.Count > 0)
                {
                    foreach (var item in OtherInstances)
                    {
                        // Alle der er inden for rækkevidde får boostet lysten til at blinke
                        item.GetComponent<BotScr>().Boost();
                        if (IsColouring)
                        {
                            item.GetComponent<BotScr>().BoostColour(MyLight.GetComponent<Light>().color, PowerNess);
                        }
                    }
                }
                StartCoroutine("Blink");
            }
            yield return new WaitForSeconds(0.01f);
        }
    }


    public void Boost()
    {
        //Influence ligger på en værdi fra 0 -1;
        Power += Power * GUIController.inst.Influence;
    }

    IEnumerator Blink()
    {
        if (isBlinking)
        {

            MyLight.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            if (isBlinking)
            {

            MyLight.SetActive(false);
            }
        }

    }

    public void ResetPower()
    {
        Power = Random.value;
    }


    public void ToggleRadius(bool val)
    {
        GetComponent<LineRenderer>().enabled = val;

    }

    public void SetRandomLightColour()
    {
        MyLight.GetComponent<Light>().color = Random.ColorHSV(0, 1,1,1,0,1,1,1);
    }
    public void BoostColour(Color cl)
    {
        if (!Steadfast)
        {
        Color mycolor = MyLight.GetComponent<Light>().color;
        MyLight.GetComponent<Light>().color = Color.Lerp(mycolor, cl, 0.25f);
        }

    }
    public void BoostColour(Color cl, float power)
    {
        if (!Steadfast)
        {
            Color mycolor = MyLight.GetComponent<Light>().color;
            MyLight.GetComponent<Light>().color = Color.Lerp(mycolor, cl, power);
        }

    }
}
