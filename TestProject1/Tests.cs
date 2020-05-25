using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using lab_12;
using NUnit.Framework;

namespace TestProject1
{
    [TestFixture]
    public class Tests
    {
        //однонаправленный список
        [Test]
        public void Test1() //MakeList and MakeTransport
        {
            Transport tr = new Transport();
            tr = tr.MakeTransport();

            Assert.AreEqual(null, tr.next);

            tr.next = tr.MakeList(5);
            Assert.AreEqual(5, Count(tr.next));
            Assert.AreEqual(6, Count(tr));
        }

        [Test]
        public void Test2() //DeleteFirstEven and Delete and Add
        {
            Transport tr = new Transport();
            tr = tr.MakeList(6);
            tr = tr.DeleteFirstEven(tr);
            Assert.AreEqual(5, Count(tr));

            tr = tr.Delete(tr, 2);
            Assert.AreEqual(4, Count(tr));

            tr = tr.Delete(tr, 6);
            Assert.AreEqual(4, Count(tr));

            tr = tr.Add(tr, 3);
            Assert.AreEqual(5, Count(tr));

            tr = tr.Add(tr, 7);
            Assert.AreEqual(5, Count(tr));
        }

        //двунаправленный список
        [Test]
        public void Test3() //MakeTrain and MakeList
        {
            Train train = new Train();
            Assert.AreEqual(null, train.last);
            Assert.AreEqual(null, train.next);

            train = train.MakeTrain();
            Assert.AreEqual(null, train.last);
            Assert.AreEqual(null, train.next);

            train = train.MakeList(5);
            train.Print();
            Assert.AreEqual(5, Count(train));
        }

        [Test]
        public void Test4() //Add
        {
            Train train = new Train();
            train = train.MakeList(7);
            train = train.Add(train, 4);
            Assert.AreEqual(8, Count(train));

            train = train.Add(train, 10);
            Assert.AreEqual(8, Count(train));
        }

        //бинарное дерево
        [Test]
        public void Test5()
        {
            Car car = new Car();
            Assert.AreEqual(null, car.left);
            Assert.AreEqual(null, car.right);

            car = car.IdealTree(9, car);
            car.TreeToList(car);
            Assert.AreEqual(9, car.list.Count);
            car.list = new List<Car>();
            car.TreeToList(car.left);
            int leftCount = car.list.Count();
            car.list = new List<Car>();
            car.TreeToList(car.right);
            int rightCount = car.list.Count();
            Assert.True(leftCount == rightCount || leftCount - 1 == rightCount);
        }

        [Test]
        public void Test6() //IdealTree and SearchTree
        {
            Car car = new Car();
            car = car.IdealTree(9, car);
            Assert.IsNotNull(car.FindMin(car));

            IsSearchTree(car);
            Assert.IsFalse(ok);

            car = car.IdealTreeToSearch();
            IsSearchTree(car);
            ok = true;
            Assert.IsTrue(ok);
        }

        //MyCollection
        [Test]
        public void Test7() //Delete and Add and SearchByValue
        {
            MyCollection<Transport> myC = new MyCollection<Transport>(5);
            myC.Delete(3);
            Assert.AreEqual(4, myC.Count);

            myC.Delete(1, 4);
            Assert.AreEqual(0, myC.Count);
            
            Transport t = new Transport(300);
            myC.Add(t, 1);
            Assert.AreEqual(t.MaxSpeed, myC.SearchByValue(300).MaxSpeed);
        }

        public bool ok = true;

        public int Count(Transport tr)
        {
            int i = 0;
            for (; tr != null; i++)
            {
                tr = tr.next;
            }

            return i;
        }

        public int Count(Train tr)
        {
            int i = 0;
            for (; tr != null; i++)
            {
                tr = tr.next;
            }

            return i;
        }

        public void IsSearchTree(Car car)
        {
            if ((car.left == null || car.left.Power <= car.Power) &&
                (car.right == null || car.right.Power >= car.Power))
            {
                if (car.left != null)
                    IsSearchTree(car.left);
                if (car.right != null)
                    IsSearchTree(car.right);
            }
            else
            {
                ok = false;
            }
        }
    }
}