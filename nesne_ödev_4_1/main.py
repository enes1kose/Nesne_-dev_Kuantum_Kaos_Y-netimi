import random
import os

# ===============================
# 1) Özel Hata Sınıfları
# C# Exception → Python Exception miras alımı
# ===============================

class KuantumCokusuExpection(Exception):
    def __init__(self, id):
        # C#: base("msg") → Python: super().__init__("msg")
        super().__init__(f"Kuantum çöküşü tespit edildi! Nesne ID: {id} stabilitesi kritik seviyenin altına düştüğü için laboratuvar karantinaya alındı.")


class KuantumSonusuExpection(Exception):
    def __init__(self, id):
        super().__init__(f"Kuantum sönüşü uyarısı! Nesne ID: {id} stabilitesi %100'ün üstüne çıktı. Nesne söndü.")


# ===============================
# 2) Abstract Class (C# → Python)
# Python’da abstract class kullanmak için ABC modülü gerekir ama zorunlu değil
# ===============================

class KuantumNesnesi:
    # C#: public string ID { get; set; } → Python: normal değişken
    def __init__(self):
        self.ID = None
        self._Stabilite = 0   # C#: private field
        self.TehlikeSeviyesi = 0

    # C#: Getter/Setter → Python property
    @property
    def Stabilite(self):
        return self._Stabilite

    @Stabilite.setter
    def Stabilite(self, value):
        # C# setter içindeki kontroller Python'a taşındı
        if value < 0:
            self._Stabilite = 0
            raise KuantumCokusuExpection(self.ID)
        elif value > 100:
            self._Stabilite = 100
            raise KuantumSonusuExpection(self.ID)
        else:
            self._Stabilite = value

    # C#: abstract void AnalizEt() → Python’da boş metod
    def AnalizEt(self):
        raise NotImplementedError("Bu metot alt sınıf tarafından ezilmelidir.")

    # C#: string DurumBilgisi()
    def DurumBilgisi(self):
        return (
            f"Nesne ID: {self.ID}, "
            f"Stabilite: {self.Stabilite}, "
            f"Tehlike Seviyesi: {self.TehlikeSeviyesi}, "
            f"Nesne türü: {self.__class__.__name__}"
        )


# ===============================
# 3) Interface (C# → Python)
# Python’da interface yok → sınıfa doğrudan metod konur
# ===============================

class IKritikal:
    def AcilDurumSogutmasi(self):
        raise NotImplementedError("Bu bir arayüz metodudur.")


# ===============================
# 4) Alt sınıflar
# ===============================

class VeriPaketi(KuantumNesnesi):
    def AnalizEt(self):
        print(f"Veri içeriği okundu. Nesne türü:{self.__class__.__name__} Mevcut Stabilite: {self.Stabilite} ve 5 puan azalıyor.")
        self.Stabilite -= 5


class KaranlikMadde(KuantumNesnesi, IKritikal):
    def AnalizEt(self):
        print(f"Nesne analiz ediliyor. Nesne türü:{self.__class__.__name__} Mevcut Stabilite: {self.Stabilite} ve 15 puan azalıyor.")
        self.Stabilite -= 15

    def AcilDurumSogutmasi(self):
        self.Stabilite += 50
        return f"Acil durum soğutması uygulandı. Mevcut Stabilite 50 puan arttı: {self.Stabilite}"


class AntiMadde(KuantumNesnesi, IKritikal):
    def AnalizEt(self):
        print(f"Nesne analiz ediliyor. Evrenin dokusu titriyor. Nesne türü:{self.__class__.__name__} Mevcut Stabilite: {self.Stabilite} ve 25 puan azalıyor.")
        self.Stabilite -= 25

    def AcilDurumSogutmasi(self):
        self.Stabilite += 50
        return f"Acil durum soğutması uygulandı. Mevcut Stabilite: {self.Stabilite} ve 50 puan artıyor."


# ===============================
# 5) Ana Program (C# Main → Python main loop)
# ===============================

def YeniNesneEkle(envanter):
    print("Yeni Kuantum Nesnesi Eklendi")

    tip = random.randint(1, 3)   # C#: Next(1,4) → Python randint

    if tip == 1:
        nesne = VeriPaketi()
    elif tip == 2:
        nesne = KaranlikMadde()
    else:
        nesne = AntiMadde()

    nesne.ID = str(random.randint(0, 9999))
    nesne.Stabilite = random.randint(20, 80)
    nesne.TehlikeSeviyesi = random.randint(1, 10)

    envanter.append(nesne)
    print(f"Nesne ID: {nesne.ID} eklendi.")


def Listele(envanter):
    print("Kuantum Nesneleri Envanteri:")
    for n in envanter:
        print(n.DurumBilgisi())


def NesneAnalizi(envanter):
    id_ = input("Analiz edilecek nesnenin ID'sini girin: ")
    nesne = next((n for n in envanter if n.ID == id_), None)
    if nesne:
        nesne.AnalizEt()
    else:
        print("Nesne bulunamadı.")


def AcilDurumSogutmasiUygula(envanter):
    id_ = input("Soğutma uygulanacak nesnenin ID'sini girin: ")
    nesne = next((n for n in envanter if n.ID == id_), None)

    if isinstance(nesne, IKritikal):
        print(nesne.AcilDurumSogutmasi())
    else:
        print("Bu nesne için acil durum soğutması uygulanamaz.")


def main():
    envanter = []

    while True:
        try:
            print("\n===== KUANTUM AMBARI KONTROL PANELİ =====")
            print("1. Yeni Nesne Ekle")
            print("2. Envanteri Listele")
            print("3. Nesneyi Analiz Et")
            print("4. Acil Durum Soğutması Yap")
            print("5. Çıkış")

            secim = input("Seçiminiz: ")

            if secim == "1":
                YeniNesneEkle(envanter)
            elif secim == "2":
                Listele(envanter)
            elif secim == "3":
                NesneAnalizi(envanter)
            elif secim == "4":
                AcilDurumSogutmasiUygula(envanter)
            elif secim == "5":
                break
            else:
                print("Geçersiz seçim.")

        except KuantumCokusuExpection as e:
            os.system('clear')
            print("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...")
            print(e)            
            break

        except KuantumSonusuExpection as e:
            print("NESNE SÖNDÜ! TAHLİYE BAŞLATILIYOR...")
            print(e)


# Programı çalıştır
main()
