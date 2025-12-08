public enum MinionRole
{
    Tank,
    Ranger,
    Healer
    // Group can add roles per game mechanics here
}

public enum BossCallout
{
    None,
    FocusPlayer,     // everyone attack player
    ProtectBoss,     // tanks body-block & healers heal boss
    SpreadOut,       // avoid AOE
    GroupUp          // cluster near boss
}

