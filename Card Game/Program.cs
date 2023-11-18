using System;
using System.Collections.Generic;


class Card
{
    public string Suit { get; set; }
    public string Rank { get; set; }

    public Card(string suit, string rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public override string ToString()
    {
        return $"{Rank} {Suit}";
    }
}

class Player
{
    public string Name { get; set; }
    public List<Card> Cards { get; set; }

    public Player(string name)
    {
        Name = name;
        Cards = new List<Card>();
    }

    public void DisplayCards()
    {
        Console.WriteLine($"{Name}'s cards: {string.Join(", ", Cards)}");
    }
}

class Game
{
    private List<Player> players;
    private List<Card> deck;

    public Game(List<Player> players)
    {
        this.players = players;
        deck = GenerateDeck();
        ShuffleDeck();
        DistributeCards();
    }

    private List<Card> GenerateDeck()
    {
        List<Card> generatedDeck = new List<Card>();
        string[] suits = new string[] { "Hearts", "Diamonds", "Clubs", "Spades" };
        string[] ranks = new string[] { "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                generatedDeck.Add(new Card(suit, rank));
            }
        }

        return generatedDeck;
    }

    private void ShuffleDeck()
    {
        Random random = new Random();
        deck = deck.OrderBy(card => random.Next()).ToList();
    }

    private void DistributeCards()
    {
        int numPlayers = players.Count;
        int cardsPerPlayer = deck.Count / numPlayers;

        for (int i = 0; i < deck.Count; i += cardsPerPlayer)
        {
            for (int j = 0; j < numPlayers; j++)
            {
                players[j].Cards.Add(deck[i + j]);
            }
        }
    }

    public void PlayGame()
    {
        while (players.All(player => player.Cards.Any()))
        {
            List<Card> cardsOnTable = players.Select(player => player.Cards.First()).ToList();

            Console.WriteLine("Cards on the table: " + string.Join(", ", cardsOnTable));

            Card maxCard = cardsOnTable.OrderByDescending(card => Array.IndexOf(new string[] { "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" }, card.Rank)).ThenBy(card => card.Rank).First();
            int winningPlayerIndex = cardsOnTable.IndexOf(maxCard);

            if (cardsOnTable.Any(card => card.Rank == "6") && maxCard.Rank == "Ace")
            {
                winningPlayerIndex = (winningPlayerIndex + 1) % players.Count;
            }

            Console.WriteLine($"{players[winningPlayerIndex].Name} takes the cards!\n");
            players[winningPlayerIndex].Cards.AddRange(cardsOnTable);

            foreach (Player player in players)
            {
                player.Cards.RemoveAt(0);
            }
        }

        Player winner = players.OrderByDescending(player => player.Cards.Count).First();
        Console.WriteLine($"{winner.Name} wins the game!");
    }
}

class Program
{
    static void Main()
    {
        Player player1 = new Player("Player 1");
        Player player2 = new Player("Player 2");

        List<Player> players = new List<Player> { player1, player2 };

        Game game = new Game(players);
        game.PlayGame();
    }
}
