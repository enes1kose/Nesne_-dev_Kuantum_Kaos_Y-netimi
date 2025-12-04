const readline = require("readline");


// Özel Exception Sınıfları

class KuantumCokusuException extends Error {
    constructor(id) {
        super(`KUANTUM ÇÖKÜŞÜ TESPİT EDİLDİ! Patlayan Nesne ID: ${id}`);
        this.name = "KuantumCokusuException";
    }
}

class KuantumSonusuException extends Error {
    constructor(id) {
        super(`KUANTUM SÖNÜŞÜ TESPİT EDİLDİ! Sönen Nesne ID: ${id}`);
        this.name = "KuantumSonusuException";
    }
}

// IKritik benzeri interface JS’de yok.

// Soyut Sınıf: KuantumNesnesi
class KuantumNesnesi {
    constructor(stabilite, tehlike) {
        this.ID = Math.random().toString(36).substring(2, 8).toUpperCase();
        this._stabilite = 0;
        this.stabilite = stabilite;
        this.tehlikeSeviyesi = tehlike;
    }

    get stabilite() {
        return this._stabilite;
    }

    set stabilite(value) {
        if (value <= 0) {
            this._stabilite = 0;
            throw new KuantumCokusuException(this.ID);
        }

        if (value > 100) {
            this._stabilite = 100;
            throw new KuantumSonusuException(this.ID);
        }

        this._stabilite = value;
    }

    analizEt() {
        throw new Error("Bu metod alt sınıflarda override edilmelidir");
    }

    durumBilgisi() {
        return `[${this.ID}] Tür: ${this.constructor.name} | Stabilite: ${this.stabilite} | Tehlike: ${this.tehlikeSeviyesi}`;
    }
}


// VeriPaketi
class VeriPaketi extends KuantumNesnesi {
    analizEt() {
        console.log(`Veri içeriği analiz edildi...`);
        this.stabilite -= 5;
    }
}

// KaranlikMadde
class KaranlikMadde extends KuantumNesnesi {
    analizEt() {
        console.log(`Karanlık madde titreşiyor...`);
        this.stabilite -= 15;
    }

    acilDurumSogutmasi() {
        console.log("Karanlık madde soğutuluyor...");
        this.stabilite += 50;
    }
}

// AntiMadde
class AntiMadde extends KuantumNesnesi {
    analizEt() {
        console.log(`Evrenin dokusu çatırdıyor...`);
        this.stabilite -= 25;
    }

    acilDurumSogutmasi() {
        console.log("Anti madde soğutması uygulanıyor...");
        this.stabilite += 50;
    }
}

// Menü Sistemi
const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

let envanter = [];

function menu() {
    console.log("\n===== KUANTUM AMBARI KONTROL PANELİ =====");
    console.log("1. Yeni Nesne Ekle");
    console.log("2. Envanteri Listele");
    console.log("3. Nesneyi Analiz Et");
    console.log("4. Acil Durum Soğutması Yap");
    console.log("5. Çıkış");

    rl.question("Seçiminiz: ", cevap => {
        try {
            switch (cevap) {
                case "1": return yeniNesneEkle();
                case "2": return listele();
                case "3": return nesneAnaliz();
                case "4": return sogutma();
                case "5": return rl.close();
                default:
                    console.log("Geçersiz seçim!");
                    return menu();
            }
        } catch (err) {
            if (err instanceof KuantumCokusuException) {
                console.log("\n==============================");
                console.log("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...");
                console.log(err.message);
                console.log("==============================");
                return rl.close();
            }

            if (err instanceof KuantumSonusuException) {
                console.log("\nUYARI: Stabilite 100'ü geçti → SÖNÜŞ!");
                console.log(err.message);
                return menu();
            }

            console.log("HATA:", err.message);
            return menu();
        }
    });
}

// MENÜ Fonksiyonları

function yeniNesneEkle() {
    let tip = Math.floor(Math.random() * 3) + 1;
    let stabilite = Math.floor(Math.random() * 51) + 50; // 50–100 arası
    let tehlike = Math.floor(Math.random() * 10) + 1;

    let nesne;

    if (tip === 1) nesne = new VeriPaketi(stabilite, tehlike);
    else if (tip === 2) nesne = new KaranlikMadde(stabilite, tehlike);
    else nesne = new AntiMadde(stabilite, tehlike);

    envanter.push(nesne);

    console.log(`Yeni nesne eklendi → ID: ${nesne.ID}`);
    menu();
}

function listele() {
    console.log("\n--- Envanter ---");
    envanter.forEach(n => console.log(n.durumBilgisi()));
    menu();
}

function nesneAnaliz() {
    rl.question("Analiz edilecek ID: ", id => {
        let nesne = envanter.find(x => x.ID === id.toUpperCase());

        if (!nesne) {
            console.log("Nesne bulunamadı!");
            return menu();
        }

        try {
            nesne.analizEt();
        } catch (err) {
            throw err;
        }

        menu();
    });
}

function sogutma() {
    rl.question("Soğutulacak ID: ", id => {
        let nesne = envanter.find(x => x.ID === id.toUpperCase());

        if (!nesne) {
            console.log("Nesne bulunamadı!");
            return menu();
        }

        if (typeof nesne.acilDurumSogutmasi !== "function") {
            console.log("Bu nesne soğutulamaz!");
            return menu();
        }

        try {
            nesne.acilDurumSogutmasi();
        } catch (err) {
            throw err;
        }

        menu();
    });
}

menu();
