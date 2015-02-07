using Jangada;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Creatures
{
    public class Skill
    {
        public string Name;
        public int TextureId;
        public float CoolDown;
        public float Distance;
        public bool AutoCast;

        public Skill(SkillsDescription skillDesc)
        {
            this.Name = skillDesc.Name;
            this.TextureId = skillDesc.TextureId;
            this.CoolDown = skillDesc.CoolDown;
            this.Distance = skillDesc.Distance;
            this.AutoCast = skillDesc.AutoCast;
        }
    }
}
