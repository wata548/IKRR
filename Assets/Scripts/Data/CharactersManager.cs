using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Extension;

namespace Data {
    public static class CharactersManager {
        
        private static Dictionary<Positions, IEntity> _entities = new();
        private static Positions _targetEnemy = Positions.Middle;
        public static Player Player = null;

        public static void Init() {
            _targetEnemy = Positions.Middle;
            Player = new Player();
            if (!_entities.TryAdd(Positions.Player, Player))
                _entities[Positions.Player] = Player;
        }
        
        public static void SetEntity(Positions pPoint, IEntity pEntity) {
            if (!_entities.TryAdd(pPoint, pEntity))
                _entities[pPoint] = pEntity;
        }

        public static Positions TargetUpdate() {
            if (_entities.TryGetValue(_targetEnemy, out var entity) && entity.IsAlive)
                return _targetEnemy;

            foreach (var position in  Positions.AllEnemy.Split()) {
                if(!_entities.TryGetValue(position, out var newTarget) || !newTarget.IsAlive)
                    continue;

                return _targetEnemy = position;
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
                pPosition |= _targetEnemy;
            }

            return pPosition
                .Split()
                .Select(position => _entities.GetValueOrDefault(position))
                .Where(entity => entity!= null)
                .ToArray();
        }

    }
}