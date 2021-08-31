using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DcToGc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GameController;

    public void startTimeKeeper(float time)
    {
        StartCoroutine(SampleCoroutine(time));
    }

    IEnumerator SampleCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        GameController.SetActive(true);
    }
}
