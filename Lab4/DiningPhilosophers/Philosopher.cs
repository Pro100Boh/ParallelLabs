using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab4.DiningPhilosophers
{
    public class Philosopher
    {
        private const int TimesToEat = 3;
        private int _timesEaten = 0;
        private readonly IList<Philosopher> _allPhilosophers;
        private readonly int _index;

        public Philosopher(IList<Philosopher> allPhilosophers, int index)
        {
            _allPhilosophers = allPhilosophers;
            _index = index;
            Name = string.Format($"Philosopher {_index}");
            State = State.Thinking;
        }

        public string Name { get; private set; }
        public State State { get; private set; }
        public Fork LeftFork { get; set; }
        public Fork RightFork { get; set; }

        public Philosopher LeftPhilosopher
        {
            get
            {
                if (_index == 0)
                    return _allPhilosophers[_allPhilosophers.Count - 1];
                else
                    return _allPhilosophers[_index - 1];
            }
        }

        public Philosopher RightPhilosopher
        {
            get
            {
                if (_index == _allPhilosophers.Count - 1)
                    return _allPhilosophers[0];
                else
                    return _allPhilosophers[_index + 1];
            }
        }

        public void EatAll()
        {
            while (_timesEaten < TimesToEat)
            {
                Think();
                if (PickUp())
                {
                    Eat();

                    PutDownLeft();
                    PutDownRight();
                }
            }
        }

        private bool PickUp()
        {
            if (Monitor.TryEnter(LeftFork))
            {
                if (Monitor.TryEnter(RightFork))
                {
                    return true;
                }
                else
                {
                    PutDownLeft();
                }
            }

            return false;
        }

        private void Eat()
        {
            State = State.Eating;
            _timesEaten++;
            Console.WriteLine(Name + " eats");
        }

        private void PutDownLeft()
        {
            Monitor.Exit(LeftFork);
        }

        private void PutDownRight()
        {
            Monitor.Exit(RightFork);
        }

        private void Think()
        {
            State = State.Thinking;
        }
    }
}
