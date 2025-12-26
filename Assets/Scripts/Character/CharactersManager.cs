using System.Collections.Generic;
using System.Linq;
using Character.Entity;
using Data;
using Extension;
using UnityEngine;

namespace Character {
    public class CharactersManager: MonoSingleton<CharactersManager> {
        protected override bool IsNarrowSingleton { get; set; } = true;
        private Dictionary<Positions, IEntity> _entities = new();

        public void Clear() => _entities.Clear();
        
        public void SetEntity(Positions pPoint, IEntity pEntity) {
            if (!_entities.TryAdd(pPoint, pEntity))
                _entities[pPoint] = pEntity;
        }
        
        public IEntity[] GetEntity(Positions pCaster, Positions pPosition) {
            
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