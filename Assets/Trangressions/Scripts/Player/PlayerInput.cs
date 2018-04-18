using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;
    PlayerWeaponSystem wSystem;

    void Start()
    {
        player = GetComponent<Player>();
        wSystem = GetComponent<PlayerWeaponSystem>();
    }

    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.OnJumpInputDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.OnJumpInputUp();
        }
        if (Input.GetMouseButtonDown(1))
        {
            //Right mouse input
            wSystem.RightWeaponInput();
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Left mouse input
            wSystem.LeftWeaponInput();
        }
    }
}