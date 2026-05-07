using System;
using System.IO;
using System.Collections;

namespace CollectionsTasks
{
    public class CD
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public ArrayList Songs { get; } = new ArrayList();
    }

    class Program
    {
        static Hashtable catalog = new Hashtable();

        static void Main(string[] args)
        {
            // Попереднє заповнення даними для прикладу
            SeedData();

            File.WriteAllText("input1.txt", "12 45 7\n89 10 23");
            File.WriteAllText("input2.txt", "Hello123World456Test789");

            Console.WriteLine("=== ЗАВДАННЯ 1: Використання Stack ===");
            Task1_Stack();

            Console.WriteLine("\n=== ЗАВДАННЯ 2: Використання Queue ===");
            Task2_Queue();

            Console.WriteLine("\n=== ЗАВДАННЯ 3: Використання ArrayList ===");
            Task3_ArrayList();

            Console.WriteLine("\n=== ЗАВДАННЯ 4: Використання Hashtable (Музичний каталог) ===");
            Task4_Hashtable();
        }

        // МЕТОД ДЛЯ ПРИКЛАДІВ
        static void SeedData()
        {
            // Додаємо Queen
            AddCD("A Night at the Opera", "Queen");
            AddSong("A Night at the Opera", "Bohemian Rhapsody");
            AddSong("A Night at the Opera", "You're My Best Friend");
            AddSong("A Night at the Opera", "Love of My Life");

            // Додаємо Pink Floyd
            AddCD("The Dark Side of the Moon", "Pink Floyd");
            AddSong("The Dark Side of the Moon", "Time");
            AddSong("The Dark Side of the Moon", "Money");
            AddSong("The Dark Side of the Moon", "Us and Them");

            // Додаємо Океан Ельзи
            AddCD("Суперсиметрія", "Океан Ельзи");
            AddSong("Суперсиметрія", "Кішка");
            AddSong("Суперсиметрія", "911");
            AddSong("Суперсиметрія", "Холодно");
        }

        // --- ДАЛІ ВАШІ МЕТОДИ БЕЗ ЗМІН (АБО З КОРИГУВАННЯМИ) ---

        static void Task1_Stack()
        {
            Stack stack = new Stack();
            using (var reader = new StreamReader("input1.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string part in parts)
                        if (double.TryParse(part, out _)) stack.Push(part);
                }
            }
            using (var writer = new StreamWriter("output1.txt"))
            {
                while (stack.Count > 0) writer.WriteLine(stack.Pop());
            }
            Console.WriteLine("Файл output1.txt створено.");
        }

        static void Task2_Queue()
        {
            Queue nonDigits = new Queue();
            Queue digits = new Queue();
            using (var reader = new StreamReader("input2.txt"))
            {
                int ch;
                while ((ch = reader.Read()) != -1)
                {
                    char c = (char)ch;
                    if (char.IsDigit(c)) digits.Enqueue(c);
                    else nonDigits.Enqueue(c);
                }
            }
            Console.Write("Результат: ");
            while (nonDigits.Count > 0) Console.Write(nonDigits.Dequeue());
            while (digits.Count > 0) Console.Write(digits.Dequeue());
            Console.WriteLine();
        }

        static void Task3_ArrayList()
        {
            Console.WriteLine("--- 3.1 Зворотний порядок чисел (ArrayList) ---");
            ArrayList numbers = new ArrayList();
            if (File.Exists("input1.txt"))
            {
                string[] lines = File.ReadAllLines("input1.txt");
                foreach (var line in lines)
                {
                    string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string part in parts) numbers.Add(part);
                }
                for (int i = numbers.Count - 1; i >= 0; i--) Console.Write(numbers[i] + " ");
                Console.WriteLine();
            }
        }

        static void Task4_Hashtable()
        {
            InitializeSampleData();
    
            bool exit = false;
            
            while (!exit)
            {
                Console.WriteLine("\n=== 🎵 МУЗИЧНИЙ КАТАЛОГ (Hashtable) ===");
                Console.WriteLine("1. Додати новий диск");
                Console.WriteLine("2. Додати пісню до диска");
                Console.WriteLine("3. Переглянути весь каталог");
                Console.WriteLine("4. Переглянути конкретний диск");
                Console.WriteLine("5. Пошук за виконавцем");
                Console.WriteLine("6. Видалити пісню з диска");
                Console.WriteLine("7. Видалити диск з каталогу");
                Console.WriteLine("8. Вийти");
                Console.Write("\nВаш вибір: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Назва диска: "); string t = Console.ReadLine();
                        Console.Write("Виконавець: "); string a = Console.ReadLine();
                        AddCD(t, a); break;
                    case "2":
                        Console.Write("Назва диска: "); string ct = Console.ReadLine();
                        Console.Write("Пісня: "); string s = Console.ReadLine();
                        AddSong(ct, s); break;
                    case "3": PrintCatalog(); break;
                    case "4":
                        Console.Write("Назва диска: "); PrintCD(Console.ReadLine()); break;
                    case "5":
                        Console.Write("Виконавець: "); SearchByArtist(Console.ReadLine()); break;
                    case "6":
                        Console.Write("Назва диска: "); string rct = Console.ReadLine();
                        Console.Write("Пісня: "); string rs = Console.ReadLine();
                        RemoveSong(rct, rs); break;
                    case "7":
                        Console.Write("Назва диска: "); RemoveCD(Console.ReadLine()); break;
                    case "8": exit = true; break;
                }
            }
        }

        static void AddCD(string title, string artist)
        {
            if (string.IsNullOrEmpty(title)) return;
            if (!catalog.ContainsKey(title))
                catalog[title] = new CD { Title = title, Artist = artist };
        }

        static void AddSong(string cdTitle, string song)
        {
            if (catalog.ContainsKey(cdTitle))
            {
                var cd = (CD)catalog[cdTitle];
                if (!cd.Songs.Contains(song)) cd.Songs.Add(song);
            }
        }

        static void RemoveCD(string title)
        {
            if (catalog.ContainsKey(title)) catalog.Remove(title);
        }

        static void RemoveSong(string cdTitle, string song)
        {
            if (catalog.ContainsKey(cdTitle))
                ((CD)catalog[cdTitle]).Songs.Remove(song);
        }

        static void PrintCatalog()
        {
            if (catalog.Count == 0) Console.WriteLine("Порожньо.");
            foreach (DictionaryEntry entry in catalog)
            {
                CD cd = (CD)entry.Value;
                Console.WriteLine($"📀 [{cd.Artist}] - {cd.Title} ({cd.Songs.Count} пісень)");
            }
        }

        static void PrintCD(string title)
        {
            if (catalog.ContainsKey(title))
            {
                CD cd = (CD)catalog[title];
                Console.WriteLine($"\n📀 {cd.Title} - {cd.Artist}");
                foreach (string s in cd.Songs) Console.WriteLine($"  ♪ {s}");
            }
            else Console.WriteLine("Не знайдено.");
        }

        static void SearchByArtist(string artist)
        {
            foreach (DictionaryEntry entry in catalog)
            {
                CD cd = (CD)entry.Value;
                if (cd.Artist.IndexOf(artist, StringComparison.OrdinalIgnoreCase) >= 0)
                    PrintCD(cd.Title);
            }
        }
        // Додайте цей метод у клас Program (після інших методів)
        static void InitializeSampleData()
        {
            // Альбом 1: The Beatles - Abbey Road
            AddCD("Abbey Road", "The Beatles");
            AddSong("Abbey Road", "Come Together");
            AddSong("Abbey Road", "Something");
            AddSong("Abbey Road", "Here Comes the Sun");
            AddSong("Abbey Road", "I Want You (She's So Heavy)");
            AddSong("Abbey Road", "Golden Slumbers");

            // Альбом 2: Pink Floyd - The Dark Side of the Moon
            AddCD("The Dark Side of the Moon", "Pink Floyd");
            AddSong("The Dark Side of the Moon", "Speak to Me");
            AddSong("The Dark Side of the Moon", "Breathe");
            AddSong("The Dark Side of the Moon", "Time");
            AddSong("The Dark Side of the Moon", "Money");
            AddSong("The Dark Side of the Moon", "Us and Them");
            AddSong("The Dark Side of the Moon", "Brain Damage");

            // Альбом 3: Queen - A Night at the Opera
            AddCD("A Night at the Opera", "Queen");
            AddSong("A Night at the Opera", "Death on Two Legs");
            AddSong("A Night at the Opera", "Lazing on a Sunday Afternoon");
            AddSong("A Night at the Opera", "Bohemian Rhapsody");
            AddSong("A Night at the Opera", "You're My Best Friend");
            AddSong("A Night at the Opera", "God Save the Queen");

            // Альбом 4: Український приклад: Океан Ельзи - Суперсиметрія
            AddCD("Суперсиметрія", "Океан Ельзи");
            AddSong("Суперсиметрія", "Там, де нас нема");
            AddSong("Суперсиметрія", "Відпусти");
            AddSong("Суперсиметрія", "Я йду");
            AddSong("Суперсиметрія", "Не питай");

            Console.WriteLine("✅ Завантажено 4 прикладні альбоми до каталогу.");
        }
    }
}
