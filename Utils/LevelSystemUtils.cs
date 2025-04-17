using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System;

namespace Kliker.Utils
{
    public static class LevelSystemUtils
    {
        private const int maxPoints = 15000;
        private const int maxLevel = 30;

        //public static bool IsLevelUpRequired(int currentLevel, int points) // check if user has enough points to level up
        //{
        //    int targetLevel = CalculateLevelByPoints(points); // calculate what user level should have while having certain amount of points
        //    return currentLevel != targetLevel;
        //}

        public static int CalculateLevelByPoints(int points)
        {
            return (int)((points * 1.0 / maxPoints) * maxLevel);
        }
    }
}
