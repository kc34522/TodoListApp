﻿using System; //輸入/輸出、數學運算、時間處理
using System.Collections.Generic; // List、Dictionary 等集合
using System.Formats.Asn1;
using System.IO;// 引入檔案讀寫
using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
// using System.Diagnostics; // 不太需要，除非你要記錄 Debug 訊息。
// using System.Net.NetworkInformation; // 除非你要寫網路程式，否則可以刪掉。



// C# 是物件導向語言，所有程式碼必需放在class（類別）裡。類別名稱可以是任意名稱。（Program只C#預設的名稱）C#主程式一定要有一個類別（class）且這個類別裡要有main方法，程式才會執行。（.Net 6+之後可以省略class, Main方法，但這種寫法只適合簡單的小程式，如果你的程式有多個類別，還是建議寫完整的 class Program。）
class Program
{

    class Todo
    {
        public int Id { get; set; } // 可以讀取 & 修改（適用於一般資料)
        public string Task { get; set; }
        public bool IsCompleted { get; set; }

        // 建構子：初始化待辦事項
        // 這裡的 Todo 其實是建構函式（Constructor），它的名稱必須跟類別 Todo 一樣，這不是剛好，而是 C# 的規則！
        // 語法規則：
        // 1. 名稱必須跟類別名相同。（這是規定！）
        // 2. 不需要寫 void 或 return，因為建構函式不會回傳值。
        // 3. 可以有參數，用來初始化物件屬性。
        public Todo(int id, string task)
        {
            Id = id;
            Task = task;
            IsCompleted = false;
        }
    }

    // 設定檔案名稱
    // static string todoFilePath = "todoList.txt";   // 未完成待辦事項
    // static string doneFilePath = "doneList.txt";   // 已完成待辦事項

    static string todoFilePath = "todoList.json"; // 存所有待辦事項（包含已完成的）


    static void LoadTodos()
    {
        if (File.Exists(todoFilePath))
        {
            string json = File.ReadAllText(todoFilePath);
            todoList = JsonSerializer.Deserialize<List<Todo>>(json) ?? new List<Todo>();
        }

        
        // if (File.Exists(todoFilePath))
        // {
        //     todoList = new List<string>(File.ReadAllLines(todoFilePath)); // 讀取檔案（陣列），讀取並回傳 string[]
        // }

        // if (File.Exists(doneFilePath))
        // {
        //     doneTodoList = new List<string>(File.ReadAllLines(doneFilePath));
        // }
    }

    static void SaveTodos()
    {
        // 將 todoList 轉換為 JSON 格式的 字串（string）。
        // new JsonSerializerOptions { WriteIndented = true }：這會讓 JSON 格式變得好閱讀（會加上縮排）。
        string json = JsonSerializer.Serialize(todoList, new JsonSerializerOptions{WriteIndented = true});

        // 儲存 JSON 時，請用 WriteAllText()，不管是單行或多行格式，JSON 都可以正確解析！
        // 這個方法適用於 多行純文字，但 不適合 JSON，因為 JSON 本來就是一個完整的字串，而 WriteAllLines() 會把 List<string> 的每個元素當成 一行獨立的字串，導致 JSON 格式錯誤。
        File.WriteAllText(todoFilePath, json);



        // File.WriteAllLines(todoFilePath, todoList); // 寫入多行。檔案不存在時：建立新檔案 並寫入; 檔案存在時的行為：覆蓋舊內容。
        // File.WriteAllLines(doneFilePath, doneTodoList);
    }


    // 讓 todoList 變成「全域變數」，整個 Program 類別都可以存取它。
    // 如果放在 Main 方法內，todoList 只能在 Main 裡面用，其他方法無法存取。
    // 因為 Main 是 static，所以 todoList 也必須是 static。
    // C# 不允許 static 方法（Main）存取非 static 的變數。
    // List<Todo>：表示「存放 Todo 物件的清單」，但 List<Todo> 本身不是 Todo。
    static List<Todo> todoList = new List<Todo>(); // 儲存待辦事項

    // Main方法為程式的進入點（entry point）程式是從這一行開始跑。（看別人的程式碼也可以去找main（））
    // 不管類別名稱是什麼，只要有這個Main方法就可以執行。（如果沒有 Main 方法，C# 就不知道從哪裡開始執行程式，會出錯！）
    // static: 不需要建立物件，程式可以直接執行。（因為 程式啟動時，C# 還沒建立任何物件，所以 Main 必須是 static，讓 .NET 直接執行它。）
    // void: 不回傳值，但可以改成 int 回傳狀態碼。
    // 如果方法 只是顯示內容 或 修改變數，但不需要回傳值(return)，就可以用 void。
    static void Main()
    {
        LoadTodos(); // 讀取檔案，載入之前存的待辦事項

        while (true)
        {
            // Console也是class，WriteLine是裡面的方法。
            Console.WriteLine("\n📌 To-Do List");
            // 方法（method）= 函式（function）
            // 方法（Method） 是 類別（Class） 裡的一個函式。
            // 函式（Function） 是比較廣義的詞，指的是「回傳值 + 參數 + 執行邏輯」的組合。
            // ShowTodos(); = 方法的呼叫(method call)
            // ShowTodos() = 方法的定義(method definition)
            // FinishTodos();
            ShowTodos();
            Console.WriteLine("\n1. 新增事項  2. 刪除事項  3. 已完成事項 4. 退出");
            Console.Write("請選擇: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTodo();
                    break; // 用來跳出當前迴圈(loop)，但不會結束整個方法。
                case "2":
                    RemoveTodo();
                    break;
                case "3":
                    DoneToDo();
                    break;
                case "4":
                    SaveTodos(); // 離開前存檔
                    return; // 退出程式 // 用來結束整個方法（Main、其他函式等），並回傳結果（如果方法有回傳值）。

                default:
                    Console.WriteLine("❌ 無效的選擇");
                    break;
            }
        }
    }

