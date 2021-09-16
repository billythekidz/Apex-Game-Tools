namespace Apex.Demo.RTS.AI
{
    using Apex.AI;
    using Serialization;
    using UnityEngine;

    public sealed class UnitAttack : ActionBase
    {
        [ApexSerialization(defaultValue = true), MemberDependency("useTemporaryTarget", false)]
        public bool useAttackTarget = true;

        [ApexSerialization(defaultValue = false), MemberDependency("useAttackTarget", false)]
        public bool useTemporaryTarget;

        public override void Execute(IAIContext context)
        {
            var c = (UnitContext)context;
            if (this.useAttackTarget)
            {
                c.unit.Attack(c.attackTarget);
            }
            else if (this.useTemporaryTarget)
            {
                c.unit.Attack(c.temporaryTarget);
            }
            else
            {
                Debug.LogWarning(this.ToString() + " has had no target type set, so no attack is executed");
            }
        }
    }
}