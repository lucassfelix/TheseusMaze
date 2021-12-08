using UnityEngine;
public class WalkUp :  ICommand
{
    public Vector2 Execute()
    {
        return Vector2.up;
    }
}

public class WalkDown : ICommand
{
    public Vector2 Execute()
    {
        return Vector2.down;
    }
}

public class WalkLeft : ICommand
{
    public Vector2 Execute()
    {
        return Vector2.left;
    }
}

public class WalkRight : ICommand
{
    public Vector2 Execute()
    {
        return Vector2.right;
    }
}

public class Wait : ICommand
{
    public Vector2 Execute()
    {
        return Vector2.zero;
    }
}