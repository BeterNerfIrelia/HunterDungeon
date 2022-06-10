using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public EffectType effectType;
    public CountType countType;
    public bool hasEffect;
    public bool hasBeenApplied = false;

    public Effect(bool he, EffectType et, CountType ct)
    {
        effectType = et;
        hasEffect = he;
        countType = ct;
    }

    public Effect(Effect effect)
    {
        effectType = effect.effectType;
        countType = effect.countType;
        hasEffect = effect.hasEffect;
    }
}
