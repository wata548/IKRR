using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Skill;
using Extension;
using Roulette;

namespace Data {
    public struct EnemyAnimation {
        public readonly Positions Caster;
        public readonly ISkill Skill;

        public EnemyAnimation(Positions pCaster, ISkill pSkill) =>
            (Caster, Skill) = (pCaster, pSkill);
    }
    
    public static class CharactersManager {
        
        //==================================================||Fields 
        private static Dictionary<Positions, IEntity> _entities = new();
        public static Player Player = null;
        public static bool IsFighting = false;
        //==================================================||Properties 
        public static Positions TargetEnemy { get; set; } = Positions.Middle;
        
        //==================================================||Methods 
        public static Queue<EnemyAnimation> GetEnemySkills() {
            var skills = new Queue<EnemyAnimation>();
            foreach (var position in Positions.AllEnemy.Split()) {
                if(!_entities.TryGetValue(position, out var target) || !target.IsAlive)
                    continue;
                
                skills.Enqueue(new(position, (target as Enemy)!.GetSkill()));
            }

            return skills;
        }
        
        public static void Init() {
            TargetEnemy = Positions.Middle;
            Player = new Player();
            if (!_entities.TryAdd(Positions.Player, Player))
                _entities[Positions.Player] = Player;
        }

        public static void OnDeathEnemy() =>
            IsFighting = Positions.AllEnemy.Split().Any(enemy => {
                if (!_entities.TryGetValue(enemy, out var target))
                    return false;
                return target.IsAlive;
            }); 
        
        public static void SetEntity(Positions pPoint, IEntity pEntity) {
            IsFighting = true;
            
            if (!_entities.TryAdd(pPoint, pEntity))
                _entities[pPoint] = pEntity;
        }

        public static Positions TargetUpdate() {
            if (_entities.TryGetValue(TargetEnemy, out var entity) && entity.IsAlive)
                return TargetEnemy;

            foreach (var position in  Positions.AllEnemy.Split()) {
                if(!_entities.TryGetValue(position, out var newTarget) || !newTarget.IsAlive)
                    continue;

                return TargetEnemy = position;
            }

            throw new NullReferenceException("Alive enemy isn't exist");
        }
        
        public static IEntity[] GetEntity(Positions pCaster, Positions pPosition) {
            if (pPosition.HasFlag(Positions.Caster)) {
                pPosition ^= Positions.Caster;
                pPosition |= pCaster;
            }
            if (pPosition.HasFlag(Positions.Target)) {
                pPosition ^= Positions.Target;
                pPosition |= TargetEnemy;
            }

            return pPosition
                .Split()
                .Select(position => _entities.GetValueOrDefault(position))
                .Where(entity => entity!= null)
                .ToArray();
        }

    }
}