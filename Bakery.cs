using System;
using System.Collections.Generic;
using System.Text;

namespace Testovoe
{
    abstract public class Bakery:IBakery
    {
        public Bakery()
        {
            this.TimeBaked = DateTime.Now;
            this.timeLastChecked = DateTime.Now.AddHours(1);
        }
        public Bakery(DateTime timeBaked)
        {
            this.timeBaked = timeBaked;
            this.timeLastChecked = timeBaked.AddHours(1);
        }
        private int id;
        private DateTime timeBaked; //Выпечено
        private DateTime timeToSell; //Время до которого должно быть продано
        private DateTime timeCritical; //Критическое время после которого надо понижать цену
        protected DateTime timeLastChecked;
        private double price;
        public virtual bool updatePrice(DateTime timeCurrent)
        {
          if (timeCurrent.CompareTo(timeLastChecked)>0) //меньше нуля - раньше, больше нуля - позже, ноль - одновременно
            {
                price *= 0.98;
                timeLastChecked = timeCurrent.AddHours(1);
                return true;
            }
            return false;
        }
        public int ID 
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public DateTime TimeToSell
        {
            get
            {
                return timeToSell;
            }
            set
            {
                timeToSell = value;
            }
        }
        public DateTime TimeCritical
        {
            get
            {
                return timeCritical;
            }
            set
            {
                timeCritical = value;
            }
        }
        public DateTime TimeBaked
        {
            get
            {
                return timeBaked;
            }
            set
            {
                timeBaked = value;
            }
        }
        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
            }
        }
        public DateTime TimeLastChecked
        {
            get
            {
                return timeLastChecked;
            }
            set
            {
                timeLastChecked = value;
            }
        }
    }
    public interface IBakery
    {
        int ID { get; set; }
        DateTime TimeToSell { get; set; }
        DateTime TimeCritical { get; set; }
        DateTime TimeBaked { get; set; }
        DateTime TimeLastChecked { get; set; }
        double Price { get; set; }
        bool updatePrice(DateTime timeCurrent);
    }
}
