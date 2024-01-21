using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Structs
{
    // 클라에서 서버로 넘길것
    /*
     * 스테이지 클리어시 넘기는것
     * - 얻은 보물들
     * - 경험지(전체, 현재) 레벨
     *
     */
    
    public struct ExpData
    {
        public ulong ulRequire; // 해당 레벨 달성시 필요한 필요량
        public ulong ulTotal; // 해당 레벨 달성시 누적된 경험치량
    }

    public struct PlayerGameData
    {
        public ulong ulExp_Current;
        public ulong ulExp_Total;

        public ulong ulStageKill;

        public ulong ulTotalKill;

        public int iLevel;    
    }

    public struct WeaponStat
    {
        // Attack Speed (sec)
        public float fAttackSpeed; 
        public int iAttackCount; // 공격 횟수
        public float fAttackRange;
        public float fAttackDelay; // 공격 속도 (초단위)
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
        // %
        public float fAttackSpeed_Buf;
        public float fAttackSpeed_DeBuf;
    }

    //class Monster
    //{

    //}

    //class PlayableCharacter
    //{
    //    template<t> m_WeaponSlot;
    //}
}
