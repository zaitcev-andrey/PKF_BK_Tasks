using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllValuesOfRotationAndPositionForCubeMaze
{
    // Значения поворота и позиции
    // 0) поворот: 0, 0, 0; позиция: 3, 22, 5
    // 1) поворот: -90, 0, 0; позиция: 3, 10, 7
    // 2) поворот: 0, 90, -90; позиция: -4, 17, 7
    // 3) поворот: 90, 0, -180; позиция: 2.9, 24, 7
    // 4) поворот: 0, -90, -270; позиция: 10, 17, 7
    // 5) поворот: 0, -90, -180; позиция: 10, 12, 12

    private static Vector3[] _rotations = 
    {
        new Vector3(0f, 0f, 0f),
        new Vector3(-90f, 0f, 0f),
        new Vector3(0f, 90f, -90f),
        new Vector3(90f, 0f, -180f),
        new Vector3(0f, -90f, -270f),
        new Vector3(0f, -90f, -180f)
    };
    private static Vector3[] _positions =
    {
        new Vector3(3f, 22f, 5f),
        new Vector3(3f, 10f, 7f),
        new Vector3(-4f, 17f, 7f),
        new Vector3(2.9f, 24f, 7f),
        new Vector3(10f, 17f, 7f),
        new Vector3(10f, 12f, 12f)
    };

    public static Vector3 GetRotation(int index)
    {
        return _rotations[index];
    }

    public static Vector3 GetPosition(int index)
    {
        return _positions[index];
    }
}
