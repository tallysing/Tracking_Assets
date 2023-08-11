
List<Asset> assets = new List<Asset>(); // Saving registered assets
static void AddAssets(List<Asset> assets) // Adds new assets
{
    do // Prompt user to register asset information
    {
        Console.Write("Please enter the asset type: ");

        string type = Console.ReadLine();

        Console.Write("Please enter the asset brand: ");

        string brand = Console.ReadLine();

        Console.Write("Please enter the asset model: ");

        string model = Console.ReadLine();

        Console.Write("Please provide the location of the office; Sweden, Spain or USA: ");

        string office = Console.ReadLine();

        Console.Write("Use this format (dd/mm/yyyy) to type the purchase date of the asset: ");

        DateTime date;

        while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out date)) // Verify valid date format
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.Write("You have entered an invalid date format. Please try again (dd/mm/yyyy): ");

            Console.ResetColor();
        }
        Console.Write("Please fill in the dollar price of the asset: ");

        int price;

        while (!int.TryParse(Console.ReadLine(), out price)) // Valid number verification
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("Only whole numbers are permitted, so please try again: ");

            Console.ResetColor();

        }
        assets.Add(new Asset(type, brand, model, office, date, price));

        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine("A new asset have been registered!");

        Console.ForegroundColor = ConsoleColor.Magenta;

        Console.Write("Press the 'q' key when you are finished, or press enter if you want to continue registering assets.");

        Console.ResetColor();

    } while (Console.ReadLine() != "q");
}

AddAssets(assets);

PresentAssets(assets);
static void PresentAssets(List<Asset> assets) // Displays the assets in a sorted table
{
    var sortedAssets = assets.OrderBy(asset => asset.Office).ThenBy(asset => asset.PurchaseDate).ToList();

    Table table = new Table(sortedAssets);

    table.Print();
}

class Asset // Asset creation map
{
    public Asset(string type, string brand, string model, string office, DateTime purchaseDate, int usdPrice)
    {
        Type = type;
        Brand = brand;
        Model = model;
        Office = office;
        PurchaseDate = purchaseDate;
        USDprice = usdPrice;
    }
    public string Type { get; }
    public string Brand { get; }
    public string Model { get; }
    public string Office { get; }
    public DateTime PurchaseDate { get; }
    public int USDprice { get; }
}

class Table // Logic of the table structure
{
    private List<Asset> assets;

    public Table(List<Asset> assets)
    {
        this.assets = assets;
    }
    private string PadCenter(string text, int width) // Centers the strings
    {
        int totalSpaces = width - text.Length;
        int leftSpaces = totalSpaces / 2;
        int rightSpaces = totalSpaces - leftSpaces;
        return new string(' ', leftSpaces) + text + new string(' ', rightSpaces);
    }
    private Dictionary<string, decimal> CurrencyPrice(Asset asset) // Corresponds the country's currency price
    {
        switch (asset.Office)
        {
            case "Spain":
                return new Dictionary<string, decimal>()
            {
                    { "EUR", 0.91m }
                };

            case "Sweden":
                return new Dictionary<string, decimal>()
            {
                { "SEK", 10.7m }
            };
            default:
                return new Dictionary<string, decimal>()
            {
                {"USD", 0m }
            };
        }
    }
    private void TableRows(Asset asset) // Holds the table rows template
    {
        var exchangeCurrency = CurrencyPrice(asset);

        decimal localPrice;

        if (exchangeCurrency.ElementAt(0).Key == "USD")
        {
            localPrice = asset.USDprice;
        }
        else
        {
            localPrice = asset.USDprice * exchangeCurrency.ElementAt(0).Value;
        }
        Console.WriteLine("| {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |", PadCenter(asset.Type, 13), PadCenter(asset.Brand, 13), PadCenter(asset.Model, 13), PadCenter(asset.Office, 13), PadCenter(asset.PurchaseDate.ToString("MM/dd/yyyy"), 13), PadCenter(asset.USDprice.ToString(), 13), PadCenter(exchangeCurrency.ElementAt(0).Key, 13), PadCenter(localPrice.ToString(), 17));
    }
    public void Print() // Builds the table
    {
        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine(new string('-', 133));

        Console.WriteLine("| {0} | {1} | {2} | {3} | {4} | {5} | {6} | {7} |", PadCenter("Type", 13), PadCenter("Brand", 13), PadCenter("Model", 13), PadCenter("Office", 13), PadCenter("Purchase Date", 13), PadCenter("Price in USD", 13), PadCenter("Currency", 13), "Local price today");

        Console.WriteLine(new string('-', 133));

        Console.ResetColor();


        foreach (Asset asset in assets)
        {
            TimeSpan timeSpan = DateTime.Now - asset.PurchaseDate; // Checking assets lifecycle

            if (timeSpan >= TimeSpan.FromDays(365 * 3 - 90))
            {
                Console.ForegroundColor = ConsoleColor.Red;

                TableRows(asset);

                Console.ResetColor();
            }
            if (timeSpan >= TimeSpan.FromDays(365 * 3 - 180))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                TableRows(asset);

                Console.ResetColor();
            }
            else
            {
                TableRows(asset);
            }
        }
    }

}

