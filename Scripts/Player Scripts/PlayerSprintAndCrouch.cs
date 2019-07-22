using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public float sprint_Speed = 10f;
    public float move_Speed = 5f;
    public float crouch_Speed = 2f;

    private Transform look_Rotation;
    private float stand_Height = 1.6f;
    private float crouch_Height = 1f;
    private bool is_Crouching;

    private PlayerFootsteps player_footsteps;

    private float sprint_Volume = 1f;
    private float crouch_Volume = 0.1f;
    private float walk_Volume_Min = 0.2f, walk_Volume_Max = 0.6f;
    private float walk_step_distance = 0.4f;
    private float sprint_step_distance = 0.25f;
    private float crouch_step_distance = 0.5f;

    private PlayerStats stats;
    private float sprint = 100f;
    private float sprintThreshold = 10;
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        look_Rotation = transform.GetChild(0);

        player_footsteps = GetComponentInChildren<PlayerFootsteps>();
        stats = GetComponent<PlayerStats>();
    }

    void Start()
    {
        player_footsteps.volume_Min = walk_Volume_Min;
        player_footsteps.volume_Max = walk_Volume_Max;
        player_footsteps.step_Distance = walk_step_distance;
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {
        if(sprint > 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !is_Crouching)
            {
                playerMovement.speed = sprint_Speed;
                player_footsteps.step_Distance = sprint_step_distance;
                player_footsteps.volume_Min = sprint_Volume;
                player_footsteps.volume_Max = sprint_Volume;
            }
        }

        if(Input.GetKeyUp(KeyCode.LeftShift)&&!is_Crouching)
        {
            playerMovement.speed = move_Speed;
            player_footsteps.step_Distance = walk_step_distance;
            player_footsteps.volume_Min = walk_Volume_Min;
            player_footsteps.volume_Max = walk_Volume_Max;
        }

        if(Input.GetKey(KeyCode.LeftShift)&&!is_Crouching)
        {
            sprint -= sprintThreshold * Time.deltaTime;

            if(sprint<=0f)
            {
                sprint = 0f;

                playerMovement.speed = move_Speed;

                player_footsteps.step_Distance = walk_step_distance;
                player_footsteps.volume_Min = walk_Volume_Min;
                player_footsteps.volume_Max = walk_Volume_Max;
            }
            stats.DisplayStaminaStats(sprint);
        }
        else
        {
            if(sprint!=100f)
            {
                sprint += (sprintThreshold / 2f) * Time.deltaTime;
                stats.DisplayStaminaStats(sprint);

                if(sprint > 100f)
                {
                    sprint = 100f;
                }
            }
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //if crouching,stand up
            if (is_Crouching)
            {
                look_Rotation.localPosition = new Vector3(0f, stand_Height, 0f);
                playerMovement.speed = move_Speed;

                player_footsteps.step_Distance = walk_step_distance;
                player_footsteps.volume_Min = walk_Volume_Min;
                player_footsteps.volume_Max = walk_Volume_Max;

                is_Crouching = false;
            }
            else
            {
                look_Rotation.localPosition = new Vector3(0f, crouch_Height, 0f);
                playerMovement.speed = crouch_Speed;

                player_footsteps.step_Distance = crouch_step_distance;
                player_footsteps.volume_Min = crouch_Volume;
                player_footsteps.volume_Max = crouch_Volume;

                is_Crouching = true;
            }
        }
    }
}
