using System;
using System.Collections.Generic;


class KuantumCokusuExpection : Exception
{
    public KuantumCokusuExpection(string id)
    : base($"Kuantum çöküşü tespit edildi! Nesne ID: {id} stabilitesi kritik seviyenin altına düştüğü için laboratuvar karantinaya alındı.")
    {
    }
}

public class KuantumSonusuExpection : Exception
{
    public KuantumSonusuExpection(string id)
    : base($"Kuantum sönüşü uyarısı! Nesne ID: {id} stabilitesi %100'ün üstüne çıktı. Nesne söndü.")
    {
    }
}
public abstract class KuantumNesnesi
{
    public string ID { get; set; }

    private double _Stabilite;

    public double Stabilite
    {
        get => _Stabilite;

        set
        {
            if (value < 0)
            {
                _Stabilite = 0;
                throw new KuantumCokusuExpection(ID);
            }
            else if (value > 100)
            {
                _Stabilite = 100;
                throw new KuantumSonusuExpection(ID);
            }
            else
                _Stabilite = value;
        }
    }

    public int TehlikeSeviyesi{ get; set; }


    public abstract void AnalizEt();


    public string DurumBilgisi()
    {
        return $"Nesne ID: {ID}, Stabilite: {Stabilite}, Tehlike Seviyesi: {TehlikeSeviyesi} Nesne türü: {this.GetType().Name}  ";
    }
}

public interface IKritikal
{
    string AcilDurumSoğutması();//NESNENİN stabilitesini 50 arttır
}

public class VeriPaketi : KuantumNesnesi
{
    public override void AnalizEt()
    {
        Console.WriteLine($"Veri içeriği okundu. Nesne türü:{this.GetType().Name} Mevcut Stabilite: {Stabilite} ve 5 paun azalıyor.");
        Stabilite -= 5;
    }
}

public class KaranlikMadde : KuantumNesnesi, IKritikal
{
    public override void AnalizEt()
    {
        Console.WriteLine($"Nesne analiz ediliyor. Nesne türü:{this.GetType().Name}  Mevcut Stabilite: {Stabilite} ve 15 puan azalıyor.");
        Stabilite -= 15;
    }

    public string AcilDurumSoğutması()
    {
        Stabilite += 50;
        return $"Acil durum soğutması uygulandı. Mevcut Stabilite: {Stabilite} ve 50 puan artıyor.";

    }
}

public class AntiMadde : KuantumNesnesi, IKritikal
{
    public override void AnalizEt()
    {
        Console.WriteLine($"Nesne analiz ediliyor. Evrenin dokusu titriyor. Nesne türü:{this.GetType().Name}  Mevcut Stabilite: {Stabilite} ve 25 puan azalıyor.");
        Stabilite -= 25;
    }

    public string AcilDurumSoğutması()
    {
        Stabilite += 50;
        return $"Acil durum soğutması uygulandı. Mevcut Stabilite: {Stabilite} ve 50 puan artıyor.";

    }
}

public class Program
{
    static Random random = new Random();

    static void Main()
    {

        List<KuantumNesnesi> nesne = new List<KuantumNesnesi>();

        while (true)
        {
            try
            {
                Console.WriteLine("\n===== KUANTUM AMBARI KONTROL PANELİ =====");
                Console.WriteLine("1. Yeni Nesne Ekle");
                Console.WriteLine("2. Envanteri Listele");
                Console.WriteLine("3. Nesneyi Analiz Et");
                Console.WriteLine("4. Acil Durum Soğutması Yap");
                Console.WriteLine("5. Çıkış");
                Console.Write("Seçiminiz: ");

                string secim = Console.ReadLine();
                switch (secim)
                {
                    case "1":
                        YeniNesneEkle(nesne);
                        break;

                    case "2":
                        Listele(nesne);
                        break;

                    case "3":
                        NesneAnalizi(nesne);
                        break;

                    case "4":
                        AcilDurumSoğutmasıUygula(nesne);
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                        break;
                }
            }
            catch (KuantumCokusuExpection hata)
            {
                Console.WriteLine("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...");
                Console.WriteLine(hata.Message);
                return;
            }
            catch (KuantumSonusuExpection hata)
            {
                Console.WriteLine("NESNE SÖNDÜ! TAHLİYE BAŞLATILIYOR...");
                Console.WriteLine(hata.Message);
            }
        }
    }

    static void YeniNesneEkle(List<KuantumNesnesi> envanter)
    {
        Console.WriteLine("Yeni Kuantum Nesnesi Eklendi");
        int tip = random.Next(1, 4);
        KuantumNesnesi nesne;
        if (tip == 1)
            nesne = new VeriPaketi();
        else if (tip == 2)
            nesne = new KaranlikMadde();
        else
            nesne = new AntiMadde();

        nesne.ID = random.Next(0, 10000).ToString();
        nesne.Stabilite = random.Next(20, 81); 
        nesne.TehlikeSeviyesi = random.Next(1, 11);

        envanter.Add(nesne);
        Console.WriteLine($"Nesne ID: {nesne.ID} eklendi.");
    }

    static void Listele(List<KuantumNesnesi> envanter)
    {
        Console.WriteLine("Kuantum Nesneleri Envanteri:");
        foreach (var nesne in envanter)
        {
            Console.WriteLine(nesne.DurumBilgisi());
        }
    }

    static void NesneAnalizi(List<KuantumNesnesi> envanter)
    {
        Console.Write("Analiz edilecek nesnenin ID'sini girin: ");
        string id = Console.ReadLine();
        var nesne = envanter.Find(n => n.ID == id);
        if (nesne != null)
        {
            nesne.AnalizEt();
        }
        else
        {
            Console.WriteLine("Nesne bulunamadı.");
        }
    }

    static void AcilDurumSoğutmasıUygula(List<KuantumNesnesi> envanter)
    {
        Console.Write("Soğutma uygulanacak nesnenin ID'sini girin: ");
        string id = Console.ReadLine();
        var nesne = envanter.Find(n => n.ID == id);
        if (nesne is IKritikal kritikalNesne)
        {
            string sonuc = kritikalNesne.AcilDurumSoğutması();
            Console.WriteLine(sonuc);
        }
        else
        {
            Console.WriteLine("Bu nesne için acil durum soğutması uygulanamaz.");
        }
    }
}
