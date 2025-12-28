using React;
using System;

[Serializable]
public class UpgradeData
{
    public readonly ReactValue<float> UpgradeDamageLevel = new ReactValue<float>();
    public readonly ReactValue<float> UpgradeShootSpeedLevel = new ReactValue<float>();
    public readonly ReactValue<float> UpgradeHealthLevel = new ReactValue<float>();
}
