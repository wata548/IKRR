using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.Skill;
using Extension;
using UI;

namespace Data {
    public struct EntityAnimation {
        public readonly Positions Caster;
        public readonly ISkill Skill;

        public EntityAnimation(Positions pCaster, ISkill pSkill) =>
            (Caster, Skill) = (pCaster, pSkill);
    }
    
    public static class CharactersManager {
        
        //==================================================||Fields 
        private static Dictionary<Positions, IEntity> _entities = new();
        public static Player Player { get; private set; } = null;
        public static bool IsFighting { get; private set; } = false;
        //==================================================||Properties 
        public static Positions TargetEnemy { get; set; } = Positions.Middle;
        
        //==================================================||Methods 
        public static Queue<EntityAnimation> GetEnemySkills() {
            var skills = new Queue<EntityAnimation>();
            foreach (var position in Positions.AllEnemy.Split()) {
                if(!_entities.TryGetValue(position, out var target) || !target.IsAlive)
                    continue;
                
                skills.Enqueue(new(position, (target as Enemy)!.GetSkill()));
            }

            return skills;
        }
        
        public static void Init(Player pPlayer) {
            TargetEnemy = Positions.Middle;
            Player = pPlayer;
            _entities[Positions.Player] = Player;
        }

        public static void OnDeathEnemy(Positions pPosition) {
            
            Player.OnKill(_entities[pPosition]);
            
            IsFighting = Positions.AllEnemy.Split().Any(enemy => {
                if (!_entities.TryGetValue(enemy, out var target))
                    return false;
                return target.IsAlive;
            }); 
        }
        
        public static void SetEnemy(int pEnemy, Positions pPosition) {
            IsFighting = true;

            var enemy = new Enemy(pPosition, pEnemy);
            _entities[pPosition] = enemy;
            var ui = UIManager.Instance.Entity.GetEnemyUI(pPosition);
            ui.SetData(enemy);
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

        public static IEntity GetEntity(Positions pTarget) => _entities[pTarget];
        
        public static IEntity[] GetEntities(Positions pCaster, Positions pPosition) {
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

        public static void OnTurnStart(bool pIsPlayer) {
            var positions = pIsPlayer ? Positions.Player : Positions.AllEnemy;
            foreach (var entity in GetEntities(Positions.None, positions)) {
                entity.OnTurnStart();
            }
        }

        public static void OnTurnEnd(bool pIsPlayer) {
            var positions = pIsPlayer ? Positions.Player : Positions.AllEnemy;
            foreach (var entity in GetEntities(Positions.None, positions)) {
                entity.OnTurnEnd();
            }
        }

    }
}