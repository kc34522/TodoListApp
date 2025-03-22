using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;

// C# 是物件導向語言，所有程式碼必需放在class（類別）裡。類別名稱可以是任意名稱。（Program只C#預設的名稱）C#主程式一定要有一個類別（class）且這個類別裡要有main方法，程式才會執行。（.Net 6+之後可以省略class, Main方法，但這種寫法只適合簡單的小程式，如果你的程式有多個類別，還是建議寫完整的 class Program。）
class Program 
{

    //讓 todoList 變成「全域變數」，整個 Program 類別都可以存取它。
    // 如果放在 Main 方法內，todoList 只能在 Main 裡面用，其他方法無法存取。
    // 因為 Main 是 static，所以 todoList 也必須是 static。
    // C# 不允許 static 方法（Main）存取非 static 的變數。
    static List<string> todoList = new List<string>(); // 儲存待辦事項

    // Main方法為程式的進入點，不管類別名稱是什麼，只要有這個Main方法就可以執行。（如果沒有 Main 方法，C# 就不知道從哪裡開始執行程式，會出錯！）
    // static: 不需要建立物件，程式可以直接執行。（因為 程式啟動時，C# 還沒建立任何物件，所以 Main 必須是 static，讓 .NET 直接執行它。）
    // void: 不回傳值，但可以改成 int 回傳狀態碼。
    // 如果方法 只是顯示內容 或 修改變數，但不需要回傳值(return)，就可以用 void。
    static void Main() 
    {
        while (true)
        {
            Console.WriteLine("\n📌 To-Do List");
            // 方法（method）= 函式（function）
            // 方法（Method） 是 類別（Class） 裡的一個函式。
            // 函式（Function） 是比較廣義的詞，指的是「回傳值 + 參數 + 執行邏輯」的組合。
            // ShowTodos(); = 方法的呼叫(method call)
            // ShowTodos() = 方法的定義(method definition)
            FinishTodos();
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
                    return; // 退出程式 // 用來結束整個方法（Main、其他函式等），並回傳結果（如果方法有回傳值）。
               
                default:
                    Console.WriteLine("❌ 無效的選擇");
                    break;
            }
        }
    }

    // static bool haveDoneToDo = false;
    // haveDoneToDo 變數其實不太必要。你目前使用 haveDoneToDo 來檢查是否有完成的待辦事項，但 doneTodoList.Count > 0 就能知道是否有完成事項了，所以可以移除 haveDoneToDo。
    static List<string> doneTodoList = new List<string>();

    static void FinishTodos()
    {
        if(doneTodoList.Count > 0){
            Console.WriteLine("\n✅ 已完成事項:");

            foreach(string task in doneTodoList)
            {
                Console.WriteLine($"✅ {task}");
            }
        }
    }

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
            return; 
        }

        for (int i = 0; i < todoList.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {todoList[i]}");
        }
    }

    static void AddTodo()
    {
        Console.Write("輸入新的待辦事項: ");
        string task = Console.ReadLine();
        todoList.Add(task);
        Console.WriteLine("✅ 已新增!");
    }

    static void RemoveTodo()
    {
        ShowTodos();
        Console.Write("請輸入要刪除的項目編號: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= todoList.Count)
        {
            Console.Write($"⚠️ 確定要刪除 '{todoList[index-1]}' 嗎?(y/n): ");
            string confirm = Console.ReadLine().ToLower();

            if(confirm == "y")
            {
                todoList.RemoveAt(index - 1);
                Console.WriteLine("🗑️ 已刪除!");
            }
            else
            {
                Console.WriteLine("🔄 取消刪除");
            }
        }
        else
        {
            Console.WriteLine("❌ 無效的編號");
        }
    }

    static void DoneToDo()
    {
        ShowTodos();
        Console.Write("請輸入已完成的項目編號: ");
        if(int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= todoList.Count)
        {
            string completedTask = todoList[index - 1];
            doneTodoList.Add(completedTask);
            todoList.RemoveAt(index-1);
            Console.WriteLine($"✅ 已完成: {completedTask}");
            // haveDoneToDo = true;
        }
        else
        {
            Console.WriteLine("❌ 無效的編號");
        }
    }
}

