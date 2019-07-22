using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private WeaponHandler[] weapons;

    private int currentWeaponIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponIndex = 0;
        weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) //select Axe
        {
            SwitchWeapons(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) //select revolver
        {
            SwitchWeapons(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //select shotgun
        {
            SwitchWeapons(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) //select assault rifle
        {
            SwitchWeapons(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) //select spear
        {
            SwitchWeapons(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)) //select bow
        {
            SwitchWeapons(5);
        }

    }

    void SwitchWeapons(int weaponIndex) //makes the current weapon inactive and activates the new weapon
    {
        if (currentWeaponIndex == weaponIndex)
            return;

        weapons[currentWeaponIndex].gameObject.SetActive(false);
        weapons[weaponIndex].gameObject.SetActive(true);
        currentWeaponIndex = weaponIndex;
    }

    public WeaponHandler GetCurrentSelectedWeapon()
    {
        return weapons[currentWeaponIndex];
    }
}
