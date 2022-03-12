using System;
using System.IO;

namespace ToDoList
{
    class Program
    {
        static void Main(string[] args)
        {
            new ToDoList().Interface();
        }
    }

    class ToDoList
    {
        string filePath = @"/ToDo/note.txt";
        string tempFilePath = @"/ToDo/note_temp.txt";
        string backupFilePath = @"/ToDo/note_backup.txt";

        int tasksCounter = 0;

        public void Interface()
        {
            bool endApp = false;

            //Create the file if needed
            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {

                }
            }
            //Count the number of tasks
            else
            {
                using (StreamReader sr = File.OpenText(filePath))
                {
                    string s;

                    while ((s = sr.ReadLine()) != null)
                    {
                        if (s.StartsWith("-"))
                        {
                            tasksCounter++;
                        }
                    }
                }
            }

            //Deleting temp file if it exists
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }


            while (!endApp)
            {
                //Interface tips for user
                {
                    Console.WriteLine("To do List in C#.\n");

                    Console.WriteLine("------------------------\n");

                    Console.WriteLine($"You have {tasksCounter} tasks.");

                    Console.WriteLine("Type:   \"a\" to add a new task.");
                    Console.WriteLine("\t\"s\" to move a task.");
                    Console.WriteLine("\t\"d\" to delete a task.");
                    Console.WriteLine("\t\"q\" to exit.");
                    Console.WriteLine("\t\"w\" to write the backup.");
                    Console.WriteLine("\t\"e\" to restore from the backup.");
                    Console.WriteLine("\t\"r\" to read the tasks. \n");
                }

                //Navigation
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        {
                            ConsoleKey s;
                            Console.WriteLine("\n\nAdding new task? y/n\n");
                            if ((s = Console.ReadKey().Key) == ConsoleKey.Y || s == ConsoleKey.Enter)
                            {
                                Console.WriteLine();
                                AddTask();
                            }
                            Console.WriteLine();
                            break;
                        }
                    case ConsoleKey.S:
                        {
                            ConsoleKey s;
                            int oldNum;
                            int newNum;

                            Console.WriteLine("\n\nMove the task? y/n\n");
                            if ((s = Console.ReadKey().Key) == ConsoleKey.Y || s == ConsoleKey.Enter)
                            {
                                Console.WriteLine();
                                Console.WriteLine("\n\nType the number of task. \n");
                                if (int.TryParse(Console.ReadLine(), out oldNum))
                                {
                                    Console.WriteLine("\n\nType the new number of task. \n");
                                    if (int.TryParse(Console.ReadLine(), out newNum) && oldNum != newNum)
                                        MoveTask(oldNum, newNum);
                                }
                            }
                            break;
                        }
                    case ConsoleKey.D:
                        {
                            ConsoleKey s;
                            int num;

                            Console.WriteLine("\n\nDeleting a task? y/n\n");
                            if ((s = Console.ReadKey().Key) == ConsoleKey.Y || s == ConsoleKey.Enter)
                            {
                                Console.WriteLine();
                                Console.WriteLine("\n\nType the number of task. \n");
                                if (int.TryParse(Console.ReadLine(), out num))
                                {
                                    DeleteTask(num);
                                }
                            }
                            Console.WriteLine();
                            break;
                        }
                    case ConsoleKey.Q:
                        {
                            endApp = true;
                            break;
                        }
                    case ConsoleKey.W:
                        {
                            BackupNotes();
                            break;
                        }
                    case ConsoleKey.E:
                        {
                            RestoreNotes();
                            break;
                        }
                    case ConsoleKey.R:
                        {
                            ConsoleKey s;
                            Console.WriteLine("\n\nRead the tasks? y/n\n");
                            if ((s = Console.ReadKey().Key) == ConsoleKey.Y || s == ConsoleKey.Enter)
                            {
                                Console.WriteLine();
                                ReadTasks();
                            }
                            Console.WriteLine();
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                Console.Clear();
            }

            Console.WriteLine("Closing the app. Type anything to leave.");
            Console.ReadKey();
        }

        void AddTask()
        {
            Console.Clear();

            //Reading file
            using (StreamReader sr = File.OpenText(filePath))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }

