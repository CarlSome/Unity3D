using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction {

    void MoveForward();
    void MoveBackward();
    void Turn(float offsetX);
    void Shoot();
    bool IsGameOver();
}
