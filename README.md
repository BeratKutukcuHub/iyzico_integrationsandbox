# Payment Gateway Strategy: Iyzico & Stripe Entegrasyonu

**.NET 9**, **Clean Architecture** (Temiz Mimari) ve **MongoDB** kullanılarak geliştirilmiş kapsamlı bir E-ticaret Ödeme Entegrasyon sistemi. Bu proje, strateji tasarım desenine uygun (strategy-ready) bir mimari ile dış ödeme ağ geçitlerinin (Iyzico ile başlanmış, Stripe'a hazır) nasıl entegre edileceğini, işlemsel (transactional) sipariş yönetimini ve stok rezervasyon mantığını sergiler.

## 🚀 Genel Bakış

Bu depo, karmaşık ödeme akışlarını yönetmek için modüler ve genişletilebilir bir backend sağlar. Stok rezervasyonundan dış ödeme sistemleri aracılığıyla ödeme alınmasına ve işlemin sonuçlandırılmasına kadar siparişin tüm yaşam döngüsünü yönetir.

### Öne Çıkan Özellikler:

- **Clean Architecture**: Sürdürülebilirlik için birbirinden ayrılmış katmanlar (Domain, UseCase, Persistence, API).
- **Transactional Consistency (İşlemsel Tutarlılık)**: Sipariş-Ödeme-Stok döngüsünde veri bütünlüğünü sağlamak için MongoDB Çoklu Doküman İşlemleri (Transactions) kullanımı.
- **Stok Rezervasyonu**: Ödeme denemeleri sırasında ürünlerin aşırı satılmasını önlemek için "bekleyen stok" (pending stock) mantığı.
- **Strateji Altyapısı**: Birden fazla ödeme sağlayıcısını destekleyecek şekilde mimari edildi (Iyzico uygulandı, Stripe altyapısı hazır).
- **Özel Güvenlik**: HMACSHA256 kullanılarak geliştirilmiş özel kimlik doğrulama şeması ve JWT benzeri token üretimi.

---

## 🛠 Teknoloji Yığını

- **Backend**: .NET 9 Web API
- **Veritabanı**: MongoDB (Transaction desteği için Replica Set ile)
- **Mapper**: AutoMapper
- **Ödeme SDK**: Iyzipay (Iyzico)
- **Frontend**: Vanilla JavaScript / HTML5 / CSS3
- **Dokümantasyon**: Özel geliştirmelerle Swagger UI

---

## 🏗 Mimari Yapı

Proje, Temiz Mimari (Clean Architecture) prensiplerine göre katmanlandırılmıştır:

1.  **Domain**: Çekirdek Varlıklar (`Order`, `Product`, `Customer`, `Payment`), Enumlar ve Değer Nesneleri (Value Objects).
2.  **UseCase (Application)**: İş mantığı, Repository arayüzleri, DTO'lar ve Servis arayüzleri.
3.  **Persistence (Infrastructure)**: MongoDB uygulaması, Unit of Work ve Repository gerçeklemeleri.
4.  **API**: Controller'lar, Middleware'ler (Correlation, Auth) ve Bağımlılık Enjeksiyonu (DI).

---

## ✨ Özellikler

- **🛒 Alışveriş Sepeti**: Basit ve kullanıcı dostu frontend sepet sistemi.
- **💳 Ödeme Entegrasyonu**:
  - Tam kapsamlı Iyzico entegrasyonu (Checkout Form & Callback yönetimi).
  - Asenkron ödeme güncellemeleri için Webhook desteği.
- **📦 Sipariş Yönetimi**:
  - Ödeme sırasında otomatik stok rezervasyonu.
  - Durum takibi (PendingStock, PendingPayment, Paid, Failed).
- **🔐 Güvenlik**:
  - `X-MyToken` üzerinden çalışan özel Kimlik Doğrulama (Authentication) sistemi.
  - Rol tabanlı yetkilendirme (Authorization).
  - Kimlik yönetimi için güvenli şifre hashleme.
- **🛡 Dayanıklılık**:
  - İstekler arası Correlation ID takibi.
  - Idempotency desteği (Frontend tarafında hazırlandı).
  - Global hata yönetimi.

---

## 🚦 Başlarken

### Gereksinimler

- .NET 9 SDK
- MongoDB (**Replica Set** etkinleştirilmiş olmalıdır - Transaction desteği için zorunludur)
- Iyzico Sandbox Hesabı (Anahtarlar `appsettings.json` dosyasında yapılandırılmıştır)

### Kurulum

1.  **Projeyi klonlayın**:

    ```bash
    git clone https://github.com/[kullanici-adiniz]/iyzico_stripe.git
    ```

2.  **MongoDB Yapılandırması**:
    MongoDB örneğinizin replica set olarak çalıştığından emin olun. Yerel olarak şu komutla başlatabilirsiniz:

    ```bash
    mongod --replSet rs0
    ```

    Ardından mongo shell üzerinde `rs.initiate()` komutunu çalıştırın.

3.  **Ayarları Güncelleyin**:
    `Iyzico_Stripe_Strategy/appsettings.json` dosyasını kontrol edin ve gerekiyorsa Bağlantı Dizesini veya Iyzico anahtarlarını güncelleyin.

4.  **Backend'i Çalıştırın**:

    ```bash
    cd Iyzico_Stripe_Strategy
    dotnet run
    ```

5.  **Frontend'i Çalıştırın**:
    `UI/index.html` dosyasını bir canlı sunucu (Live Server) ile açın (Örn: VS Code Live Server, http://127.0.0.1:5500).

---

## 📂 Proje Yapısı

```text
├── Iyzico_Stripe_Strategy/
│   ├── Controllers/     # API Uç Noktaları
│   ├── Domain/          # Varlıklar & Çekirdek Mantık
│   ├── UseCase/         # Repository'ler & DTO'lar
│   ├── Services/        # İş Servisleri (Iyzico, UoW)
│   ├── Middlewares/     # Auth, Correlation vb.
│   └── Options/         # Yapılandırma Sınıfları
└── UI/
    ├── index.html       # Ana Sayfa
    ├── index.js         # Frontend Mantığı & API Çağrıları
    └── index.css        # Premium UI Tasarımı
```

---

## 📜 Lisans

MIT Lisansı ile dağıtılmaktadır. Daha fazla bilgi için `LICENSE` dosyasına bakınız.

---

## 🤝 Katkıda Bulunma

Katkılarınız bu projenin gelişmesine büyük destek sağlayacaktır.

1. Projeyi Forklayın
2. Özellik Dalı Oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi Commit Edin (`git commit -m 'Eklendi: YeniOzellik'`)
4. Dalınıza Push Yapın (`git push origin feature/YeniOzellik`)
5. Bir Pull Request Açın

---

_Hazırlayan: [Berat Kütükçü]
