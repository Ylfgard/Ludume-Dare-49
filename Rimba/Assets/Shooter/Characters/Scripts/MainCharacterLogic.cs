using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterLogic : BaseCharacterLogic
{
    private void Update()
    {
        Move();
        Aim(MouseWorld.GetPosition());
        if (Input.GetMouseButtonDown(0)) Shoot();
    }
}
