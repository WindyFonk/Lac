using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightTakedown : MonoBehaviour
{
    [SerializeField] GameObject light;
    [SerializeField] Transform _cam;
    RaycastHit hit;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip sniperSound;

    private bool hitEnemy;
    public float countDown = 1;
    public float range;
    private bool soundPlayed;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LightTakedown();
        LightToggle();
    }

    private void LightToggle()
    {
        if (Input.GetKey(KeyCode.F))
        {
            light.SetActive(true);
        }
        else
        {
            light.SetActive(false);
        }
    }


    private void LightTakedown()
    {
        if (!light.activeSelf)
        {
            countDown = 1;
            return;

        }


        hitEnemy = Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, range, enemyMask);

        if (hitEnemy)
        {
            countDown -= Time.deltaTime;
        }

        else
        {
            countDown = 1;
        }

        if (countDown<0)
        {
            Debug.Log("Enemy Marked");
            hit.collider.gameObject.GetComponent<EnemyAnimation>().die = true;
            source.PlayOneShot(sniperSound);
            countDown = 1;
        }
    }
}
