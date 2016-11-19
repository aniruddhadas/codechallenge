<Query Kind="Program" />

void Main()
{
	var result = File.ReadLines(@"C:\codechallenge\c2\input.txt")
					 .Select(x =>
						 x.ToCharArray()
						 .Select(y => y == '.' ? '0' : y).ToArray())
	//.Take(1)
	//    .Dump()
	;

	var matrix = new SudokuMatrix(result);
	matrix.PrintMatrix();
	matrix.SolveMatrix();
}

// Define other methods and classes here
public class SudokuMatrix
{
	const int upperBound = 8;
	const int invalidNumber = 0;
	private List<int>[,] matrix = new List<int>[9, 9];

	class MatrixPosition
	{
		public int x;
		public int y;
	}

	public SudokuMatrix(IEnumerable<Char[]> lines)
	{
		foreach (var row in Enumerable.Range(0, 9))
		{
			var rowItems = lines.ElementAt(row);
			int col = 0;
			foreach (var value in rowItems.Select(x => int.Parse(x.ToString())))
			{
				matrix[row, col] = new List<int>();
				if (value == 0) {
					matrix[row, col].AddRange(Enumerable.Range(1, 9));
                }
				else {
					matrix[row, col].Add(value);
				}
				col++;
			}
		}
		//		this.PrintMatrix();
	}

	public void SolveMatrix()
	{
		PrintMatrix();
		Eliminate();
	}

	public void Eliminate()
	{
		int iteration = 0;
		while (true)
		{
			bool updated = false;
			foreach (var row in Enumerable.Range(0, 9))
			{
				foreach (var col in Enumerable.Range(0, 9))
				{
					//1. for each position go to column mates and find out what can be eliminated
					updated = updated || EliminateValuesByScanningColumnsMates(row, col);

					//2. for each position go to row mates and find out what can be eliminated
					updated = updated || EliminateValuesByScanningRowMates(row, col);

					//3. for each position go to square mates and find out what can be eliminated
					updated = updated || EliminateValuesByScanningSquareMates(row, col);
				}
			}
			iteration++;

			Console.WriteLine("Iteration:" + iteration);
			PrintMatrix();
			if (!updated) {
				return;
			}
		}
	}

	private bool EliminateValuesByScanningSquareMates(int row, int col)
	{
		var updated = false;
		if (matrix[row, col].Count == 1)
		{
			// means you are set;
			return false;
		}
		var start = this.FindBoundingSquareStartPositions(row, col);
		var end = this.FindBoundingSquareEndPositions(row, col);
		List<int> unionList = new List<int>();
		for (int i = start.x; i <= end.x; i++)
		{
			for (int j = start.y; j <= end.y; j++)
			{
				if (!(i == row && j == col))
				{
					if (matrix[i, j].Count == 1)
					{
						var elementToDelete = matrix[i, j].First();
						if (matrix[row, col].Remove(elementToDelete))
						{
							updated = true;
						}
					}
					unionList = unionList.Union(matrix[i, j]).ToList();
				}
			}
		}

		var elementsIHaveButNoOneElseCan = matrix[row, col].Except(unionList);
		if (elementsIHaveButNoOneElseCan.Count() > 1)
		{
			PrintMatrix();
			throw new Exception("Cant Do That");
		}

		if (elementsIHaveButNoOneElseCan.Count() == 1)
		{
			matrix[row, col].RemoveAll(x => x != elementsIHaveButNoOneElseCan.First());
		}
		return updated;
	}


	private bool EliminateValuesByScanningColumnsMates(int row, int col)
	{
		var updated = false;
		if (matrix[row, col].Count == 1) {
			// means you are set;
			return false;
		}
		List<int> unionList = new List<int>();
		for (int i = 0; i < 9; i++)
		{
			if (i != col)
			{
				if (matrix[row, i].Count == 1) {
					var elementToDelete = matrix[row, i].First();
					if (matrix[row, col].Remove(elementToDelete)) {
						updated = true;
					}
				}
				unionList = unionList.Union(matrix[row, i]).ToList();
			}
		}

		var elementsIHaveButNoOneElseCan = matrix[row, col].Except(unionList);
		if (elementsIHaveButNoOneElseCan.Count() > 1) {
			PrintMatrix();
			throw new Exception("Cant Do That");
		}

		if (elementsIHaveButNoOneElseCan.Count() == 1) {
			matrix[row, col].RemoveAll(x => x != elementsIHaveButNoOneElseCan.First());
		}
		return updated;
	}