    // static bool haveDoneToDo = false;
    // haveDoneToDo 變數其實不太必要。你目前使用 haveDoneToDo 來檢查是否有完成的待辦事項，但 doneTodoList.Count > 0 就能知道是否有完成事項了，所以可以移除 haveDoneToDo。
    // static List<string> doneTodoList = new List<string>();

    // static void FinishTodos()
    // {
    //     if (doneTodoList.Count > 0)
    //     {
    //         Console.WriteLine("\n✅ 已完成事項:");

    //         foreach (string task in doneTodoList)
    //         {
    //             Console.WriteLine($"✅ {task}");
    //         }
    //     }
    // }

    // 一個完整的方法（函式）包含：
    // [存取修飾詞] [static] [回傳類型] 方法名稱([參數]) 
    //{
    // 方法內部的程式碼
    // return 回傳值; // 如果回傳類型是 void，這一行可以省略
    //}
    //如果 Main 是 static，它只能呼叫 static 方法。
    static void ShowTodos()
    {
        if (todoList.Count == 0)
        {
            Console.WriteLine("✨ 沒有待辦事項");
            return; // 直接跳出 ShowTodos() 方法，不會執行後面的 foreach 迴圈！
        }

        Console.WriteLine("\n📌 待辦事項：");
        //  var (語法糖（syntactic sugar）)讓 C# 自動推斷變數類型
        // 用 var 只是讓程式碼更簡潔，本質上沒差別。
        // 如果變數型別不明顯，最好明確寫型別，不要用 var。
        // 如果 todoList 是 List<Todo>，那 todo 就是 Todo，相當於：
        // foreach (Todo todo in todoList)
        foreach(var todo in todoList)
        {
            string status = todo.IsCompleted ? "✅" : "❌";
            Console.WriteLine($"{todo.Id}. {status} {todo.Task}");
        }

        // for (int i = 0; i < todoList.Count; i++)
        // {
        //     Console.WriteLine($"{i + 1}. {todoList[i]}");
        // }
    }

    static void AddTodo()
    {
        Console.Write("輸入新的待辦事項: ");
        string task = Console.ReadLine();

        // 設定 ID（如果清單是空的，從 1 開始）
        // 在 C# 裡，如果 Todo 是一個類別（class），你可以透過 . 來存取類別中的屬性（Property）。
        int newId = todoList.Count > 0 ? todoList[todoList.Count - 1].Id + 1 : 1;

        Todo newTodo = new Todo(newId, task);

        todoList.Add(newTodo);
        SaveTodos(); // ✅ 變更後存檔
        Console.WriteLine($"✅ 已新增: {newTodo.Task}");
    }

    static void RemoveTodo()
    {
        ShowTodos();
        Console.Write("請輸入要刪除的項目編號: ");
        
        // 這裡的 id 是區域變數，跟 Todo 類別的 Id 沒有關係，它只是用來存使用者輸入的數字。id 是 TryParse 成功時存入的變數，如果轉換失敗，id 會是 0。
        // if (int.TryParse(Console.ReadLine(), out int id) && index > 0 && index <= todoList.Count) 
        // {
        //     Console.Write($"⚠️ 確定要刪除 '{todoList[index - 1]}' 嗎?(y/n): ");
        //     string confirm = Console.ReadLine().ToLower();

        //     if (confirm == "y")
        //     {
        //         todoList.RemoveAt(index - 1);
        //         SaveTodos(); // ✅ 變更後存檔

        //         Console.WriteLine("🗑️ 已刪除!");
        //     }
        //     else
        //     {
        //         Console.WriteLine("🔄 取消刪除");
        //     }
        // }
        // else
        // {
        //     Console.WriteLine("❌ 無效的編號");
        // }

        // 這裡的 id 是區域變數，跟 Todo 類別的 Id 沒有關係，它只是用來存使用者輸入的數字。id 是 TryParse 成功時存入的變數，如果轉換失敗，id 會是 0。
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Todo todo = todoList.Find(t => t.Id == id);

            if(todo != null)
            {
                todoList.Remove(todo);
                SaveTodos(); // ✅ 變更後存檔
                Console.WriteLine($"🗑️ 已刪除: {todo.Task}");
            }
            else
            {
                Console.WriteLine("❌ 找不到該編號");
            }
        }
        else
        {
            Console.WriteLine("❌ 無效的輸入");
        }
    }

    static void DoneToDo()
    {
        ShowTodos();
        Console.Write("請輸入已完成的項目編號: ");

        // if (int.TryParse(Console.ReadLine(), out int id) && index > 0 && index <= todoList.Count)
        // {
        //     string completedTask = todoList[index - 1];
        //     doneTodoList.Add(completedTask);
        //     todoList.RemoveAt(index - 1);
        //     SaveTodos(); // ✅ 變更後存檔
        //     Console.WriteLine($"✅ 已完成: {completedTask}");
        //     // haveDoneToDo = true;
        // }
        // else
        // {
        //     Console.WriteLine("❌ 無效的編號");
        // }

        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Todo todo = todoList.Find(t => t.Id == id);

            if(todo != null)
            {
                todo.IsCompleted = true;
                SaveTodos(); // ✅ 變更後存檔
                Console.WriteLine($"✅ 已標記完成: {todo.Task}");
            }
            else
            {
                Console.WriteLine("❌ 找不到該編號");
            }
        }
        else
        {
            Console.WriteLine("❌ 無效的輸入");
        }
    }
}

