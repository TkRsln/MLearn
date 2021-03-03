using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Neuron {

    //public LineRenderer dentrit;
    public LineRenderer[] aksons;
    public int active_aks = 0;
    public bool has_aks = false;
   // public bool has_dent = false;
    public GameObject output;

    public Neuron(LineRenderer[] aksons,bool hasAks, GameObject obj)
    {
        setUp( aksons, hasAks, /*hasDent*/  obj);
    }
    public Neuron( LineRenderer[] aksons, bool hasAks,int activeAks, GameObject obj)
    {
        this.active_aks = activeAks;
        setUp(/*dentrit*/aksons, hasAks, /*hasDent*/ obj);
    }
    private void setUp(/*LineRenderer dentrit*/LineRenderer[] aksons,bool hasAks,GameObject obj)
    {
       // this.dentrit = dentrit;
        this.aksons = aksons;
        this.has_aks = hasAks;
        this.output = obj;
       // this.has_dent = hasDent;
    }

    public void addAkson(Vector3 target)
    {
        GameObject ln = new GameObject("sinaps");
        ln.transform.position = output.transform.position;
        LineRenderer render=ln.AddComponent<LineRenderer>();
        render.material = new Material(Shader.Find("Sprites/Default"));
        render.SetWidth(0.2f, 0.2f);
        render.SetPosition(0, output.transform.position);
        render.SetPosition(1, target);

        if (aksons != null)
        {
            LineRenderer[] newLn = new LineRenderer[aksons.Length + 1];
            for(int i = 0; i < aksons.Length; i++)
            {
                newLn[i] = aksons[i];
            }
            newLn[aksons.Length] = render;
            aksons = newLn;
        }else
        {
            aksons = new LineRenderer[1];
            aksons[0] = render;
        }
    }
    public Vector3 getGOPosition() { return output.transform.position; }

    public void onUse()
    {
        output.GetComponent<Animator>().Play("awake");
    }
    public void refreshAksons()
    {
        foreach(LineRenderer ln in aksons)
        {
            ln.SetPosition(0, getGOPosition());
        }
    }
    public void removeAllAksons()
    {
        if (aksons == null) return;
        int size = aksons.Length;
        for(int i = 0; i < size; i++)
        {
            GameObject.Destroy(aksons[i]);
        }
        aksons = new LineRenderer[0];
    }
    public void setPosition(Vector3 pos)
    {
        output.transform.position = pos;
    }
    public void setColor(Color clr)
    {
        output.GetComponent<SpriteRenderer>().color = clr;
    }
    public Color getColor()
    {
        return output.GetComponent<SpriteRenderer>().color ;
    }
    public void refAksCol() { refAksCol(active_aks); }
    public void refAksCol(int ln)
    {
        if (aksons.Length <= ln) return;
        aksons[ln].startColor = getColor();
    }
    public void refDentCol()
    {
        //dentrit.endColor = getColor();
    }
    public void setText(string txt)
    {
        output.transform.GetChild(0).GetComponent<TextMesh>().text = txt;
    }
}
