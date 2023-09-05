using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightTakedown : MonoBehaviour
{
    [SerializeField] GameObject _light;
    [SerializeField] Transform _cam;
    RaycastHit hit;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip sniperSound;

    private bool hitEnemy;
    public float countDown = 1;
    public float range;
    private bool soundPlayed;

    private GameObject thunderSound;
    public GameManager manager;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(CheckThunderSound());
        LightTakedown();
        LightToggle();
    }

    private void LightToggle()
    {
        if (Input.GetKey(KeyCode.F))
        {
            _light.SetActive(true);
        }
        else
        {
            _light.SetActive(false);
        }
    }


    private void LightTakedown()
    {
        if (!_light.activeSelf)
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
            hit.collider.gameObject.GetComponent<EnemyAnimation>().die = true;
            source.PlayOneShot(sniperSound);
            countDown = 1;

            if (CheckThunderSound())
            {
                return;
            }
            else
            {
                manager.lose = true;
            }
        }
    }

    private bool CheckThunderSound()
    {
        thunderSound = GameObject.FindGameObjectWithTag("Thunder");

        if (thunderSound)
        {
            return true;
        }
        return false;
    }
}
