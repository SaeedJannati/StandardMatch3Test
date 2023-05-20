using System.Collections.Generic;
using UnityEngine;

namespace Match3.General
{
    [CreateAssetMenu(fileName = "GridGeneratorViewModel",menuName = "Match3/General/Grid/GridGeneratorViewModel")]
    public class GridGeneratorViewModel:ScriptableObject
    {
        public List<Color> colours;
    }
}