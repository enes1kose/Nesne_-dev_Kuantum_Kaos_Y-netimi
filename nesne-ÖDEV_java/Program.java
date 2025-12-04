import java.util.*;

// Özel Hata Sınıfları
class KuantumCokusuException extends RuntimeException {
    public KuantumCokusuException(String id) {
        super("KUANTUM ÇÖKÜŞÜ TESPİT EDİLDİ! Patlayan Nesne ID: " + id);
    }
}

class KuantumSonusuException extends RuntimeException {
    public KuantumSonusuException(String id) {
        super("KUANTUM SÖNÜŞÜ TESPİT EDİLDİ! Patlayan Nesne ID: " + id);
    }
}

// IKritik arayüzü
interface IKritik {
    void acilDurumSogutmasi(); 
}

// Soyut Sınıf: KuantumNesnesi
abstract class KuantumNesnesi {
    public String ID;
    private double stabilite;
    public int tehlikeSeviyesi;

    public double getStabilite() {
        return stabilite;
    }

    public void setStabilite(double value) {
        if (value <= 0) {
            stabilite = 0;
            throw new KuantumCokusuException(ID);
        }

        if (value > 100) {
            stabilite = 100;
            throw new KuantumSonusuException(ID);
        }

        stabilite = value;
    }

    public abstract void analizEt();

    public String durumBilgisi() {
        return "[" + ID + "] Tür: " + this.getClass().getSimpleName() 
                + " | Stabilite: " + stabilite;
    }
}

// VeriPaketi
class VeriPaketi extends KuantumNesnesi {
    @Override
    public void analizEt() {
        System.out.println("Veri içeriği okundu... " + getStabilite() + " stabilite azalıyor.");
        setStabilite(getStabilite() - 5);
    }
}

// KaranlikMadde
class KaranlikMadde extends KuantumNesnesi implements IKritik {
    @Override
    public void analizEt() {
        System.out.println("Karanlık madde analiz ediliyor... " + getStabilite() + " stabilite azalıyor.");
        setStabilite(getStabilite() - 15);
    }

    @Override
    public void acilDurumSogutmasi() {
        System.out.println("Karanlık madde soğutuluyor... " + getStabilite() + " stabilite artıyor.");
        setStabilite(getStabilite() + 50);
    }
}

// AntiMadde
class AntiMadde extends KuantumNesnesi implements IKritik {
    @Override
    public void analizEt() {
        System.out.println("Evrenin dokusu titriyor... " + getStabilite() + " stabilite azalıyor.");
        setStabilite(getStabilite() - 25);
    }

    @Override
    public void acilDurumSogutmasi() {
        System.out.println("Anti madde soğutması uygulanıyor... " + getStabilite() + " stabilite artıyor.");
        setStabilite(getStabilite() + 50);
    }
}

// Main 
public class Program {
    static Scanner sc = new Scanner(System.in);
    static Random rnd = new Random();

    public static void main(String[] args) {

        ArrayList<KuantumNesnesi> envanter = new ArrayList<>();

        while (true) {
            try {
                System.out.println("\n===== KUANTUM AMBARI KONTROL PANELİ =====");
                System.out.println("1. Yeni Nesne Ekle");
                System.out.println("2. Envanteri Listele");
                System.out.println("3. Nesneyi Analiz Et");
                System.out.println("4. Acil Durum Soğutması Yap");
                System.out.println("5. Çıkış");
                System.out.print("Seçiminiz: ");

                String secim = sc.nextLine();

                switch (secim) {
                    case "1":
                        yeniNesneEkle(envanter);
                        break;

                    case "2":
                        listele(envanter);
                        break;

                    case "3":
                        nesneAnaliz(envanter);
                        break;

                    case "4":
                        sogutma(envanter);
                        break;

                    case "5":
                        return;

                    default:
                        System.out.println("Geçersiz seçim!");
                        break;
                }

            } catch (KuantumCokusuException | KuantumSonusuException e) {
                System.out.println("\n==============================");
                System.out.println("SİSTEM ACİL DURUMA GEÇTİ!");
                System.out.println(e.getMessage());
                System.out.println("==============================");
                return;
            }
        }
    }

    // MENÜ

    static void yeniNesneEkle(ArrayList<KuantumNesnesi> envanter) {
        int tip = rnd.nextInt(3) + 1;

        KuantumNesnesi nesne;

        if (tip == 1)
            nesne = new VeriPaketi();
        else if (tip == 2)
            nesne = new KaranlikMadde();
        else
            nesne = new AntiMadde();

        nesne.ID = UUID.randomUUID().toString().substring(0, 6);
        nesne.setStabilite(rnd.nextInt(51) + 50); // 50–100
        nesne.tehlikeSeviyesi = rnd.nextInt(10) + 1;

        envanter.add(nesne);

        System.out.println("Yeni nesne eklendi: " + nesne.ID);
    }

    static void listele(ArrayList<KuantumNesnesi> envanter) {
        System.out.println("\n--- Envanter ---");
        for (var nesne : envanter)
            System.out.println(nesne.durumBilgisi());
    }

    static void nesneAnaliz(ArrayList<KuantumNesnesi> envanter) {
        System.out.print("Analiz edilecek ID: ");
        String id = sc.nextLine();

        for (var n : envanter) {
            if (n.ID.equals(id)) {
                n.analizEt();
                return;
            }
        }
        System.out.println("Nesne bulunamadı!");
    }

    static void sogutma(ArrayList<KuantumNesnesi> envanter) {
        System.out.print("Soğutulacak ID: ");
        String id = sc.nextLine();

        for (var n : envanter) {
            if (n.ID.equals(id)) {

                if (n instanceof IKritik kritik) {
                    kritik.acilDurumSogutmasi();
                } else {
                    System.out.println("Bu nesne soğutulamaz!");
                }
                return;
            }
        }

        System.out.println("Nesne bulunamadı!");
    }
}
