using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Yuddham
{
    public class PlacableTroopController : MonoBehaviour
    {
        private NavMeshAgent[] _agents;
        
        private Vector3 targetPosition;

        private void Awake()
        {
            _agents = GetComponentsInChildren<NavMeshAgent>();
        }

        public void InPreviewMode(bool enabled)
        {
            //make it transparent
        }
        

        public void InitTroop(Vector3 position, Vector3 targetPosition)
        {
            transform.position = position;
            foreach (var agent in _agents)
            {
                agent.enabled = true;
                agent.SetDestination(targetPosition);
            }
        }
    }
}
