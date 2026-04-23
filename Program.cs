using System;
using System.IO;
using System.Collections;
using System.Windows.Forms; // Для WinForms (розкоментуйте при потребі)

namespace CollectionsLab9
{
    //=========================================================================
    // Інтерфейс для уніфікованого запуску задач
    //=========================================================================
    public interface ILabTask
    {
        string Title { get; }
        string Description { get; }
        void Run();
    }

    //=========================================================================
    // Клас CD для каталогу (спільний для Task4)
    //=========================================================================
    public class CD
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public ArrayList Songs { get; } = new ArrayList();
        
        public CD(string title, string artist)
        {
            Title = title;
            Artist = artist;
        }
        
        public void AddSong(string song)
        {
            if (!Songs.Contains(song)) Songs.Add(song);
        }
        
        public bool RemoveSong(string song) => Songs.Remove(song);
        
        public override string ToString() => $"📀 [{Artist}] - {Title} ({Songs.Count} пісень)";
    }

    //=========================================================================
    // ЗАВДАННЯ 1: Stack - числа у зворотному порядку
    //=========================================================================
    public class Lab9T1 : ILabTask
    {
        public string Title => "Завдання 1: Stack";
        public string Description => "Переписати числа з файлу у зворотному порядку за допомогою Stack";
        
        private string inputFile = "input1.txt";
        private string outputFile = "output1.txt";

        public void Run()
        {
            Console.WriteLine($"\n▶ {Title}");
            Console.WriteLine(Description);
            
            // Створення тестового файлу, якщо не існує
            if (!File.Exists(inputFile))
                File.WriteAllText(inputFile, "12 45 7\n89 10 23");
            
            Stack stack = new Stack();
            
            try
            {
                // Читання чисел у стек
                using (var reader = new StreamReader(inputFile))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string part in parts)
                            if (double.TryParse(part, out _)) stack.Push(part);
                    }
                }

                // Запис у зворотному порядку
                using (var writer = new StreamWriter(outputFile))
                {
                    while (stack.Count > 0)
                        writer.WriteLine(stack.Pop());
                }

                Console.WriteLine($"✅ Файл '{outputFile}' створено. Вміст:");
                Console.WriteLine(new string('-', 40));
                Console.WriteLine(File.ReadAllText(outputFile).Trim());
                Console.WriteLine(new string('-', 40));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Помилка: {ex.Message}");
            }
        }
    }

    //=========================================================================
    // ЗАВДАННЯ 2: Queue - групування символів
    //=========================================================================
    public class Lab9T2 : ILabTask
    {
        public string Title => "Завдання 2: Queue";
        public string Description => "Вивести спочатку не-цифри, потім цифри (зберігаючи порядок) за допомогою Queue";
        
        private string inputFile = "input2.txt";

        public void Run()
        {
            Console.WriteLine($"\n▶ {Title}");
            Console.WriteLine(Description);
            
            // Створення тестового файлу, якщо не існує
            if (!File.Exists(inputFile))
                File.WriteAllText(inputFile, "Hello123World456Test789");
            
            Queue nonDigits = new Queue();
            Queue digits = new Queue();

            try
            {
                // ОДИН перегляд файлу - посимвольне читання
                using (var reader = new StreamReader(inputFile))
                {
                    int ch;
                    while ((ch = reader.Read()) != -1)
                    {
                        char c = (char)ch;
                        if (char.IsDigit(c)) digits.Enqueue(c);
                        else nonDigits.Enqueue(c);
                    }
                }

                Console.WriteLine($"\n📄 Вхідний файл: {File.ReadAllText(inputFile).Trim()}");
                Console.Write("\n📤 Результат: ");
                
                while (nonDigits.Count > 0) Console.Write(nonDigits.Dequeue());
                while (digits.Count > 0) Console.Write(digits.Dequeue());
                Console.WriteLine("\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Помилка: {ex.Message}");
            }
        }
    }

    //=========================================================================
    // ЗАВДАННЯ 3: ArrayList - альтернативна реалізація задач 1+2
    //=========================================================================
    public class Lab9T3 : ILabTask
    {
        public string Title => "Завдання 3: ArrayList";
        public string Description => "Розв'язання задач 1 та 2 за допомогою ArrayList";
        
        private string inputNums = "input1.txt";
        private string inputChars = "input2.txt";
        private string outputNums = "output1_al.txt";

        public void Run()
        {
            Console.WriteLine($"\n▶ {Title}");
            Console.WriteLine(Description);
            
            // Створення тестових файлів
            if (!File.Exists(inputNums)) File.WriteAllText(inputNums, "12 45 7\n89 10 23");
            if (!File.Exists(inputChars)) File.WriteAllText(inputChars, "Hello123World456Test789");
            
            // --- Частина 3.1: Зворотний порядок чисел ---
            Console.WriteLine("\n🔹 3.1 Зворотний порядок чисел (ArrayList)");
            ArrayList numbers = new ArrayList();
            
            try
            {
                using (var reader = new StreamReader(inputNums))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string part in parts)
                            if (double.TryParse(part, out _)) numbers.Add(part);
                    }
                }

                using (var writer = new StreamWriter(outputNums))
                    for (int i = numbers.Count - 1; i >= 0; i--)
                        writer.WriteLine(numbers[i]);

                Console.WriteLine($"✅ Вміст '{outputNums}':");
                Console.WriteLine(File.ReadAllText(outputNums).Trim());
            }
            catch (Exception ex) { Console.WriteLine($"❌ Помилка: {ex.Message}"); }

            // --- Частина 3.2: Групування символів ---
            Console.WriteLine("\n🔹 3.2 Групування символів (ArrayList)");
            ArrayList nonDigits = new ArrayList();
            ArrayList digits = new ArrayList();
            
            try
            {
                using (var reader = new StreamReader(inputChars))
                {
                    int ch;
                    while ((ch = reader.Read()) != -1)
                    {
                        char c = (char)ch;
                        if (char.IsDigit(c)) digits.Add(c);
                        else nonDigits.Add(c);
                    }
                }

                Console.Write("📤 Результат: ");
                foreach (object obj in nonDigits) Console.Write((char)obj);
                foreach (object obj in digits) Console.Write((char)obj);
                Console.WriteLine("\n");
            }
            catch (Exception ex) { Console.WriteLine($"❌ Помилка: {ex.Message}"); }
        }
    }

    //=========================================================================
    // ЗАВДАННЯ 4: Hashtable - каталог музичних дисків
    //=========================================================================
    public class Lab9T4 : ILabTask
    {
        public string Title => "Завдання 4: Hashtable";
        public string Description => "Каталог компакт-дисків: додавання, видалення, пошук за виконавцем";
        
        // Каталог з нечутливим до регістру порівнянням ключів
        private Hashtable catalog = new Hashtable(StringComparer.OrdinalIgnoreCase);

        public void Run()
        {
            Console.WriteLine($"\n▶ {Title}");
            Console.WriteLine(Description);
            
            // Демонстраційні дані
            InitializeDemoData();
            ShowMenu();
        }

        private void InitializeDemoData()
        {
            AddCD("The Dark Side of the Moon", "Pink Floyd");
            AddCD("Thriller", "Michael Jackson");
            AddCD("Abbey Road", "The Beatles");

            AddSong("The Dark Side of the Moon", "Speak to Me");
            AddSong("The Dark Side of the Moon", "Breathe");
            AddSong("The Dark Side of the Moon", "Time");
            
            AddSong("Thriller", "Wanna Be Startin' Somethin'");
            AddSong("Thriller", "Thriller");
            AddSong("Thriller", "Beat It");
            
            AddSong("Abbey Road", "Come Together");
            AddSong("Abbey Road", "Something");
        }

        private void ShowMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n" + new string('═', 50));
                Console.WriteLine("🎵 КАТАЛОГ МУЗИЧНИХ ДИСКІВ");
                Console.WriteLine(new string('═', 50));
                Console.WriteLine("1. 📋 Перегляд всього каталогу");
                Console.WriteLine("2. 🔍 Перегляд конкретного диска");
                Console.WriteLine("3. 🎤 Пошук за виконавцем");
                Console.WriteLine("4. ➕ Додати новий диск");
                Console.WriteLine("5. 🗑️  Видалити диск");
                Console.WriteLine("6. 🎵 Додати пісню до диска");
                Console.WriteLine("7. ❌ Видалити пісню з диска");
                Console.WriteLine("0. 🔙 Вихід в головне меню");
                Console.Write("\nВаш вибір: ");
                
                switch (Console.ReadLine()?.Trim())
                {
                    case "1": PrintCatalog(); break;
                    case "2": 
                        Console.Write("Назва диска: ");
                        PrintCD(Console.ReadLine()); 
                        break;
                    case "3":
                        Console.Write("Виконавець: ");
                        SearchByArtist(Console.ReadLine());
                        break;
                    case "4":
                        Console.Write("Назва диску: ");
                        string title = Console.ReadLine();
                        Console.Write("Виконавець: ");
                        string artist = Console.ReadLine();
                        AddCD(title, artist);
                        break;
                    case "5":
                        Console.Write("Назва диску для видалення: ");
                        RemoveCD(Console.ReadLine());
                        break;
                    case "6":
                        Console.Write("Назва диску: ");
                        string cdTitle = Console.ReadLine();
                        Console.Write("Назва пісні: ");
                        string song = Console.ReadLine();
                        AddSong(cdTitle, song);
                        break;
                    case "7":
                        Console.Write("Назва диску: ");
                        string cdT = Console.ReadLine();
                        Console.Write("Назва пісні: ");
                        string sng = Console.ReadLine();
                        RemoveSong(cdT, sng);
                        break;
                    case "0": exit = true; break;
                    default: Console.WriteLine("⚠️  Невірний вибір!"); break;
                }
            }
        }

        private void AddCD(string title, string artist)
        {
            if (!catalog.ContainsKey(title))
            {
                catalog[title] = new CD(title, artist);
                Console.WriteLine($"✅ Диск '{title}' додано.");
            }
            else Console.WriteLine($"⚠️  Диск '{title}' вже існує.");
        }

        private void RemoveCD(string title)
        {
            if (catalog.ContainsKey(title))
            {
                catalog.Remove(title);
                Console.WriteLine($"✅ Диск '{title}' видалено.");
            }
            else Console.WriteLine($"⚠️  Диск '{title}' не знайдено.");
        }

        private void AddSong(string cdTitle, string song)
        {
            if (catalog[cdTitle] is CD cd)
            {
                cd.AddSong(song);
                Console.WriteLine($"✅ Пісню '{song}' додано до '{cdTitle}'.");
            }
            else Console.WriteLine($"⚠️  Диск '{cdTitle}' не знайдено.");
        }

        private void RemoveSong(string cdTitle, string song)
        {
            if (catalog[cdTitle] is CD cd)
            {
                if (cd.RemoveSong(song))
                    Console.WriteLine($"✅ Пісню '{song}' видалено з '{cdTitle}'.");
                else Console.WriteLine($"⚠️  Пісня '{song}' не знайдена в диску '{cdTitle}'.");
            }
            else Console.WriteLine($"⚠️  Диск '{cdTitle}' не знайдено.");
        }

        private void PrintCatalog()
        {
            Console.WriteLine("\n" + new string('─', 50));
            if (catalog.Count == 0) { Console.WriteLine("📭 Каталог порожній."); return; }
            
            foreach (DictionaryEntry entry in catalog)
                Console.WriteLine(((CD)entry.Value).ToString());
            Console.WriteLine(new string('─', 50));
        }

        private void PrintCD(string title)
        {
            Console.WriteLine();
            if (catalog[title] is CD cd)
            {
                Console.WriteLine($"📀 Диск: {cd.Title}");
                Console.WriteLine($"🎤 Виконавець: {cd.Artist}");
                Console.WriteLine($"🎵 Пісні ({cd.Songs.Count}):");
                if (cd.Songs.Count == 0) Console.WriteLine("   (немає пісень)");
                else foreach (string song in cd.Songs) Console.WriteLine($"   • {song}");
            }
            else Console.WriteLine($"⚠️  Диск '{title}' не знайдено.");
        }

        private void SearchByArtist(string artist)
        {
            Console.WriteLine($"\n🔎 Результати пошуку '{artist}':");
            Console.WriteLine(new string('─', 50));
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
            if (!found) Console.WriteLine("📭 Записів не знайдено.");
            Console.WriteLine(new string('─', 50));
        }
    }

    //=========================================================================
    // ГОЛОВНА ПРОГРАМА: Меню та запуск задач
    //=========================================================================
    class Program
    {
        // Реєстрація всіх задач
        private static readonly ILabTask[] tasks = new ILabTask[]
        {
            new Lab9T1(),
            new Lab9T2(),
            new Lab9T3(),
            new Lab9T4()
        };

        [STAThread] // Необхідно для WinForms
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            // 🔘 ВИБЕРІТЬ РЕЖИМ: розкоментуйте потрібний варіант
            
            // === Варіант 1: Консольний режим (за замовчуванням) ===
            RunConsoleMode();
            
            // === Варіант 2: WinForms режим (розкоментуйте для активації) ===
            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Lab9Form());
        }

        //=====================================================================
        // Консольний режим з інтерактивним меню
        //=====================================================================
        static void RunConsoleMode()
        {
            bool exit = false;
            
            Console.WriteLine("╔" + new string('═', 58) + "╗");
            Console.WriteLine("║" + CenterText("ЛАБОРАТОРНА РОБОТА №9: КОЛЕКЦІЇ В C#", 58) + "║");
            Console.WriteLine("╚" + new string('═', 58) + "╝\n");
            
            while (!exit)
            {
                Console.WriteLine("\n📋 ОБЕРІТЬ ЗАВДАННЯ:");
                Console.WriteLine(new string('─', 60));
                
                for (int i = 0; i < tasks.Length; i++)
                    Console.WriteLine($"{i + 1}. {tasks[i].Title}\n   {tasks[i].Description}");
                
                Console.WriteLine($"{tasks.Length + 1}. 🔄 Запустити всі задачі послідовно");
                Console.WriteLine($"0. 🔚 Вихід");
                Console.Write("\nВаш вибір [0-{0}]: ", tasks.Length + 1);
                
                string input = Console.ReadLine()?.Trim();
                
                if (int.TryParse(input, out int choice))
                {
                    if (choice >= 1 && choice <= tasks.Length)
                    {
                        tasks[choice - 1].Run();
                        Console.WriteLine("\n⏎ Натисніть будь-яку клавішу для продовження...");
                        Console.ReadKey();
                    }
                    else if (choice == tasks.Length + 1)
                    {
                        // 🔄 Багатозадачний режим: послідовний запуск всіх задач
                        Console.WriteLine("\n🚀 ЗАПУСК ВСІХ ЗАВДАНЬ ПОСЛІДОВНО...");
                        foreach (var task in tasks)
                        {
                            task.Run();
                            Console.WriteLine("\n⏎ Натисніть будь-яку клавішу для наступного завдання...");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        Console.WriteLine("✅ Всі завдання виконано!");
                    }
                    else if (choice == 0) { exit = true; }
                    else { Console.WriteLine("⚠️  Невірний вибір!"); }
                }
                else { Console.WriteLine("⚠️  Введіть число!"); }
            }
            
            Console.WriteLine("\n👋 Дякуємо за роботу! До побачення!");
        }

        // Допоміжний метод для центрування тексту
        static string CenterText(string text, int width)
        {
            if (string.IsNullOrEmpty(text) || width <= 0) return text;
            int padding = (width - text.Length) / 2;
            return new string(' ', padding) + text;
        }
    }

    //=========================================================================
    // 🪟 WinForms форма (опціонально - розкоментуйте в Main для використання)
    //=========================================================================
    /*
    public class Lab9Form : Form
    {
        private TabControl tabControl;
        private RichTextBox outputBox;
        
        public Lab9Form()
        {
            Text = "Лабораторна робота №9: Колекції в C#";
            Size = new System.Drawing.Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;
            
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            // Панель виводу
            outputBox = new RichTextBox
            {
                Dock = DockStyle.Bottom,
                Height = 200,
                ReadOnly = true,
                Font = new System.Drawing.Font("Consolas", 9),
                ScrollBars = RichTextBoxScrollBars.Vertical
            };
            
            // Вкладки для задач
            tabControl = new TabControl { Dock = DockStyle.Fill };
            
            // Додавання вкладок
            AddTaskTab(new Lab9T1(), "Stack");
            AddTaskTab(new Lab9T2(), "Queue");
            AddTaskTab(new Lab9T3(), "ArrayList");
            AddTaskTab(new Lab9T4(), "Hashtable");
            
            // Layout
            Controls.Add(tabControl);
            Controls.Add(outputBox);
        }
        
        private void AddTaskTab(ILabTask task, string tabName)
        {
            TabPage page = new TabPage(tabName);
            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10)
            };
            
            // Опис задачі
            Label descLabel = new Label
            {
                Text = task.Description,
                AutoSize = false,
                Width = 400,
                Height = 40
            };
            panel.Controls.Add(descLabel);
            
            // Кнопка запуску
            Button runBtn = new Button
            {
                Text = "▶ Запустити",
                Width = 120,
                Height = 35,
                Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold)
            };
            runBtn.Click += (s, e) => 
            {
                outputBox.Clear();
                RedirectOutput(task.Run);
            };
            panel.Controls.Add(runBtn);
            
            page.Controls.Add(panel);
            tabControl.TabPages.Add(page);
        }
        
        // Перехоплення Console.WriteLine для виводу в RichTextBox
        private void RedirectOutput(Action action)
        {
            var oldOut = Console.Out;
            var writer = new ControlWriter(outputBox);
            Console.SetOut(writer);
            
            try { action(); }
            finally { Console.SetOut(oldOut); }
        }
    }
    
    // Допоміжний клас для перенаправлення консольного виводу в WinForms
    public class ControlWriter : TextWriter
    {
        private readonly RichTextBox target;
        
        public ControlWriter(RichTextBox tb) { target = tb; }
        
        public override void Write(char value)
        {
            if (target.InvokeRequired)
                target.Invoke(new Action(() => target.AppendText(value.ToString())));
            else
                target.AppendText(value.ToString());
        }
        
        public override Encoding Encoding => Encoding.UTF8;
    }
    */
}
