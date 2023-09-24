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
        // -1 이면 애니메이션 종료시 파괴
        // == -1 End Anim
        public float fLifeTime;
        public uint  nPierce;
    }

    public struct UnitStat
    {
        public float fHp_Base;
        public float fHp_Max;
        public float fHp_Cur;

        public float fMoveSpeed_Base;
        public float fMoveSpeed_Buf;
        public float fMoveSpeed_DeBuf;

        public float fMass_Base;
        public float fDamage_Base;
        public float fAttackSpeed_Base;

    }

    //class Monster
    //{

    //}

    //class PlayableCharacter
    //{
    //    template<t> m_WeaponSlot;
    //}
}
