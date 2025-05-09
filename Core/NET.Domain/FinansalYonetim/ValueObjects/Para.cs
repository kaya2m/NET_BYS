using NET.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Domain.FinansalYonetim.ValueObjects
{
    /// <summary>
    /// Para değer nesnesi.
    /// Para miktarını ve para birimini temsil eder.
    /// </summary>
    public class Para : ValueObject
    {
        public decimal Deger { get; }
        public string ParaBirimi { get; }

        public Para(decimal deger, string paraBirimi = "TRY")
        {
            if (deger < 0)
                throw new ArgumentException("Para değeri negatif olamaz", nameof(deger));

            if (string.IsNullOrWhiteSpace(paraBirimi))
                throw new ArgumentException("Para birimi boş olamaz", nameof(paraBirimi));

            Deger = deger;
            ParaBirimi = paraBirimi;
        }

        public Para Add(Para other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (ParaBirimi != other.ParaBirimi)
                throw new InvalidOperationException("Farklı para birimleri birbiriyle toplanamaz");

            return new Para(Deger + other.Deger, ParaBirimi);
        }

        public Para Subtract(Para other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (ParaBirimi != other.ParaBirimi)
                throw new InvalidOperationException("Farklı para birimleri birbirinden çıkarılamaz");

            decimal sonuc = Deger - other.Deger;

            if (sonuc < 0)
                throw new InvalidOperationException("Sonuç negatif olamaz");

            return new Para(sonuc, ParaBirimi);
        }

        public Para Multiply(decimal carpan)
        {
            if (carpan < 0)
                throw new ArgumentException("Çarpan negatif olamaz", nameof(carpan));

            return new Para(Deger * carpan, ParaBirimi);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Deger;
            yield return ParaBirimi;
        }

        public override string ToString()
        {
            return $"{Deger:N2} {ParaBirimi}";
        }
    }
}
