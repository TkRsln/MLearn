using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZV1;

public class Visual : MonoBehaviour,IKarar {

    public static Visual active;

    public float height = 0;
    public Transform inputs;
    public Transform outputs;
    public Transform hidden;
    public GameObject prefab_neuron;
    public GameObject prefab_sinaps;

    public Output[] nor_out;
    public NHidden[] nor_hid;
    public NInput nor_in;
    

	void Start () {
        active = this;
        Karar.addListener(this);
        ZV1.Action.Kind[] ary = (ZV1.Action.Kind[])Enum.GetValues(typeof(ZV1.Action.Kind));
        nor_out = new Output[ary.Length];
        for(int i = 0; i < 4; i++)
        {
            nor_out[i] = generateOut(ary[i]);
            nor_out[i].setPosition(outputs.position + (new Vector3(0, 2.25f, 0) - new Vector3(0, 1.5f * i, 0)));
        }
        nor_in = generateIn();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private Output generateOut(ZV1.Action.Kind kind)
    {
        GameObject neuron = Instantiate(prefab_neuron, outputs);
        return new Output(neuron, kind);
    }
    private NInput generateIn()
    {
        GameObject neuron = Instantiate(prefab_neuron, inputs);
        neuron.transform.position = inputs.position;
        return new NInput(neuron);
    }

    public NHidden createHide(string tag,ZV1.Action.Kind solve)
    {
        GameObject neuron = Instantiate(prefab_neuron, hidden);
        neuron.transform.position = hidden.position;
        return new NHidden(neuron, tag, solve);
    }

    public T[] addToDize<T>(T[] dize, T element)
    {
        if (dize == null) return new T[] { element };
        T[] newT = new T[dize.Length + 1];
        for(int i = 0; i < dize.Length; i++)
        {
            newT[i] = dize[i];
        }
        newT[dize.Length] = element;
        return newT;
    }

    public void refreshHiddenPos()
    {
        nor_in.removeAllAksons();
        Vector3[] positions = findHidPosition(nor_hid.Length);

        for (int i = 0; i < nor_hid.Length; i++)
        {
            nor_hid[i].setPosition(hidden.position + positions[i]);
            nor_hid[i].refreshAksons();
            nor_in.addAkson(nor_hid[i].getGOPosition());
        } //*/


        /*
        float toAdd = 8/((nor_hid.Length+1));
        for(int i = 0; i < nor_hid.Length; i++)
        {
            nor_hid[i].setPosition(hidden.position + (new Vector3(0, -4, 0) + new Vector3(0, toAdd * i)));
            nor_hid[i].refreshAksons();
            nor_in.addAkson(nor_hid[i].getGOPosition());
        } //*/
    }
    public Output findOutByKind(ZV1.Action.Kind kind)
    {
        if (nor_out == null ) return null;
        if (nor_out.Length == 0) return null;
        foreach (Output ou in nor_out) { if (ou.kind == kind) return ou; }
        return null;
    }
    public NHidden findHiddenByTag(string tag)
    {
        if (nor_hid == null) return null;
        if (nor_hid.Length == 0) return null;
        foreach (NHidden h in nor_hid)
        {
            if (h.isEqualTag(tag)) return h;
        }
        return null;
    }
    public Vector3[] findHidPosition(int size)
    {
        float max = 3.5f;
        bool cift = (size % 2 == 0);
        Vector3[] last = new Vector3[size];
        if (size >= 2)
        {
            int count = size / 2;
            float toAdd = max / count;
            for(int i = 1; i <= count;i++) {
                Vector3 vec = new Vector3(0, toAdd * i, 0);
                last[i+(cift?-1:0)] = vec;
                last[i+1+ (cift ? -1 : 0)] = -vec;
            }
        }
        if (!cift && size >= 1)
        {last[0] = Vector3.zero;}
        return last;
    }

    #region IKarar
    public void OnScore(Memory m)
    {

    }

    public void OnNewMemory(Memory m)
    {
        nor_in.removeAllAksons();
        NHidden ni = findHiddenByTag(m.last_tag);//
        bool contains = (ni != null);
        if (!contains)ni= createHide(m.last_tag, m.solve);
        ni.addAkson(findOutByKind(m.solve).getGOPosition());
        //ni.addAkson(findOutByKind(m.solve).getGOPosition());
        if(!contains)nor_hid = addToDize(nor_hid, ni);
        refreshHiddenPos();
        Debug.Log("ON NEW :" + m.last_tag+"-"+m.solve.ToString() + " | " + (ni == null)+" | "+nor_hid.Length );
        //nor_in.addAkson(ni.getGOPosition());

    }

    public void OnSolution(Memory m)
    {
        nor_in.onUse();
        NHidden hid = findHiddenByTag(m.last_tag);
        if (hid != null) hid.onUse();
        Output ou = findOutByKind(m.solve);
        if (ou != null) ou.onUse();
    }
    #endregion
}


// HİDDEN SOLUTİON CHANGELERİ DOGRU DEGİL, ÇOKLU SOLVE HATASI