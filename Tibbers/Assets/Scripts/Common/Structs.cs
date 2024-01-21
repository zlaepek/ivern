using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Structs
{
    // Ŭ�󿡼� ������ �ѱ��
    /*
     * �������� Ŭ����� �ѱ�°�
     * - ���� ������
     * - ������(��ü, ����) ����
     *
     */
    
    public struct ExpData
    {
        public ulong ulRequire; // �ش� ���� �޼��� �ʿ��� �ʿ䷮
        public ulong ulTotal; // �ش� ���� �޼��� ������ ����ġ��
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
        public int iAttackCount; // ���� Ƚ��
        public float fAttackRange;
        public float fAttackDelay; // ���� �ӵ� (�ʴ���)
    }
    
    public struct BulletStat
    {
        public float fBulletDamage;
        public float fKnockbackForce;
        public float fMoveSpeed;
        // -1 �̸� �ִϸ��̼� ����� �ı�
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
