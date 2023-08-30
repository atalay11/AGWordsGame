using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition
{
    public Health health;
    public Energy energy;
    public Credit credit;

    public PlayerCondition(){}
    public PlayerCondition(float _health, int _energy, int _credit)
    {
        health.Value = _health;
        energy.Value = _energy;
        credit.Value = _credit;
    }

    public class Health
    {
        public Health(){}
        public Health(float val) { Value = val; }
        public float Value {
            get { return m_val; }
            set { m_val = System.Math.Clamp(value, MIN_VALUE, MAX_VALUE); }
        }

        private float m_val = 0;
        private const float MIN_VALUE = 100;
        private const float MAX_VALUE = 100;
    }

    public class Energy
    {
        public Energy(){}
        public Energy(int val) { Value = val; }
        public int Value {
            get { return m_val; }
            set { m_val = value; }
        }

        private int m_val;
    }

    public class Credit
    {
        public Credit(){}
        public Credit(int val) { Value = val; }
        public int Value {
            get { return m_val; }
            set { m_val = value; }
        }

        private int m_val;
    }
}