            //Writing file
            using (StreamWriter sw = File.AppendText(filePath))
            {
                string s;
                DateTime taskDate;
                bool dateIsCorrect = false;
                bool textIsCorrect = false;

                Console.WriteLine("------------(x)------------\n");
                Console.Write("Type \"~\" to go back. \n");

                //Exit check. Date input & validation
                Console.Write("Date(dd.mm.yy): ");
                if(DateTime.TryParse(s = Console.ReadLine(), out taskDate) && s != "~")
                {
                    dateIsCorrect = true;
                }

                //Date and exit check. Text input & validation
                Console.Write("Plan: ");
                if (dateIsCorrect && (s = Console.ReadLine()) != "~" && s != null)
                {
                    textIsCorrect = true;
                }

                //Creating task
                if(dateIsCorrect && textIsCorrect)
                {
                    sw.WriteLine($"------------({++tasksCounter})------------\n");

                    sw.WriteLine(taskDate.ToString("d"));

                    sw.WriteLine(s + "\n");
                }

            }
        }

        void DeleteTask(int taskNum)
        {
            int newTaskNum = 1;

            File.Copy(filePath, tempFilePath);
            StreamWriter sw = new StreamWriter(filePath, false);
            StreamReader sr = new StreamReader(tempFilePath);

            while(newTaskNum < tasksCounter)
            {
                if(newTaskNum == taskNum)
                {
                    for(int i = 0; i < 5; i++)
                        sr.ReadLine();
                    taskNum = 0;
                }
                else
                {
                    sw.WriteLine($"------------({newTaskNum})------------");
                    sr.ReadLine();
                    for(int i = 0; i < 4; i++)
                        sw.WriteLine(sr.ReadLine());

                    newTaskNum++;
                }
                    
            }
            tasksCounter = newTaskNum - 1;
            
            sw.Close();
            sr.Close();
            File.Delete(tempFilePath);
        }

        void MoveTask(int oldNum, int newNum)
        {
            int newTaskNum = 1;

            File.Copy(filePath, tempFilePath);
            StreamWriter sw = new StreamWriter(filePath, false);
            StreamReader sr = new StreamReader(tempFilePath);

            //If the task gets up
            if (oldNum > newNum)
            {
                //Дичь, которая работает спустя несколько часов, но это не то, что я хотел
                //Меняет местами два таска
                {
                    /*//O1 - запись тасков с начала до нового места
                    for(int j = 0; j < newNum-1; j++)
                    {
                        sw.WriteLine($"------------({newTaskNum})------------");
                        s = sr.ReadLine();
                        for (int i = 0; i < 4; i++)
                            sw.WriteLine(sr.ReadLine());

                        newTaskNum++;
                    }

                    //O2 - пропуск тасков между старым и новым местом
                    for(int j = 0; j < (oldNum - newNum) * 5; j++)
                    {
                        s = sr.ReadLine();
                    }

                    //O3 - запись нового места таска
                    {
                        sw.WriteLine($"------------({newTaskNum})------------");
                        sr.ReadLine();
                        for (int i = 0; i < 4; i++)
                            sw.WriteLine(sr.ReadLine());

                        newTaskNum++;
                    }

                    //O4 - закрытие&открытие sr. пропуск тасков на записанных местах
                    sr.Close();
                    sr = new StreamReader(tempFilePath);
                    for(int j = 0; j < (newNum - 1) * 5; j++)
                    {
                        sr.ReadLine();
                    }

                    //O5 - запись тасков между старым и новым местом
                    for(int j = 0; j < oldNum - newNum; j++)
                    {
                        sw.WriteLine($"------------({newTaskNum})------------");
                        sr.ReadLine();
                        for (int i = 0; i < 4; i++)
                            sw.WriteLine(sr.ReadLine());

                        newTaskNum++;
                    }

                    //O6 - пропуск старого места таска и запись остальных
                    for(int j = 0; j < 5; j++)
                    {
                        sr.ReadLine();
                    }
                    for(int j = 0; j < tasksCounter - oldNum; j++)
                    {
                        sw.WriteLine($"------------({newTaskNum})------------");
                        sr.ReadLine();
                        for (int i = 0; i < 4; i++)
                            sw.WriteLine(sr.ReadLine());

                        newTaskNum++;
                    }*/
                }

                //O1 - запись тасков до вхождения нового
                for(int j = 0; j < newNum - 1; j++)
                {
                    WriteTask(sw, sr, ref newTaskNum);
                }

                //O2 - пропуск тасков до вставляемого
                for (int j = 0; j < (oldNum - newNum) * 5; j++)
                {
                    sr.ReadLine();
                }

                //O3 - запись таска на новое место
                {
                    WriteTask(sw, sr, ref newTaskNum);
                }

                //O4 - сброс sr и пропуск записанных тасков
                {
                    sr.Close();
                    sr = new StreamReader(tempFilePath);
                    for (int j = 0; j < (newNum - 1) * 5; j++)
                    {
                        sr.ReadLine();
                    }
                }

                //O5 - запись тасков до вставленного
                for(int j = 0; j < oldNum - newNum; j++)
                {
                    WriteTask(sw, sr, ref newTaskNum);
                }

                //O6 - пропуск вставленного таска и запись остальных
                for (int j = 0; j < 5; j++)
                {
                    sr.ReadLine();
                }
                for (int j = 0; j < tasksCounter - oldNum; j++)
                {
                    WriteTask(sw, sr, ref newTaskNum);
                }

            }
            //Is the task gets down
            else
            {
                //O1 - запись тасков до вставляемого
                for (int j = 0; j < oldNum - 1; j++)
                {
                    WriteTask(sw, sr, ref newTaskNum);
                }

                //O2 - пропуск вставляемого таска и запись остальных до места вставки
                for (int j = 0; j < 5; j++)
                {
                    sr.ReadLine();
                }
                for (int j = 0; j < newNum - oldNum; j++)
                {
                    WriteTask(sw, sr, ref newTaskNum);
                }

                //O3 - сброс sr и пропуск тасков до вставляемого
                {
                    sr.Close();
                    sr = new StreamReader(tempFilePath);
                    for (int j = 0; j < (oldNum - 1) * 5; j++)
                    {
                        sr.ReadLine();
                    }
                }

                //O4 - вставка таска и пропуск
                {
                    WriteTask(sw, sr, ref newTaskNum);
                    for (int j = 0; j < (newNum - oldNum) * 5; j++)
                    {
                        sr.ReadLine();
                    }
                }

                //05 - вставка тасков до конца
                for (int j = 0; j < tasksCounter - newNum; j++)
                {
                    WriteTask(sw, sr, ref newTaskNum);
                }
            }

            sw.Close();
            sr.Close();
            File.Delete(tempFilePath);
        }

        void ReadTasks()
        {
            Console.Clear();

            DateTime taskDate = default;
            string taskText = null;

            //Reading note.
            using (StreamReader sr = File.OpenText(filePath))
            {
                string s;

                int closestTask = 1;
                int taskNum = 0;
                int taskLine = 0;

                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);

                    //Calculating task and line index;
                    {
                        if (s.StartsWith("-"))
                        {
                            taskNum++;
                            taskLine = 0;
                        }
                        else{
                            taskLine++;
                        }
                    }

                    //Parsing closest task
                    {
                        if (taskLine == 2)
                            if (DateTime.Compare(Convert.ToDateTime(s), DateTime.Today) >= 0 && (DateTime.Compare(taskDate, Convert.ToDateTime(s)) > 0 || taskDate == default))
                            {
                                closestTask = taskNum;
                                taskDate = Convert.ToDateTime(s);
                            }
                        if(taskNum == closestTask && taskLine == 3)
                        {
                            taskText = s;
                        }
                    }

                }
            }


            //Summary
            {
                Console.WriteLine("------------(x)------------\n");

                Console.WriteLine("Summary:\n");


                if (taskDate != default && taskText != null)
                {
                    Console.WriteLine($"You have the closest plan on: {taskDate.ToString("d")}");
                    Console.WriteLine($"You have planned this: {taskText}\n");
                }
                else
                    Console.WriteLine("You have nothing planned.\n");



                Console.WriteLine("------------(x)------------\n");

                Console.WriteLine("Finished reading. Type anything to return.");
                Console.ReadKey();
            }
        }

        void BackupNotes()
        {
            if (!File.Exists(backupFilePath))
            {
                File.Copy(filePath, backupFilePath);
            }
            else
            {
                File.Delete(backupFilePath);
                File.Copy(filePath, backupFilePath);
            }
        }

        void RestoreNotes()
        {
            if (File.Exists(backupFilePath))
            {
                File.Delete(filePath);
                File.Copy(backupFilePath, filePath);
            }
            else
            {
                Console.WriteLine("\n\nNo backup found. Make sure it's named \"note_backup.txt\". Press any key to return.");
                Console.ReadKey();
            }

        }

        private void WriteTask(StreamWriter sw, StreamReader sr, ref int newTaskNum)
        {
            sw.WriteLine($"------------({newTaskNum++})------------");
            sr.ReadLine();
            for (int i = 0; i < 4; i++)
                sw.WriteLine(sr.ReadLine());
        }
    }

}
