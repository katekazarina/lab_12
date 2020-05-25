using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace lab_12
{
    public class Transport //однонаправленный список
    {
        public int MaxSpeed { get; set; }
        public Transport next;

        public Transport()
        {
            Thread.Sleep(50);
            Random rnd = new Random();
            MaxSpeed = rnd.Next(100, 401);
            next = null;
        }

        public Transport(int speed)
        {
            MaxSpeed = speed;
            next = null;
        }

        public virtual string Print()
        {
            return "Transport: max speed - " + MaxSpeed;
        }

        public object Clone()
        {
            return new Transport {MaxSpeed = MaxSpeed};
        }
        
        public Transport MakeTransport()
        {
            Transport t = new Transport();
            return t;
        }//создание элемента списка

        public Transport MakeList(int size)
        {
            Transport first = MakeTransport(); //первый элемент
            for (int i = 1; i < size; i++)
            {
                Transport t = MakeTransport(); //создание элемента и добавление в начало списка
                t.next = first;
                first = t;
            }

            return first;
        } //добавление в начало
        
        public void ShowList(Transport first)
        {
            if (first == null)
            {
                Console.WriteLine("лист пуст");
                return;
            }

            Transport transport = first;
            while (transport != null)
            {
                Console.WriteLine(transport.Print());
                transport = transport.next;
            }
            Console.WriteLine();
        } //печать

        public Transport Delete(Transport first, int number)
        {
            if (first == null)
            {
                Console.WriteLine("error. Список пуст");
                return null;
            }

            if (number == 1)
            {
                first = first.next;
                return first;
            }

            Transport transport = first;
            //поиск элемента для удаления и выбор предыдущего
            for (int i = 1; i < number - 1 && transport != null; i++)
            {
                transport = transport.next;
            }
            
            if (transport==null||transport.next==null)
            {
                Console.WriteLine("error. размер листа меньше выбранного номера");
                return first;
            }

            transport.next = transport.next.next; //удаление элемента
            return first;
        } //удаление элемента по номеру

        public Transport DeleteFirstEven(Transport first)
        {
            if (first == null)
            {
                Console.WriteLine("лист пуст");
                return null;
            }

            int number = 1;
            Transport transport = first;
            while (transport != null)
            {
                if (transport.MaxSpeed % 2 == 0)
                {
                    Console.WriteLine("элемент найден. " + transport.Print());
                    Delete(first, number);
                    if (number == 1)
                        return first.next;
                    return first;
                }
                transport = transport.next;
                number++;
            }
            
            Console.WriteLine("четных элементов нет");
            return first;
            } //удаление первого четного элемента
        
        public Transport Add (Transport first, int number)
        {
            Transport newTransport = MakeTransport();
            if (first == null)
            {
                first = MakeTransport();
                return first;
            }

            if (number == 1)
            {
                newTransport.next = first;
                first = newTransport;
                return first;
            }

            Transport transport = first;
            for (int i = 1; i < number - 1 && transport != null; i++)
                transport= transport.next;
            if (transport==null)
            {
                Console.WriteLine("error. размер листа меньше выбранного номера");
                return first;
            }

            newTransport.next = transport.next;
            transport.next = newTransport; //добавление нового элемента
            return first;
        } //добавление элемента по номеру
    }

    public class Car: Transport //бинарное дерево
    {
        public int Power { get; set; }
        public Car left;
        public Car right;
        public List<Car> list = new List<Car>();
        
        public Car() :base()
        {
            Random rnd = new Random();
            Power = rnd.Next(200, 1501);
            left = null;
            right = null;
        }

        public Car(int speed, int power):base(speed)
        {
            Power = power;
            left = null;
            right = null;
        }

        public override string Print()
        {
            return "Car: power - " + Power + ", max speed - " + MaxSpeed;
        }

        public Car IdealTree(int size, Car car)
        {

            if (car == null)
                car = new Car();
            
            int countLeft, countRight;
            if (size == 0)
                return null;
            countLeft = size / 2;
            countRight = size - countLeft - 1;
            car.left = IdealTree(countLeft, car.left);
            car.right = IdealTree(countRight, car.right);
            return car;
        } //идеальное дерево

        public void PrintTree(Car car, int l)
        {
            if (car != null)
            {
                PrintTree(car.left, l + 3);
                for (int i = 0; i < l; i++) Console.Write(" ");
                Console.WriteLine(car.Print());
                PrintTree(car.right, l + 3);
            }
        } //печать дерева

        public Car FindMin(Car car)
        {
            if (car == null)
            {
                Console.WriteLine("error. пустое дерево");
                return null;
            }
            TreeToList(car);
            Car[] array = list.ToArray();
            Array.Sort(array, new SortByPower());
            return array[0];
        } //поиск минимального элемента дерева

        public void TreeToList(Car car)
        {
            if (car != null)
            {
                list.Add(car);
                TreeToList(car.left);
                TreeToList(car.right);
            }
        }

        public Car IdealTreeToSearch()
        {
            Car[] array = list.ToArray();
            Array.Sort(array, new SortByPower());
            Car root = array[array.Length / 2];

            foreach (Car item in array)
            {
                item.left = null;
                item.right = null;
            }

            foreach (Car item in array)
            {
                AddToTree(item, root);
            }
            return root;
        } //создание дерева поиска на основе идеального дерева

        private void AddToTree(Car item, Car root)
        {
            if (root==item)
            {
                return;
            }

            if (item.Power < root.Power)
            {
                if (root.left != null)
                {
                    AddToTree(item, root.left);
                    return;
                }
                root.left = item;
            }
            else
            {
                if (root.right != null)
                {
                    AddToTree(item, root.right);
                    return;
                }
                root.right = item;
            }
        } //добавление элемента в дерево поиска
    }

    public class Train : Transport //двунаправленный список
    {
        public int Carriage { get; set; }
        public new Train next;
        public Train last;

        public Train() : base()
        {
            Random rnd = new Random();
            Carriage = rnd.Next(200, 1501);
            next = null;
            last = null;
        }

        public Train(int speed, int c) : base(speed)
        {
            Carriage = c;
        }

        public override string Print()
        {
            return "Train: carriage - " + Carriage + ", max speed - " + MaxSpeed;
        }

        new public object Clone()
        {
            return new Car(this.MaxSpeed, this.Carriage);
        }
        
        public Train MakeTrain()
        {
            Train t = new Train();
            return t;
        }//создание элемента списка
        
        public Train Add (Train first, int number)
        {
            Train newTrain = MakeTrain();
            if (first == null)
            {
                first = MakeTrain();
                return first;
            }

            if (number == 1)
            {
                newTrain.next = first;
                first.last = newTrain;
                first = newTrain;
                return first;
            }

            Train train = first;
            for (int i = 1; i < number - 1 && train != null; i++)
                train= train.next;
            if (train==null)
            {
                Console.WriteLine("error. размер листа меньше выбранного номера");
                return first;
            }

            newTrain.next = train.next;
            train.next = newTrain; //добавление нового элемента
            return first;
        } //добавление элемента по номеру
        
        public new Train MakeList(int size)
        {
            Train first = MakeTrain(); //первый элемент
            for (int i = 1; i < size; i++)
            {
                Train t = MakeTrain(); //создание элемента и добавление в начало списка
                t.next = first;
                first.last = t;
                first = t;
            }

            return first;
        } //добавление в начало
        
        public void ShowList(Train first)
        {
            if (first == null)
            {
                Console.WriteLine("лист пуст");
                return;
            }

            Train train = first;
            while (train != null)
            {
                Console.WriteLine(train.Print());
                train = train.next;
            }
            Console.WriteLine();
        } //печать
    }

    public class Express : Transport
    {
        public int Passengers { get; set; }

        public Express() : base()
        {
            Random rnd = new Random();
            Passengers = rnd.Next(200, 1501);
        }

        public Express(int speed, int p) : base(speed)
        {
            Passengers = p;
        }

        public override string Print()
        {
            return "Express: passengers - " + Passengers + ", max speed - " + MaxSpeed;
        }
    }

    public class SortByPower: IComparer
    {
        int IComparer.Compare(object ob1, object ob2)
        {
            Car t1 = (Car) ob1;
            Car t2 = (Car) ob2;
            if (t1.Power == t2.Power)
            {
                return 0;
            }

            return t1.Power > t2.Power ? 1 : -1;
        }
    }
}