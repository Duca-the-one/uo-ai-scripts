using Server;
using Server.Items;
using Server.Mobiles;

namespace ServUO
{
    public class Canevar : BaseCreature
    {
        [Constructable]
        public Canevar() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Canevar";
            Body = 318;
            Hue = Server.Misc.MaterialInfo.GetMaterialColor("green", -1, 0);

            SetStr(500);
            SetDex(150);
           .SetInt(200);

            SetHits(700);
            SetStam(400);
            SetMana(300);

            SetDamage(20, 30);

            SetResistance(ResistanceType.Physical, 60);
            SetResistance(ResistanceType.Fire, 40);
            SetResistance(ResistanceType.Cold, 50);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 70);

            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 50;
        }

        public override void OnGaveMeleeAttack(Mobile attacker)
        {
            base.OnGaveMeleeAttack(attacker);

            if (Utility.RandomDouble() < 0.1) // 10% chance
            {
                Poison thePoison = Poison.Lethal;
                attacker.ApplyPoison(this, thePoison);
            }
        }

        public Canevar(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}