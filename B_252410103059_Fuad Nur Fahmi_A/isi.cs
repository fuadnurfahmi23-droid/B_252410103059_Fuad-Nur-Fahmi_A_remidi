using System;
using System.Collections.Generic;

namespace B_252410103059_Fuad_Nur_Fahmi_A
{
    class Sekolah
    {
        public string idSekolah;
        public string namaSekolah;
        public int jumlahPaketDiterima;

        public Sekolah(string id, string nama)
        {
            idSekolah = id;
            namaSekolah = nama;
            jumlahPaketDiterima = 0;
        }

        public void TampilInfo()
        {
            Console.WriteLine($"ID Sekolah   : {idSekolah}");
            Console.WriteLine($"Nama Sekolah : {namaSekolah}");
            Console.WriteLine($"Jumlah Paket : {jumlahPaketDiterima}");
        }
    }

    abstract class PaketMBG
    {
        private string idPaket;
        private string namaPaket;
        private int stokDapur;

        public PaketMBG(string id, string nama, int stok)
        {
            idPaket = id;
            namaPaket = nama;

            if (stok < 0)
            {
                stokDapur = 0;
            }
            else
            {
                stokDapur = stok;
            }
        }

        public string IdPaket
        {
            get { return idPaket; }
            set { idPaket = value; }
        }

        public string NamaPaket
        {
            get { return namaPaket; }
            set { namaPaket = value; }
        }

        public int StokDapur
        {
            get { return stokDapur; }
        }
        public bool KurangiStok(int jumlah)
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Jumlah pengurangan harus lebih dari 0.");
                return false;
            }
            if (jumlah > stokDapur)
            {
                Console.WriteLine("Stok tidak cukup.");
                return false;
            }
            stokDapur -= jumlah;
            return true;
        }

        public abstract int HitungBiaya(int jumlah);
    }

    class PaketReguler : PaketMBG
    {
        public PaketReguler(string id, string nama, int stok) : base(id, nama, stok) { }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * 15000;
        }
    }

    class PaketKhusus : PaketMBG
    {
        public string jenisDiet;

        public PaketKhusus(string id, string nama, int stok, string diet) : base(id, nama, stok)
        {
            jenisDiet = diet;
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * 20000;
        }
    }

    interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    class DistribusiMBG : DapatDidistribusikan
    {
        private Sekolah sekolahTujuan;
        private PaketMBG paketDikirim;
        private int jumlah;

        public DistribusiMBG(Sekolah sekolah, PaketMBG paket, int jumlah)
        {
            sekolahTujuan = sekolah;
            paketDikirim = paket;
            this.jumlah = jumlah;
        }

        public void KirimPaket()
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Distribusi gagal: jumlah paket harus lebih dari 0.");
            }
            else if (jumlah > paketDikirim.StokDapur)
            {
                Console.WriteLine($"Distribusi gagal: stok dapur tidak cukup (stok {paketDikirim.StokDapur}, diminta {jumlah}).");
            }
            else
            {
                paketDikirim.KurangiStok(jumlah);
                sekolahTujuan.jumlahPaketDiterima += jumlah;
                Console.WriteLine($"Distribusi MBG berhasil! Sebanyak {jumlah} paket makanan telah dikirim ke sekolah {sekolahTujuan.namaSekolah}");
            }
        }
    }

    // ===== 7. Opsional =====
    class PengelolaMBG
    {
        // Hubungan PengelolaMBG -> Sekolah adalah AGGREGATION (has-a, "memiliki"):
        // PengelolaMBG menyimpan List<Sekolah>, tetapi objek Sekolah dibuat di luar
        // dan diberikan lewat TambahSekolah(). Sekolah tetap bisa ada walaupun
        // PengelolaMBG dihapus (tidak ada ketergantungan hidup).
        // Bukan composition karena Sekolah tidak dibuat/dimiliki penuh oleh PengelolaMBG.
        private List<Sekolah> semuaSekolah = new List<Sekolah>();
        private List<PaketMBG> semuaPaket = new List<PaketMBG>();

        public void TambahSekolah(Sekolah sekolah)
        {
            semuaSekolah.Add(sekolah);
        }

        public void TambahPaket(PaketMBG paket)
        {
            semuaPaket.Add(paket);
        }

        public void TampilkanDataMBG()
        {
            Console.WriteLine("=== Data Sekolah ===");
            foreach (Sekolah s in semuaSekolah)
            {
                s.TampilInfo();
                Console.WriteLine();
            }

            Console.WriteLine("=== Data Paket ===");
            foreach (PaketMBG p in semuaPaket)
            {
                Console.Write($"{p.IdPaket} - {p.NamaPaket} | Stok: {p.StokDapur}");
                if (p is PaketKhusus pk)
                    Console.Write($" | Diet: {pk.jenisDiet}");
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Sekolah s1 = new Sekolah("S01", "SDN 1 Jember");
            Sekolah s2 = new Sekolah("S02", "SMPN 2 Jember");
            s1.TampilInfo();
            s2.TampilInfo();

            PaketMBG p1 = new PaketReguler("P01", "Paket Reguler", 100);
            PaketMBG p2 = new PaketKhusus("P02", "Paket Khusus", 50, "Vegetarian");
            Console.WriteLine($"Biaya 10 {p1.NamaPaket}: Rp{p1.HitungBiaya(10)}");
            Console.WriteLine($"Biaya 10 {p2.NamaPaket}: Rp{p2.HitungBiaya(10)}");

            // Distribusi
            new DistribusiMBG(s1, p1, 30).KirimPaket();
            new DistribusiMBG(s2, p2, 0).KirimPaket();     
            new DistribusiMBG(s2, p2, 999).KirimPaket();   

            // Pengelola
            PengelolaMBG pengelola = new PengelolaMBG();
            pengelola.TambahSekolah(s1);
            pengelola.TambahSekolah(s2);
            pengelola.TambahPaket(p1);
            pengelola.TambahPaket(p2);
            pengelola.TampilkanDataMBG();
        }
    }
}