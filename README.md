# ShadowSharp
Simple .Net Wrapper for some of [Shadowverse Portal's](https://shadowverse-portal.com "Shadowverse Portal Homepage") API Endpoints

# Example
```cs
namespace Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new ShadowSharpClient();
            //Deck code is created via the game
            var data = await client.GetCardDeckAsync("qi7i");
            //Display some data in the Console
            Console.WriteLine($"First 5 Cards: {string.Join(" | ", data.Cards.Take(5).Select(x => $"Name: {x.Name}, Description: {x.Description}"))}");
            Console.ReadKey();
        }
    }
}
```
