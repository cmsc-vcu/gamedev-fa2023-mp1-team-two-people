using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music : MonoBehaviour
{
      private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}
