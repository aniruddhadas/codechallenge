<Query Kind="Program" />

void Main()
{
	var lines = File.ReadAllLines(@"c:\codechallenge\q5\Problem5Input.txt")
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
	lines.Dump();
	
	int width = lines[0].Count;

	// setup grid
	for (int row = 0; row < lines.Count; row++)
	{
		for (int cell = 0; cell < width; cell++)
		{

		}
	}
	
	// get all white cells add to queue
	
	// recurisivly eat
	
	// every visited white cell gets the same id
	
	// when we are done eating if we have white cells we take the next white cell and eat
	
	// continue until queue is empty
       
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
}


