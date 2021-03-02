using System;
using System.Collections.Generic;
using System.Text;

namespace Testovoe
{
    enum BakeryType
    {
        Bagette,
        Crousant,
        Crendel,
        Smetannik
    }
    class Factory
    {
        public Bakery CreateBakery(BakeryType type, DateTime baked)
        {
            switch (type)
            {
                case BakeryType.Bagette:
                    return new Bagette(baked);
                case BakeryType.Crendel:
                    return new Crendel(baked);
                case BakeryType.Crousant:
                    return new Crousant(baked);
                case BakeryType.Smetannik:
                    return new Smetannik(baked);
                default:
                    return new Bagette(baked);
            }
        }
    }
 
}
