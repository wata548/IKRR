using System.Collections.Generic;
using System.Linq;
using Character.Entity;
using Data;
using Extension;
using UnityEngine;

namespace Character {
    public static class CharactersManager {
        
        private static Dictionary<Positions, IEntity> _entities = new();

        public static void Clear() => _entities.Clear();
        
        public static void SetEntity(Positions pPoint, IEntity pEntity) {
            if (!_entities.TryAdd(pPoint, pEntity))
                _entities[pPoint] = pEntity;
        }
        
        public static IEntity[] GetEntity(Positions pCaster, Positions pPosition) {
            
            if ((pPosition & Positions.Caster) != Positions.None) {
                pPosition ^= Positions.Caster;
                pPosition |= pCaster;
            }

            return pPosition
                .Split()
                .Select(position => _entities.GetValueOrDefault(position))
                .Where(entity => entity!= null)
                .ToArray();
        }

    }
}