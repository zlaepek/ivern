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

        public ulong ulCoin_Current;
        public ulong ulCoin_Total;

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
        public uint nPierce;

        public Vector3 vScale;
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

    public struct OutGameStatData_Value
    {
        // 한글 이름	Name	step
        //공격력 Damage  5
        public float fDamage;
        //체력 Health  5
        public float fHealth;
        //공격 횟수   Attack_Count 	2
        public int iAttack_Count;
        //공격 속도 - 빈도 Attack_Speed    5
        public float fAttack_Speed;
        //이동 속도   Move_Speed 	5
        public float fMove_Speed;
        //투사체 속도  Projectile_Speed 	5
        public float fProjectile_Speed;
        //투사체 크기  Projectile_Scale 	2
        public float fProjectile_Scale;
    }

    public struct OutGameStatData
    {
        // 한글 이름	Name	step
        //공격력 Damage  5
        public int iDamage;
        //체력 Health  5
        public int iHealth;
        //공격 횟수   Attack_Count 	2
        public int iAttack_Count;
        //공격 속도 - 빈도 Attack_Speed    5
        public int iAttack_Speed;
        //이동 속도   Move_Speed 	5
        public int iMove_Speed;
        //투사체 속도  Projectile_Speed 	5
        public int iProjectile_Speed;
        //투사체 크기  Projectile_Scale 	2
        public int iProjectile_Scale;

    }

    public struct BuffData
    {
        // 버프 타입
        public int iType;
        
        // 남은 시간
        public float fRemain_Time;

        // 활성화 여부
        public bool isActive;

        // 중첩 횟수?
        //public int iOverlap;
    }
    //class Monster
    //{

    //}

    //class PlayableCharacter
    //{
    //    template<t> m_WeaponSlot;
    //}
}
