using System;
using System.IO;
using System.Collections;

namespace CollectionsTasks
{
    // Клас для представлення компакт-диску у каталозі
    public class CD
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        // Список пісень зберігаємо в ArrayList для відповідності до завдання 3/4
        public ArrayList Songs { get; } = new ArrayList();
    }

    class Program
    {
        // Головна таблиця каталогу на основі Hashtable
        static Hashtable catalog = new Hashtable();

        static void Main(string[] args)
        {
            // Створення тестових файлів для демонстрації роботи програми
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

        // 1. Переписати числа у зворотному порядку за допомогою Stack
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
                    {
                        // Перевіряємо, чи є частина числом, і кладемо в стек
                        if (double.TryParse(part, out _))
                            stack.Push(part);
                    }
                }
            }

            // Виймаємо зі стеку (LIFO) та записуємо у новий файл
            using (var writer = new StreamWriter("output1.txt"))
            {
                while (stack.Count > 0)
                    writer.WriteLine(stack.Pop());
            }

            Console.WriteLine("Файл output1.txt створено. Вміст:");
            Console.WriteLine(File.ReadAllText("output1.txt").Trim());
        }

        // 2. Друкувати спочатку не-цифри, потім цифри (зберігаючи порядок) за допомогою Queue
        static void Task2_Queue()
        {
            Queue nonDigits = new Queue();
            Queue digits = new Queue();

            // ОДИН перегляд файлу (посимвольне читання)
            using (var reader = new StreamReader("input2.txt"))
            {
                int ch;
                while ((ch = reader.Read()) != -1)
                {
                    char c = (char)ch;
                    if (char.IsDigit(c))
                        digits.Enqueue(c);
                    else
                        nonDigits.Enqueue(c);
                }
            }

            Console.Write("Результат: ");
            // Спочатку виводимо чергу не-цифр
            while (nonDigits.Count > 0) Console.Write(nonDigits.Dequeue());
            // Потім чергу цифр
            while (digits.Count > 0) Console.Write(digits.Dequeue());
            Console.WriteLine();
        }

        // 3. Розв'язання задач 1 та 2 за допомогою ArrayList
        static void Task3_ArrayList()
        {
            // Частина 1: Зворотний порядок чисел
            Console.WriteLine("--- 3.1 Зворотний порядок чисел (ArrayList) ---");
            ArrayList numbers = new ArrayList();
            using (var reader = new StreamReader("input1.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string part in parts)
                        if (double.TryParse(part, out _)) numbers.Add(part);
                }
            }

            using (var writer = new StreamWriter("output1_al.txt"))
            {
                // Проходимо масив у зворотному напрямку
                for (int i = numbers.Count - 1; i >= 0; i--)
                    writer.WriteLine(numbers[i]);
            }
            Console.WriteLine("Вміст output1_al.txt: " + File.ReadAllText("output1_al.txt").Trim());

            // Частина 2: Групування символів
            Console.WriteLine("\n--- 3.2 Групування символів (ArrayList) ---");
            ArrayList nonDigits = new ArrayList();
            ArrayList digits = new ArrayList();
            using (var reader = new StreamReader("input2.txt"))
            {
                int ch;
                while ((ch = reader.Read()) != -1)
                {
                    char c = (char)ch;
                    if (char.IsDigit(c)) digits.Add(c);
                    else nonDigits.Add(c);
                }
            }

            Console.Write("Результат: ");
            foreach (object obj in nonDigits) Console.Write((char)obj);
            foreach (object obj in digits) Console.Write((char)obj);
            Console.WriteLine();
        }

        // 4. Каталог музичних компакт-дисків на основі Hashtable
        static void Task4_Hashtable()
        {
            // Додавання дисків
            AddCD("The Dark Side of the Moon", "Pink Floyd");
            AddCD("Thriller", "Michael Jackson");
            AddCD("Abbey Road", "The Beatles");

            // Додавання пісень
            AddSong("The Dark Side of the Moon", "Speak to Me");
            AddSong("The Dark Side of the Moon", "Breathe");
            AddSong("The Dark Side of the Moon", "Time");
            AddSong("Thriller", "Wanna Be Startin' Somethin'");
            AddSong("Thriller", "Thriller");
            AddSong("Thriller", "Beat It");
            AddSong("Abbey Road", "Come Together");
            AddSong("Abbey Road", "Something");

            Console.WriteLine("1. Вміст каталогу:");
            PrintCatalog();

            Console.WriteLine("\n2. Перегляд окремого диска (Thriller):");
            PrintCD("Thriller");

            Console.WriteLine("\n3. Пошук за виконавцем (The Beatles):");
            SearchByArtist("The Beatles");

            Console.WriteLine("\n4. Видалення пісні та диска:");
            RemoveSong("Thriller", "Thriller");
            RemoveCD("Abbey Road");
            Console.WriteLine("Оновлений каталог після видалень:");
            PrintCatalog();
        }

        // Додати диск у каталог
        static void AddCD(string title, string artist)
        {
            if (!catalog.ContainsKey(title))
                catalog[title] = new CD { Title = title, Artist = artist };
            else
                Console.WriteLine($"[!] Диск '{title}' вже існує.");
        }

        // Видалити диск з каталогу
        static void RemoveCD(string title)
        {
            if (catalog.ContainsKey(title))
            {
                catalog.Remove(title);
                Console.WriteLine($"[OK] Диск '{title}' видалено.");
            }
            else
                Console.WriteLine($"[!] Диск '{title}' не знайдено.");
        }

        // Додати пісню до конкретного диска
        static void AddSong(string cdTitle, string song)
        {
            if (catalog.ContainsKey(cdTitle))
            {
                var cd = (CD)catalog[cdTitle];
                if (!cd.Songs.Contains(song))
                    cd.Songs.Add(song);
            }
        }

        // Видалити пісню з конкретного диска
        static void RemoveSong(string cdTitle, string song)
        {
            if (catalog.ContainsKey(cdTitle))
            {
                var cd = (CD)catalog[cdTitle];
                cd.Songs.Remove(song);
            }
        }

        // Перегляд всього каталогу
        static void PrintCatalog()
        {
            if (catalog.Count == 0) { Console.WriteLine("Каталог порожній."); return; }
            foreach (DictionaryEntry entry in catalog)
            {
                CD cd = (CD)entry.Value;
                Console.WriteLine($"📀 [{cd.Artist}] - {cd.Title} ({cd.Songs.Count} пісень)");
            }
        }

        // Перегляд вмісту одного диска
        static void PrintCD(string title)
        {
            if (catalog.ContainsKey(title))
            {
                CD cd = (CD)catalog[title];
                Console.WriteLine($"📀 Диск: {cd.Title}");
                Console.WriteLine($"🎤 Виконавець: {cd.Artist}");
                Console.WriteLine("🎵 Список пісень:");
                foreach (string song in cd.Songs)
                    Console.WriteLine($"   - {song}");
            }
            else
                Console.WriteLine($"Диск '{title}' не знайдено.");
        }

        // Пошук усіх записів заданого виконавця по всьому каталогу
        static void SearchByArtist(string artist)
        {
            bool found = false;
            foreach (DictionaryEntry entry in catalog)
            {
                CD cd = (CD)entry.Value;
                if (cd.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase))
                {
                    PrintCD(cd.Title);
                    found = true;
                }
            }
            if (!found) Console.WriteLine($"Записів виконавця '{artist}' не знайдено.");
        }
    }
}