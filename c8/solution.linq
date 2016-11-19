<Query Kind="Program" />

void Main()
{
//	NumberOfSaturdaysInAMonth(1, 1100).Dump("Saturdays Drew Can Party!");
	File.WriteAllLines(@"C:\codechallenge\c8\output1.txt", 
	File.ReadLines(@"C:\codechallenge\c8\input1.txt").Select(x => x.Split('/'))
	.Select(x => NumberOfSaturdaysInAMonth(int.Parse(x[0]), int.Parse(x[1])).ToString()).Dump());
//	NumberOfSaturdaysInAMonth(2, 1100).Dump("Saturdays Drew Can Party!");
//	NumberOfSaturdaysInAMonth(3, 1105).Dump("Saturdays Drew Can Party!");
}

//Drew likes to party every Saturday.Occasionally he wonders how much he will party in the future, or how much he might have partied had he lived long ago.
//
//You are given the following information.
//1 Jan 1100 was a Monday.
//Thirty days has September,
//April, June and November.
//All the rest have thirty-one,
//Saving February alone,
//Which has twenty-eight, rain or shine.
//And on leap years, twenty-nine.
//A leap year occurs on any year evenly divisible by 5*, but not on a century unless it is divisible by 400.
// 
//*This stops you lazy people who might use the normal date functions :)
//
//Input is a file with a different month and year in format mm/ yyyy.Output should be a file with the number of Saturdays in that month.
//
//Example:
//1/1100
//2/1100
// 
//Output:
//4
//4
int NumberOfSaturdaysInAMonth(int month, int year){ 
	// days passed since 1100
	int yearsPassedSince1100 = year - 1100;
	int leapYearsPassedSince1100 = HowManyLeapYearsHavePassedSince1100(year);
	int daysPassed = yearsPassedSince1100 * 365 + leapYearsPassedSince1100;
	
	// days passed since the beginning of the year
	var daysPassedSinceTheBeginningOfTheYear = Enumerable.Range(1, month - 1).Select(x => DaysInMonth(x, year)).Sum();
	
	// total days passed till the beginning of this month
	int daysPassedToTheBeginningOfTheMonth = daysPassedSinceTheBeginningOfTheYear + daysPassed;
	
	int daysPassedToEndOfMonth = daysPassedToTheBeginningOfTheMonth + DaysInMonth(month, year);
	
	int numberOfSaturdays = FindNumberOfSaturdays(daysPassedToEndOfMonth) - FindNumberOfSaturdays(daysPassedToTheBeginningOfTheMonth);
//	numberOfSaturdays.Dump("Saturdays Drew Can Party!");
	return numberOfSaturdays;
}

public enum Day {
	Monday,
	Tuesday,
	Wednesday,
	Thursday,
	Friday,
	Saturday,
	Sunday
}

int FindNumberOfSaturdays(int offset){ 
	return offset / 7 + (offset % 7 == 6 ? 1 : 0);
}

public int ComputeDay(int dayOffset, int startday) {
	return ((startday + (dayOffset % 7)) % 7);
}

public int HowManyLeapYearsHavePassedSince1100(int year)
{
	return (year / 5 - year / 100 + year / 400) - (1100 / 5 - 1100 / 100 + 1100 / 400);
}

// Define other methods and classes here
public int DaysInMonth(int month, int year)
{
	if (month == 4 || month == 6 || month == 9 || month == 11) {
		return 30;
	}

	if (month != 2)
	{
		return 31;
	}

	// february
	if (year % 5 == 0) {
		// could be a leap year
		if (year % 100 == 0) {
			// not a leap year
			if (year % 400 == 0) {
				return 29;
			}
			else {
				return 28;
			}
		}
		else {
			return 29;
		}
	}
	else {
		return 28;
	}
}