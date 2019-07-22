using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private WeaponManager manager;
    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    private bool isAiming;
    private Animator zoomCamera;
    private bool zoomed;

    private Camera main;
    private GameObject crossHair;

    [SerializeField]
    private GameObject arrowPrefab, spearPrefab;

    [SerializeField]
    private Transform bowArrowStartPosition;
    void Awake()
    {
        manager = GetComponent<WeaponManager>();

        zoomCamera = transform.Find("Look Rotation").transform.Find("FPCamera").GetComponent<Animator>();

        crossHair = GameObject.FindWithTag("Crosshair");

        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ShootWeapon();
        ZoomInAndOut();
    }

    void ShootWeapon()
    {
        if(manager.GetCurrentSelectedWeapon().fireType==WeaponFireType.MULTIPLE)
        {
            if(Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                manager.GetCurrentSelectedWeapon().ShootAnimation();
                BulletFired();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            { 
                if (manager.GetCurrentSelectedWeapon().tag == "Axe")
                {
                    manager.GetCurrentSelectedWeapon().ShootAnimation();
                }
             

                if(manager.GetCurrentSelectedWeapon().bulletType==WeaponBulletType.BULLET)
                {
                    manager.GetCurrentSelectedWeapon().ShootAnimation();
                    BulletFired();
             
                }
                else
                {
                    if (isAiming)
                    {
                        manager.GetCurrentSelectedWeapon().ShootAnimation();
                        if (manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                        {
                            ThrowArrowOrSpear(true);
                        }
                        else if(manager.GetCurrentSelectedWeapon().bulletType==WeaponBulletType.SPEAR)
                        {
                            ThrowArrowOrSpear(false);
                        }
                     
                    }
                }
               
            }
        }
    }


    void ZoomInAndOut()
    {
        if(manager.GetCurrentSelectedWeapon().aimType==WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomCamera.Play("ZoomIn");
                crossHair.SetActive(false);
            }

            if (Input.GetMouseButtonUp(1))
            {
                zoomCamera.Play("ZoomOut");
                crossHair.SetActive(true);
            }
        }


        if (manager.GetCurrentSelectedWeapon().aimType == WeaponAim.SELF_AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                manager.GetCurrentSelectedWeapon().Aim(true);
                isAiming = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                manager.GetCurrentSelectedWeapon().Aim(false);
                isAiming = false;
            }
        }
    }

    void ThrowArrowOrSpear(bool throwArrow)
    {
        if (throwArrow)
        {
            GameObject Arrow = Instantiate(arrowPrefab);
            Arrow.transform.position = bowArrowStartPosition.position;
            Arrow.GetComponent<BowAndArrow>().Launch(main);
        }
        else
        {
            GameObject Spear = Instantiate(spearPrefab);
            Spear.transform.position = bowArrowStartPosition.position;
            Spear.GetComponent<BowAndArrow>().Launch(main);
        }      
    }

    void BulletFired()
    {
        RaycastHit hit;
        if(Physics.Raycast(main.transform.position,main.transform.forward,out hit))
        {
            if(hit.transform.tag =="Enemy")
            {
                hit.transform.GetComponent<Health>().ApplyDamage(damage);
            }
        }
    }
}
