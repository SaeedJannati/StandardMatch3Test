using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.General
{
    [Serializable]
    public class TilesGrid
    {
        #region Fields

        private List<int> elements;

        #endregion

        #region Properties

        [field: SerializeField] public int rows { get; private set; }
        [field: SerializeField] public int columns { get; private set; }
        public int count => rows * columns;

        public int this[int row, int col]
        {
            get
            {
                var index = row * columns + col;
                if (index > elements.Count - 1)
                {
                    Debug.LogError(
                        $"Subscript out of range|Index:{index},row:{row},col:{col},count:{elements.Count},width:{columns},height:{rows}");
                    return 0;
                }

                return this.elements[index];
            }
            set
            {
                var index = row * columns + col;
                if (index > elements.Count - 1)
                {
                    Debug.LogError("Subscript out of range");
                    return;
                }

                this.elements[row * columns + col] = value;
            }
        }

        public int this[int index]
        {
            get => elements[index];
            set => elements[index] = value;
        }

        #endregion

        #region Operators

        public static bool operator ==(TilesGrid a, TilesGrid b)
        {
            if (a is null)
                return false;
            if (b is null)
                return false;
            var rows = a.rows;
            if (b.rows != rows)
                return false;
            var cols = a?.columns??0;
            if (b.columns != cols)
                return false;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (a[i, j] != b[i, j])
                        return false;
                }
            }

            return true;
        }

        public static bool operator !=(TilesGrid a, TilesGrid b)
        {
            return !(a == b);
        }

        #endregion

        #region Constructors

        public TilesGrid(int aRows, int aCols)
        {
            rows = aRows;
            columns = aCols;
            elements = new(aRows * aCols);
            for (int i = 0; i < count; i++)
            {
                elements.Add(0);
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            if(elements==default)
                return string.Empty;
            var outPut = "[";
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    outPut += $"{this[i, j]},";
                }
                outPut += "\n";
            }
            outPut += "]";
            return outPut;
        }

        #endregion
    }
}