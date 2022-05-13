using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG
{
    public class PlayerMovementBehvaior : MonoBehaviour
    {
        Transform _cameraObject;
        InputHandlerBehavior inputHandler;
        Vector3 _moveDirection;

        [HideInInspector]
        public Transform _myTransform;

        public new Rigidbody rigidbody;
        public GameObject _normalCamera;

        [Header("Stats")]
        [SerializeField]
        float _movementSpeed = 5;
        [SerializeField]
        float _rotationSpeed = 10;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandlerBehavior>();
            _cameraObject = Camera.main.transform;
            _myTransform = transform;
        }

        public void Update()
        {
            float delta = Time.deltaTime;

            inputHandler.TickInput(delta);

            _moveDirection = _cameraObject.forward * inputHandler._vertical;
            _moveDirection += _cameraObject.right * inputHandler._horizontal;
            _moveDirection.Normalize();

            float speed = _movementSpeed;
            _moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(_moveDirection, _normalVector);
            rigidbody.velocity = projectedVelocity;
        }

        #region Movement
        Vector3 _normalVector;
        Vector3 _targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler._moveAmount;

            targetDir = _cameraObject.forward * inputHandler._vertical;
            targetDir += _cameraObject.right * inputHandler._horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
                targetDir = _myTransform.forward;

            float rs = _rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(_myTransform.rotation, tr, rs * delta);

            _myTransform.rotation = targetRotation;
        }


        #endregion

    }
}

