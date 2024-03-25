using System.Diagnostics;

var wordList = new List<string>();

try
{
    GetWordList();
    while (true)
    {
        PrintWordList();
        DoMenu();
    }

}
catch (OperationCanceledException)
{
    return;
}

void DoMenu()
{
    Console.WriteLine();
    Console.WriteLine("Menu:");
    Console.WriteLine("1. Add a word");
    Console.WriteLine("2. Edit a word");
    Console.WriteLine("3. Remove a word");
    Console.WriteLine("4. Clear the list");
    Console.WriteLine("5. Print to PDF");
    Console.WriteLine("6. Exit");
    Console.Write("Enter a number: ");
    var choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            Console.Write("Enter a word: ");
            var word = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(word))
            {
                break;
            }
            wordList.Add(word);
            break;
        case "2":
            Console.Write("Enter the number of the word to edit: ");
            var str = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(str))
            {
                break;
            }
            if (!int.TryParse(str, out var index))
            {
                Console.WriteLine("Invalid index");
                break;
            }
            if (index < 1 || index > wordList.Count)
            {
                Console.WriteLine("Invalid index");
                break;
            }
            Console.Write("Enter replacement word: ");
            var replacement = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(replacement))
            {
                break;
            }
            wordList[index - 1] = replacement;
            break;
        case "3":
            Console.Write("Enter the number of the word to remove: ");
            var str2 = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(str2))
            {
                break;
            }
            if (!int.TryParse(str2, out var index2))
            {
                Console.WriteLine("Invalid index");
                break;
            }
            if (index2 < 1 || index2 > wordList.Count)
            {
                Console.WriteLine("Invalid index");
                break;
            }
            wordList.RemoveAt(index2 - 1);
            break;
        case "4":
            wordList.Clear();
            break;
        case "5":
            SaveOpenPdf(FlashcardMaker.Library.PrintToPdf(wordList));
            break;
        case "6":
            throw new OperationCanceledException();
        default:
            Console.WriteLine("Invalid choice");
            break;
    }
}

void PrintWordList()
{
    Console.WriteLine();
    Console.WriteLine("Word list:");
    int i = 1;
    foreach (var word in wordList)
    {
        Console.WriteLine(i++ + ": " + word);
    }
}


void GetWordList()
{
    while (true)
    {
        Console.Write("Enter a word: ");
        var word = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(word))
        {
            break;
        }
        wordList.Add(word);
    }
}

void SaveOpenPdf(Stream ms)
{
    using var file = File.Create("wordlist.pdf");
    ms.CopyTo(file);
    Console.WriteLine("PDF saved to wordlist.pdf");
    // Open the PDF in the default PDF viewer
    Process.Start(new ProcessStartInfo("wordlist.pdf") { UseShellExecute = true });
}
