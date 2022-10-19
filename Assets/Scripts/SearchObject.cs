using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchObject : MonoBehaviour
{
    [SerializeField] AudioClip winkSound;
    [SerializeField] private int soundInterval; //seconds

    public bool pingAudio;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SoundOut());
    }
    
    IEnumerator SoundOut()
    {
        while (pingAudio) {
            AudioSource.PlayClipAtPoint(winkSound, transform.position);
            yield return new WaitForSeconds(soundInterval);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
