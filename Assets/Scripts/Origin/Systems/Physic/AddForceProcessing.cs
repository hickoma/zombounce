using Components;
using Components.Events;
using LeopotamGroup.Ecs;
using UnityEngine;

namespace Systems.Physic
{
    [EcsInject]
	public class AddForceProcessing : IEcsRunSystem
    {
        private EcsWorld _world;
		private Player m_Player = null;
        private EcsFilter<AddForceEvent> _addForceEventFilter;

        public void Run()
        {
            if (_addForceEventFilter.EntitiesCount > 0)
            {
                var forceVector = _addForceEventFilter.Components1[0].ForceVector;

				if (m_Player == null)
				{
					m_Player = GameEventsController.Instance.m_Player;
				}

                m_Player.Rigidbody.AddForce(forceVector);

                _world.RemoveEntity(_addForceEventFilter.Entities[0]);
            }
        }
    }
}