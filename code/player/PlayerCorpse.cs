using System.Collections.Generic;

using Sandbox;

namespace TTTReborn.Player
{
    public partial class PlayerCorpse : ModelEntity
    {
        public TTTPlayer Player { get; set; }
        public List<Particles> Ropes = new();
        public List<PhysicsJoint> RopeSprings = new();
        public string KillerWeapon { get; set; }
        public bool IsIdentified { get; set; } = false;
        public bool WasHeadshot { get; set; } = false;
        public bool Suicide { get; set; } = false;
        public float Distance { get; set; } = 0f;
        public float KilledTime { get; private set; }
        public string[] Perks { get; set; }

        public PlayerCorpse()
        {
            MoveType = MoveType.Physics;
            UsePhysicsCollision = true;

            SetInteractsAs(CollisionLayer.Debris);
            SetInteractsWith(CollisionLayer.WORLD_GEOMETRY);
            SetInteractsExclude(CollisionLayer.Player | CollisionLayer.Debris);

            KilledTime = Time.Now;
        }

        public void CopyFrom(TTTPlayer player)
        {
            Player = player;

            SetModel(player.GetModelName());
            TakeDecalsFrom(player);

            this.CopyBonesFrom(player);
            this.SetRagdollVelocityFrom(player);

            foreach (Entity child in player.Children)
            {
                if (child is ModelEntity e)
                {
                    string model = e.GetModelName();

                    if (model == null || !model.Contains("clothes"))
                    {
                        continue;
                    }

                    ModelEntity clothing = new ModelEntity();
                    clothing.SetModel(model);
                    clothing.SetParent(this, true);
                }
            }
        }

        public void ApplyForceToBone(Vector3 force, int forceBone)
        {
            PhysicsGroup.AddVelocity(force);

            if (forceBone < 0)
            {
                return;
            }

            PhysicsBody corpse = GetBonePhysicsBody(forceBone);

            if (corpse != null)
            {
                corpse.ApplyForce(force * 1000);
            }
            else
            {
                PhysicsGroup.AddVelocity(force);
            }
        }

        public void ClearAttachments()
        {
            foreach (Particles rope in Ropes)
            {
                rope.Destroy(true);
            }

            foreach (PhysicsJoint spring in RopeSprings)
            {
                spring.Remove();
            }

            Ropes.Clear();
            RopeSprings.Clear();
        }

        protected override void OnDestroy()
        {
            ClearAttachments();
        }

        public void CopyConfirmationData(ConfirmationData confirmationData)
        {
            IsIdentified = confirmationData.Identified;
            WasHeadshot = confirmationData.Headshot;
            KilledTime = confirmationData.Time;
            Distance = confirmationData.Distance;
            Suicide = confirmationData.Suicide;
        }

        public ConfirmationData GetConfirmationData()
        {
            return new ConfirmationData
            {
                Identified = IsIdentified,
                Headshot = WasHeadshot,
                Time = KilledTime,
                Distance = Distance,
                Suicide = Suicide
            };
        }
    }
}
