using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testovoe
{
    public partial class Form1 : Form
    {

        public List<Bagette> baggetes = new List<Bagette>();
        public List<Crousant> crousants = new List<Crousant>();
        public List<Crendel> crendels = new List<Crendel>();
        public List<Smetannik> smetanniks = new List<Smetannik>();
        public Form1()
        {
            InitializeComponent();
            foreach (Type type in
                        Assembly.GetAssembly(typeof(Bakery)).GetTypes()
                        .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Bakery))))
            {
                comboBox1.Items.Add(type.ToString().Remove(0,9));
            }
            comboBox1.SelectedIndex = 0;
            dataGridView1.Columns.Add("Name","Name");                                   //1
            dataGridView1.Columns.Add("Baked", "Baked at:"); //datetime                 //2
            dataGridView1.Columns.Add("Critical", "Become Critical at:"); //datetime    //3
            dataGridView1.Columns.Add("Disposed", "Disposed at:"); //datetime           //4
            dataGridView1.Columns.Add("Price", "Current price:");                       //5
            dataGridView1.Columns.Add("PriceProg", "Next price:");                      //6
            dataGridView1.Columns.Add("Update", "Next price at:"); //datetime           //7

            SQLUpdate();
            updateGridView();
        }

        public void SQLUpdate()
        {
            using (TestDBContext db = new TestDBContext())
            {
                baggetes = db.Bagettes.ToList();
                crousants = db.Crousants.ToList();
                crendels = db.Crendels.ToList();
                smetanniks = db.Smetanniks.ToList();
            }
        }
        //------------------------------------------------------------------//
        //Была идея сделать всё через рефлексию и игнорировать фабрики      //
        //------------------------------------------------------------------//

        /*public IBakery GetBakery(string pluginName, ref int errorCode)
        {
            if (pluginName.Length < 3)
            {
                errorCode = 1; //Недостаточная длинна
                return null;
            }
            foreach (Type type in
                    Assembly.GetAssembly(typeof(Bakery)).GetTypes()
                    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Bakery))))
            {
                if (type.ToString().Contains(pluginName))
                    return (IBakery)Activator.CreateInstance(type);
            }
            errorCode = 2;// Не найдено ни одного вхождения
            return null;
        }*/
        private void button1_Click(object sender, EventArgs e)
        {
            using (TestDBContext db = new TestDBContext())
            {
                //int errorCode = 0;
                string DataBaseName = comboBox1.SelectedItem.ToString();
                Factory factory = new Factory();
                switch (DataBaseName) //Не нашёл как в энтити фреймворке подменять таблицу для вставки в неё
                {
                    case ("Bagette"):
                        {
                            Bakery bakery = factory.CreateBakery(BakeryType.Bagette,DateTime.Now);
                            db.Bagettes.Add((Bagette)bakery);   //Пришлось проводить вручную потому что не понял как исправить 
                                                                // то что абстрактный тип не приводился к типу наследнику в EF
                            db.SaveChanges();
                        }
                        break;
                    case ("Crousant"):
                        {
                            Bakery bakery = factory.CreateBakery(BakeryType.Crousant,DateTime.Now);
                            db.Crousants.Add((Crousant)bakery);
                            db.SaveChanges();
                        }
                        break;
                    case ("Crendel"):
                        {
                            Bakery bakery = factory.CreateBakery(BakeryType.Crendel,DateTime.Now);
                            db.Crendels.Add((Crendel)bakery);
                            db.SaveChanges();
                        }
                        break;
                    case ("Smetannik"):
                        {
                            Bakery bakery = factory.CreateBakery(BakeryType.Smetannik,DateTime.Now);
                            db.Smetanniks.Add((Smetannik)bakery);
                            db.SaveChanges();
                        }
                        break;
                }
            }
            SQLUpdate();
            updateGridView();
        }

        public void updateGridView()
        {
            dataGridView1.Rows.Clear();
            foreach (var bakery in baggetes)
            {
                //Проверка на удаление
                if (bakery.TimeCritical.CompareTo(DateTime.Now)>0)//меньше нуля - раньше, больше нуля - позже, ноль - одновременно
                {
                    if (bakery.updatePrice(DateTime.Now))
                    {
                        using (TestDBContext db = new TestDBContext())
                        {
                            var item = db.Bagettes.Find(bakery.ID);
                            item = bakery;
                            db.SaveChanges();
                        }
                        //update
                    }
                    dataGridView1.Rows.Add("Багет", bakery.TimeBaked.ToString(), bakery.TimeCritical.ToString(),
                        bakery.TimeToSell.ToString(), bakery.Price, (bakery.Price * 0.98), bakery.TimeLastChecked - DateTime.Now);
                }
                else
                {
                    using (TestDBContext db = new TestDBContext())
                    {
                        var item = db.Bagettes.Find(bakery.ID);
                        item = bakery;
                        db.Bagettes.Remove(item);
                        db.SaveChanges();
                        SQLUpdate();
                    }
                }

            }
            foreach (var bakery in crousants)
            {
                //Проверка на удаление
                if (bakery.TimeCritical.CompareTo(DateTime.Now) > 0)//меньше нуля - раньше, больше нуля - позже, ноль - одновременно
                {
                    if (bakery.updatePrice(DateTime.Now))
                    {
                        using (TestDBContext db = new TestDBContext())
                        {
                            var item = db.Crousants.Find(bakery.ID);
                            item = bakery;
                            db.SaveChanges();
                        }
                        //update
                    }
                    dataGridView1.Rows.Add("Круасан", bakery.TimeBaked.ToString(), bakery.TimeCritical.ToString(),
                        bakery.TimeToSell.ToString(), bakery.Price, (bakery.Price * 0.98), bakery.TimeLastChecked - DateTime.Now);
                }
                else
                {
                    using (TestDBContext db = new TestDBContext())
                    {
                        var item = db.Crousants.Find(bakery.ID);
                        item = bakery;
                        db.Crousants.Remove(item);
                        db.SaveChanges();
                        SQLUpdate();
                    }
                }
            }
            foreach (var bakery in crendels)
            {
                //Проверка на удаление
                if (bakery.TimeCritical.CompareTo(DateTime.Now) > 0)//меньше нуля - раньше, больше нуля - позже, ноль - одновременно
                {
                    if (bakery.updatePrice(DateTime.Now))
                    {
                        using (TestDBContext db = new TestDBContext())
                        {
                            var item = db.Crendels.Find(bakery.ID);
                            item = bakery;
                            db.SaveChanges();
                        }
                        //update
                    }
                    if (DateTime.Now.CompareTo(bakery.TimeCritical) > 0)
                        dataGridView1.Rows.Add("Крендель", bakery.TimeBaked.ToString(), bakery.TimeCritical.ToString(),
                            bakery.TimeToSell.ToString(), bakery.Price, (0), DateTime.Now - bakery.TimeToSell);
                    else
                        dataGridView1.Rows.Add("Крендель", bakery.TimeBaked.ToString(), bakery.TimeCritical.ToString(),
                        bakery.TimeToSell.ToString(), bakery.Price, (bakery.Price * 0.5), bakery.TimeCritical - DateTime.Now);
                }
                else
                {
                    using (TestDBContext db = new TestDBContext())
                    {
                        var item = db.Crendels.Find(bakery.ID);
                        item = bakery;
                        db.Crendels.Remove(item);
                        db.SaveChanges();
                        SQLUpdate();
                    }
                }
            }
            foreach (var bakery in smetanniks)
            {
                //Проверка на удаление
                if (bakery.TimeCritical.CompareTo(DateTime.Now) > 0)//меньше нуля - раньше, больше нуля - позже, ноль - одновременно
                {
                    if (bakery.updatePrice(DateTime.Now))
                    {
                        using (TestDBContext db = new TestDBContext())
                        {
                            var item = db.Smetanniks.Find(bakery.ID);
                            item = bakery;
                            db.SaveChanges();
                        }
                        //update
                    }
                    dataGridView1.Rows.Add("Сметанник", bakery.TimeBaked.ToString(), bakery.TimeCritical.ToString(),
                        bakery.TimeToSell.ToString(), bakery.Price, (bakery.Price * 0.96), bakery.TimeLastChecked - DateTime.Now);
                }
                else
                {
                    using (TestDBContext db = new TestDBContext())
                    {
                        var item = db.Smetanniks.Find(bakery.ID);
                        item = bakery;
                        db.Smetanniks.Remove(item);
                        db.SaveChanges();
                        SQLUpdate();
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateGridView();
        }
    }
}
