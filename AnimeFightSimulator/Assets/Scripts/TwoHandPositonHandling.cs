using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoHandPositonHandling : MonoBehaviour
{
    public Transform hand1;

    public Transform hand2;

    public Transform player;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

        transform.position = (hand1.position + hand2.position) / 2;
        transform.forward = transform.position - player.position;
    }
}
