using System;
using Arixen.ScriptSmith;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Yuddham
{
    public class TurtleAgent : Agent
    {
        [SerializeField] private Transform _goal;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _turnSpeed;

        private Renderer _renderer;
        
        private int _currentEpisode = 0;
        private float _cumulativeReward = 0f;
        
        public override void Initialize()
        {
            _renderer = GetComponent<Renderer>();
            _cumulativeReward = 0f;
            _currentEpisode = 0;
        }

        public override void OnEpisodeBegin()
        {
            LoggerService.Debug("OnEpisodeBegin" + _currentEpisode);
            _currentEpisode++;
            _cumulativeReward = 0f;
            _renderer.material.color = Color.blue;

            SpawnObjects();
        }

        private void SpawnObjects()
        {
            transform.localPosition = new Vector3(0,.15f,0);
            transform.rotation = Quaternion.identity;
            
            float randomAngle = Random.Range(0f, 360f);
            Vector3 randDirection = Quaternion.Euler(0,randomAngle,0) * Vector3.forward;

            float randomDistance = Random.Range(1f, 2.5f);

            Vector3 goalPosition = transform.localPosition + randDirection * randomDistance;

            _goal.localPosition = new Vector3(goalPosition.x,0.3f,goalPosition.z);
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            float goalPosX_norm = _goal.localPosition.x/5f;
            float goalPosZ_norm = _goal.localPosition.z/5f;
            
            float turtlePosX_norm = transform.localPosition.x/5f;
            float turtlePosZ_norm = transform.localPosition.z/5f;
            
            float tutleRot_norm = (transform.localRotation.eulerAngles.y/360f) *2 -1;

            sensor.AddObservation(goalPosX_norm);
            sensor.AddObservation(goalPosZ_norm);
            sensor.AddObservation(turtlePosX_norm);
            sensor.AddObservation(turtlePosZ_norm);
            sensor.AddObservation(tutleRot_norm);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            MoveAgent(actions.DiscreteActions);
            
            AddReward(-2f/MaxStep);

            _cumulativeReward = GetCumulativeReward();
        }

        private void MoveAgent(ActionSegment<int> actionsDiscreteActions)
        {
            var action = actionsDiscreteActions[0];
            switch (action)
            {
                case 1:     //Move forward
                    transform.position += transform.forward * _moveSpeed * Time.deltaTime;
                    break;
                case 2:     //Rotate left
                    transform.Rotate(0,-_turnSpeed * Time.deltaTime,0f);
                    break;
                case 3:     //Rotate right
                    transform.Rotate(0,_turnSpeed*Time.deltaTime,0);
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 31)   //Goal layer
            {
                GoalReached();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.layer == 6)     //Wall layer
            {
                if (_renderer != null)
                {
                    _renderer.material.color = Color.red;
                }
                AddReward(-.05f);
            }
        }

        private void OnCollisionStay(Collision other)
        {
            if(other.gameObject.layer == 6)     //Wall layer
            {
                AddReward(-.01f* Time.fixedDeltaTime);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.layer == 6) //Wall layer
            {
                if (_renderer != null)
                {
                    _renderer.material.color = Color.blue;
                }
            }
        }

        private void GoalReached()
        {
            LoggerService.Debug("Goal Reached - episode"+_currentEpisode);
            AddReward(1.0f);
            _cumulativeReward = GetCumulativeReward();
            
            EndEpisode();
        }
    }
}
