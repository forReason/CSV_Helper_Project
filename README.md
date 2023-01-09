# CSV_Helper_Project
csv library for loading/saving csv files relatively quickly and easily
This repository serves as a quick csv parser and editor. Supports CSV standard, including multi line cell values.
As always with my projects, this library is focused around simplicity and speed. test it for your your own purpose.

## Supports:
- csv standard
- multi line cells
- adding/removing columns
- renaming columns
- headers and getting cells by column name
- custom delimiters
- non std. encodings
- adding, removing and inserting records

## does not support (yet?)
- automatic cell to type (eg. cell -> double) parsing. 
- automatic recognition if there are headers or not
- automatic encoding identification
- automatic delimiter type recognition
- automatic data type proofing (this is a string library, for IO purposes)

# sample usage:
### Create a Table
```
// create table class
CSV_Helper_Project.Table allRunResults = new CSV_Helper_Project.Table();
// set headers
allRunResults.SetColumnNames(new[] { "ChestName", "Start Invest", "Average Wagered Coins", "Average Survived Rounds", "Max Balance" });
// add data records
foreach (RunResult result in results)
{
    int index = allRunResults.AppendEmptyRecord();
    allRunResults.SetCell("ChestName", index, result.ChestName);
    allRunResults.SetCell("Start Invest", index, startCash.ToString());
    allRunResults.SetCell("Average Wagered Coins", index, result.WageredCoins.ToString());
    allRunResults.SetCell("Average Survived Rounds", index, result.BettingRounds.ToString());
    allRunResults.SetCell("Max Balance", index, result.MaxBalance.ToString());
}
allRunResults.WriteTableToFile("sample.csv");
```

### read a table
```
// create table class
CSV_Helper_Project.Table runs = new CSV_Helper_Project.Table();
// load table
runs.LoadFromFile(fi.FullName, hasHeaders: true);
// access table length
RunResult[] results = new RunResult[runs.Length];
// pull data from table into class
for (int i = 0; i < runs.Length; i++)
{
    results[i] = new RunResult() 
    { 
        ChestName = runs.GetCell(i, "ChestName"),
        BettingRounds = double.Parse(runs.GetCell(i, "Average Survived Rounds")),
        WageredCoins = double.Parse(runs.GetCell(i, "Average Wagered Coins")),
        MaxBalance = double.Parse(runs.GetCell(i, "Max Balance"))
    };
}
```

### modify a table
```
Table table = new Table();

// Set the column names of the table
string[] columnNames = new string[] { "ID", "Name", "Email" };
table.SetColumnNames(columnNames);

// Add a new record to the table
string[] record1 = new string[] { "1", "John Smith", "john@example.com" };
table.AppendRecord(record1);

// Add another record to the table
string[] record2 = new string[] { "2", "Jane Doe", "jane@example.com" };
table.AppendRecord(record2);

// Insert an empty record at the beginning of the table
table.InsertEmptyRecord(0);

// Remove the second record from the table
table.RemoveRecord(1);

// Overwrite the first record with new values
string[] updatedRecord = new string[] { "1", "John Doe", "john.doe@example.com" };
table.OverwriteOrInsertRecord(updatedRecord, "ID");

// Search for a record with the email "jane@example.com" and remove it if it exists
LookupValue lookup = new LookupValue("jane@example.com", "Email");
table.RemoveRecordIfExists(lookup);
```
