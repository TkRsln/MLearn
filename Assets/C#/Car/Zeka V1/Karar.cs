using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Car;
using System;

namespace ZV1 { 
    public class Karar : MonoBehaviour,IObject,IRespawn {

        public static Karar active;
        public static List<IKarar> listtener = new List<IKarar>();

        public string last_tag = null;
        public GameObject last_go = null;

        [Header("OnEnter")]
        private float last_time = 0f;
        public float time_wait = 5f;

        //[Header("try")]
        //private float last_try_time = 0f;
        //public float time_try_wait = 5f;
        [Header("Memory")]
        public Memory last_solve = null;
        public float last_solve_time=0f;

        public List<Memory> learn = new List<Memory>();

        // Use this for initialization
        void Start () {
            active = this;
            DObject.addListener(this);
            Respawn.addListener(this);
            startEngine();
        }

        // Update is called once per frame
        void Update () {
           // startEngine();
	    }
        public static void addListener(IKarar karar)
        {
            listtener.Add(karar);
        }

        public Memory rememberByReason(ERespawn reason)
        {
            Memory m = null;
            float score = 0f;
            foreach (Memory mem in learn)
            {
                if (mem.reason==reason)
                {
                    if (score < mem.score) m = mem;
                }
            }
            return m;
        }
        public Memory rememberLow(ERespawn reason)
        {
            Memory m = null;
            
            foreach (Memory mem in learn)
            {
                if (mem.reason == reason)
                {
                    m = mem;
                }
            }
            return m;
        }
        public Memory rememberByTag(string tag) {
            Memory m = null;
            float score = -1f;
            foreach(Memory mem in learn)
            {
                if (mem.last_tag != null) { 
                    if (mem.last_tag.ToLower().Equals(tag.ToLower()))
                    {
                        if (score <= mem.score) m = mem;
                    }else if (mem.last_tag.ToLower()==(tag.ToLower()))
                    {
                        if (score <= mem.score) m = mem;
                    }else if (mem.last_tag.ToLower().Contains(tag.ToLower()))
                    {
                        if (score <= mem.score) m = mem;
                    }
                }
            }
            return m;
        }

        public void checkOut(string tag,GameObject  go)
        {
            //if (tag.Equals("Untagged")) return;
//            Debug.Log("Karar OnEnter: " + tag);
            last_tag = tag;
            last_time = Time.time;
            last_go = go;

            Memory m = rememberByTag(tag);
            // Debug.Log("Solve: " + (last_solve != null));
            if (m != null)
            {
                last_solve = m;
                Action.active.apply(last_solve.solve);
                sendEvent(2, last_solve);                       // EVET SENDER 2
                last_solve_time = Time.time;
            }
        }
        public void removeAllSolve(string tag)
        {
            int size = learn.Count;
            List<Memory> temp = new List<Memory>();
            for(int i = 0; i < size; i++)
            {
                Memory m = learn[i];
                if (m.last_tag.Equals(tag))
                {
                    temp.Add(m);
                }
            }
            foreach(Memory m in temp) { learn.Remove(m); }
        }

        void IObject.OnEnter(string tag,GameObject go)
        {
            checkOut(tag,go); 
        }

        void IObject.OnExit(string tag,GameObject go)
        {

        }

        void IRespawn.OnRespawn(ERespawn reason)
        {
            Motor.active.stopAll();
            startEngine();

            if (Time.time < time_wait + last_solve_time) //SOLVE BAŞARISIZ
            {
                //if (last_solve != null)
               // {
                    learn.Remove(last_solve);
                    last_solve.score--;
                    learn.Add(last_solve);
                    sendEvent(1, last_solve);                       // EVET SENDER 1

                if (last_solve.score <= 0)
                    {
                        Action.Kind[] solves = notTried(reason);
                        if (solves.Length != 0)
                        {
                            Memory newMem = new Memory(last_tag, reason, solves[0], 1);
                            learn.Add(newMem);
                            sendEvent(0, newMem);                       // EVET SENDER 0 _ YENİ SİNAPS
                        }
                        else
                        {
                            //ERespawn[] kinds = (ERespawn[])Enum.GetValues(typeof(Action.Kind));
                            Memory m = rememberByTag(last_tag); // rememberLow(reason);
                            removeAllSolve(last_tag);
                            m.score++;
                            learn.Add(new Memory(last_tag,m.reason,Action.Kind.Ateş,1));
                            sendEvent(0, m);                       // EVET SENDER 0 _ YENİ SİNAPS
                    }
                  }
                //}
            }
            else // SOLVE BAŞARILI
            {
                if (Time.time < time_wait + last_time&&last_tag!=null) { 
                    Action.Kind[] solves = notTried(reason);
                    if (solves.Length != 0)
                    {
                        Memory newMem = new Memory(last_tag, reason, solves[0], 1);
                        learn.Add(newMem);
                        sendEvent(0, newMem);                       // EVET SENDER 0 _ YENİ NEURON
                    }
                }
                else { }
            }


          //  if (Time.time < time_wait + last_time) Debug.Log("Object: "+ last_tag + " | Left Time: "+((time_wait + last_time)-Time.time)+" | Reason: "+reason.ToString() );
          //  else Debug.Log("Object: NuLL"  + " | Left Time: " + ((time_wait + last_time) - Time.time) + " | Reason: " + reason.ToString());
        }

        public Action.Kind[] notTried(ERespawn reason)
        {
            int size = Enum.GetNames(typeof(Action.Kind)).Length;
            int[] finded = new int[size];
            for (int i = 0; i < size; i++) finded[i] = i;
            foreach(Memory m in learn)
            {
                if (m.reason == reason)
                {
                    finded[(int)m.solve] = 0;
                }
            }
            int size_last = 0;
            foreach (int i in finded) if (i != 0) size_last++;
            int[] newFinded = new int[size_last];
            int count = 0;
            for (int i = 0; i < finded.Length; i++) { if (finded[i] != 0) { newFinded[count] = finded[i];count++; } }
            Array en = Enum.GetValues(typeof(Action.Kind));
            Action.Kind[] last = new Action.Kind[size_last];
            for(int i = 0; i < last.Length; i++)
            {
                last[i] =(Action.Kind) en.GetValue(newFinded[i]);
            }
            return last;

        }

        public void startEngine()
        {
            Thread t =new Thread(()=>
            {
                Thread.Sleep(4500);
                Motor.active.startAll(180);
            });
            t.Start();
            
        }
        public void sendEvent(int i,Memory m)
        {
            foreach(IKarar kr in listtener)
            {
                if (i == 0) kr.OnNewMemory(m);  // YENI TECRUBE
                if (i == 1) kr.OnScore(m);      // FAIL OLDUGUNDA
                if (i == 2) kr.OnSolution(m);   // UYGULAMADA
            }
        }

    }

    
}

/*** 
 * REMEMBER Ile OnEnter daki cismi tarat( TARATICI FONKSİYONLARI KUR- MEMORY KAYDET) YUKSEK PUANLI DÖNDÜR
 * EGER REMEMDE GELEN MEMORY NULL İSE YENİ BİR TANE OLUŞTUR
 * EĞER BAŞARILI İSE PUAN ARTTIR
 * DEĞİL İSE YENİ SOLUTİON KUR PUANINI YUKSEK YAP
 */ 
