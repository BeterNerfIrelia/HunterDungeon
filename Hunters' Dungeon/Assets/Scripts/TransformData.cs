using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformData
{
    public int id;
    public string name;
    public string description;
    public string imageName;
    public int damage;
    public Effect effect;
    public bool isTransform;
    public bool isImmune;

    public TransformData() { }

    public TransformData(int id, string name, string description, string imageName, int damage, Effect effect, bool isTransform, bool isImmune)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.imageName = imageName;
        this.damage = damage;
        this.effect = new Effect(effect);
        this.isTransform = isTransform;
        this.isImmune = isImmune;
    }

    public TransformData(TransformData tData)
    {
        id = tData.id;
        name = tData.name;
        description = tData.description;
        imageName = tData.imageName;
        damage = tData.damage;
        effect = new Effect(tData.effect);
        isTransform = tData.isTransform;
        isImmune = tData.isImmune;
    }
}
