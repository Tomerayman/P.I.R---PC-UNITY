using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputWrapper : MonoBehaviour
{
    playerMovement controller;

    float walkInput = 0;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (walkInput != 0)
        {
            char direction = (walkInput > 0) ? 'r' : 'l';
            controller.move(direction);
        }
    }



    public void iMove(InputAction.CallbackContext context)
    {
        walkInput = context.ReadValue<float>();
    }

    public void iShoot(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            controller.shoot();
        }
    }

    public void iSlowTime(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            controller.slowTime(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            controller.slowTime(false);
        }
    }


    public void iReset(InputAction.CallbackContext context) // TODO: remove eventually.
    {
        if (context.phase == InputActionPhase.Started)
        {
            SceneManager.LoadScene("TomerScene");
        }
    }
}
