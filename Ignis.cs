using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class Ignis : BaseCreature
    {
        [Constructable]
        public Ignis() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Ignis";
            Body = 89;
            BaseSoundID = 362;

            SetStr(750);
            SetDex(150);
           .SetInt(500);

            SetHits(10000);
            SetStam(1000);
            SetMana(5000);

            SetDamage(40, 50);

            SetDamageType(ResistanceType.Fire, 75);
            SetDamageType(ResistanceType.Physical, 25);

            SetResistance(ResistanceType.Physical, 90);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 30);
            SetResistance(ResistanceType.Poison, 20);
            SetResistance(ResistanceType.Energy, 40);

            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Research, 100.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);

            Fame = 25000;
            Karma = -25000;

            PackGold(5000, 10000);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (from != null && from.Alive && Utility.RandomDouble() < 0.2)
            {
                IgnisBreath breath = new IgnisBreath(from.Location, this.Map);
                breath.MoveToWorld(new Point3D(from.X + Utility.RandomMinMax(-10, 10), from.Y + Utility.RandomMinMax(-10, 10), from.Z), this.Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Gems, 2);
            AddLoot(LootPack.LootItem(new DragonHeart(), 1));
            AddLoot(LootPack.LootItem(new FireScales(), 5));
        }

        public Ignis(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class IgnisBreath : Item
    {
        private Mobile m_Caster;

        [Constructable]
        public IgnisBreath(Point3D loc, Map map) : base(0x26AC)
        {
            Movable = false;
            Hue = 1157;

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(BreathEffect));
        }

        private void BreathEffect(object state)
        {
            if (Deleted || Map == null || m_Caster == null || !m_Caster.Alive)
                return;

            Effects.SendLocationEffect(Location, Map, 0x3709, 16, 2, EffectLayer.Waist);
            PlaySound(0x658);

            TimeSpan duration = TimeSpan.FromSeconds(1.0);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m == null || m == m_Caster || !CanBeHarmful(m) || m.IsDeadBondedPet)
                    continue;

                m.Combatant = m_Caster;
                DoFireDamage(m, Utility.RandomMinMax(50, 100));
                ApplyPoison(m, Poison.Lethal);
            }

            Delete();
        }

        public IgnisBreath(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.WriteMobile(m_Caster);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Caster = reader.ReadMobile();
        }
    }

    [FlipableAttribute(0x1E36, 0x1E37)]
    public class DragonHeart : Item
    {
        [Constructable]
        public DragonHeart() : base(0x1E36)
        {
            Name = "dragon heart";
            Weight = 1.0;
        }

        public DragonHeart(Serial serial) : base(serial)
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

    [FlipableAttribute(0x1F8B, 0x1F8C)]
    public class FireScales : Item
    {
        [Constructable]
        public FireScales() : base(0x1F8B)
        {
            Name = "fire scales";
            Weight = 0.1;
        }

        public FireScales(Serial serial) : base(serial)
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