using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Entity
{
    public GameObject lootPrefab;

    private void Start()
    {
        _entityRender = GetComponent<SpriteRenderer>();
    }

    public override void Die()
    {
        base.Die();
        Instantiate(lootPrefab, transform.position, Quaternion.identity);
    }
}
