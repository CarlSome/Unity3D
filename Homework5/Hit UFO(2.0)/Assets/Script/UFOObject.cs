using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOObject : MonoBehaviour {

    private Vector3 size;
    private Color color;
    private float speed;
    private Vector3 direction;

    public void SetUFOSize(Vector3 _size)
    {
        size = _size;
    }

    public void SetUFOColor(Color _color)
    {
        color = _color;
    }

    public void SetUFOSpeed(float _speed)
    {
        speed = _speed;
    }

    public void SetUFODirection(Vector3 _direction)
    {
        direction = _direction;
    }

    public Vector3 GetUFOSize()
    {
        return size;
    }

    public Color GetUFOColor()
    {
        return color;
    }

    public float GetUFOSpeed()
    {
        return speed;
    }

    public Vector3 GetUFODirection()
    {
        return direction;
    }
}
