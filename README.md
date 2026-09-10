# 🎟️ Etkinlik Yönetim Uygulaması

## 📌 Proje Hakkında
Bu proje, kullanıcıların etkinlikleri görüntüleyebilmesi ve yönetebilmesi amacıyla **ASP.NET Core MVC** kullanılarak geliştirilmiştir.

Kullanıcılar sisteme kayıt olabilir, giriş yapabilir, profil bilgilerini güncelleyebilir ve kendi etkinliklerini oluşturup düzenleyebilir. Yönetici rolündeki kullanıcılar ise sistemdeki tüm etkinlikler üzerinde işlem yapabilir.

## 🧰 Kullanılan Teknolojiler

- **Backend:** .NET 8, ASP.NET Core MVC
- **Database:** MSSQL
- **ORM:** Entity Framework Core
- **Frontend:** Razor Views, Bootstrap
- **Calendar:** FullCalendar
- **Text Editor:** CKEditor
- **Authentication:** Cookie Authentication
- **Architecture:** N Katmanlı Mimari
- **Version Control:** Git, GitHub

## ✨ Özellikler

- Kullanıcı kayıt ve giriş işlemleri
- Profil yönetimi
- AES tabanlı parola şifreleme
- Etkinlik oluşturma, düzenleme ve silme
- Etkinlik görseli yükleme
- Etkinlik doğrulama kontrolleri (tarih ilişkisi, tekil başlık, dosya boyutu vb.)
- Public etkinlik listesi ve detay sayfası
- Aylık takvim görünümü
- Admin ve User rol yönetimi
- Rol bazlı etkinlik düzenleme ve silme yetkilendirmesi

## ⚙️ Kurulum ve Çalıştırma

1. Projeyi klonlayın:
```bash
git clone https://github.com/halekrm/EventManagementApp.git
cd EventManagementApp
```

2. `Web/appsettings.json` dosyasındaki MSSQL bağlantı ayarlarını kendi sisteminize göre düzenleyin.

3. Migration'ları veritabanına uygulayın:
```bash
dotnet ef database update --project Repositories --startup-project Web
```

4. Projeyi çalıştırın:
```bash
dotnet run --project Web
```

> **Not:** Boş bir veritabanında uygulama ilk kez çalıştırıldığında örnek kullanıcılar ve etkinlikler `SeedData` aracılığıyla otomatik olarak oluşturulur.

## 👥 Test Hesapları

| Rol | E-posta | Şifre |
| :--- | :--- | :--- |
| **Admin** | `admin@test.com` | `Admin1234` |
| **User** | `user@test.com` | `User1234` |