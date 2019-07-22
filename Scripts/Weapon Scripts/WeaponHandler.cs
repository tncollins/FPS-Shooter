using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponAim
{
    NONE,
    SELF_AIM,
    AIM
}

public enum WeaponFireType
{
    SINGLE,
    MULTIPLE
}

public enum WeaponBulletType
{
    BULLET,
    ARROW,
    SPEAR,
    NONE
}

public class WeaponHandler : MonoBehaviour
{
    private Animator animator;

    public WeaponAim aimType;

    [SerializeField]
    private GameObject muzzle;

    [SerializeField]
    private AudioSource shoot, reload;

    public WeaponFireType fireType;
    public WeaponBulletType bulletType;
    public GameObject attackPoint;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void ShootAnimation()
    {
        animator.SetTrigger("Shoot");
    }


    public void Aim(bool canAim)
    {
        animator.SetBool("Aim", canAim);
    }

    void TurnOnMuzzle()
    {
        muzzle.SetActive(true);
    }

    void TurnOffMuzzle()
    {
        muzzle.SetActive(false);
    }

    void PlayShootSound()
    {
        shoot.Play();
    }

    void PlayReloadSound()
    {
        reload.Play();
    }


    void TurnOnAttackPoint()
    {
        attackPoint.SetActive(true);
    }

    void TurnOffAttackPoint()
    {
        if(attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(true);
        } 
    }
}
