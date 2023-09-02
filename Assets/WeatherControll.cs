using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherControll : MonoBehaviour
{
    public GameObject thunder;
    float count = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count -= Time.deltaTime;
        if (count < 0)
        {
            SpawnThunder();
            count = Random.value*10+10;
        }
    }

    private void SpawnThunder()
    {
        GameObject _thunder = Instantiate(thunder);
        Destroy(_thunder, 3);
    }
}
