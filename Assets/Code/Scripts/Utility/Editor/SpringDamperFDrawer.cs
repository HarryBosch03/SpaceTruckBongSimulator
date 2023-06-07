using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceTruckBongSimulator.Utility.Editor
{
    [CustomPropertyDrawer(typeof(SpringDamperF))]
    public sealed class SpringDamperFDrawer : PropertyDrawer
    {
        private const float GraphHeight = 80.0f;

        private Material material;
        
        public bool Foldout
        {
            get => EditorPrefs.GetBool($"{GetType().FullName}.Foldout", false);
            set => EditorPrefs.SetBool($"{GetType().FullName}.Foldout", value);
        }
        public float InitialPosition
        {
            get => EditorPrefs.GetFloat($"{GetType().FullName}.InitialPosition", 0.0f);
            set => EditorPrefs.SetFloat($"{GetType().FullName}.InitialPosition", value);
        }
        public float InitialVelocity
        {
            get => EditorPrefs.GetFloat($"{GetType().FullName}.InitialVelocity", 0.0f);
            set => EditorPrefs.SetFloat($"{GetType().FullName}.InitialVelocity", value);
        }
        public float SimulationTarget
        {
            get => EditorPrefs.GetFloat($"{GetType().FullName}.SimulationTarget", 0.0f);
            set => EditorPrefs.SetFloat($"{GetType().FullName}.SimulationTarget", value);
        }
        public float SimulationTime
        {
            get => EditorPrefs.GetFloat($"{GetType().FullName}.SimulationTime", 0.0f);
            set => EditorPrefs.SetFloat($"{GetType().FullName}.SimulationTime", value);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var l = EditorGUIUtility.singleLineHeight + 2;
            var res = EditorGUI.GetPropertyHeight(property) + l;
            if (Foldout) res += GraphHeight + 2 + l * 4;
            return res;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);

            if (!material) material = new Material(Shader.Find("Hidden/Internal-Colored"));
            
            var l = EditorGUIUtility.singleLineHeight;
            position.y += EditorGUI.GetPropertyHeight(property);
            position.height = l;
            Rect next()
            {
                var res = position;
                position.y += res.height + 2;
                return res;
            }
            
            Foldout = EditorGUI.Foldout(next(), Foldout, property.displayName + " [Simulator]", true);
            
            if (!Foldout) return;
            position.height = GraphHeight;
            DrawSim(next(), property);
            position.height = l;

            InitialPosition = EditorGUI.FloatField(next(), "Initial Position", InitialPosition);
            InitialVelocity = EditorGUI.FloatField(next(), "Initial Velocity", InitialVelocity);
            SimulationTarget = EditorGUI.FloatField(next(), "Simulation Target", SimulationTarget);
            SimulationTime = EditorGUI.FloatField(next(), "Simulation Time (s)", SimulationTime);
        }

        private void DrawSim(Rect rect, SerializedProperty property)
        {
            var points = new List<Vector2>();
            var position = InitialPosition;
            var velocity = InitialVelocity;
            var dt = Time.fixedDeltaTime;

            var spring = property.FindPropertyRelative("spring").floatValue;
            var damper = property.FindPropertyRelative("damper").floatValue;

            var min = 0.0f;
            var max = 0.0f;
            
            for (var t = 0.0f; t < SimulationTime; t += dt)
            {
                var force = (SimulationTarget - position) * spring - velocity * damper;
                
                position += velocity * dt;
                velocity += force * dt;
                points.Add(new Vector2(t / SimulationTime, position));

                min = Mathf.Min(min, position);
                max = Mathf.Max(max, position);
            }

            var range = max - min;
            min -= range * 0.1f;
            max += range * 0.1f;
            
            Vector2 guiToGridSpace(Vector2 p)
            {
                return new Vector2
                {
                    x = Mathf.Lerp(0.0f, rect.width, p.x),
                    y = Mathf.Lerp(0.0f, rect.height,  Mathf.InverseLerp(min, max, p.y)),
                };
            }

            GUI.BeginClip(rect);
            GL.PushMatrix();
            GL.Clear(true, false, Color.black);
            material.SetPass(0);
            
            GL.Begin(GL.QUADS);
            GL.Color(Color.black);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(rect.width, 0.0f, 0.0f);
            GL.Vertex3(rect.width, rect.height, 0.0f);
            GL.Vertex3(0.0f, rect.height, 0.0f);
            GL.End();

            GL.Begin(GL.LINES);
            void line(Vector2 a, Vector2 b, Color color)
            {
                GL.Color(color);
                GL.Vertex(guiToGridSpace(a));
                GL.Vertex(guiToGridSpace(b));
            }

            line(Vector2.zero, Vector2.right, Color.grey);
            line(new Vector2(0.0f, SimulationTarget), new Vector2(1.0f, SimulationTarget), Color.green * 0.5f);

            for (var i = 0; i < points.Count - 1; i++)
            {
                var a = points[i];
                var b = points[i + 1];
                line(a, b, Color.green);
            }

            for (var p = 0.0f; p < 1.0f; p += 0.5f / SimulationTime)
            {
                line(new Vector2(p, min), new Vector2(p, max), Color.grey);
            }
            
            GL.End();
            GL.PopMatrix();
            GUI.EndClip();
        }
    }
}