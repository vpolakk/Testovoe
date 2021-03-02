using System;
using System.Collections.Generic;
using System.Text;

namespace Testovoe
{
    public class Bagette:Bakery
    {
        public Bagette() :base()
        {
            TimeCritical = DateTime.Now.AddDays(1);
            TimeToSell = DateTime.Now.AddDays(2);
            Price = 40;
        }
        public Bagette(DateTime timeBaked) :base(timeBaked)
        {
            TimeCritical = timeBaked.AddDays(1);
            TimeToSell = timeBaked.AddDays(2);
            Price = 40;
        }
    }
    public class Crousant :Bakery
    {
        public Crousant() :base()
        {
            TimeCritical = DateTime.Now.AddHours(12);
            TimeToSell = DateTime.Now.AddDays(1);
            Price = 60;
        }
        public Crousant(DateTime timeBaked) : base(timeBaked)
        {
            TimeCritical = timeBaked.AddHours(12);
            TimeToSell = timeBaked.AddDays(1);
            Price = 60;
        }
    }
    public class Crendel :Bakery
    {
        public Crendel() : base()
        {
            TimeCritical = DateTime.Now.AddHours(12);
            TimeToSell = DateTime.Now.AddDays(1);
            TimeLastChecked = TimeCritical;
            Price = 60;
        }
        public Crendel(DateTime timeBaked) : base(timeBaked)
        {
            TimeCritical = timeBaked.AddHours(12);
            TimeToSell = timeBaked.AddDays(1);
            Price = 60;
        }
        public override bool updatePrice(DateTime timeCurrent)
        {
            if (timeCurrent.CompareTo(TimeCritical) > 0)//меньше нуля - раньше, больше нуля - позже, ноль - одновременно
            {
                Price = Price * 0.5;
                TimeLastChecked = TimeCritical;
                return true;
            }
            return false;
        }
    }
    public class Smetannik : Bakery
    {
        public Smetannik() : base()
        {
            TimeCritical = DateTime.Now.AddHours(12);
            TimeToSell = DateTime.Now.AddDays(1);
            Price = 60;
        }
        public Smetannik(DateTime timeBaked) : base(timeBaked)
        {
            TimeCritical = timeBaked.AddHours(12);
            TimeToSell = timeBaked.AddDays(1);
            Price = 60;
        }
        public override bool updatePrice(DateTime timeCurrent)
        {
            if (timeCurrent.CompareTo(timeLastChecked) > 0)//меньше нуля - раньше, больше нуля - позже, ноль - одновременно
            {
                Price = Price * 0.96;
                timeLastChecked = timeCurrent.AddHours(1);
                return true;
            }
            return false;
        }
    }
}
