using System.Text;
using UnitTest_Helper_Library_Framework;

namespace CSV_Helper_UnitTests.Parser
{
    public class LoadFile_Tests
    {
        /// <summary>
        /// This test method tests a very basic write load approach for several encoding cases
        /// </summary>
        /// <exception cref="FileLoadException"></exception>
        [Fact]
        public void Test_Loading_Encoded_Files()
        {
            string testName = "Test_Loading_Encoded_Files";
            TestFolder_Helper.CleanFolder(testName);
            string[] lines =
            {
                "\"Name\",\"Age\",Gender",
                "Alfons,13,Male",
                "Francesca Haller, 22, Female",
                "\"Adenauer, Konrad\", 77, Male"
            };
            QuickCsv.Net.Parser parser = new QuickCsv.Net.Parser();
            foreach (Encoding encoding in Constants.Encodings)
            {
                File.WriteAllLines($"{testName}\\testfile_{encoding.EncodingName}.csv", lines, encoding);
                List<string[]> fields = parser.ParseCSVFile($"{testName}\\testfile_{encoding.EncodingName}.csv",splitChar: ',', encoding);
                // Headers
                if (fields[0][0] != "Name") throw new FileLoadException($"Loaded String {fields[0][0]} does not match saved string \"Name\" ! - Encoding: {encoding.EncodingName}");
                if (fields[0][1] != "Age") throw new FileLoadException($"Loaded String {fields[0][1]} does not match saved string \"Age\" ! - Encoding: {encoding.EncodingName}" );
                if (fields[0][2] != "Gender") throw new FileLoadException($"Loaded String {fields[0][2]} does not match saved string \"Gender\" ! - Encoding: {encoding.EncodingName}");
                // Alfons
                if (fields[1][0] != "Alfons") throw new FileLoadException($"Loaded String {fields[1][0]} does not match saved string \"Alfons\" ! - Encoding: {encoding.EncodingName}");
                if (fields[1][1] != "13") throw new FileLoadException($"Loaded String {fields[1][1]} does not match saved string \"13\" ! - Encoding: {encoding.EncodingName}");
                if (fields[1][2] != "Male") throw new FileLoadException($"Loaded String {fields[1][2]} does not match saved string \"Male\" ! - Encoding: {encoding.EncodingName}");
                // Francesca
                if (fields[2][0] != "Francesca Haller") throw new FileLoadException($"Loaded String {fields[2][0]} does not match saved string \"Francesca Haller\" ! - Encoding: {encoding.EncodingName}");
                if (fields[2][1] != "22") throw new FileLoadException($"Loaded String {fields[2][1]} does not match saved string \"22\" ! - Encoding: {encoding.EncodingName}");
                if (fields[2][2] != "Female") throw new FileLoadException($"Loaded String {fields[2][2]} does not match saved string \"Female\" ! - Encoding: {encoding.EncodingName}");
                // Konrad
                if (fields[3][0] != "Adenauer, Konrad") throw new FileLoadException($"Loaded String {fields[3][0]} does not match saved string \"Adenauer, Konrad\" ! - Encoding: {encoding.EncodingName}");
                if (fields[3][1] != "77") throw new FileLoadException($"Loaded String {fields[3][1]} does not match saved string \"77\" ! - Encoding: {encoding.EncodingName}");
                if (fields[3][2] != "Male") throw new FileLoadException($"Loaded String {fields[3][2]} does not match saved string \"Male\" ! - Encoding: {encoding.EncodingName}");
            }
        }
        /// <summary>
        /// this test method tries to load some multiline cells and checks if cells around it get affected
        /// </summary>
        /// <exception cref="FileLoadException"></exception>
        [Fact]
        public void Test_Loading_Multiline_Records()
        {
            string testName = "Test_Loading_Multiline_Records";
            TestFolder_Helper.CleanFolder(testName);
            string[] testRecords =
            {
                $"Test Person{Environment.NewLine}" +
                $"Teststreet Nr. 2{Environment.NewLine}" +
                $"4000 Testington{Environment.NewLine}" +
                $"Testcountry{Environment.NewLine}" +
                $"Testplanet 3{Environment.NewLine}" +
                $"Galaxy 87B{Environment.NewLine}" +
                $"3rd Universe from the Left",

                $"Violets are Red{Environment.NewLine}" +
                $"Roses are Blue{Environment.NewLine}" +
                $"This Peom makes no sense{Environment.NewLine}" +
                $"But Computers DO!",

                $"How\r \rare\n \nyou\r\n \r\n?"
            };
            string[] lines =
            {
                $"test 1,test 2,test 3",
                $"test 4,\"{testRecords[0]}\",test 5",
                $"test 6,\"{testRecords[1]}\",test 7",
                $"test 8,\"{testRecords[2]}\", test 9",
                $"test 10, test 11, test 12"
            };
            Encoding encoding = Encoding.UTF8;
            File.WriteAllLines($"{testName}\\testfile_{encoding.EncodingName}.csv", lines, encoding);
            QuickCsv.Net.Parser parser = new QuickCsv.Net.Parser();
            List<string[]> fields = parser.ParseCSVFile($"{testName}\\testfile_{encoding.EncodingName}.csv", splitChar: ',', encoding);
            if (!fields[0][0].Equals("test 1")) throw new FileLoadException($"Loaded String \"{fields[0][0]}\" does not match saved string \"test 1\" ! - Encoding: {encoding.EncodingName}");
            if (!fields[0][1].Equals("test 2")) throw new FileLoadException($"Loaded String \"{fields[0][1]}\" does not match saved string \"test 2\" ! - Encoding: {encoding.EncodingName}");
            if (!fields[0][2].Equals("test 3")) throw new FileLoadException($"Loaded String \"{fields[0][2]}\" does not match saved string \"test 3\" ! - Encoding: {encoding.EncodingName}");

            if (fields[1][0] != "test 4") throw new FileLoadException($"Loaded String \"{fields[1][0]}\" does not match saved string \"test 4\" ! - Encoding: {encoding.EncodingName}");
            if (fields[1][1] != testRecords[0]) throw new FileLoadException($"Loaded String \"{fields[1][1]}\" does not match saved string \"{testRecords[0]}\" ! - Encoding: {encoding.EncodingName}");
            if (fields[1][2] != "test 5") throw new FileLoadException($"Loaded String \"{fields[1][2]}\" does not match saved string \"test 5\" ! - Encoding: {encoding.EncodingName}");

            if (fields[2][0] != "test 6") throw new FileLoadException($"Loaded String \"{fields[2][0]}\" does not match saved string \"test 6\" ! - Encoding: {encoding.EncodingName}");
            if (fields[2][1] != testRecords[1]) throw new FileLoadException($"Loaded String \"{fields[2][1]}\" does not match saved string \"{testRecords[1]}\" ! - Encoding: {encoding.EncodingName}");
            if (fields[2][2] != "test 7") throw new FileLoadException($"Loaded String \"{fields[2][2]}\" does not match saved string \"test 7\" ! - Encoding: {encoding.EncodingName}");

            if (fields[3][0] != "test 8") throw new FileLoadException($"Loaded String \"{fields[3][0]}\" does not match saved string \"test 8\" ! - Encoding: {encoding.EncodingName}");
            if (fields[3][1] != testRecords[2]) throw new FileLoadException($"Loaded String \"{fields[3][1]}\" does not match saved string \"{testRecords[2]}\" ! - Encoding: {encoding.EncodingName}");
            if (fields[3][2] != "test 9") throw new FileLoadException($"Loaded String \"{fields[3][2]}\" does not match saved string \"test 9\" ! - Encoding: {encoding.EncodingName}");

            if (fields[4][0] != "test 10") throw new FileLoadException($"Loaded String \"{fields[4][0]}\" does not match saved string \"test 10\" ! - Encoding: {encoding.EncodingName}");
            if (fields[4][1] != "test 11") throw new FileLoadException($"Loaded String \"{fields[4][1]}\" does not match saved string \"test 11\" ! - Encoding: {encoding.EncodingName}");
            if (fields[4][2] != "test 12") throw new FileLoadException($"Loaded String \"{fields[4][2]}\" does not match saved string \"test 12\" ! - Encoding: {encoding.EncodingName}");
        }
    }
}
