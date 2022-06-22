﻿using System;
using System.Diagnostics;
using OpenGL.Mathematics;
using OpenGL.Platform;

namespace OpenGL.Game
{
    public class Camera
    {
        #region Properties

        public Transform Transform { get; protected set; }
        public float MoveStepDistance { get; set; } = 2;
        public float RotationStepAngle { get; set; } = 20;

        #endregion

        #region Constructor

        public Camera()
        {
            Transform = new Transform();
            Move(new Vector3(0, 0, 1));
            
            InitActions();
        }

        #endregion

        #region Private Methods

        private void Move(Vector3 toMove)
        {
            Transform.Position += toMove;
        }

        private void Rotate(Vector3 rotation)
        {
            Transform.Rotation += rotation;
        }

        private void InitActions()
        {
            CreateRepeatInput('w', MoveForward);
            CreateRepeatInput('s', MoveBack);
            CreateRepeatInput('a', MoveLeft);
            CreateRepeatInput('d', MoveRight);

            CreateRepeatInput('q', RotateLeft);
            CreateRepeatInput('e', RotateRight);
        }

        public static void CreateRepeatInput(char key, Event.RepeatEvent method)
        {
            Event @event = new Event(method);
            Input.Subscribe(key, @event);
        }

        #endregion

        #region Public Methods

        public void MoveForward(float dt)
        {
            Move(Transform.GetForward() * MoveStepDistance * dt);
        }

        public void MoveBack(float dt)
        {
            Move(Transform.GetForward() * -1  * MoveStepDistance * dt);
        }

        public void MoveLeft(float dt)
        {
            Move(Transform.GetRight() * -1 * MoveStepDistance * dt);
        }

        public void MoveRight(float dt)
        {
            Move(Transform.GetRight() * MoveStepDistance * dt);
        }

        public void RotateLeft(float dt)
        {
            Rotate(new Vector3(0f, RotationStepAngle, 0f) * dt);
        }

        public void RotateRight(float dt)
        {
            Rotate(new Vector3(0f, -RotationStepAngle, 0f) * dt);
        }

        public Matrix4 GetRts()
        {
            return Transform.GetRts();
        }

        #endregion
    }
}