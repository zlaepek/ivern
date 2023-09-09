using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Structs
{
    public struct WeaponStat
    {
        // Attack Speed (sec)
        public float fAttackSpeed; 
        public int iAttackCount;
        public float fAttackRange;
        public float fAttackDelay;
    }
    
    public struct BulletStat
    {
        public float fBulletDamage;
        public float fKnockbackForce;
        public float fMoveSpeed;
    }

    public struct UnitStat
    {
        public float fBaseHp;
        public float fMaxHp;
        public float fCurrentHp;

        public float fBaseMoveSpeed;
        public float fBaseMass;
        public float fBaseDamage;
        public float fBaseAttackSpeed;
    }

    //class Monster
    //{

    //}

    //class PlayableCharacter
    //{
    //    template<t> m_WeaponSlot;
    //}
}
