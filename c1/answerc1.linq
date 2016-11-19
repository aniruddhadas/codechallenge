<Query Kind="Program" />

void Main()
{
	var result = File.ReadLines(@"C:\codechallenge\c1\input.txt")
	.Select(line => line.Split(' ').Select(str => int.Parse(str)))
	.Select(line => line.Sum())
	.Dump();
	
	File.WriteAllLines(@"C:\codechallenge\c1\output.txt", result.Select(x => x.ToString()));
}

// Define other methods and classes here
