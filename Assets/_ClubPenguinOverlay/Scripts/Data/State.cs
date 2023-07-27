using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "State")]
public class State : ScriptableObject
{
    [SerializeField] private List<Choice> choises = new List<Choice>();

    private float GetTotalWeight()
    {
        float totalWeight = 0f;
        foreach (Choice choise in choises)
        {
            totalWeight += choise.weight;
        }
        return totalWeight;
    }

    public PenguinState GetRandomState()
    {
        float randomValue = Random.Range(0f, GetTotalWeight());

        // Find the choice corresponding to the random value
        foreach (Choice choice in choises)
        {
            if (randomValue < choice.weight)
            {
                return choice.state;
            }
            randomValue -= choice.weight;
        }
        return PenguinState.None;
    }
}