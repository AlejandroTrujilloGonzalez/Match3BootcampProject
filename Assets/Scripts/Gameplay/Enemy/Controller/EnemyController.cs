using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController
{
    private EnemyModel model;

    public EnemyController(string name, int life)
    {
        model = new EnemyModel(name, life);
    }

    public string GetName()
    {
        return model.Name;
    }

    public int GetLife()
    {
        return model.Life;
    }

    public void SetLife(int life)
    {
        model.Life = life;
    }

    public void TakeDamage(int damage)
    {
        model.Life -= damage;
    }
}
