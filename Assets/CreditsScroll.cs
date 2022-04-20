using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField]
    float vel;

    [SerializeField]
    RectTransform rt;

    // Update is called once per frame
    void Update()
    {
        rt.position += new Vector3(0, vel * Time.deltaTime, 0);
    }
}
