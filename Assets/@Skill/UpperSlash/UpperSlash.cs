using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperSlash : Skill
{
    public override void UseSkill(GameObject target)
    {
        Rigidbody2D targetRigid = target.GetComponent<Rigidbody2D>();

        targetRigid.velocity = Vector2.zero;
        targetRigid.AddForce(new Vector2(0f, knockbackForce), ForceMode2D.Impulse);

        target.GetComponent<Enemy>().TakeDamage(damage, duration);
    }
}
