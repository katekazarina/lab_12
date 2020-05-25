using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.CompilerServices;

namespace lab_12
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int userChoice;
            do
            {
                Console.Clear();
                Console.WriteLine("Выберите задание:" +
                                  "\n1. Однонаправленный список" +
                                  "\n2. Двунаправленный список" +
                                  "\n3. Бинарное дерево" +
                                  "\n4. Коллекция" +
                                  "\n0. Выход");
                userChoice = Int32.Parse(Console.ReadLine());
                switch (userChoice)
                {
                    case 1:
                        Task1();
                        break;
                    case 2:
                        Task2();
                        break;
                    case 3:
                        Task3();
                        break;
                    case 4:
                        Task4();
                        break;
                }

                Console.ReadLine();
            } while (userChoice != 0);
        }

        public static void Task1()
        {
            Transport list = new Transport();
            Transport first = list.MakeList(5);
            list.ShowList(first);
            Console.WriteLine("удаление первого четного элемента");
            first = list.DeleteFirstEven(first);
            list.ShowList(first);
            Console.WriteLine("удаление 5 элемента");
            first = list.Delete(first, 5);
            list.ShowList(first);
            list = null;
        }

        public static void Task2()
        {
            Console.WriteLine("двунаправленный список из 5 элементов");
            Train list = new Train();
            Train first = list.MakeList(5);
            list.ShowList(first);
            Console.WriteLine("добавление элемента по номеру - 3");
            first = list.Add(first, 3);
            list.ShowList(first);
            Console.ReadLine();
            list = null;
        }

        public static void Task3()
        {
            Car tree = new Car();
            Car car = new Car();
            Console.WriteLine("идеально сбалансированное дерево из 6 элементов");
            car = tree.IdealTree(5, car);
            tree.PrintTree(car, 0);
            Console.WriteLine("поиск минимального элемента дерева");
            Console.WriteLine("min - " + tree.FindMin(car).Print());
            Console.WriteLine("преобразование идеального дерева в дерево поиска");
            car = tree.IdealTreeToSearch();
            tree.PrintTree(car, 0);
        }

        public static void Task4()
        {
            MyCollection<Transport> myCollection = new MyCollection<Transport>(6);
            myCollection.Print();
            Console.WriteLine("copy");
            myCollection.Copy().Print();
            Console.WriteLine("clone");
            myCollection.Clone().Print();
            Transport tr = new Transport(380);
            Console.WriteLine("добавление элемента по номеру 4");
            myCollection.Add(tr, 4);
            myCollection.Print();
            Console.WriteLine("добавление нескольких эементов, начиная с 1");
            List<Transport> list = new List<Transport>();
            list.Add((Transport)tr.Clone());
            list.Add(new Transport(578));
            list.Add(new Transport(375));
            myCollection.Add(list, 1);
            myCollection.Print();
            Console.WriteLine("удаление двух элементов с третьего номера");
            myCollection.Delete(3, 2);
            Console.WriteLine("перебор коллекции циклом foreach:");
            foreach (Transport item in myCollection)
            {
                Console.WriteLine(item.Print());
            }
            Console.WriteLine(myCollection.SearchByValue(-100).Print());
        }
    }


    public class MyCollection<T> : IEnumerable
    {
        public int Count = 0;
        public Transport First;

        public MyCollection()
        {
            Count = 0;
        }

        public MyCollection(int size)
        {
            First = new Transport();
            First = First.MakeList(size);
            Count = size;
        }

        public MyCollection(MyCollection<Train> c)
        {
            Count = c.Count;
            First = c.First;
        }

        public void Add(Transport item, int number)
        {
            if (item == null) return;

            Count++;

            if (First == null)
            {
                First = item;
                return;
            }

            if (number == 1)
            {
                item.next = First;
                First = item;
                return;
            }

            Transport transport = First;
            for (int i = 1; i < number - 1 && transport != null; i++)
                transport = transport.next;
            if (transport == null)
            {
                Console.WriteLine("error. размер листа меньше выбранного номера");
                return;
            }

            item.next = transport.next;
            transport.next = item; //добавление нового элемента
        }

        public void Add(List<Transport> items, int number)
        {
            foreach (Transport item in items)
            {
                Add(item, number);
                number++;
            }
        }

        public void Delete(int number)
        {
            Count--;
            First.Delete(First, number);
        }

        public void Delete(int number, int length)
        {
            if (Count < number + length - 1)
            {
                Console.WriteLine("error");
                return;
            }

            for (int i = number; i < number + length; i++)
            {
                Delete(number);
            }
        }

        public Transport SearchByValue(int maxspeed)
        {
            int count = 1;
            foreach (Transport item in this)
            {
                if (item.MaxSpeed == maxspeed)
                {
                    Console.WriteLine("Элемент под номером " + count);
                    return item;
                }

                count++;
            }

            Console.WriteLine("Элемента нет в коллекции");
            return null;
        }

        public MyCollection<Transport> Copy()
        {
            MyCollection<Transport> tmp = new MyCollection<Transport>();
            tmp.Count = Count;
            tmp.First = First;
            return tmp;
        }

        public MyCollection<Transport> Clone()
        {
            MyCollection<Transport> tmp = new MyCollection<Transport>();
            tmp.Count = Count;
            int number = 1;
            foreach (Transport item in this)
            {
                Transport tr = (Transport)item.Clone();
                tmp.Add(tr, number);
                number++;
            }

            return tmp;
        }

        public IEnumerator GetEnumerator()
        {
            return new MyCollectionEnumerator(Count, First);
        }

        public void Print()
        {
            First.ShowList(First);
        }
    }

    class MyCollectionEnumerator : IEnumerator
    {
        public int Count = 0;
        public Transport First;
        public int position = -1;
        private Transport _current;

        public MyCollectionEnumerator(int count, Transport t)
        {
            Count = count;
            First = t;
        }

        public object Current
        {
            get
            {
                if (position == -1 || position >= Count)
                    throw new InvalidOperationException();
                Transport tmp = First;
                for (int i = 0; i < position; i++)
                {
                    tmp = tmp.next;
                }

                _current = tmp;
                return tmp;
            }
        } //возвращает элемент на текущей позиции

        public bool MoveNext()
        {
            if (position < Count - 1)
            {
                position++;
                return true;
            }

            return false; //последовательность закончилась
        }

        public void Reset()
        {
            position = -1;
        } //устанавливает на начальную позицию
    }
}