using Jangada;
using JangadaWinClient.Content;
using JangadaWinClient.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Creatures
{
    public class Creature
    {
        #region Atributes
        Model model;
        public int ModelId;
        public SmallPortrait smallPortrait;
        public string Guid;
        public Vector3 position = new Vector3(8, 1, -3);
        public Quaternion rotation = Quaternion.Identity;
        public Matrix worldMatrix = Matrix.Identity;
        public Stats Stats = new Stats();
        public int health;
        public int mana;
        #endregion

        #region Constructors
        public Creature(Model _model)
        {
            model = _model;
            this.health = 75;
            this.mana = 80;
            this.smallPortrait = new SmallPortrait(TexturesHolder.BG_SMALL_PORTRAIT, TexturesHolder.BG_BARS, this);
        }

        public Creature(Model _model, PlayerDescription playerDesc)
        {
            model = _model;
            this.smallPortrait = new SmallPortrait(TexturesHolder.BG_SMALL_PORTRAIT, TexturesHolder.BG_BARS, this);
            SetDescription(playerDesc);
        }

        public Creature(CreatureDescription creatureDesc)
        {
            this.smallPortrait = new SmallPortrait(TexturesHolder.BG_SMALL_PORTRAIT, TexturesHolder.BG_BARS, this);
            SetDescription(creatureDesc);
        }
        #endregion
        


        #region Setters
        private void SetAll(string guid, Position position, QuaternionMessage rotation, bool hasStats, StatsDescription stats)
        {
            this.Guid = guid;
            this.position = Util.fromPosition(position);
            this.rotation = Util.fromQuaternionMessage(rotation);
            if (hasStats)
            {
                this.health = stats.HP;
                this.mana = stats.MP;
                this.Stats.CONS = stats.CONS;
                this.Stats.WIS = stats.WIS;
                this.Stats.INT = stats.INT;
                this.Stats.DEX = stats.DEX;
                this.Stats.STR = stats.STR;
            }
            else
            {
                this.health = 75;
                this.mana = 80;
            }
        }


        public void SetDescription(CreatureDescription creatureDesc)
        {
            this.SetAll(creatureDesc.CreatureGuid, creatureDesc.CreaturePosition, creatureDesc.CreatureRotation,
                creatureDesc.HasStats, creatureDesc.Stats);
            this.ModelId = creatureDesc.ModelId;
            this.model = Util.getModel(this.ModelId);
        }

        public void SetDescription(PlayerDescription playerDesc)
        {
            this.SetAll(playerDesc.PlayerGuid, playerDesc.PlayerPosition, playerDesc.PlayerRotation, 
                playerDesc.HasStats, playerDesc.Stats);
        }
        #endregion


        #region Getters
        public int GetMaxHealth()
        {
            return (this.Stats.CONS * 100);
        }

        public int GetMaxMana()
        {
            return (this.Stats.INT * 100);
        }

        public int GetHPPct()
        {
            return GetHPPct(100);
        }

        public int GetHPPct(int valueMax)
        {
            return (health * valueMax) / GetMaxHealth();
        }

        public int GetMPPct()
        {
            return GetMPPct(100);
        }

        public int GetMPPct(int valueMax)
        {
            return (mana * valueMax) / GetMaxMana();
        }
        #endregion

        #region Utils
        public List<BoundingSphere> GetSpheres()
        {
            List<BoundingSphere> bs = new List<BoundingSphere>();
            foreach (ModelMesh mesh in model.Meshes)
            {
                bs.Add(mesh.BoundingSphere);
            }
            return bs;
        }

        public void RecreateWorld()
        {
            worldMatrix = Matrix.CreateRotationY(MathHelper.Pi)
                * Matrix.CreateFromQuaternion(rotation)
                * Matrix.CreateTranslation(position);
        }

        public void Draw(NewCamera camera)
        {
            RecreateWorld();
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    if (Util.getWorld().IsSelected(this))
                    {
                        effect.AmbientLightColor = Color.YellowGreen.ToVector3();
                    }
                    effect.World = transforms[mesh.ParentBone.Index]
                        * worldMatrix;
                    effect.View = camera.viewMatrix;
                    effect.Projection = camera.projectionMatrix;
                }
                mesh.Draw();
            }
        }

        public void MoveForward(float speed)
        {
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, 1), this.rotation);
            this.position += addVector * speed;
        }

        public void MoveBackward(float speed)
        {
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -1), this.rotation);
            this.position += addVector * speed;
        }

        public void Yaw(float amount)
        {
            this.rotation *= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(amount), 0, 0);
        }

        public void ChangeBoneTransform(int boneIndex, Matrix t)
        {
            model.Bones[boneIndex].Transform = t * model.Bones[boneIndex].Transform;
        }

        public float? Intersects(Ray cursorRay)
        {
            float? lowValue = null;
            foreach (BoundingSphere sphere in GetSpheres())
            {
                BoundingSphere nSphere = new BoundingSphere(sphere.Center + position, sphere.Radius);
                float? curValue = cursorRay.Intersects(nSphere);
                if (curValue.HasValue && (!lowValue.HasValue || lowValue.Value > curValue))
                {
                    lowValue = curValue;
                }
            }
            return lowValue;
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Creature p = (Creature)obj;
            return (Guid.Equals(p.Guid));
        }
        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
        #endregion
    }
}
