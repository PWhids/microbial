using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    // Start is called before the first frame update
    Renderer r;
    Material mat;
    Vector3[] poses;
    Vector3[] vels;
    int count;
    void Start()
    {
        r = GetComponent<Renderer>();
        // mat = Material.find
        count = 5;
        poses = new Vector3[count];
        vels = new Vector3[count];
        initParts();
    }

    void initParts()
    {
        for (int i = 0; i < count; i++)
        {
            poses[i] = Random.insideUnitSphere;
            vels[i] = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    void updateControls()
    {
        // Functions of time here can be replaced with input controls
        vels[0].x += 0.0000075f*Mathf.Sin(Time.realtimeSinceStartup);
        vels[0].y += 0.000003f * Mathf.Sin(3.1f*Time.realtimeSinceStartup);
    }

    void updateSim()
    {
        updateControls();
        for (int i = 0; i < count - 1; i++)
        {
            for (int j = i + 1; j < count; j++)
            {
                Vector3 diff = poses[i] - poses[j];
                float rad = 0.05f;
                float mag = diff.magnitude;
                float fc = 0.0000005f / (mag * mag * mag + 0.05f);
                if (mag < 0.2)
                {
                    fc += 0.001f * (mag - 0.2f);
                }
                vels[i] -= fc * diff;
                vels[j] += fc * diff;
            }
        }

        for (int i = 0; i < count; i++)
        {
            Vector3 diff = poses[i];
            float rad = 0.05f;
            float mag = diff.magnitude;
            float fc = 0.000002f / (mag * mag * mag + 0.05f);
            vels[i] -= fc * diff;
            vels[i] *= 0.999f;
            poses[i] += vels[i];
        }

    }

    // Update is called once per frame
    void Update()
    {
        //float vl = Mathf.PingPong(Time.time, 1.0f);
        //r.material.SetFloat("_Speedy", vl);

        for (int sc = 0; sc < 10; sc++)
        {
            updateSim();
        }

        for (int i = 0; i < count; i++)
        {
            
            r.material.SetVector("_Pos" + (i + 1), poses[i]);
            r.material.SetVector("_Vel" + (i + 1), vels[i]);
        }
    }
}
