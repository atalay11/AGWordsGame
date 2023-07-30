using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Letter<TChar>
{
    public TChar value;

    public Letter(TChar _value) { value = _value; }

    public static implicit operator TChar(Letter<TChar> letter)
    {
        return letter.value;
    }

    public static Letter<TChar> GenerateRandomLetter()
    {
        if (typeof(TChar) == typeof(char))
        {
            int randomNumber = m_Random.Next(26);
            char randomCharacter = (char)('A' + randomNumber); // For uppercase letters
            return new Letter<TChar>((TChar)(object)randomCharacter); 
        }
        else
        {
            throw new System.NotImplementedException("Only char character is implemented!");
        }
    }

    static System.Random m_Random = new System.Random();
}