	private bool EliminateValuesByScanningRowMates(int row, int col)
	{
		var updated = false;
		if (matrix[row, col].Count == 1)
		{
			// means you are set;
			return false;
		}
		List<int> unionList = new List<int>();
		for (int i = 0; i < 9; i++)
		{
			if (i != row)
			{
				if (matrix[i, col].Count == 1)
				{
					var elementToDelete = matrix[i, col].First();
					if (matrix[row, col].Remove(elementToDelete))
					{
						updated = true;
					}
				}
				unionList = unionList.Union(matrix[i, col]).ToList();
			}
		}

		var elementsIHaveButNoOneElseCan = matrix[row, col].Except(unionList);
		if (elementsIHaveButNoOneElseCan.Count() > 1)
		{
			PrintMatrix();
			throw new Exception("Cant Do That");
		}

		if (elementsIHaveButNoOneElseCan.Count() == 1)
		{
			matrix[row, col].RemoveAll(x => x != elementsIHaveButNoOneElseCan.First());
		}
		return updated;
	}

	private MatrixPosition FindBoundingSquareStartPositions(int x, int y)
	{
		return new MatrixPosition()
		{
			x = (x / 3) * 3,
			y = (y / 3) * 3
		};
	}
	
	private MatrixPosition FindBoundingSquareEndPositions(int x, int y)
	{
		return new MatrixPosition()
		{
			x = (((x / 3) + 1) * 3) - 1,
			y = (((y / 3) + 1) * 3) - 1
		};

	}

	public void Initialize()
	{
		foreach (var row in Enumerable.Range(0, 9))
		{
			foreach (var col in Enumerable.Range(0, 9))
			{
				if (GetElement(row, col) == "0")
				{
					this.matrix[row, col].AddRange(Enumerable.Range(1, 9));
				}
			}
		}
	}

	public void PrintMatrix()
	{
		Console.WriteLine("**Matrix**");
		foreach (var row in Enumerable.Range(0, 9))
		{
			foreach (var col in Enumerable.Range(0, 9))
			{
				Console.Write(GetElement(row, col) + " ");
			}
			Console.WriteLine();
		}
		//		return matrix;
	}

	public void SetElement(int x, int y, int element)
	{
		this.matrix[x, y].Add(element);
	}
	//
	//
	public string GetElement(int x, int y)
	{
		return string.Join("|", this.matrix[x, y]);
	}
	//	
	//	public bool MatrixValidate()
	//	{
	//		//1. check if all the rows have all elements just once
	//		return ValidateRows() &&
	//		//2. check if all the columns have all elements just once
	//		ValidateColumns() &&
	//		//3. check if each 3x3 matrix has all the elements just once
	//		ValidateSquares();
	//	}
	//
	//	public bool ValidateColumns()
	//	{
	//		return Enumerable.Range(0, upperBound).All(z => MatrixValidate(new MatrixPosition() { x = z, y = 0 }, new MatrixPosition() { x = z, y = upperBound }));
	//    }
	//
	//	public bool ValidateRows()
	//	{
	//		return Enumerable.Range(0, upperBound).All(z => MatrixValidate(new MatrixPosition() { x = 0, y = z }, new MatrixPosition() { x = upperBound, y = z }));
	//	}
	//	
	//    public bool ValidateSquares()
	//	{
	//		foreach (var elem1 in Enumerable.Range(0, 2))
	//		{
	//			foreach (var elem2 in Enumerable.Range(0, 2))
	//			{
	//				return MatrixValidate(new MatrixPosition() { x = elem1 * 3, y = elem2 * 3 }, new MatrixPosition() { x = (elem1 * 3) + 2, y = (elem2 * 3) + 2 });
	//			}
	//		}
	//		return false;
	//	}
	//
	//	private bool MatrixValidate(MatrixPosition start, MatrixPosition end)
	//	{
	//		HashSet<int> numberSet = new HashSet<int>();
	//		for (int i = start.x; i < end.x; i++)
	//		{
	//			for (int j = start.y; i < end.y; i++)
	//			{
	//				if (numberSet.Contains(matrix[i, j])) {
	//					return false;
	//				}
	//				
	//				if (matrix[i, j] != invalidNumber) {
	//					numberSet.Add(matrix[i,j]);
	//				}
	//			}
	//		}
	//		return true;
	//	}
}