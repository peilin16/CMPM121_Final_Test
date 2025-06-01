using System.Collections;  // <== THIS is required for IEnumerator
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


public abstract class ModifierSpell
{
    public string name;
    public string description;
    public bool isActive = true;
    public bool one_time = false;
    public ModifierSpell() { }
    public ModifierSpell(JObject obj) {
        name = obj["name"]?.ToString();
        description = obj["description"]?.ToString();
    }
    
    public virtual Spell Cast(Spell spell) { return spell; }//Ͷ��ʱִ�еķ���
    public virtual Spell OnHit(Spell spell, Controller other) { return spell; }// ������е���ʱִ�еķ��� 
    public virtual Spell OnHitToWall(Spell spell, Controller other) { return spell; }// �������ǽ��ʱִ�еķ��� 
    public virtual Spell Application(Spell spell) { return spell; } //����Ҫʱִ��һ�εķ������������һ����ֵ
    public virtual IEnumerator CastWithCoroutine(Spell spell)//Ͷ��ʱִ�е�Э��
    {
        yield break; 
    }
    public virtual IEnumerator OnHitWithCoroutine(Spell spell, Controller other)//������е���ʱִ�еĵ�Э��
    {
        yield break;
    }

}