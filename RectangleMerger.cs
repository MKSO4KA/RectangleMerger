public class RectangleMerger
{
    public class Cell
    {
        public (bool, bool, ushort, byte) Value; // Значение ячейки
        public bool Merged;

        public Cell((bool, bool, ushort, byte) value)
        {
            Value = value;
            Merged = false;
        }

        public bool IsSameValue(Cell other)
        {
            return this.Value == other.Value;
        }
    }

    public class Rectangle
    {
        public int X, Y, Width, Height;
        public (bool, bool, ushort, byte) Value;

        public Rectangle(int x, int y, int width, int height, (bool, bool, ushort, byte) value)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Value = value;
        }
    }

    private Cell[,] grid;
    private List<Rectangle> rectangles;

    public RectangleMerger(int n, int m, (bool, bool, ushort, byte)[,] values)
    {
        grid = new Cell[n, m];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                grid[i, j] = new Cell(values[i, j]);
            }
        }
        rectangles = new List<Rectangle>();
    }

    public void MergeRectangles()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (!grid[i, j].Merged)
                {
                    Merge(i, j);
                }
            }
        }
    }

    private void Merge(int i, int j)
    {
        if (i < 0 || i >= grid.GetLength(0) || j < 0 || j >= grid.GetLength(1) || grid[i, j].Merged)
        {
            return;
        }

        (bool, bool, ushort, byte) value = grid[i, j].Value;
        int width = 1;
        int height = 1;

        // Проверка вправо
        for (int k = 1; k < grid.GetLength(1); k++)
        {
            if (j + k < grid.GetLength(1) && grid[i, j + k].IsSameValue(grid[i, j]) && !grid[i, j + k].Merged)
            {
                width++;
            }
            else
            {
                break;
            }
        }

        // Проверка вниз
        for (int k = 1; k < grid.GetLength(0); k++)
        {
            bool canMerge = true;
            for (int l = 0; l < width; l++)
            {
                if (i + k >= grid.GetLength(0) || !grid[i + k, j + l].IsSameValue(grid[i, j]) || grid[i + k, j + l].Merged)
                {
                    canMerge = false;
                    break;
                }
            }
            if (canMerge)
            {
                height++;
            }
            else
            {
                break;
            }
        }

        // Помечаем ячейки как объединенные
        for (int x = i; x < i + height; x++)
        {
            for (int y = j; y < j + width; y++)
            {
                grid[x, y].Merged = true;
            }
        }

        rectangles.Add(new Rectangle(j, i, width, height, value));
    }

    public List<Rectangle> GetMergedRectangles()
    {
        return rectangles;
    }
}
