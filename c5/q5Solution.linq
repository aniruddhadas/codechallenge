<Query Kind="Program" />

void Main()
{
	var lines = File.ReadAllLines(@"c:\codechallenge\q5\Problem5Input3.txt")
					.Select(line =>
					{
						var rowNodes = line.ToCharArray()
							.Select(nodeChar => Node.CreateNodeFrom(nodeChar)).ToList();

						AddRowBoundaries(rowNodes);
						
						return rowNodes;
                    }).ToList();
										
	var lineWidth = lines.ElementAt(0).Count;	
	var topLine = Enumerable.Range(0, lineWidth).Select(x => (Node)new Wall()).ToList();
	var bottomLine = Enumerable.Range(0, lineWidth).Select(x => (Node)new Wall()).ToList();	
	lines.Insert(0, topLine);
	lines.Add(bottomLine);	
	//lines.Dump();
	
	int width = lines[0].Count;

	// setup grid
	for (int row = 0; row < lines.Count; row++)
	{
		for (int col = 0; col < width; col++)
		{
			Node cell = lines.ElementAt(row).ElementAt(col);

			// add north
			if (row != 0)
			{
				cell.North = lines.ElementAt(row-1).ElementAt(col);
			}

			// add south
			if (row != lines.Count - 1)
			{
				cell.South = lines.ElementAt(row + 1).ElementAt(col);
			}

			// add west
			if (col != 0)
			{
				cell.West = lines.ElementAt(row).ElementAt(col - 1);
			}

			// add east
			if (col != lineWidth - 1)
			{
				cell.East = lines.ElementAt(row).ElementAt(col + 1);
			}
		}
	}

	//lines.Dump();
	foreach (var row in lines)
	{
		foreach (var cell in row)
		{
			Console.Write(cell.ToString());
		}
		Console.WriteLine();
	}


	// get all white cells add to queue
	var emptyCellQueue = lines.SelectMany(l => l.Where(cell => cell is Empty));
	emptyCellQueue.Count().Dump();

	// recurisivly eat
	int regionCount = 0;
	while (emptyCellQueue.Any())
	{
		var emptyCell = emptyCellQueue.First();
		Sweep(emptyCell);
		regionCount++;
		emptyCellQueue = emptyCellQueue.Where(cell => !cell.Visited);
	}
	
	// every visited white cell gets the same id
	// when we are done eating if we have white cells we take the next white cell and eat
	
	// continue until queue is empty
    regionCount.Dump();   
}


public static void Sweep(Node startingPoint)
{
	if (startingPoint is Wall || startingPoint.Visited)
	{
		return;
	}
	
	startingPoint.Visited = true;
	Sweep(startingPoint.North);
	Sweep(startingPoint.South);
	Sweep(startingPoint.East);
	Sweep(startingPoint.West);
}

public static void AddRowBoundaries(List<Node> nodes)
{
	nodes.Insert(0, new Wall());
	nodes.Add(new Wall());
}


// Define other methods and classes here
public abstract class Node
{
	public abstract bool Visited { get; set; }

	public Node North { get; set; }
	public Node South { get; set; }
	public Node East { get; set; }
	public Node West { get; set; }

	public static Node CreateNodeFrom(Char input)
	{
		switch (input)
		{
			case '.':
				return new Empty();
			case 'x':
				return new Wall();
			default:
				throw new ArgumentException("unknown node input " + input);
		}
	}
}

public class Empty : Node
{
	private bool _visited = false;
	public override bool Visited
	{
		get { return this._visited; }
		set { this._visited = value; }
	}

	public override string ToString()
	{
		return "e";
    }

}

public class Wall : Node
{
	public override bool Visited
	{
		get { return true; }
		set
		{
			throw new NotSupportedException();
		}
	}


	public override string ToString()
	{
		return "x";
	}
}


