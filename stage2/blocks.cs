using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blocks : MonoBehaviour
{
    public Transform[] wallbricks;

    public bool faceDir;
    public int hole0;
    public int hole1;

    public void setwall()
    {
        faceDir = Random.value < 0.5;
        hole0 = Random.Range(1, 9);
        hole1 = hole0 + 1;

        if (faceDir)
            transform.localRotation = Quaternion.identity;
        else
            transform.localRotation = Quaternion.Euler(0,90,0);

        for(int i = 0; i < wallbricks.Length; i++)
        {
            Transform t = wallbricks[i];
            if(i == hole0 || i == hole1)
                t.gameObject.SetActive(false);
            else
                t.gameObject.SetActive(true);
        }
    }
}
