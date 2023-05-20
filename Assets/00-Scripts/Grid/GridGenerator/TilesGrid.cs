using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3.General
{
    [Serializable]
    public class TilesGrid
    {
        #region Fields

        public List<TileGridElement> elements { get; private set; }

        #endregion

        #region Properties

        [field: SerializeField] public int rows { get; private set; }
        [field: SerializeField] public int columns { get; private set; }
        public int count => rows * columns;

        public TileGridElement this[int row, int col]
        {
            get
            {
                var index = row * columns + col;
                if (index > elements.Count - 1)
                {
                    Debug.LogError(
                        $"Subscript out of range|Index:{index},row:{row},col:{col},count:{elements.Count},width:{columns},height:{rows}");
                    return default;
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

        public TileGridElement this[int index]
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
            GenerateElements();

        }

        void GenerateElements()
        {
            elements = new(rows * columns);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var element = new TileGridElement(-1,i,j);
                    elements.Add(element);
                }
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
                    outPut += $"{this[i, j].value},";
                }
                outPut += "\n";
            }
            outPut += "]";
            return outPut;
        }

        #endregion
    }

    [Serializable]
    public class TileGridElement
    {
        #region Fields

        public int value { get; private set; }
        public int row{ get; private set; }
        public int col{ get; private set; }
        public bool needCheck{ get; private set; }
        #endregion

        #region Constructors

        public TileGridElement(int aValue, int aRow, int aCol, bool aNeedCheck = false)
        {
            value = aValue;
            row = aRow;
            col = aCol;
            needCheck = aNeedCheck;
        }
        #endregion

        #region Methods

        public void SetValue(int aValue,bool aNeedCheck=true)
        {
            value = aValue;
            needCheck = aNeedCheck;
        }


        #endregion

        #region Operators

        public static bool operator ==(TileGridElement a, TileGridElement b)
        {
            if (a is null)
                return false;
            if (b is null)
                return false;
            return a.value==b.value;
        }

        public static bool operator !=(TileGridElement a, TileGridElement b)
        {
            return !(a == b);
        }

        #endregion
    }
}